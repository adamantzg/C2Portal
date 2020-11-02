using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Portal
{
	public interface IApiClient
	{
		void AddDefaultRequestHeader(string key, string value);

		Task<HttpResponseMessage> GetAsync(string url);

	}
}
