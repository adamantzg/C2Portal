using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using C2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal;
using Portal.Controllers;
using System.Collections.Generic;
using System.Globalization;
using Portal.Model;
using Customer = Portal.Model.Customer;


namespace PortalTest.Controllers
{
	[TestClass]
	public class OrderApiControllerTests
	{
		private OrderApiController controller;
		private UnitOfWorkMock unitOfWork;
		private ApiClientMock apiClient;
		private CacheMock cache;
		
		[TestInitialize()]
		public void Init()
		{
			unitOfWork = new UnitOfWorkMock();
			apiClient = new ApiClientMock();
			cache = new CacheMock();
			controller = new OrderApiController(unitOfWork, new MailHelper(new MimeMailerMock(), new RegistryReaderMock()), apiClient,
				new JwtDecoderMock(), cache);
			controller.Request = new HttpRequestMessage();
		}

		[TestMethod, TestCategory("OrderApiController")]
		public void GetOrder()
		{
			unitOfWork.Data = Utils.CreateAdminAndUser();
			//NO user
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "0");
			var result = controller.GetOrder("o", "c");
			Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewOrderHistory}
			};
			cache.Set($"permissions_2", permissions , null);
			result = controller.GetOrder("o", "c1");
			Assert.IsNotNull(result);
			Assert.IsNotNull(apiClient.Parameters);
			Assert.IsTrue(apiClient.Parameters.ContainsKey("order_no"));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("customer_code"));
			Assert.AreEqual(unitOfWork.Data.Users.FirstOrDefault(u=>u.id == 2)?.customer_code, apiClient.Parameters["customer_code"]);

			//Branch admin
			unitOfWork.Data = Utils.CreateAdminAndUser();
			apiClient.Data.Order = new Order();
			unitOfWork.Data.Customers = new List<Customer>
			{
				new Customer {code = "c1"},
				new Customer {code = "c2", invoice_customer = "c3"},
				new Customer { code = "c3"}
			};
			
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			cache.Set($"permissions_3", permissions , null);
			unitOfWork.Data.Users.Add(new User {id=3, Roles = new List<Role>{new Role {id = Role.BranchAdmin}}, customer_code = "c3"});
			var res = controller.GetOrder("o","c3").Result;
			Assert.IsNotNull(res);
			Assert.AreEqual("c3",apiClient.Parameters["customer_code"]);
			res = controller.GetOrder("o","c2").Result;
			Assert.IsNotNull(res);
			Assert.AreEqual("c2",apiClient.Parameters["customer_code"]);

			res = controller.GetOrder("o", "c1").Result;
			Assert.IsNotNull(res);
			Assert.AreNotEqual("c1", apiClient.Parameters["customer_code"]);

			//permissions
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			cache.Remove("permissions_2");
			res = controller.GetOrder("o", "c2").Result;
			Assert.IsNotNull(res);
			Assert.IsInstanceOfType(res, typeof(UnauthorizedResult));
		}

		[TestMethod, TestCategory("OrderApiController")]
		public void GetOrders()
		{
			var customer_code = "c2";
			unitOfWork.Data = Utils.CreateAdminAndUser();

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			
			DateTime? dateFrom = DateTime.Today.AddDays(-2), dateTo = DateTime.Today;
			int recFrom = 1, recTo = 3;
			SortDirection direction = SortDirection.Asc;
			string sortBy = "sort", searchText = "search";

			apiClient.Data.Orders = new List<Order>
			{
				new Order {order_no = "1"},
				new Order {order_no =  "2"}
			};

			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewOrderHistory}
			};
			cache.Set($"permissions_2", permissions , null);

			var result = controller.GetOrders(customer_code, dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText ).Result;
			Assert.IsNotNull(result);
			Assert.IsNotNull(apiClient.Parameters);
			Assert.IsTrue(apiClient.Parameters.ContainsKey("customer"));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("dateFrom"));
			Assert.AreEqual(dateFrom, DateTime.Parse(apiClient.Parameters["dateFrom"],CultureInfo.InvariantCulture.DateTimeFormat));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("dateTo"));
			Assert.AreEqual(dateTo, DateTime.Parse(apiClient.Parameters["dateTo"],CultureInfo.InvariantCulture.DateTimeFormat));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("from"));
			Assert.AreEqual(recFrom, int.Parse(apiClient.Parameters["from"]));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("to"));
			Assert.AreEqual(recTo, int.Parse(apiClient.Parameters["to"]));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("direction"));
			Assert.AreEqual(direction, Enum.Parse(typeof(SortDirection), apiClient.Parameters["direction"]));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("searchText"));
			Assert.AreEqual(searchText, apiClient.Parameters["searchText"]);
			Assert.AreEqual(customer_code, apiClient.Parameters["customer"]);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<List<Order>>>));
			Assert.AreEqual(apiClient.Data.Orders.Count, ((result as OkNegotiatedContentResult<Task<List<Order>>>)?.Content?.Result)?.Count );

			//Test unauthorized
			customer_code = "c1";
			result = controller.GetOrders(customer_code, dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText ).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

			//Branch admin
			unitOfWork.Data = Utils.CreateAdminAndUser();
			apiClient.Data.Orders = new List<Order>();
			unitOfWork.Data.Customers = new List<Customer>
			{
				new Customer {code = "c1"},
				new Customer {code = "c2", invoice_customer = "c3"},
				new Customer { code = "c3"}
			};
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			cache.Set($"permissions_3", permissions , null);
			unitOfWork.Data.Users.Add(new User {id=3, Roles = new List<Role>{new Role {id = Role.BranchAdmin}}, customer_code = "c3"});
			result = controller.GetOrders("c3", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<List<Order>>>));
			result = controller.GetOrders("c2", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<List<Order>>>));

			Assert.IsInstanceOfType(controller.GetOrders("c1", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result, typeof(UnauthorizedResult));

			//Test permissions
			cache.Remove("permissions_2");
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			result = controller.GetOrders("c2", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
			
			unitOfWork.Data.Users.FirstOrDefault(u=>u.id == 2).Permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewOrderHistory}
			};
			result = controller.GetOrders("c2", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));

		}

		[TestMethod, TestCategory("OrderApiController")]
		public void GetOrderTotals()
		{
			var customer_code = "c2";
			unitOfWork.Data = Utils.CreateAdminAndUser();

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			
			DateTime? dateFrom = DateTime.Today.AddDays(-2), dateTo = DateTime.Today;
			string searchText = "search";
			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewOrderHistory}
			};
			
			cache.Set($"permissions_2", permissions , null);
			
			var result = controller.GetOrderTotals(customer_code, dateFrom, dateTo, searchText ).Result;
			Assert.IsNotNull(result);
			Assert.IsNotNull(apiClient.Parameters);
			Assert.IsTrue(apiClient.Parameters.ContainsKey("customer"));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("dateFrom"));
			Assert.AreEqual(dateFrom, DateTime.Parse(apiClient.Parameters["dateFrom"],CultureInfo.InvariantCulture.DateTimeFormat));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("dateTo"));
			Assert.AreEqual(dateTo, DateTime.Parse(apiClient.Parameters["dateTo"],CultureInfo.InvariantCulture.DateTimeFormat));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("searchText"));
			Assert.AreEqual(searchText, apiClient.Parameters["searchText"]);
			Assert.AreEqual(customer_code, apiClient.Parameters["customer"]);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<OrderTotals>>));

			//Test unauthorized
			customer_code = "c1";
			result = controller.GetOrderTotals(customer_code, dateFrom, dateTo, searchText ).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

			//Branch admin
			unitOfWork.Data = Utils.CreateAdminAndUser();
			apiClient.Data.OrderTotals = new OrderTotals();
			unitOfWork.Data.Customers = new List<Customer>
			{
				new Customer {code = "c1"},
				new Customer {code = "c2", invoice_customer = "c3"},
				new Customer { code = "c3"}
			};
			cache.Set($"permissions_3", permissions , null);
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			unitOfWork.Data.Users.Add(new User {id=3, Roles = new List<Role>{new Role {id = Role.BranchAdmin}}, customer_code = "c3"});
			result = controller.GetOrderTotals("c3", dateFrom, dateTo,  searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<OrderTotals>>));
			result = controller.GetOrderTotals("c2", dateFrom, dateTo,  searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<OrderTotals>>));

			Assert.IsInstanceOfType(controller.GetOrderTotals("c1", dateFrom, dateTo, searchText).Result, typeof(UnauthorizedResult));

			//Check permissions
			cache.Remove("permissions_2");
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			result = controller.GetOrderTotals("c2", dateFrom, dateTo,  searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));


		}

		[TestMethod, TestCategory("OrderApiController")]
		public void GetOrderDetails()
		{
			var order_no = "order";
			var result = controller.GetOrderDetails(order_no).Result;
			Assert.IsNotNull(result);
			Assert.IsTrue(apiClient.Parameters.ContainsKey("order_no"));
			Assert.AreEqual(order_no, apiClient.Parameters["order_no"]);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<List<OrderDetail>>>));

		}
	}
}
