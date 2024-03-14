using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Core.BmonResources;
using Org.BmonOddsGen.Core.ScoreRuleset;
using Org.BmonOddsGen.Clients;
using Org.BmonOddsGen.Host;
using Org.OpenAPITools.Model;
using Newtonsoft.Json;

namespace Org.BmonOddsGen.Core.Generate;

public class MatchGeneratorException : Exception
{
	public MatchGeneratorException(string message) : base(message) { }
};

public interface IMatchGenerator
{
	void GenerateMatchesTick();
}

public class CreationStats
{
	public int generateTicks = 0;
	public int storeUpdates = 0;
	public int matchesCreated = 0;
	public int matchesSent = 0;
	public int matchesExcepted = 0;
	public int matchUpdates = 0;
	public int matchUpdatesSent = 0;
	public int matchUpdatesExcepted = 0;
}

public class MatchGenerator : IMatchGenerator
{
	private readonly ILogger<MatchGenerator> _logger;
	public readonly Dictionary<string, MatchDto> _liveMatches;
	private readonly IBmonResourceStore _bmonResourceStore;
	private readonly IBmonMatchApi _bmonMatchApi;
	private readonly IOptions<EnviromentConfiguration> _env;
	private readonly CreationStats _creationStats;

	private readonly int _maxLiveGames;
	private List<LeagueDto>? _leagues;
	private List<PlayerDto>? _players;
	// TODO: overcomplicating things by not sending player dto's in MatchDto
	private List<PlayerDto> _inPlayPlayers;
	private List<SportDto>? _sports;
	private Dictionary<long, List<long>> _matchIdToPlayerIdsMap;

	public MatchGenerator(ILogger<MatchGenerator> logger, IBmonResourceStore bmonResourceStore, IOptions<EnviromentConfiguration> env, IBmonMatchApi bmonMatchApi)
	{
		_logger = logger;
		_liveMatches = new Dictionary<string, MatchDto>();
		_bmonResourceStore = bmonResourceStore;
		_maxLiveGames = env.Value.GEN_MAX_LIVE_GAMES ?? 10;
		_bmonMatchApi = bmonMatchApi;
		_matchIdToPlayerIdsMap = new Dictionary<long, List<long>>();
		_players = new List<PlayerDto>();
		_inPlayPlayers = new List<PlayerDto>();
		_env = env;
		_creationStats = new CreationStats();
	}

	public void GenerateMatchesTick()
	{
		_creationStats.generateTicks++;
		if (_creationStats.generateTicks % 10 == 0)
		{

			_logger.LogDebug(JsonConvert.SerializeObject(_creationStats));
		}

		// TODO: check for new sports/leagues/players after the first load and merge them 
		if (_sports is null)
		{
			var wasUpdated = UpdateLocalStore();
			if (wasUpdated is false)
			{
				// Will automatically be retried next time.
				return;
			}
		}

		var liveMatchCountDiff = _maxLiveGames - _liveMatches.Count;
		if (liveMatchCountDiff > 0)
		{
			Random random = new Random();

			for (var i = 0; i < liveMatchCountDiff; i++)
			{
				// These nullchecks shouldn't be necessary..
				if (_players is null || _players.Count < 2 || _leagues is null || _leagues.Count is 0 || _sports is null || _sports.Count is 0)
				{
					break;
				}

				var lIndex = random.Next(0, _leagues.Count);
				var sIndex = random.Next(0, _sports.Count);

				var p1Index = random.Next(_players.Count);
				var player1 = _players[p1Index];
				_players.RemoveAt(p1Index);
				_inPlayPlayers.Add(player1);

				var p2Index = random.Next(_players.Count);
				var player2 = _players[p2Index];
				_players.RemoveAt(p2Index);
				_inPlayPlayers.Add(player2);

				CreateNewMatch(_sports[sIndex], _leagues[lIndex], player1, player2);
			}
		}

		UpdateExistingMatches();
	}

	// Returns true if the store was updated
	private bool UpdateLocalStore()
	{
		if (_bmonResourceStore._leagueList is null || _bmonResourceStore._playerList is null || _bmonResourceStore._sportList is null)
		{
			_logger.LogWarning("BmonResourceStore has missing records, store might not be ready yet");
			return false;
		}

		_leagues = new List<LeagueDto>(_bmonResourceStore._leagueList);
		_players = new List<PlayerDto>(_bmonResourceStore._playerList);
		_sports = new List<SportDto>(_bmonResourceStore._sportList);

		_creationStats.storeUpdates++;

		return true;
	}

	//TODO: turn this to async code
	private void CreateNewMatch(SportDto sportDto, LeagueDto leagueDto, PlayerDto playerDto1, PlayerDto playerDto2)
	{
		MatchUpsertDtoMatchState matchStateDto = new MatchUpsertDtoMatchState("0-0", 1, "0-0");
		var matchName = $"{playerDto1.Firstname} {playerDto1.Lastname} v {playerDto2.Firstname} {playerDto2.Lastname}";
		var playerIds = new List<long>() { playerDto1.Id, playerDto2.Id };
		var matchUpsert = new MatchUpsertDto(0, matchName, true, leagueDto, sportDto, playerIds, matchStateDto);

		MatchDto insertedMatch;
		_creationStats.matchesSent++;
		try
		{
			// TODO: xrsf tokens should be optional for non-web
			insertedMatch = _bmonMatchApi.CreateMatch("x-x-x-x-x", matchUpsert);
			_creationStats.matchesCreated++;
		}
		catch (Exception e)
		{
			_logger.LogError(e.Message);
			_creationStats.matchesExcepted++;
			return;
		}

		insertedMatch.Live = true;
		_matchIdToPlayerIdsMap.Add(insertedMatch.Id, playerIds);

		_liveMatches.Add(insertedMatch.Name, insertedMatch);
	}

	private void UpdateExistingMatches()
	{
		Random random = new Random();
		foreach (var liveMatchPair in _liveMatches)
		{
			var liveMatch = liveMatchPair.Value;
			var playerIds = _matchIdToPlayerIdsMap[liveMatch.Id];
			var ruleset = ScoreService.GetRulesetOnSport(liveMatch.Sport.Name, liveMatch.MatchState, _env);
			ruleset.IncrementScore(random.Next(0, 2));
			if (ruleset.HasMatchEnded())
			{
				liveMatch.Live = false;
				_liveMatches.Remove(liveMatchPair.Key);
				ResetPlayerDictionaries(playerIds);
			}

			// TODO: find the c# alternative of mapstruct
			// TODO: also fix the sloppy playerIds, fix MatchDto and send the playerDto's
			MatchUpsertDtoMatchState matchStateDto = new MatchUpsertDtoMatchState(liveMatch.MatchState.PointScore, liveMatch.MatchState.ServingIndex, liveMatch.MatchState.SetScore);
			if (playerIds is null)
			{
				throw new Exception("Missing playerIds for Match: " + liveMatch.Id);
			}

			var matchUpsert = new MatchUpsertDto(liveMatch.Id, liveMatch.Name, liveMatch.Live, liveMatch.League, liveMatch.Sport, playerIds, matchStateDto);

			_creationStats.matchUpdatesSent++;
			try
			{
				_bmonMatchApi.UpdateMatchAndStates("x-x-x-x-x", liveMatch.Id, matchUpsert);
				_creationStats.matchUpdates++;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				_creationStats.matchUpdatesExcepted++;
			}
		}
	}

	private void ResetPlayerDictionaries(List<long> playerIds)
	{
		// TODO: figure out why the compiler is complaining about a null ref
		if (_players is null)
		{
			return;
		}

		var p1 = _inPlayPlayers.Find(playerDto => playerDto.Id == playerIds[0]);
		var p2 = _inPlayPlayers.Find(playerDto => playerDto.Id == playerIds[1]);
		if (p1 is not null)
		{
			_players.Add(p1);
			_inPlayPlayers.Remove(p1);
		}

		if (p2 is not null)
		{
			_players.Add(p2);
			_inPlayPlayers.Remove(p2);
		}
	}
}
