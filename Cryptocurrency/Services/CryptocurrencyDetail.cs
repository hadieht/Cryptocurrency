using Cryptocurrency.Domain;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Domain.Enum;
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

		public async Task<ServiceResult<bool>> IsCryptoCurrencyNameValid(string symbol)
		{
			logger.LogDebug($"Validate Currency Name {symbol}");

			var allName = await cryptoMarketProxyService.GetCryptocurrencyList();

			if(!allName.Success || allName.Result == null)
			{
				return new ServiceResult<bool>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			if (allName.Result.Where(t => t.Symbol.ToLower() == symbol).Any())
			{
				return new ServiceResult<bool>(true);
			}

			return new ServiceResult<bool>(false);
		}
		public async Task<ServiceResult<ShowCryptoPrices>> ShowCryptoPrices(string symbol)
		{
			logger.LogDebug($"Show Currency Info {symbol}");

			var result = new ShowCryptoPrices();

			var rates = await exchangeRateProxyService.GetExchangeRate();

			if (!rates.Success || rates.Result == null)
			{
				return new ServiceResult<ShowCryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			logger.LogDebug($"Exchange Rate Count {rates.Result.Rates.Count()}");

			var info = await cryptoMarketProxyService.GetCryptoLatestPrice(symbol);

			if (!info.Success || info.Result == null)
			{
				return new ServiceResult<ShowCryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			result.Name = info.Result.Name;
			result.Symbol = info.Result.Symbol;
			result.LastUpdated = info.Result.LastUpdated;
			result.CurrenciesRates = new List<CurrenciesRate>();

			foreach (var currency in config.Value.Currencies.Split(","))
			{
				var rate = rates.Result.Rates[currency];
				result.CurrenciesRates.Add(new CurrenciesRate { Currency = currency, Price = rate * info.Result.Price });
			}

			return new ServiceResult<ShowCryptoPrices>(result);

		}

	}
}
