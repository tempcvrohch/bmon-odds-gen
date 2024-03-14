using Microsoft.AspNetCore.Mvc;

namespace Org.BmonOddsGen.Core.Health;

// TODO: this doesn't really represent the health of the server, instead check if the match generator is actually running and incrementing ticks
[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
	[HttpGet]
	public ActionResult<string> Get()
	{
		return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
	}
}