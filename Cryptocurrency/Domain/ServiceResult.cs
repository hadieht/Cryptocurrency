using System.Collections.Generic;

namespace Cryptocurrency.Domain
{
	public class ServiceResult<TResult>
	{
		public TResult Result { get; set; }
		public bool Success { get; set; }
		public IEnumerable<ErrorResult> Errors { get; set; }

		public ServiceResult(TResult result)
				: this(true, result, new List<ErrorResult>())
		{
		}

		public ServiceResult(ErrorResult error)
				: this(success: false, result: default(TResult), errors: new List<ErrorResult>() { error })
		{
		}

		public ServiceResult(bool success, TResult result, IEnumerable<ErrorResult> errors)
		{
			this.Success = success;
			this.Result = result;
			this.Errors = errors;
		}
	}
}
