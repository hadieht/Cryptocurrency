using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public interface ICryptoMarketProxyService
	{
		Task<List<CryptoNameDto>> GetCryptoMap();
		Task<CryptoLatestPriceDto> GetCryptoLatestPrice(string symbole);
	}
}