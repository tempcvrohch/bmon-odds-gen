using Org.BmonOddsGen.Error;

namespace Org.BmonOddsGen.Core.Exceptions;

public class BmonStateException : HttpResponseException
{
	public BmonStateException(int statusCode, object? value = null) : base(statusCode, value) { }
}