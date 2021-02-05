using Cryptocurrency.Services.Proxy;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public class AppMain : IAppMain
	{
		private readonly ILogger logger;
		private readonly ICryptoMarketService cryptoMarketService;

		public AppMain(ILogger<AppMain> logger , ICryptoMarketService cryptoMarketService)
		{
			this.logger = logger;
			this.cryptoMarketService = cryptoMarketService;
		}
		public async Task Start()
		{
		
			//var x = await cryptoMarketService.GetCryptoMap();

			await MainMenuAsync();

		}

		private async Task<bool> MainMenuAsync()
		{

			return true;
		}
	}
}
