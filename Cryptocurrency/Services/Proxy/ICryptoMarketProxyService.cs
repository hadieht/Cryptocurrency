using Cryptocurrency.Domain;
using Cryptocurrency.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptocurrency.Services.Proxy
{
	public interface ICryptoMarketProxyService
	{
		Task<ServiceResult<List<CryptoNameDto>>> GetCryptocurrencyList();
		Task<ServiceResult<CryptoPrices>> GetCryptoLatestPrice(string symbole);
	}
}