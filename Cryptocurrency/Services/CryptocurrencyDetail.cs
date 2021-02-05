using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Services.Proxy;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public class CryptocurrencyDetail : ICryptocurrencyDetail
	{
		private readonly IJsonSerializer jsonSerializer;
		private readonly ILogger<CryptocurrencyDetail> logger;
		private readonly IExchangeRateProxyService exchangeRateProxyService;
		private readonly ICryptoMarketProxyService cryptoMarketProxyService;
		private readonly IOptions<SupportiveCurrenciesSetting> config;

		public CryptocurrencyDetail(IExchangeRateProxyService exchangeRateProxyService,
																ICryptoMarketProxyService cryptoMarketProxyService,
																IJsonSerializer jsonSerializer,
																ILogger<CryptocurrencyDetail> logger,
																IOptions<SupportiveCurrenciesSetting> config)
		{
			this.jsonSerializer = jsonSerializer;
			this.logger = logger;
			this.exchangeRateProxyService = exchangeRateProxyService;
			this.cryptoMarketProxyService = cryptoMarketProxyService;
			this.config = config;
		}

		public async Task<bool> IsCryptoCurrencyNameValid(string symbol)
		{
			var allName = await cryptoMarketProxyService.GetCryptoMap();

			if (allName.Where(t => t.Symbol.ToLower() == symbol).Any())
			{
				return true;
			}

			return false;
		}
		public async Task<List<ShowCryptoPrices>> ShowInfo(string symbol)
		{
			var result = new List<ShowCryptoPrices>();

			var rates = await exchangeRateProxyService.GetExchangeRate();

			var latestPrice = await cryptoMarketProxyService.GetCryptoLatestPrice(symbol);

			foreach (var currency in config.Value.Currencies.Split(","))
			{
				var rate = rates.Rates[currency];

				result.Add(new ShowCryptoPrices { Symbol = currency, Price = rate * latestPrice });
			}

			return result;

		}

	}
}
