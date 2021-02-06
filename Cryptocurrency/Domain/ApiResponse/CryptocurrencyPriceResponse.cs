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
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "symbol")]
		public string Symbol { get; set; }

		[JsonProperty(PropertyName = "last_updated")]
		public DateTime LastUpdated { get; set; }

		[JsonProperty(PropertyName = "quote")]
		public Dictionary<string, QuotesItems> quotes { get; set; }

	}

	public class QuotesItems
	{
		[JsonProperty(PropertyName = "price")]
		public decimal Price { get; set; }

	}
}
