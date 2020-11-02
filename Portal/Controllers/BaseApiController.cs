using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using Portal.JWT;
using Portal.Model;

namespace Portal.Controllers
{
	public class BaseApiController : ApiController
	{
		protected IApiClient apiClient;

		protected IUnitOfWork uow;
		protected IMailHelper mailHelper;
		protected IMyJwtDecoder jwtDecoder;
		protected ICache cache;

		public BaseApiController(IUnitOfWork unitOfWork, IMailHelper mailHelper, IApiClient apiClient, IMyJwtDecoder jwtDecoder, ICache cache)
		{
			uow = unitOfWork;
			this.apiClient = apiClient;
			this.mailHelper = mailHelper;
			this.jwtDecoder = jwtDecoder;
			apiClient?.AddDefaultRequestHeader("apiKey", Properties.Settings.Default.jwtKey);
			this.cache = cache;
		}

		public User GetCurrentUser()
		{
			var user_id = jwtDecoder.GetUserIdFromToken(Request.Headers.Authorization.Parameter);
			if (user_id != null)
			{
				var user =  uow.UserRepository.Get(u=>u.id == user_id, includeProperties: "Roles").FirstOrDefault();
				if (user != null)
					user.Permissions = GetUserPermissions(user);
				return user;
			}
				
			return null;
		}

		public List<Permission> GetUserPermissions(User user)
		{
			var permissions = cache.Get($"permissions_{user.id}");
			if (permissions == null)
				permissions = uow.UserRepository.GetPermissions(user);
			return permissions as List<Permission>;
		}
		
	}
}
