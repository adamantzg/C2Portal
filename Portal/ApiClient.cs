using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Portal
{
	public class ApiClient : IApiClient
	{
		private HttpClient client;

		public ApiClient()
		{
			client = new HttpClient();
		}

		public void AddDefaultRequestHeader(string key, string value)
		{
			client.DefaultRequestHeaders.Add(key, value);
		}

		public Task<HttpResponseMessage> GetAsync(string url)
		{
			return client.GetAsync(url);
		}
	}
}