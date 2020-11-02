using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Portal.Model;
using PortalTest.Database;

namespace PortalTest
{
	[TestClass]
	public class UserRolesTests : DatabaseTestBase
	{
		
		[TestInitialize]
		public void SetupTests()
		{
			uow = new UnitOfWork(new Model("name=PortalTest.Properties.Settings.connString"));
			CleanUp();
		}

		[TestCleanup]
		public void CleanUp()
		{
			using (var conn = GetAndOpenConnection())
			{
				conn.Execute("DELETE FROM user");
				conn.Execute("DELETE FROM customer");
				conn.Execute("DELETE FROM role WHERE id > 3");
			}
		}

		#region user
		[TestMethod, TestCategory("Portal Database")]
		public void InsertUser()
		{
			var user = new User {name = "Name", lastname = "Surname", username = "username", Roles = new List<Role>{new Role {id = Role.User}, new Role{id=Role.BranchAdmin}}};
			uow.UserRepository.Insert(user);
			uow.Save();
			var id = user.id;

			using (var conn = GetAndOpenConnection())
			{
				var dbUser = conn.QueryFirstOrDefault<User>("SELECT * FROM user WHERE id = @id", new {id});
				Assert.IsNotNull(dbUser);
				Assert.AreEqual(id, dbUser.id);
				var roles = conn.Query("SELECT * FROM user_role WHERE user_id = @id", new {id});
				Assert.IsNotNull(roles);
				Assert.AreEqual(2, roles.Count());
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void InsertUserWithRolesInContext()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, username: "test", password: "pass", roles: new List<Role>{new Role {id=Role.User}, new Role {id = Role.BranchAdmin}});
				var user = uow.UserRepository.Get(u => u.id == id, includeProperties: "Roles");

				var user2 = new User {name = "Name", lastname = "Surname", username = "username", Roles = new List<Role>{new Role {id = Role.User}, new Role{id=Role.BranchAdmin}}};
				uow.UserRepository.Insert(user2);
				uow.Save();
				id = user2.id;

				var dbUser = conn.QueryFirstOrDefault<User>("SELECT * FROM user WHERE id = @id", new {id});
				Assert.IsNotNull(dbUser);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void UpdateUser()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn);
				var user = new User {id = id, name = "Name", lastname = "Lastname", address = "adr"};
				uow.UserRepository.Update(user);
				uow.Save();

				var dbUser = conn.QueryFirstOrDefault<User>("SELECT * FROM user WHERE id = @id", new {id});
				Assert.IsNotNull(dbUser);
				Assert.AreEqual("adr", dbUser.address);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void InsertSession()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn);
				var user = new User {id = id, token = "token"};
				uow.UserRepository.InsertSession(user, "ip");

				var dbSession = conn.QueryFirstOrDefault<UserSession>("SELECT * FROM user_session WHERE user_id = @id", new {id});
				Assert.IsNotNull(dbSession);
				Assert.AreEqual("ip", dbSession.ip_addr);
				Assert.AreEqual("token", dbSession.token);
				Assert.IsNotNull(dbSession.dateCreated);
				Assert.IsTrue((DateTime.UtcNow -  dbSession.dateCreated.Value).Seconds < 2);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserById()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, name: "test", roles: new List<Role>{new Role {id=Role.User}, new Role {id = Role.BranchAdmin}});
				var user = uow.UserRepository.GetByID(id);
				Assert.IsNotNull(user);
				Assert.AreEqual("test", user.name);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserWithRoles()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, name: "test", roles: new List<Role>{new Role {id=Role.User}, new Role {id = Role.BranchAdmin}});
				var user = uow.UserRepository.Get(u=>u.id == id, includeProperties:"Roles").FirstOrDefault();
				Assert.IsNotNull(user);
				Assert.IsTrue(user.isBranchAdmin);
				Assert.IsFalse(user.isAdmin);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserWithCustomer()
		{
			using (var conn = GetAndOpenConnection())
			{
				//TODO
				var customer_code = "Cust";
				InsertTestCustomer(conn,customer_code);
				var id = InsertTestUser(conn, customer_code: customer_code);
				var user = uow.UserRepository.Get(u => u.id == id, includeProperties: "Customer").FirstOrDefault();
				Assert.IsNotNull(user);
				Assert.IsNotNull(user.Customer);

			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserWithSessions()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, insertSession: true);
				var user = uow.UserRepository.Get(u => u.id == id, includeProperties: "Sessions").FirstOrDefault();
				Assert.IsNotNull(user);
				Assert.IsNotNull(user.Sessions);
				Assert.AreEqual(1, user.Sessions.Count);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserByUsernamePassword()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, username: "test", password: "pass");
				var user = uow.UserRepository.Get(u => u.username == "test" && u.password == "pass").FirstOrDefault();
				Assert.IsNotNull(user);
				Assert.AreEqual("test", user.username);
			}
		}

		#region GenericMethods

		[TestMethod, TestCategory("Portal Database")]
		public void DeleteByIds()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "First");
				var id2 = InsertTestUser(conn, "Second");
				uow.UserRepository.DeleteByIds(new List<int>{id1, id2});
				var count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM user");
				Assert.AreEqual(0, count);
			}
		}

		//Test generic methods
		[TestMethod, TestCategory("Portal Database")]
		public void Delete()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "First");
				uow.UserRepository.Delete(id1);
				uow.Save();
				var count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM user");
				Assert.AreEqual(0, count);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void DeleteDetached()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "First");
				uow.UserRepository.Delete(new User {id = id1});
				uow.Save();
				var count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM user");
				Assert.AreEqual(0, count);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void Update()
		{
			using (var conn = GetAndOpenConnection())
			{
				InsertTestCustomer(conn, "Code","Customer");
				uow.CustomerRepository.Update(new Customer {code = "Code", name= "Customer 1"});
				uow.Save();
				var count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM customer WHERE name = 'Customer 1'");
				Assert.AreEqual(1, count);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void LoadCollection()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "First", insertSession: true);
				var user = uow.UserRepository.GetByID(id1);
				Assert.IsNotNull(user);
				Assert.IsNull(user.Sessions);
				uow.UserRepository.LoadCollection(user, "Sessions");
				Assert.IsNotNull(user.Sessions);
				Assert.AreEqual(1, user.Sessions.Count);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetOrderByTakeSkip()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "Ben");
				var id2 = InsertTestUser(conn, "Andrew");
				var id3 = InsertTestUser(conn, "Charles");

				var users = uow.UserRepository.Get(orderBy: u => u.OrderBy(x => x.name)).ToList();
				Assert.IsNotNull(users);
				Assert.AreEqual(3, users.Count);
				Assert.AreEqual("Andrew", users[0].name);

				users = uow.UserRepository.Get(orderBy: u => u.OrderBy(x => x.name), skip: 2).ToList();
				Assert.IsNotNull(users);
				Assert.AreEqual(1, users.Count);
				Assert.AreEqual("Charles", users[0].name);

				users = uow.UserRepository.Get(orderBy: u => u.OrderByDescending(x => x.name), take: 2).ToList();
				Assert.IsNotNull(users);
				Assert.AreEqual(2, users.Count);
				Assert.AreEqual("Charles", users[0].name);

				users = uow.UserRepository.Get(orderBy: u => u.OrderByDescending(x => x.name), take: 2, skip: 1).ToList();
				Assert.IsNotNull(users);
				Assert.AreEqual(2, users.Count);
				Assert.AreEqual("Ben", users[0].name);
			}

		}

		[TestMethod, TestCategory("Portal Database")]
		public void Copy()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, "n", "l", password: "p");
				var user2 = new User {id = id, name = "name", lastname = "surname", password = "pass"};
				var user1 = uow.UserRepository.GetByID(id);

				uow.UserRepository.Copy(user2, user1);
				Assert.AreEqual(user1.name, user2.name);
				Assert.AreEqual(user1.lastname, user2.lastname);
				Assert.AreEqual(user1.password, user2.password);
			}
			
		}



		#endregion


		#endregion

		#region roles

		[TestMethod, TestCategory("Portal Database")]
		public void RolesWithPermission()
		{
			var roles = uow.RoleRepository.Get(includeProperties: "Permissions").ToList();
			Assert.IsNotNull(roles);
			Assert.IsNotNull(roles.FirstOrDefault(r=>r.id == Role.Admin)?.Permissions);
			Assert.IsTrue(roles.FirstOrDefault(r=>r.id == Role.Admin)?.Permissions?.Count > 0);
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserPermissionRolesOnly()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, "test", roles: new List<Role> {new Role {id = Role.Admin}});
				var dbPermissionsAdmin =
					conn.Query<Permission>("SELECT permission_id as id FROM role_permission WHERE role_id = @role_id",
						new {role_id = Role.Admin}).ToList();
				/*var dbPermissionsBranch = conn.Query<Permission>("SELECT permission_id as id FROM role_permission WHERE role_id = @role_id",
					new {role_id = Role.BranchAdmin}).ToList();*/
				//admin
				var userPermissions = uow.UserRepository.GetPermissions(id);
				Assert.AreEqual(dbPermissionsAdmin.Count, userPermissions.Count);

				//two roles, should return higher role count
				id = InsertTestUser(conn, "branch",
					roles: new List<Role> {new Role {id = Role.Admin}, new Role {id = Role.BranchAdmin}});
				userPermissions = uow.UserRepository.GetPermissions(id);
				Assert.AreEqual(dbPermissionsAdmin.Count, userPermissions.Count);

				//test GetPermissions that gets user  object
				var user = uow.UserRepository.Get(u => u.id == id, includeProperties: "Roles.Permissions").FirstOrDefault();
				Assert.IsNotNull(user);
				userPermissions = uow.UserRepository.GetPermissions(user);
				Assert.AreEqual(dbPermissionsAdmin.Count, userPermissions.Count);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserPermissionsUserException()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestUser(conn, "test", roles: new List<Role> {new Role {id = Role.User}}, 
					userPermissions: new List<UserPermission>{new UserPermission {permission_id = (int) PermissionId.ViewInvoiceHistory, deny = true}});
				var dbPermissionsUser =
					conn.Query<Permission>("SELECT permission_id as id FROM role_permission WHERE role_id = @role_id",
						new {role_id = Role.User}).ToList();
				var userPermissions = uow.UserRepository.GetPermissions(id);
				Assert.AreNotEqual(dbPermissionsUser.Count, userPermissions.Count);
				Assert.IsTrue(dbPermissionsUser.Count > userPermissions.Count);

				//grant
				id = InsertTestUser(conn, "test", roles: new List<Role> {new Role {id = Role.User}}, 
					userPermissions: new List<UserPermission>{new UserPermission {permission_id = (int) PermissionId.ViewUserAdministration, grant = true}});
				
				userPermissions = uow.UserRepository.GetPermissions(id);
				Assert.AreNotEqual(dbPermissionsUser.Count, userPermissions.Count);
				Assert.IsTrue(dbPermissionsUser.Count < userPermissions.Count);
			}

			
		}

		[TestMethod, TestCategory("Portal Database")]
		public void GetUserPermissionsCustomerException()
		{
			using (var conn = GetAndOpenConnection())
			{
				var cust_code = "cust";
				InsertTestCustomer(conn, cust_code, 
					permissions: new List<CustomerPermission>
					{
						new CustomerPermission {permission_id = (int) PermissionId.ViewInvoiceHistory, deny = true}
					} );

				var id = InsertTestUser(conn, "test", roles: new List<Role> {new Role {id = Role.User}}, customer_code: cust_code);
				
				var dbPermissionsUser =
					conn.Query<Permission>("SELECT permission_id as id FROM role_permission WHERE role_id = @role_id",
						new {role_id = Role.User}).ToList();
				var userPermissions = uow.UserRepository.GetPermissions(id);
				Assert.AreNotEqual(dbPermissionsUser.Count, userPermissions.Count);
				Assert.IsTrue(dbPermissionsUser.Count > userPermissions.Count);

				//grant
				conn.Execute("UPDATE customer_permission SET `grant` = 1, deny = 0, permission_id = @permission_id", 
					new {permission_id = PermissionId.ViewUserAdministration});
				
				userPermissions = uow.UserRepository.GetPermissions(id);
				Assert.AreNotEqual(dbPermissionsUser.Count, userPermissions.Count);
				Assert.IsTrue(dbPermissionsUser.Count < userPermissions.Count);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void InsertRole()
		{
			var role = new Role { name = "Test role", Permissions = new List<Permission>
			{
				new Permission
				{
					id = (int) PermissionId.ViewAccountDetails
				}, new Permission
				{
					id= (int) PermissionId.ViewInvoiceHistory
				}
			}};
			uow.RoleRepository.Insert(role);
			uow.Save();
			var id = role.id;

			using (var conn = GetAndOpenConnection())
			{
				var dbRole = conn.QueryFirstOrDefault<User>("SELECT * FROM role WHERE id = @id", new {id});
				Assert.IsNotNull(dbRole);
				Assert.AreEqual(id, dbRole.id);
				var permissions = conn.Query<Permission>("SELECT permission_id as id FROM role_permission WHERE role_id = @id", new {id}).ToList();
				Assert.IsNotNull(permissions);
				Assert.AreEqual(2, permissions.Count);
				Assert.AreEqual((int) PermissionId.ViewAccountDetails, permissions[0].id);
			}
		}

		[TestMethod, TestCategory("Portal Database")]
		public void UpdateRole()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id = InsertTestRole(conn, "test role", new List<Permission>
				{
					new Permission {id = (int) PermissionId.ViewAccountDetails},
					new Permission {id = (int) PermissionId.ViewInvoiceHistory}
				});

				var role = new Role
				{
					id = id,
					Permissions = new List<Permission>
					{
						new Permission {id = (int) PermissionId.ViewAccountDetails},
						new Permission {id = (int) PermissionId.ViewOrderHistory}
					}
				};
				uow.RoleRepository.Update(role);
				uow.Save();
				var dbPermissions =
					conn.Query<Permission>("SELECT permission_id as id FROM role_permission WHERE role_id = @id",
						new {id}).ToList();
				Assert.IsNotNull(dbPermissions);
				Assert.AreEqual(2, dbPermissions.Count);
				Assert.AreEqual((int) PermissionId.ViewOrderHistory, dbPermissions[1].id);
				Assert.IsNull(dbPermissions.FirstOrDefault(p=>p.id == (int) PermissionId.ViewInvoiceHistory));
			}
		}

#endregion
	}
}
