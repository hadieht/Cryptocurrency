using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.Domain.ApiResponse
{
	class CryptoPriceResponse
	{
		public Data Data { get; set; }

	}

	public partial class Data
	{
		public Btc Btc { get; set; }
	}

	public partial class Btc
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Symbol { get; set; }
		public string Slug { get; set; }
		public long NumMarketPairs { get; set; }
		public DateTimeOffset LastUpdated { get; set; }
		public Quote Quote { get; set; }
	}

	public partial class Quote
	{
		public Usd Usd { get; set; }
	}

	public partial class Usd
	{
		public double Price { get; set; }
		public DateTimeOffset LastUpdated { get; set; }
	}


}
