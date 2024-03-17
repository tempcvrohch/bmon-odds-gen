using Org.OpenAPITools.Model;
using Org.OpenAPITools.Api;
using Org.BmonOddsGen.Host;
using Microsoft.Extensions.Options;

namespace Org.BmonOddsGen.Clients;

public interface IBmonMatchApi
{
	MatchDto GetMatchById(long id, int operationIndex = 0);
	MatchDto CreateMatch(MatchUpsertDto matchUpsertDto, int operationIndex = 0);
	void UpdateMatchAndStates(long id, MatchUpsertDto matchUpsertDto, int operationIndex = 0);
}

public class BmonMatchApi : MatchApi, IBmonMatchApi
{
	private readonly HttpClient _httpClient;

	public BmonMatchApi(HttpClient httpClient, IOptions<EnviromentConfiguration> env) : base(env.Value.BMON_BACKEND_URL ?? throw new Exception("Missing .env variable BMON_BACKEND_URL."))
	{
		_httpClient = httpClient;
	}
}