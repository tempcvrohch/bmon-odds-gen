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

	public GenerateBackgroundService(ILogger<GenerateBackgroundService> logger, IGenerateSignaler generateBackgroundService, IMatchGenerator matchGenerator)
	{
		_logger = logger;
		_generateBackgroundService = generateBackgroundService;
		_matchGenerator = matchGenerator;
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
					_timer = new Timer(DoWork, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
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
		_matchGenerator.GenerateMatchesTick();
	}
}
