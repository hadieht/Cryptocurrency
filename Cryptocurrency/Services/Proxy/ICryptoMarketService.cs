using Cryptocurrency.Domain.ApiResponse;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public interface ICryptoMarketService
	{
		Task<CryptoMapInfo> GetCryptoMap();
	}
}