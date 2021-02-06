using Cryptocurrency.Domain.Enum;

namespace Cryptocurrency.Domain
{
	public class ErrorResult
	{
		public ErrorType Type { get; set; }
		public string Message { get; set; }
	}
}
