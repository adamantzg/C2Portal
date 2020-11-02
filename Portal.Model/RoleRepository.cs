using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefactorThis.GraphDiff;

namespace Portal.Model
{
	public class RoleRepository : GenericRepository<Role>
	{
		public RoleRepository(DbContext context) : base(context)
		{

		}

		public override void Insert(Role entity)
		{
			if(entity.Permissions != null)
			{
				for (var i=0;i< entity.Permissions.Count;i++)
				{
					var permission = entity.Permissions[i];
					var p = context.Set<Permission>().Local.FirstOrDefault(r => r.id == permission.id);
					if (p == null)
						context.Set<Permission>().Attach(permission);
					else
					{
						entity.Permissions[i] = p;
					}
				}
			}
			base.Insert(entity);
		}

		public override void Update(Role entityToUpdate)
		{
			context.UpdateGraph(entityToUpdate, m => m.AssociatedCollection(r => r.Permissions));
		}
	}
}
