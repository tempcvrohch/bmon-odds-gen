using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Host;
using Org.BmonOddsGen.Clients;
using Org.OpenAPITools.Api;

namespace Org.BmonOddsGen.Core.Generate;

[Route("/[controller]")]
[ApiController]
public class GenerateController : ControllerBase
{
	private readonly IGenerateSignaler _generateSignaler;

	public GenerateController(IGenerateSignaler generateSignaler)
	{
		_generateSignaler = generateSignaler;
	}

	[HttpGet("start")]
	public IActionResult Start()
	{
		_generateSignaler.Signal(SignalerState.START);
		return Ok();
	}

	[HttpGet("stop")]
	public IActionResult Stop()
	{
		_generateSignaler.Signal(SignalerState.STOP);
		return Ok();
	}

	[HttpGet("kstart")]
	public IActionResult KStart()
	{
		
		return Ok();
	}
}