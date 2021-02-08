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

			var cryptocurrensieTitles = await cryptoMarketProxyService.GetCryptocurrencyList(); // Get Cryptocurrency List 

			if (cryptocurrensieTitles == null || !cryptocurrensieTitles.Success || cryptocurrensieTitles.Result == null)
			{
				return new ServiceResult<bool>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			if (cryptocurrensieTitles.Result.Where(t => t.Symbol.ToLower() == symbol).Any()) // Is symbol exist in List?
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

			var exchangeRates = await exchangeRateProxyService.GetExchangeRate(); // Get latest exchange rate of currencies

			if (exchangeRates == null || !exchangeRates.Success || exchangeRates.Result == null || !exchangeRates.Result.Rates.Any())
			{
				return new ServiceResult<ShowCryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			logger.LogDebug($"Exchange Rate Count {exchangeRates.Result.Rates.Count()}");

			var cryptoLatestData = await cryptoMarketProxyService.GetCryptoLatestPrice(symbol); // Get latest price of entered symbol

			if (cryptoLatestData == null || !cryptoLatestData.Success || cryptoLatestData.Result == null)
			{
				return new ServiceResult<ShowCryptoPrices>(new ErrorResult { Type = ErrorType.GeneralError });
			}

			result.Name = cryptoLatestData.Result.Name;  // create model for show in  FrontEnd
			result.Symbol = cryptoLatestData.Result.Symbol;
			result.LastUpdated = cryptoLatestData.Result.LastUpdated;
			result.CurrenciesRates = new List<CurrenciesRate>();

			var currencies = new List<string>();
			if (config.Value == null || config.Value.Currencies == null || !config.Value.Currencies.Any()) // config dont have any currency for change
			{
				currencies.Add("USD"); // Add Default Value
			}
			else
			{
				currencies.AddRange(config.Value?.Currencies.Split(",").ToList()); // split supportive currencies
			}

			foreach (var currency in currencies)
			{
				var rate = exchangeRates.Result.Rates[currency]; // find rate of  any currency
				result.CurrenciesRates.Add(new CurrenciesRate { Currency = currency, Price = rate * cryptoLatestData.Result.Price });
			}

			return new ServiceResult<ShowCryptoPrices>(result);

		}

	}
}
