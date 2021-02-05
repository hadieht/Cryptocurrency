using System;
using System.Collections.Generic;

namespace Cryptocurrency.Domain.ApiResponse
{
	public class ExchangeratesApiResponse
	{
		public Dictionary<string, double> Rates { get; set; }

		public string Base { get; set; }

		public DateTime Date { get; set; }
	}
}
