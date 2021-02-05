using Cryptocurrency.Domain.Dto;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public interface IExchangeRateProxyService
	{
		Task<ExchangeRateDto> GetExchangeRate();
	}
}