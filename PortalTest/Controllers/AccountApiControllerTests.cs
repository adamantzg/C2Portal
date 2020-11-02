using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Controllers;
using Portal.Model;
using System.ServiceModel.Channels;

namespace PortalTest.Controllers
{
	/// <summary>
	/// Summary description for AccountApiControllerTests
	/// </summary>
	[TestClass]
	public class AccountApiControllerTests
	{
		public AccountApiControllerTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private AccountApiController controller;
		private UnitOfWorkMock unitOfWork;
		private CacheMock cache;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>


		#region Additional test attributes

		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		[TestInitialize()]
		public void Init()
		{
			unitOfWork = new UnitOfWorkMock();
			cache = new CacheMock();
			controller = new AccountApiController(unitOfWork, null, null, null, cache);
		}
		
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod, TestCategory("Account Api")]
		public void InvalidLogin()
		{
			unitOfWork.Data = new MockData
			{
				Users = new List<User>
				{
					new User {id = 1, username = "username", password = "password", customer_code = "cust"}
				}
			};
			var result = controller.Login("xxx", "xxx");
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);
			
		}

		[TestMethod, TestCategory("Account Api")]
		public void LoginNoCustomerInUserObject()
		{
			//For token, email and roles are required
			var mockData = new MockData
			{
				Users = new List<User>
				{
					new User {id = 1, username = "username", password = "password", customer_code = "cust", email = "email", 
						Roles = new List<Role>
						{
							new Role {id = Role.User}
						}
					}
				},
				Customers = new List<Customer>
				{
					new Customer {code = "cust"}
				}
			};
			unitOfWork.Data = mockData;
			var result = controller.Login("username", "password");
			Assert.IsNotNull(result);
			var user = mockData.Users.FirstOrDefault();
			Assert.IsNotNull(user);
			Assert.IsNotNull(user.Customer);
			Assert.IsNotNull(user.token);
			Assert.IsNotNull(user.lastLogin);
			Assert.IsTrue(unitOfWork.Saved);
			Assert.IsNotNull(user.Sessions);
			Assert.AreEqual(1, user.Sessions.Count);
		}

		[TestMethod, TestCategory("Account Api")]
		public void LoginIpAddress()
		{
			//For token, email and roles are required
			var mockData = new MockData
			{
				Users = new List<User>
				{
					new User {id = 1, username = "username", password = "password", customer_code = "cust", email = "email", 
						Roles = new List<Role>
						{
							new Role {id = Role.User}
						},
						Customer = new Customer {code = "cust"}
					}
				}
			};
			unitOfWork.Data = mockData;
			controller.Request = new HttpRequestMessage();

			var context = new HttpContext(
				new HttpRequest("", "http://tempuri.org", ""),
				new HttpResponse(new StringWriter())
			);
			
			Mock<HttpContextWrapper> moqWrapper = new Mock<HttpContextWrapper>(context);
			Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
			moqRequest.SetupGet(x => x.UserHostAddress).Returns("100.100");
			moqWrapper.Setup(x => x.Request).Returns(moqRequest.Object);
			
			//MS_HttpContext prop
			controller.Request.Properties["MS_HttpContext"] = moqWrapper.Object;
			var result = controller.Login("username", "password");
			Assert.IsNotNull(result);
			var user = mockData.Users.FirstOrDefault();
			Assert.IsNotNull(user);
			Assert.IsNotNull(user.Sessions);
			Assert.AreEqual(1, user.Sessions.Count);
			Assert.AreEqual("100.100", user.Sessions[0].ip_addr);

			//Remoteendpoint ip
			user.Sessions.Clear();
			controller.Request.Properties.Remove("MS_HttpContext");
			RemoteEndpointMessageProperty prop = new RemoteEndpointMessageProperty("200.200", 80);
			controller.Request.Properties[RemoteEndpointMessageProperty.Name] = prop;
			result = controller.Login("username", "password");
			Assert.IsNotNull(result);
			Assert.IsNotNull(user);
			Assert.IsNotNull(user.Sessions);
			Assert.AreEqual(1, user.Sessions.Count);
			Assert.AreEqual("200.200", user.Sessions[0].ip_addr);

			//Httpcontext current
			/*user.Sessions.Clear();
			controller.Request.Properties.Remove(RemoteEndpointMessageProperty.Name);
			context.Request.ServerVariables.Add("REMOTE_ADDR", "300.300");
			HttpContext.Current = context;
			result = controller.Login("username", "password");
			Assert.IsNotNull(result);
			Assert.IsNotNull(user);
			Assert.IsNotNull(user.Sessions);
			Assert.AreEqual(1, user.Sessions.Count);
			Assert.AreEqual("300.300", user.Sessions[0].ip_addr);*/

		}

		[TestMethod, TestCategory("Account Api")]
		public void LoginSavePermissions()
		{
			var mockData = new MockData
			{
				Users = new List<User>
				{
					new User {id = 1, username = "username", password = "password", customer_code = "cust", email = "email", 
						Roles = new List<Role>
						{
							new Role {id = Role.User}
						},
						Customer = new Customer {code = "cust"},
						Permissions = new List<Permission>
						{
							new Permission
							{
								id = (int) PermissionId.ViewAccountDetails
							}
						}
					}
				}
			};
			unitOfWork.Data = mockData;
			var result = controller.Login("username", "password");
			var permissions = cache.Get("permissions_1") as List<Permission>;
			Assert.IsNotNull(permissions);
			Assert.AreEqual(1, permissions.Count );
			Assert.IsNotNull(result);
			var objPermissions = result.GetType().GetProperty("permissions")?.GetValue(result);
			Assert.IsNotNull(objPermissions);
			Utils.TestCollection(objPermissions, 1);
			;
		}

		[TestMethod, TestCategory("Account Api")]
		public void GetUserPermissions()
		{
			var mockData = new MockData
			{
				Users = new List<User>
				{
					new User {id = 1, username = "username", password = "password", customer_code = "cust", email = "email", 
						Roles = new List<Role>
						{
							new Role {id = Role.User}
						},
						Customer = new Customer {code = "cust"},
						Permissions = new List<Permission>
						{
							new Permission
							{
								id = (int) PermissionId.ViewAccountDetails
							}
						}
					}
				}
			};
			unitOfWork.Data = mockData;

			var permissions = controller.GetUserPermissions(mockData.Users[0]);
			Assert.AreEqual(1, permissions.Count);

			cache.Set("permissions_1", new List<Permission>(), null);
			permissions = controller.GetUserPermissions(mockData.Users[0]);
			Assert.AreEqual(0, permissions.Count);

		}
	}
}

