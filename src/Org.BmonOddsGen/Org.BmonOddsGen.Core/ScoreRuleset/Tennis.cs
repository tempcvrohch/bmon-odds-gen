using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Core.Exceptions;
using Org.BmonOddsGen.Host;
using Org.OpenAPITools.Model;

namespace Org.BmonOddsGen.Core.ScoreRuleset;

public class BmonScoreException : BmonStateException
{
	public BmonScoreException(int statusCode, object? value = null) : base(statusCode, value) { }
}

public class Tennis : IScoreRuleset
{
	private MatchStateDto _matchStateDto;
	private readonly int _setWinningScore;
	private readonly int _maxSetsPerMatch;
	private readonly int _setDiffForWin;

	public Tennis(MatchStateDto matchStateDto, IOptions<EnviromentConfiguration> env)
	{
		_matchStateDto = matchStateDto;
		_setWinningScore = env.Value.ODDSGEN_TENNIS_WINNING_SCORE ?? 7;
		_maxSetsPerMatch = env.Value.ODDSGEN_TENNIS_MAX_SETS_PER_MATCH ?? 5;
		_setDiffForWin = env.Value.ODDSGEN_TENNIS_SET_DIFF_FOR_WIN ?? 3;
	}

	public MatchStateDto IncrementScore(int winnerIndex)
	{
		if (winnerIndex > 1)
		{
			throw new BmonScoreException(500, winnerIndex);
		}

		IncrementPointScore(winnerIndex);

		return _matchStateDto;
	}

	/**
	* As explained in the server project this is a terrible way to manage scores but a leftover from b365 times.
	* TODO: implement tiebreak sets
	*/
	public bool HasMatchEnded()
	{
		// For now, assume the match has ended if 5 sets have been played.
		var setScore = _matchStateDto.SetScore;
		var splitSetScore = setScore.Split(",");
		
		// No tiebreakers
		if (splitSetScore.Count() >= _maxSetsPerMatch && splitSetScore[splitSetScore.Count() - 1].Contains(_setWinningScore.ToString()))
		{
			return true;
		}

		// TODO: for now assume 2 players per team; 
		var roundsWonOnPlayerIndex = new int[2];
		foreach (var set in splitSetScore)
		{
			if (set.Contains(_setWinningScore.ToString()))
			{
				// the set was completed, check the position of the "7" to decide who the winner is.
				roundsWonOnPlayerIndex[set.IndexOf(_setWinningScore.ToString()) == 0 ? 0 : 1]++;
			}
		}

		var roundsWonDiff = roundsWonOnPlayerIndex[0] - roundsWonOnPlayerIndex[1];
		return Math.Abs(roundsWonDiff) >= _setDiffForWin;
	}

	/**
	* Example of a back and forth game with the second player winning:
	* 0-0, 15-0, 15-15, 30-15, 30-30, 40-30, 40-40(deuce), 30-40, 30-40(w)
	*/
	public void IncrementPointScore(int winnerIndex)
	{
		var pointScore = _matchStateDto.PointScore;
		if (pointScore == null || pointScore == "" || !pointScore.Contains("-"))
		{
			throw new BmonScoreException(500, pointScore);
		}

		var scoreIntArray = ScoreParser.FromScoreString(pointScore);
		scoreIntArray[winnerIndex] += 15;
		var winningPlayerScore = scoreIntArray[winnerIndex];
		var otherPlayerIndex = winnerIndex == 0 ? 1 : 0;
		var otherPlayerScore = scoreIntArray[otherPlayerIndex];

		if (winningPlayerScore == 55 && otherPlayerScore == 40)
		{
			// This was a deuce round, so put the winning player at advantage.
			scoreIntArray[winnerIndex] = 40;
			scoreIntArray[otherPlayerIndex] = 30;
		}
		else if (winningPlayerScore == 55 && otherPlayerScore <= 30)
		{
			// The player won the game, so reset point score and increment set score instead.
			_matchStateDto.PointScore = "0-0";
			IncrementSetScore(winnerIndex);
			return;
		}

		// Tennis game scoring goes from 0-15-30-40
		if (scoreIntArray[winnerIndex] > 40)
		{
			scoreIntArray[winnerIndex] = 40;
		}

		_matchStateDto.PointScore = string.Join("-", scoreIntArray);
	}

	/**
	* Example input: "5-7, 0-2"
	* TODO: implement advantage on setpoints.
	*/
	public void IncrementSetScore(int winnerIndex)
	{
		var setScore = _matchStateDto.SetScore;
		var splitSetScore = setScore.Split(",");
		var lastSplitSetScore = splitSetScore[splitSetScore.Length - 1];

		var scoreIntArray = ScoreParser.FromScoreString(lastSplitSetScore);

		scoreIntArray[winnerIndex]++;

		var updatedLastSplitSetScore = string.Join("-", scoreIntArray);
		splitSetScore[splitSetScore.Length - 1] = updatedLastSplitSetScore;

		_matchStateDto.SetScore = string.Join(",", splitSetScore);
		if (scoreIntArray[winnerIndex] == _setWinningScore && splitSetScore.Length < _maxSetsPerMatch)
		{

			// The set has been won, start a new set.
			_matchStateDto.SetScore = _matchStateDto.SetScore;
			if (!HasMatchEnded())
			{
				_matchStateDto.SetScore += ",0-0";
			}
		}
	}
}