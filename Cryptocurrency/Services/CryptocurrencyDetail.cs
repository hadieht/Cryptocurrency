using Cryptocurrency.Services.Proxy;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Logging;
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

		public CryptocurrencyDetail(IExchangeRateProxyService exchangeRateProxyService,
																		ICryptoMarketProxyService cryptoMarketProxyService,
																		IJsonSerializer jsonSerializer,
																		ILogger<CryptocurrencyDetail> logger)
		{
			this.jsonSerializer = jsonSerializer;
			this.logger = logger;
			this.exchangeRateProxyService = exchangeRateProxyService;
			this.cryptoMarketProxyService = cryptoMarketProxyService;
		}

		public async Task<bool> IsCryptoCurrencyNameValid(string title)
		{
			var allName = await cryptoMarketProxyService.GetCryptoMap();

			if (allName.Where(t => t.Name.ToLower() == title || t.Symbol.ToLower() == title).Any())
			{
				return true;
			}

			return false;
		}
		public async Task<bool> CaculateInfo()
		{
			var result = await exchangeRateProxyService.GetExchangeRate();

			return false;

		}

	}
}
