using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Host;
using Org.BmonOddsGen.Clients;
using Org.OpenAPITools.Api;

namespace Org.BmonOddsGen.Core.Generate;

[Route("api/[controller]")]
[ApiController]
public class GenerateController : ControllerBase
{
	private readonly IGenerateSignaler _generateSignaler;
	private readonly IGenerateService _generateService;

	public GenerateController(IGenerateSignaler generateSignaler, IGenerateService generateService)
	{
		_generateSignaler = generateSignaler;
		_generateService = generateService;
	}

	[HttpGet("start")]
	public IActionResult Start()
	{
		_generateSignaler.Signal(SignalerState.START);
		return Ok("aaa");
	}

	[HttpGet("stop")]
	public IActionResult Stop()
	{
		return Ok("bbb");
		//generateSignaler.Signal(SignalerState.STOP);
	}
}