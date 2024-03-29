using NLog;
using NLog.Web;
using Org.BmonOddsGen.Host;
using Org.BmonOddsGen.Clients;
using Org.BmonOddsGen.Error;
using Org.BmonOddsGen.Core.BmonResources;
using Org.BmonOddsGen.Core.Generate;
using System.Diagnostics;
// using Org.OpenAPITools.Api;

if (args.Contains("--wait-for-debugger"))
{
	Debugger.Launch();

	while (args.Contains("--wait-for-debugger") && !Debugger.IsAttached)
	{
		var process = Process.GetCurrentProcess();

		Console.WriteLine("Process id: {0} Awaiting debugger...", process.Id);
		Thread.Sleep(1000);
	}
}
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
	var builder = WebApplication.CreateBuilder(args);
	builder.Configuration.AddEnvironmentVariables();
	builder.Services.Configure<EnviromentConfiguration>(builder.Configuration);
	builder.Services.Configure<OpenApiConfiguration>(builder.Configuration);

	builder.Services.AddHttpClient<IBmonMatchApi, BmonMatchApi>((provider, client) =>
	{
		// TODO: figure out how to get this from EnviromentConfiguration instead.
		client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BMON_BACKEND_URL") ?? throw new Exception("Required .env variable BMON_BACKEND_URL is missing."));
	});
	builder.Services.AddLogging();
	builder.Logging.ClearProviders();
	builder.Host.UseNLog();
	builder.Services.AddControllers(options =>
		{
			options.Filters.Add<HttpResponseExceptionFilter>();
		});
	builder.Services.AddSingleton<IBmonResourceStore, BmonResourceStore>();
	builder.Services.AddSingleton<IGenerateSignaler, GenerateSignaler>();
	builder.Services.AddSingleton<IMatchGenerator, MatchGenerator>();
	builder.Services.AddHostedService<BmonTimedHostedService>();
	builder.Services.AddHostedService<GenerateBackgroundService>();

	var webApplication = builder.Build();

	webApplication.UseHttpsRedirection();
	if (webApplication.Environment.IsDevelopment())
	{
		webApplication.UseExceptionHandler("/error-development");
	}
	else
	{
		webApplication.UseExceptionHandler("/error");
	}

	webApplication.UseAuthorization();
	// TODO: figure out how to get this from EnviromentConfiguration instead.
	webApplication.UsePathBase(new PathString(Environment.GetEnvironmentVariable("GEN_API_PREFIX") ?? "/api"));
	webApplication.UseRouting();
	webApplication.MapControllers();
	webApplication.Run();
}
catch (Exception e)
{
	logger.Error(e, "Fatal exception");
}
finally
{
	LogManager.Shutdown();
}