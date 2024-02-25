using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Core.Exceptions;
using Org.BmonOddsGen.Host;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;

namespace Org.BmonOddsGen.Core.BmonResources;

public interface IBmonResourceStore
{
	List<PlayerDto>? _playerList { get; }
	List<SportDto>? _sportList { get; }
	List<LeagueDto>? _leagueList { get; }
	Task UpdateRemoteResources();
}

public class BmonResourceStore : IBmonResourceStore
{
	private object _storeLock;
	public List<PlayerDto>? _playerList { get; private set; }
	public List<SportDto>? _sportList { get; private set; }
	public List<LeagueDto>? _leagueList { get; private set; }

	private SportsApi _sportsApi;
	private PlayersApi _playersApi;
	private LeaguesApi _leaguesApi;
	private ILogger<BmonResourceStore> _logger;
	private EnviromentConfiguration _env;

	public BmonResourceStore(ILogger<BmonResourceStore> logger, IOptions<EnviromentConfiguration> env)
	{
		_storeLock = new object();
		_env = env.Value;
		if (_env.BMON_BACKEND_URL is null)
		{
			throw new BmonStateException(500, "Required .env variable BMON_BACKEND_URL is missing.");
		}

		_sportsApi = new SportsApi(_env.BMON_BACKEND_URL);
		_playersApi = new PlayersApi(_env.BMON_BACKEND_URL);
		_leaguesApi = new LeaguesApi(_env.BMON_BACKEND_URL);
		_logger = logger;
	}

	public async Task UpdateRemoteResources()
	{
		_logger.LogDebug("UpdateRemoteResources: updating bmon resources");
		var taskL = FetchLeagues();
		var taskP = FetchPlayers();
		var taskS = FetchSports();

		await taskL;
		await taskP;
		await taskS;

		lock (_storeLock)
		{
			_playerList = taskP.Result;
			_sportList = taskS.Result;
			_leagueList = taskL.Result;
		}

		_logger.LogDebug("UpdateRemoteResources: done");
	}

	public async Task<List<LeagueDto>> FetchLeagues()
	{
		var leagueList = await _leaguesApi.GetAllLeaguesAsync();
		if (leagueList is null || leagueList.Count == 0)
		{
			throw new BmonStateException(500);
		}

		return leagueList;
	}

	public async Task<List<PlayerDto>> FetchPlayers()
	{
		var playerList = await _playersApi.GetAllPlayersAsync();
		if (playerList is null || playerList.Count == 0)
		{
			throw new BmonStateException(500);
		}

		return playerList;
	}

	public async Task<List<SportDto>> FetchSports()
	{
		var sportList = await _sportsApi.GetAllSportsAsync();
		if (sportList is null || sportList.Count == 0)
		{
			throw new BmonStateException(500);
		}

		return sportList;
	}
}