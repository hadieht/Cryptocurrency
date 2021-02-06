using System;
using System.Collections.Generic;

namespace Cryptocurrency.Domain.Dto
{
	public class ShowCryptoPrices
	{
		public string Symbol { get; set; }
		public string Name { get; set; }
		public DateTime LastUpdated { get; set; }
		public List<CurrenciesRate> CurrenciesRates { get; set; }
	}


	public class CurrenciesRate
	{
		public string Currency { get; set; }
		public decimal Price { get; set; }
	}
}
