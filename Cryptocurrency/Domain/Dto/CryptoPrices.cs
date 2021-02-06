using System;

namespace Cryptocurrency.Domain.Dto
{
	public class CryptoPrices
	{
		public decimal Price { get; set; }
		public string Symbol { get; set; }
		public string Name { get; set; }
		public DateTime LastUpdated { get; set; }

	}
}
