using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cryptocurrency.Shared
{
	public class JsonSerializer : IJsonSerializer
	{
		internal JsonSerializerOptions setting;
		public JsonSerializer()
		{
			setting = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};

		}
		public T Deserialize<T>(string value)
		{
			return System.Text.Json.JsonSerializer.Deserialize<T>(value, setting);
		}

		public async Task<T> DeserializeHttpContent<T>(HttpContent content)
		{
			using (var contentStream = await content.ReadAsStreamAsync())
			{
				return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(contentStream, setting);
			}
		}

		public string Serialize<T>(T data)
		{
			return System.Text.Json.JsonSerializer.Serialize<T>(data, setting);
		}

		public T DeserializeByNewtonsoft<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

	}
}
