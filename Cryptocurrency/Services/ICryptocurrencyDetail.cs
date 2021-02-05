using Cryptocurrency.Domain.Dto;
using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public interface ICryptocurrencyDetail
	{
		Task<bool> IsCryptoCurrencyNameValid(string symbol);
		Task<ShowCryptoFiat> ShowInfo(string symbol);

	}
}