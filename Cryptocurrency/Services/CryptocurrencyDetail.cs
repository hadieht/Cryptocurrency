using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Services.Proxy;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public class CryptocurrencyDetail : ICryptocurrencyDetail
	{
		private readonly IJsonSerializer jsonSerializer;
		private readonly ILogger<CryptocurrencyDetail> logger;
		private readonly IExchangeRateProxyService exchangeRateProxyService;
		private readonly ICryptoMarketProxyService cryptoMarketProxyService;
		private readonly IOptions<SupportiveCurrencies> config;

		public CryptocurrencyDetail(IExchangeRateProxyService exchangeRateProxyService,
																ICryptoMarketProxyService cryptoMarketProxyService,
																IJsonSerializer jsonSerializer,
																ILogger<CryptocurrencyDetail> logger,
																IOptions<SupportiveCurrencies> config)
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
		public async Task<ShowCryptoFiat> ShowInfo(string symbol)
		{
			var result = new Dictionary<string, decimal>();

			var rates = await exchangeRateProxyService.GetExchangeRate();

			var latestPrice = await cryptoMarketProxyService.GetCryptoLatestPrice(symbol);

			//foreach (var currency in config.Value.Titles)
			//{

			//}

			return new ShowCryptoFiat { Fiats = result };

		}

	}
}
