namespace Org.BmonOddsGen.Core.BmonResources;

public class BmonTimedHostedService : IHostedService, IDisposable
{
	private readonly ILogger<BmonTimedHostedService> _logger;
	private readonly IServiceScopeFactory _scopeFactory;
	private Timer? _timer;

	public BmonTimedHostedService(ILogger<BmonTimedHostedService> logger, IServiceScopeFactory scopeFactory)
	{
		_logger = logger;
		_scopeFactory = scopeFactory;
	}

	public Task StartAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("StartAsync");
		_timer = new Timer(DoWork, null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(1));
		return Task.CompletedTask;
	}

	private async void DoWork(object? state)
	{
		using (var scope = _scopeFactory.CreateScope())
		{
			var generateService = scope.ServiceProvider.GetRequiredService<IBmonResourceStore>();
			try
			{
				await generateService.UpdateRemoteResources();
			}
			catch (Exception e)
			{
				_logger.LogError("Failed to update bmon resources: {Source}", e.Source);
			}
		}
	}

	public Task StopAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("StopAsync");
		_timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
}