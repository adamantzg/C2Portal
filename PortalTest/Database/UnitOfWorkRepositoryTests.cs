using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Model;

namespace PortalTest.Database
{
	[TestClass]
	public class UnitOfWorkRepositoryTests : DatabaseTestBase
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
			}
		}

		[TestMethod, TestCategory("UnitOfWorkTests")]
		public void Get()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "a", "b");
				var id2 = InsertTestUser(conn, "c", "d");

				var user = uow.UserRepository.GetByID(id1);
				Assert.IsNotNull(user);
				Assert.AreEqual(id1, user.id);
			}
		}

		[TestMethod, TestCategory("UnitOfWorkTests")]
		public void UnitOfWork()
		{
			var unitOfWork = new UnitOfWork();
			Assert.IsNotNull(unitOfWork);
			

			Assert.IsNotNull(unitOfWork.UserRepository);
			Assert.IsNotNull(unitOfWork.HolidayRepository);
			Assert.IsNotNull(unitOfWork.ProductRepository);
			Assert.IsNotNull(unitOfWork.CustomerRepository);
			Assert.IsNotNull(unitOfWork.RoleRepository);
		}

		[TestMethod, TestCategory("UnitOfWorkTests")]
		public void LoadRefCollection()
		{
			using (var conn = GetAndOpenConnection())
			{
				var code = "cust";
				InsertTestCustomer(conn, code);
				var id1 = InsertTestUser(conn, "a", "b", customer_code: code, roles: new List<Role>
				{
					new Role {id=Role.User}
				});

				var user = uow.UserRepository.GetByID(id1);
				Assert.IsNotNull(user);
				Assert.IsNull(user.Customer);
				Assert.IsNull(user.Roles);
				uow.UserRepository.LoadReference(user, "Customer");
				Assert.IsNotNull(user.Customer);

				uow.UserRepository.LoadCollection(user, "Roles");
				Assert.IsNotNull(user.Roles);
				Assert.AreEqual(1, user.Roles.Count);

			}
		}

		[TestMethod, TestCategory("UnitOfWorkTests")]
		public void DeleteAll()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "a", "b");
				var id2 = InsertTestUser(conn, "c", "d");

				uow.UserRepository.DeleteAll();
				var userCount = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM user");
				Assert.AreEqual(0, userCount);
			}
		}

		[TestMethod, TestCategory("UnitOfWorkTests")]
		public void BulkInsert()
		{
			using (var conn = GetAndOpenConnection())
			{
				var users = new List<User>
				{
					new User {name = "a", lastname = "b"},
					new User {name = "c", lastname = "d"}
				};
				uow.UserRepository.BulkInsert(users);
				uow.Save();
				var userCount = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM user");
				Assert.AreEqual(2, userCount);
			}
		}

		[TestMethod, TestCategory("UnitOfWorkTests")]
		public void Detach()
		{
			using (var conn = GetAndOpenConnection())
			{
				var id1 = InsertTestUser(conn, "a", "b");
				
				var user = uow.UserRepository.GetByID(id1);
				Assert.IsNotNull(user);
				uow.UserRepository.Detach(user);
				user.name = "aa";
				uow.Save();
				var dbUser = conn.Query<User>("SELECT * FROM user WHERE id = @id", new {id = id1}).FirstOrDefault();
				
				Assert.IsNotNull(dbUser);
				Assert.AreNotEqual("aa", dbUser.name);

			}
		}
	}
}
