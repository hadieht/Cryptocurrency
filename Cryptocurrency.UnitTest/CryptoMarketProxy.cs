using AutoFixture;
using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Services.Proxy;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net.Http;
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
			var cache = new Mock<Cache>();
			var config = new Moq.Mock<IOptions<CoinMarketCapSetting>>();

			var cryptoMarketProxyService = new CryptoMarketProxyService(httpClientFactory.Object, config.Object, jsonSerializer.Object, logger.Object, cache.Object);
			object output;
			cache.Setup(a => a.CacheData.TryGetValue(It.IsAny<string>(), out output)).Returns(false);
			config.Setup(a => a.Value.ApiKey).Returns("url");
			//Act

			var result = await cryptoMarketProxyService.GetCryptoMap().ConfigureAwait(false);

			//Assert

			cache.Verify(c => c.CacheData.Set(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

			Assert.True(result != null);
		}
	}
}
