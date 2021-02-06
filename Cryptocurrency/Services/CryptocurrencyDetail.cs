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
			if (string.IsNullOrWhiteSpace(symbol))
			{
				return new ServiceResult<bool>(new ErrorResult { Type = ErrorType.BlankNotValid });
			}

			logger.LogDebug($"Validate Currency Name {symbol}");

			var cryptocurrensieTitles = await cryptoMarketProxyService.GetCryptocurrencyList();

			if(!cryptocurrensieTitles.Success || cryptocurrensieTitles.Result == null)
			{
				return new ServiceResult<bool>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			if (cryptocurrensieTitles.Result.Where(t => t.Symbol.ToLower() == symbol).Any())
			{
				return new ServiceResult<bool>(true);
			}

			return new ServiceResult<bool>(false);
		}
		public async Task<ServiceResult<ShowCryptoPrices>> ShowCryptoPrices(string symbol)
		{
			if (string.IsNullOrWhiteSpace(symbol))
			{
				return new ServiceResult<ShowCryptoPrices>(new ErrorResult { Type = ErrorType.BlankNotValid });
			}

			logger.LogDebug($"Show Currency Info {symbol}");

			var result = new ShowCryptoPrices();

			var exchangeRates = await exchangeRateProxyService.GetExchangeRate();

			if (!exchangeRates.Success || exchangeRates.Result == null)
			{
				return new ServiceResult<ShowCryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			logger.LogDebug($"Exchange Rate Count {exchangeRates.Result.Rates.Count()}");

			var cryptoCaltestInfo = await cryptoMarketProxyService.GetCryptoLatestPrice(symbol);

			if (!cryptoCaltestInfo.Success || cryptoCaltestInfo.Result == null)
			{
				return new ServiceResult<ShowCryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			result.Name = cryptoCaltestInfo.Result.Name;
			result.Symbol = cryptoCaltestInfo.Result.Symbol;
			result.LastUpdated = cryptoCaltestInfo.Result.LastUpdated;
			result.CurrenciesRates = new List<CurrenciesRate>();

			foreach (var currency in config.Value.Currencies.Split(","))
			{
				var rate = exchangeRates.Result.Rates[currency];
				result.CurrenciesRates.Add(new CurrenciesRate { Currency = currency, Price = rate * cryptoCaltestInfo.Result.Price });
			}

			return new ServiceResult<ShowCryptoPrices>(result);

		}

	}
}
