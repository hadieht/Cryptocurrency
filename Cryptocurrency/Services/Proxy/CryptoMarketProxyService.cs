using Cryptocurrency.Domain;
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

		public async Task<ServiceResult<List<CryptoNameDto>>> GetCryptocurrencyList()
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
						return new ServiceResult<List<CryptoNameDto>>(new ErrorResult { Type = ErrorType.ApiCallError });
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
				return new ServiceResult<List<CryptoNameDto>>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			return new ServiceResult<List<CryptoNameDto>>(result);
		}

		public async Task<ServiceResult<CryptoPrices>> GetCryptoLatestPrice(string symbole)
		{
			var result = new CryptoPrices();
			try
			{
				var response = await client.GetAsync(config.Value.LatestPriceApiUrl + "?symbol=" + symbole).ConfigureAwait(false);
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					logger.LogError($"Error on Get Get Crypto Latest Price API with Status Code : {response.StatusCode}");
					return new ServiceResult<CryptoPrices>(new ErrorResult { Type = ErrorType.ApiCallError });
				}

				var json = await response.Content.ReadAsStringAsync();

				var info = jsonSerializer.DeserializeByNewtonsoft<CryptocurrencyPriceResponse>(json);

				if (info.DataItems == null || !info.DataItems.Any() || !info.DataItems.FirstOrDefault().Value.quotes.Any())
				{
					logger.LogDebug("Error on get data from price json! ");
					return new ServiceResult<CryptoPrices>(new ErrorResult { Type = ErrorType.ApiCallError });
				}

				var mainData = info.DataItems.FirstOrDefault().Value;
				result.Name = mainData.Name;
				result.Symbol = mainData.Symbol;
				result.LastUpdated = mainData.LastUpdated;

				var priceInfo = mainData.quotes.FirstOrDefault().Value;

				result.Price = priceInfo.Price;

				return new ServiceResult<CryptoPrices>(result);

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error on GetCryptoLatestPrice! ");
				return new ServiceResult<CryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

		}


	}
}
