using Cryptocurrency.Services.Proxy;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public class AppMain : IAppMain
	{
		private readonly ILogger logger;
		private readonly ICryptocurrencyDetail  cryptocurrencyDetail;

		public AppMain(ILogger<AppMain> logger,
									 ICryptocurrencyDetail cryptocurrencyDetail)
		{
			this.logger = logger;
			this.cryptocurrencyDetail = cryptocurrencyDetail;
		}
		public async Task Start()
		{
			bool showMenu = true;
			while (showMenu)
			{
				showMenu = await MainMenu();
			}
		}

		private async Task<bool> MainMenu()
		{
			Console.WriteLine("Enter a Cryptocurrency Name OR Q for Exit: ");

			var input = Console.ReadLine().ToLower();
			if (input == "q")
				return false;
			else
			{
				var isValid = await cryptocurrencyDetail.IsCryptoCurrencyNameValid(input);
				if (!isValid)
				{
					Console.WriteLine("Enterid Name Not Valid!");
					return true;
				}
				var info = await cryptocurrencyDetail.ShowInfo(input);

				Console.WriteLine("Enterid Name Is Correct");
				Console.WriteLine("*********************************************************");
				return true;
			}

		}
	}
}
