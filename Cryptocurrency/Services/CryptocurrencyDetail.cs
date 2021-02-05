using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Services.Proxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public class CryptocurrencyDetail : ICryptocurrencyDetail
	{
		private readonly ILogger<CryptocurrencyDetail> logger;
		private readonly IExchangeRateProxyService exchangeRateProxyService;
		private readonly ICryptoMarketProxyService cryptoMarketProxyService;
		private readonly IOptions<SupportiveCurrenciesSetting> config;

		public CryptocurrencyDetail(IExchangeRateProxyService exchangeRateProxyService,
																ICryptoMarketProxyService cryptoMarketProxyService,
																ILogger<CryptocurrencyDetail> logger,
																IOptions<SupportiveCurrenciesSetting> config)
		{
			this.logger = logger;
			this.exchangeRateProxyService = exchangeRateProxyService;
			this.cryptoMarketProxyService = cryptoMarketProxyService;
			this.config = config;
		}

		public async Task<bool> IsCryptoCurrencyNameValid(string symbol)
		{
			logger.LogDebug($"Validate Currency Name {symbol}");

			var allName = await cryptoMarketProxyService.GetCryptoMap();

			if (allName.Where(t => t.Symbol.ToLower() == symbol).Any())
			{
				return true;
			}

			return false;
		}
		public async Task<List<ShowCryptoPrices>> ShowInfo(string symbol)
		{
			logger.LogDebug($"Show Currency Info {symbol}");

			var result = new List<ShowCryptoPrices>();

			var rates = await exchangeRateProxyService.GetExchangeRate();

			logger.LogDebug($"Exchange Rate Count {rates.Rates.Count()}");

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
