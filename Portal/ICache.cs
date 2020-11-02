using System;

namespace Portal
{
	public interface ICache
	{
		object Get(string key);

		void Set(string key, object value, DateTime? expiration);
	}
}
