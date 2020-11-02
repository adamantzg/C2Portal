using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ApiService.Controllers
{
	public class OnlyAuthorizedAttribute : AuthorizationFilterAttribute
	{
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			IEnumerable<string> values;
			var key = string.Empty;
			if (actionContext.Request.Headers.TryGetValues("apiKey", out values))
				key = values.FirstOrDefault();
			if ((key != null && key == Properties.Settings.Default.apiKey) || actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0)
				base.OnAuthorization(actionContext);
			else
				actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Not authorized.") };
		}
	}
}