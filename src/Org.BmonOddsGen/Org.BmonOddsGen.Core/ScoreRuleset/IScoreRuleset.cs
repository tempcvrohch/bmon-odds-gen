using Org.OpenAPITools.Model;

namespace Org.BmonOddsGen.Core.ScoreRuleset;

public interface IScoreRuleset {
	MatchStateDto IncrementScore(int winnerIndex);
	void IncrementPointScore(int winnerIndex);
	void IncrementSetScore(int winnerIndex);
	bool HasMatchEnded();
}