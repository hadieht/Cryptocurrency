using Cryptocurrency.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Cryptocurrency
{
	class Program
	{
		static async Task Main(string[] args)
		{		
			var sp = Startup.ConfigureServices();
			var applicationService = sp.GetService<IAppMain>();
			await applicationService.Start();
		}
	}
}
