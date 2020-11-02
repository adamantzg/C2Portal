using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Portal.Model;

namespace PortalTest.Database
{
	public class DatabaseTestBase
	{
		protected UnitOfWork uow;

		protected MySqlConnection GetAndOpenConnection()
		{
			var c = new MySqlConnection(Properties.Settings.Default.connString);
			c.Open();
			return c;
		}

		protected int InsertTestUser(MySqlConnection conn, string name = "Name", string lastname = "Surname", string username = "user", string password = "password", 
			string customer_code = null, List<Role> roles = null, bool insertSession = false, List<UserPermission> userPermissions = null)
		{
			var cmd = new MySqlCommand("INSERT INTO user (name,lastname, username, password, customer_code) VALUES(@name, @lastname, @username, @password, @customer_code)", conn);
			cmd.Parameters.AddWithValue("@name", name);
			cmd.Parameters.AddWithValue("@lastname", lastname);
			cmd.Parameters.AddWithValue("@username", username);
			cmd.Parameters.AddWithValue("@password", password);
			cmd.Parameters.AddWithValue("@customer_code", (object) customer_code ?? DBNull.Value);
			cmd.ExecuteNonQuery();
			var id = Convert.ToInt32(conn.ExecuteScalar("SELECT last_insert_id()"));

			if (roles != null)
			{
				cmd.CommandText = "INSERT INTO user_role(user_id, role_id) VALUES(@user_id, @role_id)";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@user_id", 0);
				cmd.Parameters.AddWithValue("@role_id", 0);
				foreach (var r in roles)
				{
					cmd.Parameters[0].Value = id;
					cmd.Parameters[1].Value = r.id;
					cmd.ExecuteNonQuery();
				}
			}

			if (insertSession)
			{
				cmd.CommandText = "INSERT INTO user_session (user_id,dateCreated) VALUES(@p0,@p1)";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@p0", id);
				cmd.Parameters.AddWithValue("@p1", DateTime.Today);
				cmd.ExecuteNonQuery();
			}

			if (userPermissions != null)
			{
				cmd.CommandText = "INSERT INTO user_permission(user_id, permission_id, `grant`, deny) VALUES(@user_id, @permission_id, @grant, @deny)";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@user_id", 0);
				cmd.Parameters.AddWithValue("@permission_id", 0);
				cmd.Parameters.AddWithValue("@grant", false);
				cmd.Parameters.AddWithValue("@deny", false);
				foreach (var up in userPermissions)
				{
					cmd.Parameters[0].Value = id;
					cmd.Parameters[1].Value = up.permission_id;
					cmd.Parameters[2].Value = up.grant;
					cmd.Parameters[3].Value = up.deny;
					cmd.ExecuteNonQuery();
				}
			}

			return id;
		}

		protected void InsertTestCustomer(MySqlConnection conn, string code = "C0000", string name = "customer", List<CustomerPermission> permissions = null)
		{
			var cmd = new MySqlCommand("INSERT INTO customer (code, name) VALUES(@code, @name)", conn);
			cmd.Parameters.AddWithValue("@code", code);
			cmd.Parameters.AddWithValue("@name", name);
			cmd.ExecuteNonQuery();

			if (permissions != null)
			{
				cmd.CommandText = @"INSERT INTO customer_permission(customer_code, permission_id, `grant`, deny) 
									VALUES(@customer_code, @permission_id, @grant, @deny)";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@customer_code", "");
				cmd.Parameters.AddWithValue("@permission_id", 0);
				cmd.Parameters.AddWithValue("@grant", false);
				cmd.Parameters.AddWithValue("@deny", false);
				foreach (var cp in permissions)
				{
					cmd.Parameters[0].Value = code;
					cmd.Parameters[1].Value = cp.permission_id;
					cmd.Parameters[2].Value = cp.grant;
					cmd.Parameters[3].Value = cp.deny;
					cmd.ExecuteNonQuery();
				}
			}
		}

		protected int InsertTestRole(MySqlConnection conn, string name = "Name", List<Permission> permissions = null)
		{
			var cmd = new MySqlCommand("INSERT INTO role (name) VALUES(@name)", conn);
			cmd.Parameters.AddWithValue("@name", name);
			cmd.ExecuteNonQuery();
			var id = Convert.ToInt32(conn.ExecuteScalar("SELECT last_insert_id()"));

			if (permissions != null)
			{
				cmd.CommandText = "INSERT INTO role_permission(role_id, permission_id) VALUES(@role_id, @permission_id)";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@role_id", 0);
				cmd.Parameters.AddWithValue("@permission_id", 0);
				foreach (var p in permissions)
				{
					cmd.Parameters[0].Value = id;
					cmd.Parameters[1].Value = p.id;
					cmd.ExecuteNonQuery();
				}
			}

			return id;
		}
	}
}
