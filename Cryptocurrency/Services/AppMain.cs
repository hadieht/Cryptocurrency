using Cryptocurrency.Services.Proxy;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public class AppMain : IAppMain
	{
		private readonly ILogger logger;
		private readonly ICryptocurrencyDetail cryptocurrencyDetail;

		public AppMain(ILogger<AppMain> logger,
									 ICryptocurrencyDetail cryptocurrencyDetail)
		{
			this.logger = logger;
			this.cryptocurrencyDetail = cryptocurrencyDetail;
		}
		public async Task Start()
		{
			logger.LogDebug("Start App");

			bool showMenu = true;
			while (showMenu)
			{
				showMenu = await MainMenu();
			}
		}

		private async Task<bool> MainMenu()
		{
			Console.WriteLine("Enter a Cryptocurrency Symbol OR Q for Exit: ");

			var input = Console.ReadLine().ToLower();
			if (input == "q")
				return false;
			else
			{
				var isValid = await cryptocurrencyDetail.IsCryptoCurrencyNameValid(input);
				if (!isValid)
				{
					Console.WriteLine("Enterid Symbol Not Valid!");
					logger.LogDebug("Currency Symbol Validate failed");
					return true;
				}

				var info = await cryptocurrencyDetail.ShowInfo(input);
				Console.WriteLine("Result:");

				foreach (var item in info)
				{
					Console.WriteLine($"{item.Symbol}  = {string.Format("{0:0.######}", item.Price)}");
				}
				Console.WriteLine("*********************************************************");
				return true;
			}

		}
	}
}
