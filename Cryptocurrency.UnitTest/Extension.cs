using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.UnitTest
{
	public static class Extension
	{
		public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, MemoryCacheEntryOptions options)
		{
			using (var entry = cache.CreateEntry(key))
			{
				if (options != null)
				{
					entry.SetOptions(options);
				}

				entry.Value = value;
			}

			return value;
		}


		public static TItem Get<TItem>(this IMemoryCache cache, object key)
		{
			TItem value;
			cache.TryGetValue<TItem>(key, out value);
			return value;
		}

		public static bool TryGetValue<TItem>(this IMemoryCache cache, object key, out TItem value)
		{
			object result;
			if (cache.TryGetValue(key, out result))
			{
				value = (TItem)result;
				return true;
			}

			value = default(TItem);
			return false;
		}

	
	}

	public static class MockMemoryCacheService
	{
		public static IMemoryCache GetMemoryCache(object expectedValue)
		{
			var mockMemoryCache = new Mock<IMemoryCache>();
			mockMemoryCache
					.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
					.Returns(true);
			return mockMemoryCache.Object;
		}
	}
}
