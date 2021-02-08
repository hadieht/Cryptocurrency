using Cryptocurrency.Domain.Enum;
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
		public async Task Start() // Strat app from here 
		{
			logger.LogDebug("Start App");

			bool showMenu = true;
			while (showMenu) // show console menu unit prees q
			{
				showMenu = await MainMenu();
			}
		}

		private async Task<bool> MainMenu()
		{
			Console.Write("Enter a Cryptocurrency Symbol (Q = Exit) : ");

			var input = Console.ReadLine().ToLower();
			if (input == "q")
				return false;
			else
			{
				var isValid = await cryptocurrencyDetail.IsCryptoCurrencyNameValid(input); // control symbol is a valid name

				if (!isValid.Success)
				{
					if (isValid.Error.Type == ErrorType.BlankNotValid)
					{
						Console.WriteLine("Empty Not Valid");
					}
					else
					{
						logger.LogError("Error on call Is Valid");
						Console.WriteLine("Problem on get data occured!");
					}
					return true;
				}

				if (!isValid.Result) // show message and return to menu
				{
					Console.WriteLine("Enterid Symbol Not Valid!");
					logger.LogDebug("Currency Symbol Validate failed");
					Console.WriteLine("*********************************************************");
					return true;
				}

				var info = await cryptocurrencyDetail.ShowCryptoPrices(input); // get data for symbol and proccess to show

				if (!info.Success || info.Result == null)
				{
					if (isValid.Error.Type == ErrorType.BlankNotValid)
					{
						Console.WriteLine("Empty Not Valid");
					}
					else
					{
						logger.LogError("Error on get crypto info");
						Console.WriteLine("Problem on get data occured!");
					}
					return true;
				}

				Console.WriteLine("Result:");

				Console.WriteLine($"Name = {info.Result.Name}  Symbol = {info.Result.Symbol}  Last Update = {info.Result.LastUpdated}");

				foreach (var item in info.Result.CurrenciesRates)
				{
					Console.WriteLine($"{item.Currency}  = {string.Format("{0:0.######}", item.Price)}");
				}
				Console.WriteLine("*********************************************************");
				return true;
			}

		}
	}
}
