namespace Org.BmonOddsGen.Core.ScoreRuleset;

public static class ScoreParser
{
	// "1-0" -> [1, 0]
	public static int[] FromScoreString(string score)
	{
		var splitScore = score.Split("-");
		var splitIntScore = new int[splitScore.Length];
		for (var i = 0; i < splitScore.Length; i++)
		{
			splitIntScore[i] = int.Parse(splitScore[i]);
		}

		return splitIntScore;
	}
}