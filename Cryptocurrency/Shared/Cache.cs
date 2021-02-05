using Microsoft.Extensions.Caching.Memory;

namespace Cryptocurrency.Shared
{
	public class Cache
	{
		public MemoryCache CacheData { get; set; }
		public Cache()
		{
			CacheData = new MemoryCache(new MemoryCacheOptions
			{
				SizeLimit = 1024
			});
		}
	}
}
