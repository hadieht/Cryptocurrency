using System.Collections.Generic;

namespace Cryptocurrency.Domain.Dto
{
	public class ExchangeRateDto
	{
		public Dictionary<string, decimal> Rates { get; set; }

		public string Base { get; set; }
	}
}
