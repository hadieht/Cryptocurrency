using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Domain.Enum;
using Cryptocurrency.Domain.Mapping;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
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

		public async Task<CryptoLatestPriceDto> GetCryptoLatestPrice(string symbole)
		{
			var result = new CryptoLatestPriceDto();
			try
			{
				var response = await client.GetAsync(config.Value.LatestPriceApiUrl + "?symbol=" + symbole).ConfigureAwait(false);
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					logger.LogError($"Error on Get GetCryptoLatestPrice with Status Code : {response.StatusCode}");
					return null;
				}

				var json = await response.Content.ReadAsStringAsync();

				dynamic oa = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(json);


				logger.LogDebug($"Executing GetCryptoLatestPrice ");

				//result.Name = data.Data.Btc.Name;
				//result.Symbol = data.Data.Btc.Symbol;
				//result.Price = data.Data.Btc.Quote.Usd.Price;

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error on GetCryptoLatestPrice! ");
			}

			return result;
		}

	}
}
