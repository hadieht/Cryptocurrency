using System.Collections.Generic;

namespace Cryptocurrency.Domain
{
	public class ServiceResult<TResult>
	{
		public TResult Result { get; set; }
		public bool Success { get; set; }
		public ErrorResult Error { get; set; }

		public ServiceResult(TResult result): this(true, result,  null)
		{
		}

		public ServiceResult(ErrorResult error)	: this(success: false, result: default(TResult), error)
		{
		}

		public ServiceResult(bool success, TResult result, ErrorResult error)
		{
			this.Success = success;
			this.Result = result;
			this.Error = error;
		}
	}
}
