using Cryptocurrency.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public interface ICryptocurrencyDetail
	{
		Task<bool> IsCryptoCurrencyNameValid(string symbol);
		Task<ShowCryptoPrices> ShowCryptoPrices(string symbol);

	}
}