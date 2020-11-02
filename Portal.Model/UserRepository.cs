using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;

namespace Portal.Model
{
	public interface IUserRepository: IGenericRepository<User>
	{
		void InsertSession(User user, string ip_addr);
		List<Permission> GetPermissions(User user);
		List<Permission> GetPermissions(int id);
	}

	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		public UserRepository(DbContext context) : base(context)
		{
		}

		public override void Insert(User entity)
		{
			if(entity.Roles != null)
			{
				for (var i=0;i< entity.Roles.Count;i++)
				{
					var role = entity.Roles[i];
					var ro = context.Set<Role>().Local.FirstOrDefault(r => r.id == role.id);
					if (ro == null)
						context.Set<Role>().Attach(role);
					else
					{
						entity.Roles[i] = ro;
					}
				}
			}
			base.Insert(entity);
		}

		public override void Update(User entityToUpdate)
		{
			context.UpdateGraph(entityToUpdate, m => m.AssociatedCollection(u => u.Roles));
		}

		public void InsertSession(User user, string ip_addr)
		{
			context.Database.ExecuteSqlCommand("INSERT INTO user_session (user_id,dateCreated,ip_addr,token) VALUES(@p0,@p1,@p2,@p3)", user.id, DateTime.UtcNow, ip_addr, user.token);
		}

		public List<Permission> GetPermissions(User user)
		{
			var result = new List<Permission>();
			HashSet<int> hPermissions = new HashSet<int>();
			List<Permission> allPermissions = context.Set<Permission>().ToList();
			if (user.Roles != null)
			{
				//roles
				foreach (var r in user.Roles)
				{
					if(r.Permissions == null)
						context.Entry(r).Collection("Permissions").Load();
					foreach (var p in r.Permissions)
					{
						hPermissions.Add(p.id);
					}
				}
				//check user exceptions
				var userpPermissions = context.Set<UserPermission>().Where(u => u.user_id == user.id).ToList();
				foreach (var up in userpPermissions)
				{
					if(up.grant == true)
						hPermissions.Add(up.permission_id);
					if (up.deny == true)
						hPermissions.Remove(up.permission_id);
				}
				//check customer exceptions
				var custPermissions = context.Set<CustomerPermission>().Where(c => c.customer_code == user.customer_code).ToList();
				foreach (var cp in custPermissions)
				{
					if(cp.grant == true)
						hPermissions.Add(cp.permission_id);
					if (cp.deny == true)
						hPermissions.Remove(cp.permission_id);
				}


				result = allPermissions.Where(p => hPermissions.Contains(p.id)).ToList();

			}

			return result;
		}

		public List<Permission> GetPermissions(int id)
		{
			var user = dbSet.Find(id);
			LoadCollection(user, "Roles");
			return GetPermissions(user);
		}
	}
}