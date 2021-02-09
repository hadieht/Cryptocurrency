using Cryptocurrency.Domain;
using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Domain.Enum;
using Cryptocurrency.Domain.Mapping;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public class ExchangeRateProxyService : IExchangeRateProxyService
	{
		private readonly HttpClient client;
		private readonly IJsonSerializer jsonSerializer;
		private readonly ILogger logger;
		private readonly IOptions<ExchangeratesApiSetting> config;

		public ExchangeRateProxyService(IHttpClientFactory httpClientFactory,
																		IOptions<ExchangeratesApiSetting> config,
																		IJsonSerializer jsonSerializer,
																		ILogger<CryptoMarketProxyService> logger)
		{
			this.client = httpClientFactory.CreateClient();
			this.jsonSerializer = jsonSerializer;
			this.logger = logger;
			this.config = config;
		}

		public async Task<ServiceResult<ExchangeRateDto>> GetExchangeRate()
		{
			var apiResponse = await client.GetAsync(config.Value.ApiUrl).ConfigureAwait(false);
			if (apiResponse.StatusCode != System.Net.HttpStatusCode.OK)
			{
				logger.LogError($"Error on Get Exchange Rate with status code {apiResponse.StatusCode}");
				return new ServiceResult<ExchangeRateDto>(new ErrorResult { Type = ErrorType.ApiCallError });
			}

			var exchangeRates = await jsonSerializer.DeserializeHttpContent<ExchangeratesApiResponse>(apiResponse.Content);

			return new ServiceResult<ExchangeRateDto>(exchangeRates.ToDto());

		}
	}
}
