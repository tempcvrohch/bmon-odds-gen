namespace Org.BmonOddsGen.Core.Generate;

public interface IGenerateService
{

}

public class GenerateService : IGenerateService
{
	private ILogger<GenerateService> _logger;

	public GenerateService(ILogger<GenerateService> logger){
		_logger = logger;
	}
}