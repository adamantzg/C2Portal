using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Portal
{
	public class WebClient : IWebClient
	{
		protected System.Net.WebClient webClient;

		public WebClient()
		{
			this.webClient = new System.Net.WebClient();
		}

		public Stream OpenRead(string url)
		{
			return webClient.OpenRead(url);
		}
	}
}