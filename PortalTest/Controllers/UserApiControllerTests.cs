using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Controllers;
using Portal.Model;


namespace PortalTest.Controllers
{
	[TestClass]
	public class UserApiControllerTests
	{
		private UserApiController controller;
		private UnitOfWorkMock unitOfWork;
		private MailHelperMock mailHelper;
		private JwtDecoderMock jwtDecoder;
		private EncrypterMock encrypter;
		private CacheMock cache;
		
		[TestInitialize()]
		public void Init()
		{
			unitOfWork = new UnitOfWorkMock();
			mailHelper = new MailHelperMock();
			jwtDecoder = new JwtDecoderMock();
			encrypter = new EncrypterMock();
			cache = new CacheMock();
			controller = new UserApiController(unitOfWork, mailHelper, new ApiClientMock(), jwtDecoder, encrypter, cache);
			controller.Request = new HttpRequestMessage();
		}

		[TestMethod, TestCategory("UserApiController")]
		public void GetUser()
		{
			var user = new User
			{
				id = 1,
				name = "user",
				address = "add",
				password = "pass",
				email = "email",
				customer_code = "cust",
				token = "tok",
				isInternal = false,
				lastname = "last",
				lastLogin = DateTime.Today,
				phone = "phone",
				username = "username",
				Customer = new Customer
				{
					code = "c",
					address1 = "add",
					address2= "add",
					address3= "add",
					address4= "add",
					address5= "add",
					address6= "add",
					currency= "curr",
					analysis_codes_1 = "a",
					county= "c",
					town_city = "t",
					name = "n"
				},
				Roles = new List<Role>
				{
					new Role {id = Role.User, name="User"}
				},
				Permissions = new List<Permission>
				{
					new Permission {id = (int) PermissionId.ViewAccountDetails}
				}
			};
			unitOfWork.Data = new MockData
			{
				Users = new List<User>
				{
					user
				}
			};
			var data = controller.GetUser(1);
			string[] properties =
			{
				"id", "name", "address", "password", "email", "customer_code", "token", "isInternal", "lastname",
				"lastLogin", "phone", "username"
			};
			string[] customerProperties =
			{
				"code",
				"address1",
				"address2",
				"address3",
				"address4",
				"address5",
				"address6",
				"currency",
				"analysis_codes_1",
				"county",
				"town_city",
				"name"
			};
			foreach (var prop in properties)
			{
				var origPropInfo = user.GetType().GetProperty(prop);
				var dataPropInfo = data.GetType().GetProperty(prop);
				if (origPropInfo != null && dataPropInfo != null)
				{
					Assert.AreEqual(origPropInfo.GetValue(user), dataPropInfo.GetValue(data));
				}
			}

			Assert.AreEqual(false,data.GetType().GetProperty("isBranchAdmin")?.GetValue(data));
			Assert.AreEqual(false,data.GetType().GetProperty("isTopAdmin")?.GetValue(data));
			Assert.IsNotNull(data.GetType().GetProperty("customer")?.GetValue(data));
			var cust = data.GetType().GetProperty("customer")?.GetValue(data);
			foreach (var prop in customerProperties)
			{
				var origPropInfo = user.Customer.GetType().GetProperty(prop);
				var dataPropInfo = cust.GetType().GetProperty(prop);
				if (origPropInfo != null && dataPropInfo != null)
				{
					Assert.AreEqual(origPropInfo.GetValue(user.Customer), dataPropInfo.GetValue(cust));
				}
			}

			var rolesProp = data.GetType().GetProperty("roles");
			Assert.IsNotNull(rolesProp);
			var roleInterface = rolesProp.PropertyType.GetInterfaces()
				.FirstOrDefault(i => i == typeof(IEnumerable));
			Assert.IsNotNull(roleInterface);
			Assert.IsNotNull(rolesProp.PropertyType.GenericTypeArguments);
			Assert.AreEqual(1,rolesProp.PropertyType.GenericTypeArguments.Length);
			var idProp = rolesProp.PropertyType.GenericTypeArguments[0].GetProperty("id");
			var nameProp = rolesProp.PropertyType.GenericTypeArguments[0].GetProperty("name");
			Assert.IsNotNull(idProp);
			Assert.IsNotNull(nameProp);

			var roleData = rolesProp.GetValue(data);
			Assert.IsNotNull(roleData);
			var collection = (roleData as IEnumerable).Cast<object>().ToList();
			Assert.IsNotNull(collection);
			Assert.AreEqual(1, collection.Count);
			
			Assert.AreEqual(user.Roles[0].id, idProp.GetValue(collection[0]));
			Assert.AreEqual(user.Roles[0].name, nameProp.GetValue(collection[0]));

			var permProp = data.GetType().GetProperty("permissions");
			Assert.IsNotNull(permProp);
			TestCollection(permProp.GetValue(data), 1);
		}

		[TestMethod, TestCategory("UserApiController")]
		public void GetUsersModel()
		{
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			var users = new List<User>
			{
				new User
				{
					id = 1,
					Customer = new Customer(),
					Roles = new List<Role>
					{
						new Role {id = Role.Admin}
					}
				},
				new User
				{
					id = 2,
					customer_code = "branch",
					Customer = new Customer
					{
						invoice_customer = "branch"
					},
					Roles = new List<Role>
					{
						new Role {id = Role.BranchAdmin}
					}
				},
				new User
				{
					id = 3,
					customer_code = "branch2",
					Customer = new Customer
					{
						invoice_customer = "branch"
					},
					Roles = new List<Role>
					{
						new Role {id = Role.User}
					}
				},
				new User
				{
					id = 4,
					customer_code = "branch2",
					Customer = new Customer(),
					Roles = new List<Role>
					{
						new Role {id = Role.User}
					}
				}
			};
			unitOfWork.Data = new MockData
			{
				Users = users,
				Roles = new List<Role>
				{
					new Role {id=Role.User},
					new Role {id = Role.BranchAdmin},
					new Role {id = Role.Admin},
					new Role { id = 4}
				}
			};
			var model = controller.GetUsersModel();
			var usersProp = model.GetType().GetProperty("users");
			var rolesProp = model.GetType().GetProperty("roles");
			Assert.IsNotNull(usersProp);
			Assert.IsNotNull(rolesProp);
			Assert.IsNotNull(usersProp.PropertyType.GenericTypeArguments);
			Assert.AreEqual(1, usersProp.PropertyType.GenericTypeArguments.Length);
			Assert.IsNotNull(rolesProp.PropertyType.GenericTypeArguments);
			Assert.AreEqual(1, rolesProp.PropertyType.GenericTypeArguments.Length);

			TestCollection(usersProp.GetValue(model), 4);
			TestCollection(rolesProp.GetValue(model), 4);

			//Branch admin
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			model = controller.GetUsersModel();
			usersProp = model.GetType().GetProperty("users");
			rolesProp = model.GetType().GetProperty("roles");
			Assert.IsNotNull(usersProp);
			Assert.IsNotNull(rolesProp);
			Assert.IsNotNull(usersProp.PropertyType.GenericTypeArguments);
			Assert.AreEqual(1, rolesProp.PropertyType.GenericTypeArguments.Length);

			TestCollection(usersProp.GetValue(model), 2);
			TestCollection(rolesProp.GetValue(model), 2);
		}

		[TestMethod, TestCategory("UserApiController")]
		public void CustomerSearch()
		{
			var mockData = new MockData
			{
				Users = new List<User>
				{
					new User
					{
						id = 1,
						customer_code = "c1",
						Roles = new List<Role>
						{
							new Role {id = Role.Admin}
						}
					},
					new User
					{
						id = 2,
						customer_code = "c2",
						Roles = new List<Role>
						{
							new Role {id = Role.User}
						}
					},
					new User
					{
						id = 3,
						customer_code = "c2",
						Roles = new List<Role>
						{
							new Role {id = Role.BranchAdmin}
						}
					}
				},
				Customers = new List<Customer>
				{
					new Customer
					{
						code = "c1",
						name = "",
						address6 = "adr 1"
					},
					new Customer
					{
						code = "c3",
						address6 = "adr 2",
						name = "xx",
						invoice_customer = "c2"
					},
					new Customer
					{
						code = "c2",
						name = "yy",
						address6 = "adr 3"
					},
					new Customer
					{
						code = "c4",
						invoice_customer = "c2",
						analysis_codes_1 = "CLOSED",
						address6 = "",
						name = "zz"
					},
					new Customer
					{
						code = "n1",
						name = "xxcyy",
						address6 = ""
					}
				}
			};
			unitOfWork.Data = mockData;
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			var customers = controller.CustomerSearch("c", null);
			TestCollection(customers, 5);

			customers = controller.CustomerSearch("2", Role.User);
			TestCollection(customers, 2);

			customers = controller.CustomerSearch("dr", Role.User);
			TestCollection(customers, 3);

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			customers = controller.CustomerSearch("c", Role.User);
			TestCollection(customers, 1);
		}

		[TestMethod, TestCategory("UserApiController")]
		public void GetCustomer()
		{
			var mockData = Utils.CreateAdminAndUser();
			mockData.Customers = new List<Customer>
			{
				new Customer {code = "c1", address6 = ""},
				new Customer {code = "c2", invoice_customer = "c3"},
				new Customer { code = "c3"}
			};
			mockData.Users.Add(new User
			{
				id = 3,
				customer_code = "c3",
				Roles = new List<Role>
				{
					new Role{ id = Role.BranchAdmin}
				}
			});
			mockData.Users.Add( new User
			{
				id = 4,
				Roles = new List<Role>
				{
					new Role{ id = Role.User}
				},
				isInternal =  true
			});
			unitOfWork.Data = mockData;

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");

			var customer = controller.GetCustomer("c1  ");
			Assert.IsNotNull(customer);
			customer = controller.GetCustomer("c4");
			Assert.IsNull(customer);

			//User can get only its customer
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var regularUser = mockData.Users[1];
			var branchAdmin = mockData.Users[2];
			customer = controller.GetCustomer(
				mockData.Users.FirstOrDefault(u => u.customer_code != regularUser.customer_code)?.customer_code);
			Assert.IsNull(customer);
			customer = controller.GetCustomer(regularUser.customer_code);
			Assert.IsNotNull(customer);

			//Check if admin can reach any customer
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			Assert.IsTrue(mockData.Customers.All(c=>controller.GetCustomer(c.code) != null));

			//Branch admin should get customer 2 and 3
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			Assert.IsNotNull(controller.GetCustomer(branchAdmin.customer_code));
			Assert.IsNotNull(controller.GetCustomer(regularUser.customer_code));
			Assert.IsNull(controller.GetCustomer(mockData.Users[0].customer_code));

			//Internal user can get any customer like admin
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "4");
			Assert.IsTrue(mockData.Customers.All(c=>controller.GetCustomer(c.code) != null));

		}

		[TestMethod, TestCategory("UserApiController")]
		public void CreateUser()
		{
			unitOfWork.Data = new MockData
			{
				Users = new List<User>
				{
					new User
					{
						id = 1,
						email = "user",
						customer_code = "c1",
						Roles = new List<Role>
						{
							new Role {id = Role.Admin}
						}
					},
					new User
					{
						id=2,
						username = "user",
						Roles = new List<Role>
						{
							new Role{id=Role.User}
						}
					},
					new User
					{
						id = 3,
						username = "usr",
						customer_code = "c0",
						Roles = new List<Role>
						{
							new Role {id = Role.BranchAdmin}
						}
					},
					new User
					{
						id = 4,
						username = "usr",
						customer_code = "c2",
						Roles = new List<Role>
						{
							new Role {id = Role.BranchAdmin}
						}
					}
				},
				Customers = new List<Customer>
				{
					new Customer {code = "c0"},
					new Customer {code = "c1", invoice_customer = "c0"},
					new Customer {code = "c2"}
				}
			};
			var user = new User
			{
				id = 0,
				name = "name"
			};
			//Try as regular user
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.Create(user);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.Unauthorized, message?.StatusCode);

			//admin, should get validation error
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.Create(user);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			message = result as HttpResponseMessage;
			Assert.IsNotNull(message);
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);
			var text = message.Content.ReadAsStringAsync().Result;
			Assert.IsTrue(text.Contains("Name"));

			//Duplicate email or username
			user.lastname = "last";
			user.email = "user";	//duplicate
			user.username = "username";
			user.customer_code = "c0";
			result = controller.Create(user);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			message = result as HttpResponseMessage;
			Assert.IsNotNull(message);
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);
			text = message.Content.ReadAsStringAsync().Result;
			Assert.IsTrue(text.Contains("already exists"));

			//Customer doesn't exist
			user.email = "email";
			user.customer_code = "xxx";
			result = controller.Create(user);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			message = result as HttpResponseMessage;
			Assert.IsNotNull(message);
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);
			text = message.Content.ReadAsStringAsync().Result;
			Assert.IsTrue(text.Contains("No customer"));

			//Customer not allowed
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			user.customer_code = "c2";
			result = controller.Create(user);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			message = result as HttpResponseMessage;
			Assert.IsNotNull(message);
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);
			text = message.Content.ReadAsStringAsync().Result;
			Assert.IsTrue(text.Contains("You are not allowed to"));

			//Correct
			user.customer_code = "c0";
			result = controller.Create(user);
			Assert.IsNotNull(result);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			Assert.AreEqual(5, unitOfWork.Data.Users.Count);
			Assert.IsTrue(unitOfWork.Saved);
			
			string[] properties =
			{
				"id", "name", "address", "password", "email", "customer_code", "token", "isInternal", "lastname",
				"lastLogin", "phone", "username"
			};
			CompareObjects(user, result, properties);
		}

		[TestMethod, TestCategory("UserApiController")]
		public void GetCustomerUIObject()
		{
			var customer = new Customer {code = "c1", name = "n1"};
			var result = UserApiController.GetCustomerUIObject(customer);
			var combinedProp = result.GetType().GetProperty("combined_name");
			Assert.IsNotNull(combinedProp);
			var data = combinedProp.GetValue(result).ToString();
			Assert.AreEqual($"{customer.code} {customer.name}", data);

			customer.address6 = "XL3";
			result = UserApiController.GetCustomerUIObject(customer);
			data = combinedProp.GetValue(result).ToString();
			Assert.AreEqual($"{customer.code} {customer.name} ({customer.address6})", data);
			
		}

		[TestMethod, TestCategory("UserApiController")]
		public void UpdateUser()
		{
			unitOfWork.Data = new MockData
			{
				Users = new List<User>
				{
					new User
					{
						id = 1,
						email = "user",
						customer_code = "c1",
						Roles = new List<Role>
						{
							new Role {id = Role.Admin}
						}
					},
					new User
					{
						id=2,
						username = "user",
						Roles = new List<Role>
						{
							new Role{id=Role.User}
						}
					},
					new User
					{
						id = 3,
						username = "usr",
						customer_code = "c0",
						Roles = new List<Role>
						{
							new Role {id = Role.BranchAdmin}
						}
					},
					new User
					{
						id = 4,
						username = "usr",
						customer_code = "c2",
						Roles = new List<Role>
						{
							new Role {id = Role.BranchAdmin}
						}
					}
				},
				Customers = new List<Customer>
				{
					new Customer {code = "c0"},
					new Customer {code = "c1", invoice_customer = "c0"},
					new Customer {code = "c2"}
				}
			};

			var user = new User
			{
				id = 1,
				name = "name",
				email = "email"
			};
			//Try as regular user
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.Update(user);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.Unauthorized, message?.StatusCode);

			//admin, should get validation error
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.Update(user);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			message = result as HttpResponseMessage;
			Assert.IsNotNull(message);
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);
			var text = message.Content.ReadAsStringAsync().Result;
			Assert.IsTrue(text.Contains("Name"));

			//Correct
			user.lastname = "last";
			user.username = "username";
			user.customer_code = "c0";
			result = controller.Update(user);
			Assert.IsNotNull(result);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			
			Assert.IsTrue(unitOfWork.Saved);

			string[] properties =
			{
				"id", "name", "address", "password", "email", "customer_code", "token", "isInternal", "lastname",
				"lastLogin", "phone", "username"
			};
			CompareObjects(user, result, properties);
		}

		[TestMethod, TestCategory("UserApiController")]
		public void DeleteUser()
		{
			unitOfWork.Data = new MockData
			{
				Users = new List<User>
				{
					new User
					{
						id = 1,
						email = "user",
						customer_code = "c1",
						Roles = new List<Role>
						{
							new Role {id = Role.Admin}
						}
					},
					new User
					{
						id=2,
						username = "user",
						Roles = new List<Role>
						{
							new Role{id=Role.User}
						}
					},
					new User
					{
						id = 3,
						username = "usr",
						customer_code = "c0",
						Roles = new List<Role>
						{
							new Role {id = Role.BranchAdmin}
						}
					},
					new User
					{
						id = 4,
						username = "usr",
						customer_code = "c2",
						Roles = new List<Role>
						{
							new Role {id = Role.BranchAdmin}
						}
					}
				},
				Customers = new List<Customer>
				{
					new Customer {code = "c0"},
					new Customer {code = "c1", invoice_customer = "c0"},
					new Customer {code = "c2"}
				}
			};

			//User
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.Delete(4);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.Unauthorized, message?.StatusCode);

			//Correct
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			result = controller.Delete(4);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			Assert.AreEqual(3, unitOfWork.Data.Users.Count);
			Assert.AreEqual(true, result);
			

		}

		[TestMethod, TestCategory("UserApiController")]
		public void UpdateUserData()
		{
			unitOfWork.Data = new MockData
			{
				Users = new List<User>
				{
					new User
					{
						id = 1,
						email = "user",
						customer_code = "c1",
						Roles = new List<Role>
						{
							new Role {id = Role.Admin}
						}
					},
					new User
					{
						id = 2,
						username = "user",
						address = "address",
						customer_code = "c2",
						Customer = null,
						Roles = new List<Role>
						{
							new Role {id = Role.User}
						}
					}
				},
				Customers = new List<Customer>
				{
					new Customer
					{
						code = "c2",
						address2 = "address"
					}
				}
			};

			var data = new User
			{
				id = 1,
				email = "user",
				customer_code = "c2",
				Customer = new Customer
				{
					code = "c2",
					address2 = "address2"
				}
			};

			//User with no rights
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.UpdateData(data);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.Unauthorized, message?.StatusCode);

			//Correct user, change customer data
			data.id = 2;
			data.address = "address2";
			result = controller.UpdateData(data);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			Assert.IsTrue(unitOfWork.Saved);
			Assert.AreEqual(data.Customer.address2, unitOfWork.Data.Customers.FirstOrDefault(c=>c.code == data.customer_code)?.address2);
			Assert.AreEqual(data.email, unitOfWork.Data.Users.FirstOrDefault(u=>u.id == data.id)?.email);
			Assert.IsFalse(string.IsNullOrEmpty(mailHelper.Data.Subject));

			//Non existing user
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			data.id = 3;
			result = controller.UpdateData(data);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);
		}

		[TestMethod, TestCategory("UserApiController")]
		public void ActivateUser()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();

			//GetSiteUrl needs this
			
			HttpContext.Current = Utils.GetDummyHttpContext();
			
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.Activate(2);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.Unauthorized, message?.StatusCode);

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.Activate(2);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			Assert.IsFalse(string.IsNullOrEmpty(mailHelper.Data.Subject));

			result = controller.Activate(3);
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.InternalServerError, message?.StatusCode);
		}

		

		[TestMethod, TestCategory("UserApiController")]
		public void CheckUser()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			var result = controller.CheckUser(new ChangePassword {code = "3"});
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(HttpStatusCode.BadRequest, message?.StatusCode);

			result = controller.CheckUser(new ChangePassword {code = "2"});
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			Assert.IsInstanceOfType(result, typeof(ChangePassword));
			Assert.IsFalse(string.IsNullOrEmpty((result as ChangePassword)?.ruleText));

		}

		[TestMethod, TestCategory("UserApiController")]
		public void UpdatePassword()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();

			var cp = new ChangePassword
			{
				code = "A",
				password1 = "pass1",
				password2 = "pass2"
			};

			var result = controller.UpdatePassword(cp);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.BadRequest);

			cp.code = "2";
			//Passwords don't match
			result = controller.UpdatePassword(cp);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.BadRequest);

			cp.password2 = "pass1";
			//Passwords break rules (8 minimum)
			result = controller.UpdatePassword(cp);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.BadRequest);

			cp.password1 = cp.password2 = "Password";	//one upper case, 8 chars
			result = controller.UpdatePassword(cp);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			Assert.IsTrue(unitOfWork.Saved);
			Assert.AreEqual(cp.password1, unitOfWork.Data.Users.FirstOrDefault(u=>u.id == 2)?.password);

		}

		[TestMethod, TestCategory("UserApiController")]
		public void ResetPassword()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.ResetPassword(1);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.Unauthorized);

			HttpContext.Current = Utils.GetDummyHttpContext();

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.ResetPassword(3);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.InternalServerError);

			result = controller.ResetPassword(2);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			Assert.IsTrue(unitOfWork.Saved);
			Assert.IsNull(unitOfWork.Data.Users.FirstOrDefault(u=>u.id == 2)?.password);
			Assert.IsFalse(string.IsNullOrEmpty(mailHelper.Data.Subject));

		}

		[TestMethod, TestCategory("UserApiController")]
		public void SendPasswordRecoveryLink()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			var result = controller.SendPasswordRecoveryLink("xxx");
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.InternalServerError);

			HttpContext.Current = Utils.GetDummyHttpContext();
			
			result = controller.SendPasswordRecoveryLink(unitOfWork.Data.Users.FirstOrDefault(u=>!string.IsNullOrEmpty(u.email))?.email);
			Assert.IsNotInstanceOfType(result, typeof(HttpResponseMessage));
			
			Assert.IsFalse(string.IsNullOrEmpty(mailHelper.Data.Subject));

		}

		[TestMethod, TestCategory("UserApiController")]
		public void PasswordRules()
		{
			var rule = new PasswordRule(8,1);
			string errorMessage;
			Assert.IsFalse(rule.ValidatePassword("pass", out errorMessage));
			Assert.IsFalse(rule.ValidatePassword("password", out errorMessage));
			Assert.IsTrue(rule.ValidatePassword("Password", out errorMessage));
			rule.SpecialChars = true;
			Assert.IsFalse(rule.ValidatePassword("Password", out errorMessage));
			Assert.IsTrue(rule.ValidatePassword("Passwor#", out errorMessage));
		}

		[TestMethod, TestCategory("UserApiController")]
		public void GetRolesModel()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			unitOfWork.Data.Roles = new List<Role>
			{
				new Role {id = Role.User, name = "User", Permissions = new List<Permission>()},
				new Role {id = Role.BranchAdmin, name = "Branch", Permissions = new List<Permission>()},
			};
			unitOfWork.Data.Permissions = new List<Permission>
			{
				new Permission {id = 1, name = "1"},
				new Permission {id = 2, name = "2"}
			};
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.GetRolesModel();
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.GetRolesModel();
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));
			var rolesProp = result.GetType().GetProperty("roles");
			var permissionsProp = result.GetType().GetProperty("permissions");
			Assert.IsNotNull(rolesProp);
			Assert.IsNotNull(permissionsProp);
			var rolesData = rolesProp.GetValue(result); 
			TestCollection(rolesData,2);
			TestCollection(permissionsProp.GetValue(result),2);
			var rolesCollection = (rolesData as IEnumerable).Cast<object>().ToList();
			permissionsProp = rolesCollection[0].GetType().GetProperty("permissions");
			Assert.IsNotNull(permissionsProp);

		}

		[TestMethod, TestCategory("UserApiController")]
		public void CreateRole()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			unitOfWork.Data.Roles = new List<Role>
			{
				new Role(),
				new Role()
			};
			
			var role = new Role {name = "test", Permissions = new List<Permission>
			{
				new Permission{id = 1},
				new Permission {id = 2}
			}};
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.CreateRole(role);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.CreateRole(role);
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));
			Assert.AreEqual(3, unitOfWork.Data.Roles.Count);
			
			CompareObjects(unitOfWork.Data.Roles.Last(), role, new[] {"name"});
			var permProp = result.GetType().GetProperty("permissions");
			Assert.IsNotNull(permProp);
			TestCollection(permProp.GetValue(result), 2);
			var permData = permProp.GetValue(result) as IEnumerable<object>;
			Assert.IsNotNull(permData.First().GetType().GetProperty("id"));
		}

		[TestMethod, TestCategory("UserApiController")]
		public void UpdateRole()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			unitOfWork.Data.Roles = new List<Role>
			{
				new Role{id = Role.Admin, name = "test"},
				new Role{ id = 4, name = "test"}
			};
			
			var role = new Role {id = 4, name = "test", Permissions = new List<Permission>
			{
				new Permission{id = 1},
				new Permission {id = 2}
			}};
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.UpdateRole(role);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.UpdateRole(role);
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));
			
			CompareObjects(unitOfWork.Data.Roles[1], role, new[] {"id", "name"});
			var permProp = result.GetType().GetProperty("permissions");
			Assert.IsNotNull(permProp);
			TestCollection(permProp.GetValue(result), 2);
			var permData = permProp.GetValue(result) as IEnumerable<object>;
			Assert.IsNotNull(permData.First().GetType().GetProperty("id"));

			role.id = Role.Admin;
			result = controller.UpdateRole(role);
			//Admin shouldn't be modified
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.BadRequest);
		}

		[TestMethod, TestCategory("UserApiController")]
		public void DeleteRole()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			unitOfWork.Data.Roles = new List<Role>
			{
				new Role{id = 1, name = "admin"},
				new Role {id = 4, name = "restricted"}
			};

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var result = controller.DeleteRole(1);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			result = controller.DeleteRole(1);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.BadRequest);

			result = controller.DeleteRole(4);
			Assert.AreEqual(true, result);

		}

		protected void TestCollection(object collection, int expectedCount, string propName = null, int index = 0, object valueToCompareTo = null )
		{
			Utils.TestCollection(collection, expectedCount, propName, index, valueToCompareTo);
		}

		protected void CompareObjects(object first, object second, IList<string> properties)
		{
			foreach (var prop in properties)
			{
				var origPropInfo = first.GetType().GetProperty(prop);
				var dataPropInfo = second.GetType().GetProperty(prop);
				if (origPropInfo != null && dataPropInfo != null)
				{
					Assert.AreEqual(origPropInfo.GetValue(first), dataPropInfo.GetValue(second));
				}
			}
		}

		
	}
}
