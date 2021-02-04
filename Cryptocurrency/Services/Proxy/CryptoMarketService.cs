using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public class CryptoMarketService : ICryptoMarketService
	{
		private readonly HttpClient client;
		private readonly IJsonSerializer jsonSerializer;
		private readonly ILogger logger;
		private readonly IOptions<CoinMarketCapSetting> config;

		public CryptoMarketService(IHttpClientFactory httpClientFactory,
															IOptions<CoinMarketCapSetting> config,
															IJsonSerializer jsonSerializer,
															ILogger<CryptoMarketService> logger)
		{
			this.client = httpClientFactory.CreateClient();
			this.client.DefaultRequestHeaders.Add("Accepts", "application/json");
			this.client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", config.Value.ApiKey);
			this.jsonSerializer = jsonSerializer;
			this.logger = logger;
			this.config = config;
		}

		public async Task<CryptoMapInfo> GetCryptoMap()
		{
			var response = await client.GetAsync(config.Value.MapApiUrl).ConfigureAwait(false);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				logger.LogError("Error on Get Crypto List");
				return null;
			}
			var result = await jsonSerializer.DeserializeHttpContent<CryptoMapInfo>(response.Content);
			return result;
		}

	}
}
