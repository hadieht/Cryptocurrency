using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Cryptocurrency.Domain.ApiResponse
{
	class CryptocurrencyPriceResponse
	{
		[JsonProperty(PropertyName = "data")]
		public Dictionary<string, DataItem> DataItems { get; set; }
	}

	public class DataItem
	{
		public string name { get; set; }
		public string symbol { get; set; }
		public DateTime last_updated { get; set; }

		[JsonProperty(PropertyName = "quote")]
		public Dictionary<string, QuotesItems> quotes { get; set; }

	}

	public class QuotesItems
	{
		public decimal price { get; set; }

	}
}
