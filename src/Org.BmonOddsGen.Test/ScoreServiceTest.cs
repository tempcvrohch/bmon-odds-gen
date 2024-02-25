using Microsoft.Extensions.Options;
using Moq;
using Org.BmonOddsGen.Core.ScoreRuleset;
using Org.BmonOddsGen.Host;
using Org.OpenAPITools.Model;

namespace Org.BmonOddsGen.Test;

public class ScoreServiceTest
{
	private readonly IOptions<EnviromentConfiguration> _env;

	public ScoreServiceTest()
	{
		var env = new Mock<IOptions<EnviromentConfiguration>>();
		env.Setup(e => e.Value).Returns(new EnviromentConfiguration()
		{
			BMON_BACKEND_URL = "http://localhost:1212",
			ODDSGEN_MAX_LIVE_GAMES = 20
		});
		_env = env.Object;
	}

	[Theory]
	[InlineData("Tennis", 0, "2-2", "3-2")]
	[InlineData("Tennis", 1, "2-2", "2-3")]
	[InlineData("Tennis", 0, "0-2", "1-2")]
	[InlineData("Tennis", 1, "0-0", "0-1")]
	[InlineData("Tennis", 0, "0-2,0-0", "0-2,1-0")]
	[InlineData("Tennis", 1, "0-2,0-0", "0-2,0-1")]
	[InlineData("Tennis", 0, "0-2,2-3", "0-2,3-3")]
	[InlineData("Tennis", 1, "0-2,2-3", "0-2,2-4")]
	[InlineData("Tennis", 0, "0-2,2-3,10-10", "0-2,2-3,11-10")]
	[InlineData("Tennis", 1, "0-2,2-3,10-10", "0-2,2-3,10-11")]
	public void IncrementSetScore_ValidSetScore_ReturnsIncrementedSetScore(string sport, int winnerIndex, string score, string expectedScore)
	{
		var matchState = new MatchStateDto();
		var tennis = ScoreService.GetRulesetOnSport(sport, matchState, _env);
		matchState.SetScore = score;
		tennis.IncrementSetScore(winnerIndex);

		Assert.Equal(matchState.SetScore, expectedScore);
	}

	[Theory]
	[InlineData("Tennis", 0, "0-0", "15-0")]
	[InlineData("Tennis", 1, "0-0", "0-15")]
	[InlineData("Tennis", 0, "15-0", "30-0")]
	[InlineData("Tennis", 1, "30-30", "30-40")]
	[InlineData("Tennis", 0, "30-40", "40-40")]
	[InlineData("Tennis", 1, "40-40", "30-40")]
	[InlineData("Tennis", 0, "40-30", "0-0")]
	[InlineData("Tennis", 1, "30-40", "0-0")]
	public void IncrementPointScore_ValidPointScore_ReturnsIncrementedPointScore(string sport, int winnerIndex, string score, string expectedScore)
	{
		var matchState = new MatchStateDto();
		var tennis = ScoreService.GetRulesetOnSport(sport, matchState, _env);
		matchState.PointScore = score;
		matchState.SetScore = "0-0";
		tennis.IncrementPointScore(winnerIndex);

		Assert.Equal(matchState.PointScore, expectedScore);
	}

	[Theory]
	[InlineData("Tennis", 0, "40-30", "0-0", "0-0", "1-0")]
	[InlineData("Tennis", 1, "30-40", "0-0", "0-0", "0-1")]
	[InlineData("Tennis", 0, "40-30", "0-0", "6-7,2-1", "6-7,3-1")]
	[InlineData("Tennis", 1, "30-40", "0-0", "6-7,2-1", "6-7,2-2")]
	public void IncrementPointScore_ValidPointScore_ReturnsIncrementedSetScore(string sport, int winnerIndex, string score, string expectedScore, string setScore, string expectedSetScore)
	{
		var matchState = new MatchStateDto();
		var tennis = ScoreService.GetRulesetOnSport(sport, matchState, _env);
		matchState.PointScore = score;
		matchState.SetScore = setScore;
		tennis.IncrementPointScore(winnerIndex);

		Assert.Equal(matchState.PointScore, expectedScore);
		Assert.Equal(matchState.SetScore, expectedSetScore);
	}

	[Theory]
	[InlineData("Tennis", 0, "40-30", "0-0", "6-5", "7-5,0-0")]
	[InlineData("Tennis", 1, "30-40", "0-0", "6-6", "6-7,0-0")]
	[InlineData("Tennis", 0, "40-30", "0-0", "7-6,6-5", "7-6,7-5,0-0")]
	[InlineData("Tennis", 1, "30-40", "0-0", "7-6,2-7,3-6", "7-6,2-7,3-7,0-0")]
	[InlineData("Tennis", 1, "30-40", "0-0", "7-6,2-7,3-7,5-7,6-6", "7-6,2-7,3-7,5-7,6-7")]
	public void IncrementPointScore_ValidPointScore_ReturnsFreshSet(string sport, int winnerIndex, string score, string expectedScore, string setScore, string expectedSetScore)
	{
		var matchState = new MatchStateDto();
		var tennis = ScoreService.GetRulesetOnSport(sport, matchState, _env);
		matchState.PointScore = score;
		matchState.SetScore = setScore;
		tennis.IncrementPointScore(winnerIndex);

		Assert.Equal(matchState.PointScore, expectedScore);
		Assert.Equal(matchState.SetScore, expectedSetScore);
	}

	[Theory]
	[InlineData("Tennis", "7-6,2-7,3-7,5-7,6-7")]
	[InlineData("Tennis", "7-6,7-6,7-6")]
	[InlineData("Tennis", "6-7,6-7,6-7")]
	public void HasMatchEnded_ValidSetScore_ReturnsTrue(string sport, string setScore)
	{
		var matchState = new MatchStateDto();
		var tennis = ScoreService.GetRulesetOnSport(sport, matchState, _env);
		matchState.SetScore = setScore;

		Assert.True(tennis.HasMatchEnded());
	}

	[Theory]
	[InlineData("Tennis", "7-6,2-7,3-7,5-7,6-6")]
	[InlineData("Tennis", "7-3,5-7,2-7,2-7")]
	public void HasMatchEnded_ValidSetScore_ReturnsFalse(string sport, string setScore)
	{
		var matchState = new MatchStateDto();
		var tennis = ScoreService.GetRulesetOnSport(sport, matchState, _env);
		matchState.SetScore = setScore;

		Assert.False(tennis.HasMatchEnded());
	}
}