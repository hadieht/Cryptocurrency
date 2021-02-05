using Cryptocurrency.Domain.ApiResponse;
using Cryptocurrency.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public interface ICryptoMarketService
	{
		Task<List<CryptoNameDto>> GetCryptoMap();
	}
}