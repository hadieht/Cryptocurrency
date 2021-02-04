using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocurrency.Shared
{
	public interface IJsonSerializer
	{
		T Deserialize<T>(string value);
		Task<T> DeserializeHttpContent<T>(HttpContent content);
		string Serialize<T>(T data);
	}
}
