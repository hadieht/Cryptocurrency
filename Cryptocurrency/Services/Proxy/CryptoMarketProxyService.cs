using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Domain.Enum;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public class CryptoMarketProxyService : ICryptoMarketProxyService
	{
		private readonly HttpClient client;
		private readonly IJsonSerializer jsonSerializer;
		private readonly ILogger logger;
		private readonly IOptions<CoinMarketCapSetting> config;
		private readonly Cache cache;

		public CryptoMarketProxyService(IHttpClientFactory httpClientFactory,
																		IOptions<CoinMarketCapSetting> config,
																		IJsonSerializer jsonSerializer,
																		ILogger<CryptoMarketProxyService> logger,
																		Cache cache)
		{
			this.client = httpClientFactory.CreateClient();
			this.client.DefaultRequestHeaders.Add("Accepts", "application/json");
			this.client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", config.Value.ApiKey);
			this.jsonSerializer = jsonSerializer;
			this.logger = logger;
			this.config = config;
			this.cache = cache;
		}

		public async Task<List<CryptoNameDto>> GetCryptoMap()
		{
			var result = new List<CryptoNameDto>();
			try
			{
				if (!cache.CacheData.TryGetValue(CacheKey.CryptocurrencyList, out result))
				{
					var response = await client.GetAsync(config.Value.MapApiUrl).ConfigureAwait(false);
					if (response.StatusCode != System.Net.HttpStatusCode.OK)
					{
						logger.LogError($"Error on Get Crypto List with Staus Code : {response.StatusCode}");
						return null;
					}

					var data = await jsonSerializer.DeserializeHttpContent<CryptoMapResponse>(response.Content);

					logger.LogDebug($"Executing GetCryptoMap: {data.Data?.Count()}.");

					result = data.Data.Select(a => new CryptoNameDto
					{
						Symbol = a.Symbol,
						Name = a.Name
					}).ToList();

					var cacheEntryOptions = new MemoryCacheEntryOptions()
							.SetSlidingExpiration(TimeSpan.FromHours(1))
							.SetSize(1024);

					logger.LogDebug("Set crypto list In cache data");

					cache.CacheData.Set(CacheKey.CryptocurrencyList, result, cacheEntryOptions);
				}

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error on GetCryptoMap! ");
			}

			return result;
		}

		public async Task<decimal> GetCryptoLatestPrice(string symbole)
		{
			try
			{
				var response = await client.GetAsync(config.Value.LatestPriceApiUrl + "?symbol=" + symbole).ConfigureAwait(false);
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					logger.LogError($"Error on Get Get Crypto Latest Price API with Status Code : {response.StatusCode}");
					return 0;
				}

				var json = await response.Content.ReadAsStringAsync();

				return ExtractPriceFromDynamicJson(json);

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error on GetCryptoLatestPrice! ");
			}

			return 0;
		}

		private decimal ExtractPriceFromDynamicJson(string json)
		{
			logger.LogDebug($"Begin Extract price from json : {json}");

			try
			{
				dynamic root = jsonSerializer.DeserializeByNewtonsoft<ExpandoObject>(json);

				var rootDic = (IDictionary<string, object>)root;

				var dataNode = rootDic.FirstOrDefault(a => a.Key.ToLower() == "data").Value;

				var FirstNode = (IDictionary<string, object>)dataNode;

				var cryptoNode = FirstNode.Values.FirstOrDefault();

				var cryptoDict = (IDictionary<string, object>)cryptoNode;

				var cryptoQuote = cryptoDict.FirstOrDefault(b => b.Key.ToLower() == "quote").Value;

				var quoteDict = (IDictionary<string, object>)cryptoQuote;

				var firstCurrency = quoteDict.Values.FirstOrDefault();

				var pricesDict = (IDictionary<string, object>)firstCurrency;

				var price = pricesDict.FirstOrDefault(b => b.Key.ToLower() == "price").Value;

				return Convert.ToDecimal(price);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error on Extract Price From DynamicJson! ");
			}
			return 0;
		}
	}
}
