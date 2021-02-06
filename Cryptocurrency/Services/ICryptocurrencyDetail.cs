using Cryptocurrency.Domain;
using Cryptocurrency.Domain.Dto;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public interface ICryptocurrencyDetail
	{
		Task<ServiceResult<bool>> IsCryptoCurrencyNameValid(string symbol);
		Task<ServiceResult<ShowCryptoPrices>> ShowCryptoPrices(string symbol);

	}
}