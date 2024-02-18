using System.Collections.Concurrent;

namespace Org.BmonOddsGen.Core.Generate;

public enum SignalerState
{
	INIT,
	START,
	STOP
}

public interface IGenerateSignaler
{
	void Signal(SignalerState newState);
	Task<SignalerState> ReadSignal(CancellationToken cancellationToken);
}

public class GenerateSignaler : IGenerateSignaler
{
	private readonly ConcurrentQueue<SignalerState> _concurrentQueue;
	private readonly SemaphoreSlim _semaphoreSlim;

	public GenerateSignaler()
	{
		_concurrentQueue = new ConcurrentQueue<SignalerState>();
		_semaphoreSlim = new SemaphoreSlim(0);
	}

	public void Signal(SignalerState newState)
	{
		_concurrentQueue.Enqueue(newState);
		_semaphoreSlim.Release();
	}

	public async Task<SignalerState> ReadSignal(CancellationToken cancellationToken)
	{
		await _semaphoreSlim.WaitAsync(cancellationToken);
		_concurrentQueue.TryDequeue(out var newState);
		return newState;
	}
}
