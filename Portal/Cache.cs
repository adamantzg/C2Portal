using System;
using System.Runtime.Caching;

namespace Portal {
	public class Cache : ICache
	{
		public object Get(string key)
		{
			return MemoryCache.Default.Get(key);
		}

		public void Set(string key, object value, DateTime? expiration = null)
		{
			if (expiration == null)
				expiration = DateTime.Today.AddMonths(1);
			MemoryCache.Default.Set(key, value, expiration.Value);
		}

		
	}
}
