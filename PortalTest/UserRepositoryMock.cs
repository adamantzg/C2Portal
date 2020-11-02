using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Model;

namespace PortalTest
{
	public class UserRepositoryMock : GenericRepositoryMock<User>, IUserRepository
	{
		public UserRepositoryMock(IList<User> data) : base(data)
		{
		}

		public void InsertSession(User user, string ip_addr)
		{
			if(user.Sessions == null)
				user.Sessions = new List<UserSession>();
			user.Sessions.Add(new UserSession
			{
				dateCreated = DateTime.Today,
				ip_addr = ip_addr,
				token = user.token,
				user_id = user.id
			});
		}

		public List<Permission> GetPermissions(User user)
		{
			return user?.Permissions;
		}

		public List<Permission> GetPermissions(int id)
		{
			var user = Get(u => u.id == id).FirstOrDefault();
			return GetPermissions(user);
		}
	}
}
