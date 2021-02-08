using AutoFixture;
using Cryptocurrency.Domain;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Services;
using Cryptocurrency.Services.Proxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cryptocurrency.UnitTest
{
	public class CryptocurrencyDetailUnitTest
	{

		[Fact]
		public async Task FailedScenario_IsCryptoCurrencyNameValid_EmtpyString()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var option = Options.Create(new SupportiveCurrenciesSetting());
			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			var symbol = string.Empty;
			//Act

			var result = await cryptocurrencyDetailService.IsCryptoCurrencyNameValid(symbol);

			//Assert
			Assert.True(result != null);
			Assert.False(result.Success);
			Assert.NotNull(result.Error);
			Assert.Equal(Domain.Enum.ErrorType.BlankNotValid, result.Error.Type);
			marketProxyServiceMock.Verify(x => x.GetCryptocurrencyList(), Times.Never);
		}

		[Fact]
		public async Task FailedScenario_IsCryptoCurrencyNameValid_ApiHasError()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var fixture = new Fixture();

			var option = Options.Create(new SupportiveCurrenciesSetting());

			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			ServiceResult<List<CryptoNameDto>> list = null;

			marketProxyServiceMock.Setup(a => a.GetCryptocurrencyList()).ReturnsAsync(list);

			var symbol = fixture.Create<string>();

			//Act

			var result = await cryptocurrencyDetailService.IsCryptoCurrencyNameValid(symbol);

			//Assert
			Assert.True(result != null);
			Assert.False(result.Success);
			Assert.NotNull(result.Error);
			Assert.Equal(Domain.Enum.ErrorType.GeneralError, result.Error.Type);
			marketProxyServiceMock.Verify(x => x.GetCryptocurrencyList(), Times.Once);
		}


		[Fact]
		public async Task RightScenario_IsCryptoCurrencyNameValid_SymbolISContain()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var fixture = new Fixture();
			var symbol = fixture.Create<string>();
			var option = Options.Create(new SupportiveCurrenciesSetting());

			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			var list = fixture.CreateMany<CryptoNameDto>(10).ToList();
			list.Add(new CryptoNameDto { Symbol = symbol });

			var callResult = new ServiceResult<List<CryptoNameDto>>(list);

			marketProxyServiceMock.Setup(a => a.GetCryptocurrencyList()).ReturnsAsync(callResult);

			//Act

			var result = await cryptocurrencyDetailService.IsCryptoCurrencyNameValid(symbol);

			//Assert
			Assert.True(result != null);
			Assert.True(result.Success);
			Assert.Null(result.Error);
			Assert.True(result.Result);
			marketProxyServiceMock.Verify(x => x.GetCryptocurrencyList(), Times.Once);
		}


		[Fact]
		public async Task RightScenario_IsCryptoCurrencyNameValid_SymbolNotContain()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var fixture = new Fixture();
			var symbol = fixture.Create<string>();

			var option = Options.Create(new SupportiveCurrenciesSetting());

			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			var list = fixture.CreateMany<CryptoNameDto>(10).ToList();

			var callResult = new ServiceResult<List<CryptoNameDto>>(list);

			marketProxyServiceMock.Setup(a => a.GetCryptocurrencyList()).ReturnsAsync(callResult);

			//Act

			var result = await cryptocurrencyDetailService.IsCryptoCurrencyNameValid(symbol);

			//Assert
			Assert.True(result != null);
			Assert.True(result.Success);
			Assert.Null(result.Error);
			Assert.False(result.Result);
			marketProxyServiceMock.Verify(x => x.GetCryptocurrencyList(), Times.Once);
		}


		[Fact]
		public async Task FailedScenario_ShowCryptoPrices_EmtpyString()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var option = Options.Create(new SupportiveCurrenciesSetting());
			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			var symbol = string.Empty;

			//Act

			var result = await cryptocurrencyDetailService.ShowCryptoPrices(symbol);

			//Assert
			Assert.True(result != null);
			Assert.False(result.Success);
			Assert.NotNull(result.Error);
			Assert.Equal(Domain.Enum.ErrorType.BlankNotValid, result.Error.Type);
		}

		[Fact]
		public async Task FailedScenario_ShowCryptoPrices_CryptoMarketApiHasError()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var fixture = new Fixture();
			var symbol = fixture.Create<string>();
			var option = Options.Create(new SupportiveCurrenciesSetting());

			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);
			marketProxyServiceMock.Setup(a => a.GetCryptoLatestPrice(symbol)).ReturnsAsync(new ServiceResult<CryptoPrices>(false, null, null));

			var exchangerate = fixture.Create<ExchangeRateDto>();

			exchangeRateProxyServiceMock.Setup(a => a.GetExchangeRate()).ReturnsAsync(new ServiceResult<ExchangeRateDto>(exchangerate));

			//Act

			var result = await cryptocurrencyDetailService.ShowCryptoPrices(symbol);

			//Assert
			Assert.True(result != null);
			Assert.False(result.Success);
			Assert.NotNull(result.Error);
			Assert.Equal(Domain.Enum.ErrorType.GeneralError, result.Error.Type);

			marketProxyServiceMock.Verify(a => a.GetCryptoLatestPrice(It.IsAny<string>()), Times.Once);
			exchangeRateProxyServiceMock.Verify(x => x.GetExchangeRate(), Times.Once);

		}


		[Fact]
		public async Task FailedScenario_ShowCryptoPrices_ExchangeRateApiHasError()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var fixture = new Fixture();
			var symbol = fixture.Create<string>();
			var option = Options.Create(new SupportiveCurrenciesSetting());

			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			var cryptoPrices = fixture.Create<CryptoPrices>();

			marketProxyServiceMock.Setup(a => a.GetCryptoLatestPrice(symbol)).ReturnsAsync(new ServiceResult<CryptoPrices>(cryptoPrices));

			exchangeRateProxyServiceMock.Setup(a => a.GetExchangeRate()).ReturnsAsync(new ServiceResult<ExchangeRateDto>(true, null, null));

			//Act

			var result = await cryptocurrencyDetailService.ShowCryptoPrices(symbol);

			//Assert
			Assert.True(result != null);
			Assert.False(result.Success);
			Assert.NotNull(result.Error);
			Assert.Equal(Domain.Enum.ErrorType.GeneralError, result.Error.Type);
			marketProxyServiceMock.Verify(a => a.GetCryptoLatestPrice(It.IsAny<string>()), Times.Never);
			exchangeRateProxyServiceMock.Verify(x => x.GetExchangeRate(), Times.Once);
		}


		[Fact]
		public async Task RightScenario_ShowCryptoPrices_WithoutConfig()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting());

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var fixture = new Fixture();
			var symbol = fixture.Create<string>();
			var option = Options.Create(new SupportiveCurrenciesSetting());

			var cryptoPrices = fixture.Create<CryptoPrices>();
			var rate = new Dictionary<string, decimal>();
			rate.Add("USD", 1);
			var exchangerate = fixture.Build<ExchangeRateDto>()
					.With(x => x.Rates, rate)
					.Create();
			marketProxyServiceMock.Setup(a => a.GetCryptoLatestPrice(symbol)).ReturnsAsync(new ServiceResult<CryptoPrices>(cryptoPrices));

			exchangeRateProxyServiceMock.Setup(a => a.GetExchangeRate()).ReturnsAsync(new ServiceResult<ExchangeRateDto>(exchangerate));

			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			//Act

			var result = await cryptocurrencyDetailService.ShowCryptoPrices(symbol);

			//Assert
			Assert.True(result != null);
			Assert.True(result.Success);
			Assert.Null(result.Error);
			Assert.Equal(result.Result.Name, cryptoPrices.Name);
			Assert.Equal(result.Result.Symbol, cryptoPrices.Symbol);
			Assert.Equal(result.Result.LastUpdated, cryptoPrices.LastUpdated);
			Assert.Equal(result.Result.CurrenciesRates.Count, exchangerate.Rates.Count);
			marketProxyServiceMock.Verify(a => a.GetCryptoLatestPrice(It.IsAny<string>()), Times.Once);
			exchangeRateProxyServiceMock.Verify(x => x.GetExchangeRate(), Times.Once);
		}


		[Fact]
		public async Task RightScenario_ShowCryptoPrices_WithConfig()
		{
			//Arrange
			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptocurrencyDetail>>();

			var marketProxyServiceMock = new Mock<ICryptoMarketProxyService>();
			var exchangeRateProxyServiceMock = new Mock<IExchangeRateProxyService>();

			var fixture = new Fixture();
			var symbol = fixture.Create<string>();
			var option = Options.Create(new SupportiveCurrenciesSetting());

			var cryptoPrices = fixture.Create<CryptoPrices>();

			var exchangerate = fixture.Create<ExchangeRateDto>();

			var optionsMock = new Mock<IOptions<SupportiveCurrenciesSetting>>();
			var currencies = exchangerate.Rates.Keys;

			optionsMock.SetupGet(o => o.Value).Returns(new SupportiveCurrenciesSetting { Currencies = string.Join(",", currencies) });

			marketProxyServiceMock.Setup(a => a.GetCryptoLatestPrice(symbol)).ReturnsAsync(new ServiceResult<CryptoPrices>(cryptoPrices));

			exchangeRateProxyServiceMock.Setup(a => a.GetExchangeRate()).ReturnsAsync(new ServiceResult<ExchangeRateDto>(exchangerate));

			var cryptocurrencyDetailService = new CryptocurrencyDetail(exchangeRateProxyServiceMock.Object, marketProxyServiceMock.Object, logger.Object, optionsMock.Object);

			//Act

			var result = await cryptocurrencyDetailService.ShowCryptoPrices(symbol);

			//Assert
			Assert.True(result != null);
			Assert.True(result.Success);
			Assert.Null(result.Error);
			Assert.Equal(result.Result.Name, cryptoPrices.Name);
			Assert.Equal(result.Result.Symbol, cryptoPrices.Symbol);
			Assert.Equal(result.Result.LastUpdated, cryptoPrices.LastUpdated);
			Assert.Equal(result.Result.CurrenciesRates.Count, exchangerate.Rates.Count);
			exchangeRateProxyServiceMock.Verify(x => x.GetExchangeRate(), Times.Once);
			marketProxyServiceMock.Verify(a => a.GetCryptoLatestPrice(It.IsAny<string>()), Times.Once);
		}


	}
}
