using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Portal.Model
{
	public class Product
	{
		public int id { get; set; }
		public string code { get; set; }
		public string name { get; set; }
		public string description { get; set; }		
	}

	public class Customer
	{
		[Key]
		public string code { get; set; }
		public string name { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string address3 { get; set; }
		public string address4 { get; set; }
		public string address5 { get; set; }
		public string address6 { get; set; }
		public string town_city { get; set; }
		public string county { get; set; }
		public string currency { get; set; }
		public string analysis_codes_1 { get; set; }
		public string invoice_customer { get; set; }
	}

	public class User
	{
		public int id { get; set; }
		public string name { get; set; }
		public string lastname { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string email { get; set; }
		public string customer_code { get; set; }
		public string address { get; set; }
		public string phone { get; set; }
		public bool? isInternal { get; set; }
		public DateTime? lastLogin { get; set; }

		[NotMapped]
		public string token { get; set; }

		public virtual Customer Customer { get; set; }
		public virtual List<Role> Roles { get; set; }
		public virtual List<UserSession> Sessions { get; set; }

		[NotMapped]
		public List<Permission> Permissions { get; set; }

		public bool isAdmin => Roles?.Count(r => r.id == Role.Admin) > 0;
		public bool isBranchAdmin => Roles?.Count(r => r.id == Role.BranchAdmin) > 0;

		public bool HasPermission(PermissionId permId)
		{
			return Permissions?.Count(p => p.id == (int) permId) > 0;
		}
	}

	public class UserSession
	{
		public int id { get; set; }
		public int? user_id { get; set; }
		public DateTime? dateCreated { get; set; }
		public string token { get; set; }
		public string ip_addr { get; set; }
	}

	public class Role
	{
		public const int User = 1;
		public const int BranchAdmin = 2;
		public const int Admin = 3;

		public int id { get; set; }
		public string name { get; set; }

		public List<Permission> Permissions { get; set; }
	}
    public class Holiday
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
    }

	[Table("permission")]
	public class Permission
	{
		public int id { get; set; }
		public string name { get; set; }
	}

	[Table("user_permission")]
	public class UserPermission
	{
		public int user_id { get; set; }
		public int permission_id { get; set; }
		public bool? grant { get; set; }
		public bool? deny { get; set; }

	}

	[Table("customer_permission")]
	public class CustomerPermission
	{
		public string customer_code { get; set; }
		public int permission_id { get; set; }
		public bool? grant { get; set; }
		public bool? deny { get; set; }

	}

	public enum PermissionId
	{
		ViewAccountDetails = 1,
		ViewHolidayAdministration = 2,
		ViewInvoiceHistory = 3,
		ViewOrderHistory = 4,
		ViewStockSearch = 5,
		ViewUserAdministration = 6,
		ViewRoleAdministration = 7
	}
}