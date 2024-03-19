namespace Org.BmonOddsGen.Host;

// TODO: remove these nullables, check and throw once.
public class EnviromentConfiguration
{
	public string? DOTNET_HTTP_PORTS { get; set; }
	public string? DOTNET_ENVIRONMENT { get; set; }

	public string? BMON_BACKEND_URL { get; set; }
	public int? GENERATE_INTERVAL_TICK_MS { get; set; }
	public int? GEN_MAX_LIVE_GAMES { get; set; }
	public int? GEN_TENNIS_MAX_SETS_PER_MATCH{ get; set; }
	public int? GEN_TENNIS_SET_DIFF_FOR_WIN{ get; set; }
	public int? GEN_TENNIS_WINNING_SCORE{ get; set; }

	public string? KAFKA_URL { get; set; }
	public string? KAFKA_GROUP_ID { get; set; }
	public string? KAFKA_MATCHES_TOPIC { get; set; }
}