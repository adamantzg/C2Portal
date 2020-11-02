using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using Portal.JWT;
using Portal.Model;

namespace Portal.Controllers
{
	public class AccountApiController : BaseApiController
	{
		public AccountApiController(IUnitOfWork unitOfWork,IMailHelper mailHelper, IApiClient apiClient, IMyJwtDecoder jwtDecoder, ICache cache) 
			: base(unitOfWork, mailHelper, apiClient, jwtDecoder, cache)
		{
		}

		[Route("api/login")]
		[HttpPost]
		public object Login(string username, string password)
		{
			var user = uow.UserRepository.Get(u => (u.username == username || u.email == username) && u.password == password, includeProperties: "Customer, Roles").FirstOrDefault();
			if (user == null)
				return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Invalid username or password") };
			if (user.Customer == null && !string.IsNullOrEmpty(user.customer_code))	//fix for code with spaces
				user.Customer = uow.CustomerRepository.GetByID(user.customer_code);
			user.token = JwtManager.CreateToken(user, Properties.Settings.Default.tokenExpiration);
			user.lastLogin = DateTime.Now;
			uow.Save();
			uow.UserRepository.InsertSession(user, GetClientIp(Request));
			user.Permissions = uow.UserRepository.GetPermissions(user);
			cache.Set($"permissions_{user.id}", user.Permissions , null);
			return UserApiController.GetUIObject(user);
		}

		private string GetClientIp(HttpRequestMessage request = null)
		{
			request = request ?? Request;

			if (request?.Properties?.ContainsKey("MS_HttpContext") == true)
			{
				return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
			}
			else if (request?.Properties?.ContainsKey(RemoteEndpointMessageProperty.Name) == true)
			{
				RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
				return prop.Address;
			}
			else if (HttpContext.Current != null)
			{
				return HttpContext.Current.Request.UserHostAddress;
			}
			else
			{
				return null;
			}
		}
	}
}