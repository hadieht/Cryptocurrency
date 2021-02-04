using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{

	public class AppMain : IAppMain
	{

		private readonly ILogger logger;

		public AppMain(ILogger<AppMain> logger)
		{
			this.logger = logger;
		}
		public async Task Start()
		{
			logger.LogError("error");
			logger.LogInformation("info");
			await MainMenuAsync();

		}

		private async Task<bool> MainMenuAsync()
		{

			return true;
		}
	}
}
