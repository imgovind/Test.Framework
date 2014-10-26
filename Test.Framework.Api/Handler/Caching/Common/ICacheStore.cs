using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Test.Framework.Api
{
	public interface ICacheStore : IDisposable
	{

		bool TryGetValue(CacheKey key, out HttpResponseMessage response);
		void AddOrUpdate(CacheKey key, HttpResponseMessage response);
		bool TryRemove(CacheKey key);
		void Clear();

	}
}
