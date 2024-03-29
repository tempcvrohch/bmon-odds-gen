
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BmonOddsGen.Core.Generate;
using Org.BmonOddsGen.Core.ScoreRuleset;
using Org.OpenAPITools.Model;
using Moq;
using Org.BmonOddsGen.Core.BmonResources;
using Org.BmonOddsGen.Host;
using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Clients;

namespace Org.BmonOddsGen.Test;

public class MatchGeneratorTest
{
	ILogger<MatchGenerator> _logger;
	IBmonResourceStore _bmonResourceStore;
	IOptions<EnviromentConfiguration> _env;

	public MatchGeneratorTest()
	{
		var logger = new Mock<ILogger<MatchGenerator>>();
		_logger = logger.Object;

		var env = new Mock<IOptions<EnviromentConfiguration>>();
		env.Setup(e => e.Value).Returns(new EnviromentConfiguration()
		{
			BMON_BACKEND_URL = "http://localhost:1212",
			GEN_MAX_LIVE_GAMES = 20
		});
		_env = env.Object;

		var bmonResourceStore = new Mock<IBmonResourceStore>();
		bmonResourceStore.Setup(e => e._sportList).Returns(new List<SportDto>(){
			new SportDto(0, DateTime.Now, DateTime.Now, "Tennis"){}
		});

		bmonResourceStore.Setup(e => e._leagueList).Returns(new List<LeagueDto>(){
			new LeagueDto(0, DateTime.Now, DateTime.Now, "League smallboys"){},
			new LeagueDto(1, DateTime.Now, DateTime.Now, "League bigboys"){},
		});

		bmonResourceStore.Setup(e => e._playerList).Returns(new List<PlayerDto>(){
			new PlayerDto(0, DateTime.Now, DateTime.Now, "Michael", "Jackson", "michael-jackson", "US"){},
			new PlayerDto(1, DateTime.Now, DateTime.Now, "Pierce", "Lemon", "pierce-lemon", "US"){},
			new PlayerDto(2, DateTime.Now, DateTime.Now, "Lebron", "James", "lebron-james", "US"){},
			new PlayerDto(3, DateTime.Now, DateTime.Now, "Arjen", "Robben", "arjen-robben", "NL"){},
			new PlayerDto(4, DateTime.Now, DateTime.Now, "Michael", "Shumagger", "michael-schumagger", "DU"){},
			new PlayerDto(5, DateTime.Now, DateTime.Now, "Max", "Verstappen", "max-verstappen", "NL"){},
			new PlayerDto(6, DateTime.Now, DateTime.Now, "Lewis", "Hammilton", "lewis-hammilton", "UK}"){},
		});
		_bmonResourceStore = bmonResourceStore.Object;
	}

	[Fact]
	public void GenerateMatchesTick_InitialEmptyState_HasCreatedMatches(){
		var bmonMatchApi = new Mock<IBmonMatchApi>();
		var matchIndex = 0;
		bmonMatchApi.Setup(b => b.CreateMatch(It.IsAny<string>(), It.IsAny<MatchUpsertDto>(), 0))
			.Returns((string uuid, MatchUpsertDto matchUpsertDto, int noIdea) => {
				return new MatchDto(id: matchIndex++, name: matchUpsertDto.Name, league: new LeagueDto(name: "big boys"), sport: new SportDto(name: "Tennis"), matchState: new MatchStateDto(pointScore: "0-0", setScore: "0-0"));
			});
		bmonMatchApi.Setup(b => b.UpdateMatchAndStates(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<MatchUpsertDto>(), It.IsAny<int>()));

		var matchGenerator = new MatchGenerator(_logger, _bmonResourceStore, _env, bmonMatchApi.Object);
		matchGenerator.GenerateMatchesTick();

		bmonMatchApi.Verify(b => b.CreateMatch(It.IsAny<string>(), It.IsAny<MatchUpsertDto>(), 0), Times.Exactly(3));
		bmonMatchApi.Verify(b => b.UpdateMatchAndStates(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<MatchUpsertDto>(), It.IsAny<int>()), Times.Exactly(3));
		
		Assert.Equal(3, matchGenerator._liveMatches.Count);
	}
}