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
			Console.Write("Enter a Cryptocurrency Symbol (Q=Exit) : ");

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
					Console.WriteLine("*********************************************************");
					return true;
				}

				var info = await cryptocurrencyDetail.ShowCryptoPrices(input);
				Console.WriteLine("Result:");

				Console.WriteLine($"Name = {info.Name}  Symbol = {info.Symbol}  Last Update = {info.LastUpdated}");

				foreach (var item in info.CurrenciesRates)
				{
					Console.WriteLine($"{item.Currency}  = {string.Format("{0:0.######}", item.Price)}");
				}
				Console.WriteLine("*********************************************************");
				return true;
			}

		}
	}
}
