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
		private readonly IMemoryCache cache;

		public CryptoMarketProxyService(IHttpClientFactory httpClientFactory,
																		IOptions<CoinMarketCapSetting> config,
																		IJsonSerializer jsonSerializer,
																		ILogger<CryptoMarketProxyService> logger,
																		IMemoryCache cache)
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
				if (!cache.TryGetValue(CacheKey.CryptocurrencyList, out result))
				{
					var apiResponse = await client.GetAsync(config.Value.MapApiUrl).ConfigureAwait(false);
					if (apiResponse.StatusCode != System.Net.HttpStatusCode.OK)
					{
						logger.LogError($"Error on Get Crypto List with Staus Code : {apiResponse.StatusCode}");
						return new ServiceResult<List<CryptoNameDto>>(new ErrorResult { Type = ErrorType.ApiCallError });
					}

					var cryptoList = await jsonSerializer.DeserializeHttpContent<CryptoMapResponse>(apiResponse.Content);

					logger.LogDebug($"Get creypto List, Crypto Count : {cryptoList.Data?.Count()}.");

					result = cryptoList.Data.Select(a => new CryptoNameDto
					{
						Symbol = a.Symbol,
						Name = a.Name
					}).ToList();

					logger.LogDebug("Cache , Insert crypto list");

					cache.Set(CacheKey.CryptocurrencyList, result);
				}

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error on Get Crypto List!");
				return new ServiceResult<List<CryptoNameDto>>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			return new ServiceResult<List<CryptoNameDto>>(result);
		}

		public async Task<ServiceResult<CryptoPrices>> GetCryptoLatestPrice(string symbole)
		{
			if (string.IsNullOrWhiteSpace(symbole))
			{
				return new ServiceResult<CryptoPrices>(new ErrorResult { Type = ErrorType.BlankNotValid });
			}

			var result = new CryptoPrices();
			try
			{
				var apiResponse = await client.GetAsync(config.Value.LatestPriceApiUrl + "?symbol=" + symbole).ConfigureAwait(false);

				if (apiResponse.StatusCode != System.Net.HttpStatusCode.OK)
				{
					logger.LogError($"Error on Get Get Crypto Latest Price API with Status Code : {apiResponse.StatusCode}");
					return new ServiceResult<CryptoPrices>(new ErrorResult { Type = ErrorType.ApiCallError });
				}

				var json = await apiResponse.Content.ReadAsStringAsync();

				var cryptoLatestInfo = jsonSerializer.DeserializeByNewtonsoft<CryptocurrencyPriceResponse>(json);

				if (cryptoLatestInfo.DataItems == null || !cryptoLatestInfo.DataItems.Any() || !cryptoLatestInfo.DataItems.FirstOrDefault().Value.quotes.Any())
				{
					logger.LogDebug($"Error on get data from price json data : {json} ");
					return new ServiceResult<CryptoPrices>(new ErrorResult { Type = ErrorType.ApiCallError });
				}

				var mainData = cryptoLatestInfo.DataItems.FirstOrDefault().Value;
				result.Name = mainData.Name;
				result.Symbol = mainData.Symbol;
				result.LastUpdated = mainData.LastUpdated;

				var priceInfo = mainData.quotes.FirstOrDefault().Value;

				result.Price = priceInfo.Price;

				return new ServiceResult<CryptoPrices>(result);

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error on Get Crypto Latest Price! ");
				return new ServiceResult<CryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

		}


	}
}
