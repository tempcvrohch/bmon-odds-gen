using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Host;

namespace Org.BmonOddsGen.Core.Generate;

public class GenerateBackgroundException : Exception
{
	public GenerateBackgroundException(string message) : base(message) { }
};

public class GenerateBackgroundService : BackgroundService
{
	private readonly ILogger<GenerateBackgroundService> _logger;
	private readonly IGenerateSignaler _generateBackgroundService;
	private Timer? _timer;
	private readonly IMatchGenerator _matchGenerator;
	private readonly IOptions<EnviromentConfiguration> _options;
	private readonly int DEFAULT_INTERVAL_TICKS_MS = 1000;
	public GenerateBackgroundService(ILogger<GenerateBackgroundService> logger, IGenerateSignaler generateBackgroundService, IMatchGenerator matchGenerator, IOptions<EnviromentConfiguration> options)
	{
		_logger = logger;
		_generateBackgroundService = generateBackgroundService;
		_matchGenerator = matchGenerator;
		_options = options;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			var signal = await _generateBackgroundService.ReadSignal(cancellationToken);

			switch (signal)
			{
				case SignalerState.START:
					if (_timer is not null)
					{
						throw new GenerateBackgroundException("Tried to start timer with an already existing timer running.");
					}
					var interval = _options.Value.GENERATE_INTERVAL_TICK_MS ?? DEFAULT_INTERVAL_TICKS_MS;
					_logger.LogInformation("Starting matchgenerator timer, tick every {0} ms.", interval);
					_timer = new Timer(DoWork, null, TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(interval));
					break;
				case SignalerState.STOP:
					if (_timer is null)
					{
						throw new GenerateBackgroundException("Tried to stop timer without an existing timer.");
					}

					_timer?.Change(Timeout.Infinite, 0); // Not sure if this is needed.
					_timer?.Dispose();
					break;
			}
		}
	}

	public void DoWork(object? state)
	{
		try
		{
			_matchGenerator.GenerateMatchesTick();
		}
		catch (Exception e)
		{
			_logger.LogDebug(e.ToString());
		}
	}
}
