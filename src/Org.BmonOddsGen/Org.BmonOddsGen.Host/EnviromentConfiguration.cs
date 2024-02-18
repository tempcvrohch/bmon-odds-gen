namespace Org.BmonOddsGen.Host;

// TODO: remove these nullables, check and throw once.
public class EnviromentConfiguration
{
	public string? DOTNET_HTTP_PORTS { get; set; }
	public string? DOTNET_ENVIRONMENT { get; set; }

	public string? BMON_BACKEND_URL { get; set; }
	public int? ODDSGEN_MAX_LIVE_GAMES { get; set; }
}