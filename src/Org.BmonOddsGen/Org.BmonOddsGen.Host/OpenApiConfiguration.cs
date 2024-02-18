using Org.OpenAPITools.Client;

namespace Org.BmonOddsGen.Host;

public class OpenApiConfiguration : Configuration {
	public OpenApiConfiguration(string basePath) : base(){
		BasePath = basePath;
	}
}