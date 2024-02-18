using Org.BmonOddsGen.Core.Exceptions;
using Org.OpenAPITools.Model;

namespace Org.BmonOddsGen.Core.ScoreRuleset;

public class BmonUnsupportedSportException : BmonStateException
{
	public BmonUnsupportedSportException(int statusCode, object? value = null) : base(statusCode, value) { }
}

public class ScoreService
{
	public static IScoreRuleset GetRulesetOnSport(string sportName, MatchStateDto matchStateDto)
	{
		switch (sportName)
		{
			case "Tennis": return new Tennis(matchStateDto);
			default: throw new BmonUnsupportedSportException(500, sportName);
		}
	}
}