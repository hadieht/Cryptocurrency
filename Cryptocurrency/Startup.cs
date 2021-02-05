using Cryptocurrency.Domain.AppConfig;
using Cryptocurrency.Services;
using Cryptocurrency.Services.Proxy;
using Cryptocurrency.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;

namespace Cryptocurrency
{
	public static class Startup
	{
		public static IConfiguration Configuration { get; }
		static Startup()
		{
			var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
			config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			Configuration = config.Build();


		}
		public static ServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddHttpClient();

			var coinMarketCapSettingSection = Configuration.GetSection("CoinMarketCapSetting");
			services.Configure<CoinMarketCapSetting>(coinMarketCapSettingSection);

			var exchangeratesApiSettingSection = Configuration.GetSection("ExchangeratesApiSetting");
			services.Configure<ExchangeratesApiSetting>(exchangeratesApiSettingSection);

			var supportiveCurrencies = Configuration.GetSection("SupportiveCurrencies");
			services.Configure<SupportiveCurrenciesSetting>(supportiveCurrencies);

			services.AddSingleton<IJsonSerializer, JsonSerializer>();
			services.AddSingleton<Cache>();

			services.AddScoped<IAppMain, AppMain>();
			services.AddScoped<ICryptoMarketProxyService, CryptoMarketProxyService>();
			services.AddScoped<IExchangeRateProxyService, ExchangeRateProxyService>();
			services.AddScoped<ICryptocurrencyDetail, CryptocurrencyDetail>();

			var serilogLogger = new LoggerConfiguration()
																									.ReadFrom.Configuration(Configuration)
																									.CreateLogger();

			services.AddLogging(builder =>
			{
				builder.AddSerilog(logger: serilogLogger, dispose: true);
			});

			return services.BuildServiceProvider();
		}

	}
}
