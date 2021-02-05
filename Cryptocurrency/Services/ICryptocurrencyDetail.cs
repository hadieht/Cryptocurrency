using System.Threading.Tasks;

namespace Cryptocurrency.Services
{
	public interface ICryptocurrencyDetail
	{
		Task<bool> IsCryptoCurrencyNameValid(string title);
		Task<bool> CaculateInfo();
	}
}