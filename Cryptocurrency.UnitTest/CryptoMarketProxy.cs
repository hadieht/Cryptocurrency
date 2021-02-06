using AutoFixture;
using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Domain.Dto;
using Cryptocurrency.Domain.Enum;
using Cryptocurrency.Services.Proxy;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cryptocurrency.UnitTest
{
	public class CryptoMarketProxy
	{
		[Fact]
		public async Task FailedScenario_GetCryptoMap_ResponseNotValid()
		{
			//Arrange

			var httpClientFactory = new Mock<IHttpClientFactory>();

			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptoMarketProxyService>>();
			var jsonSerializer = new Mock<Shared.IJsonSerializer>();
			var cache = new Mock<IMemoryCache>();

			var optionsMock = new Mock<IOptions<CoinMarketCapSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new CoinMarketCapSetting());
			var httpMessageHandler = new Mock<HttpMessageHandler>();
			var fixture = new Fixture();

			httpMessageHandler.Protected()
					.Setup<Task<HttpResponseMessage>>(
							"SendAsync",
							ItExpr.IsAny<HttpRequestMessage>(),
							ItExpr.IsAny<CancellationToken>()
					)
					.ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
					{
						HttpResponseMessage response = new HttpResponseMessage();
						response.StatusCode = System.Net.HttpStatusCode.BadRequest;
						return response;
					});

			var httpClient = new HttpClient(httpMessageHandler.Object);
			httpClient.BaseAddress = fixture.Create<Uri>();

			httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
			//var memoryCache = MockMemoryCacheService.GetMemoryCache(null);
			var option = Options.Create(new CoinMarketCapSetting());

			var cryptoMarketProxyService = new CryptoMarketProxyService(httpClientFactory.Object, optionsMock.Object, jsonSerializer.Object, logger.Object, cache.Object);

			//Act

			var result = await cryptoMarketProxyService.GetCryptocurrencyList().ConfigureAwait(false);

			//Assert
			Assert.True(result != null);
			Assert.False(result.Success);
			Assert.NotNull(result.Error);
			Assert.Equal(Domain.Enum.ErrorType.ApiCallError, result.Error.Type);

		}



		[Fact]
		public async Task RightScenario_GetCryptoMap_WithoutCache()
		{
			//Arrange

			var httpClientFactory = new Mock<IHttpClientFactory>();

			var logger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptoMarketProxyService>>();
			var jsonSerializer = new Mock<Shared.IJsonSerializer>();
			var cache = new Mock<IMemoryCache>();

			var optionsMock = new Mock<IOptions<CoinMarketCapSetting>>();
			optionsMock.SetupGet(o => o.Value).Returns(new CoinMarketCapSetting());
			var httpMessageHandler = new Mock<HttpMessageHandler>();
			var fixture = new Fixture();
			var finalResult = fixture.Create<List<CryptoNameDto>>();
			var cryptoResponse = fixture.Create<CryptoMapResponse>();
			jsonSerializer.Setup(a => a.DeserializeHttpContent<CryptoMapResponse>(It.IsAny<HttpContent>())).ReturnsAsync(cryptoResponse);


			httpMessageHandler.Protected()
					.Setup<Task<HttpResponseMessage>>(
							"SendAsync",
							ItExpr.IsAny<HttpRequestMessage>(),
							ItExpr.IsAny<CancellationToken>()
					)
					.ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
					{
						HttpResponseMessage response = new HttpResponseMessage();
						response.StatusCode = System.Net.HttpStatusCode.OK;
						response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(finalResult)); 
						return response;
					});

			var httpClient = new HttpClient(httpMessageHandler.Object);

			var memoryCache = MockMemoryCacheService.GetMemoryCache(null);

			httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

			var option = Options.Create(new CoinMarketCapSetting());

			var cryptoMarketProxyService = new CryptoMarketProxyService(httpClientFactory.Object, optionsMock.Object, jsonSerializer.Object, logger.Object, cache.Object);

			//Act

			var result = await cryptoMarketProxyService.GetCryptocurrencyList().ConfigureAwait(false);

			//Assert
			Assert.True(result != null);
			Assert.True(result.Success);
			Assert.Null(result.Error);
			//object d;
			//cache.Verify(x => x.Set(CacheKey.CryptocurrencyList, result), Times.Once);
			//cache.Verify(x => x.TryGetValue(It.IsAny<CacheKey>(), out d), Times.Once);
		}

	}
}
