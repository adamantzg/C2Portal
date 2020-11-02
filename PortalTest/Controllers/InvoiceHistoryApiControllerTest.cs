using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using C2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Controllers;
using Portal.Model;
using Customer = Portal.Model.Customer;

namespace PortalTest.Controllers
{
	[TestClass]
	public class InvoiceHistoryApiControllerTest
	{
		private InvoiceHistoryApiController controller;
		private UnitOfWorkMock unitOfWork;
		private MailHelperMock mailHelper;
		private PdfManagerMock pdfManager;
		private JwtDecoderMock jwtDecoder;
		private HttpRequestMessage request;
		private ApiClientMock apiClient;
		private WebClientMock webClient;
		private CacheMock cache;

		[TestInitialize]
		public void Init()
		{
			unitOfWork = new UnitOfWorkMock();
			mailHelper = new MailHelperMock();
			pdfManager = new PdfManagerMock();
			jwtDecoder = new JwtDecoderMock();
			apiClient = new ApiClientMock();
			request = new HttpRequestMessage();
			webClient = new WebClientMock();
			cache = new CacheMock();
			controller = new InvoiceHistoryApiController(unitOfWork, mailHelper, apiClient, pdfManager, webClient,jwtDecoder, cache);
			controller.Request = new HttpRequestMessage();
		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void CustomerTotals()
		{
			var customer_code = "c000";
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = customer_code}}
			};
			apiClient.Data.CustomerTotals = new CustomerTotals
			{
				balance = 100,
				creditLimit = 1000,
				numOfInvoices = 10,
				orderTotal = 10000
			};

			DateTime? dateFrom = DateTime.Today.AddDays(-2), dateTo = DateTime.Today.AddDays(2);

			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewInvoiceHistory}
			};
			cache.Set($"permissions_1", permissions , null);

			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			controller.Request = request;
			var result = controller.getCustomerTotals(customer_code, dateFrom, dateTo, "").Result;
			Assert.IsNotNull(result);
			Assert.IsNotNull(apiClient.Parameters);
			Assert.IsTrue(apiClient.Parameters.ContainsKey("customer"));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("dateFrom"));
			Assert.AreEqual(dateFrom, DateTime.Parse(apiClient.Parameters["dateFrom"],CultureInfo.InvariantCulture.DateTimeFormat));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("dateTo"));
			Assert.AreEqual(dateTo, DateTime.Parse(apiClient.Parameters["dateTo"],CultureInfo.InvariantCulture.DateTimeFormat));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("searchText"));
			Assert.AreEqual("", apiClient.Parameters["searchText"]);
			Assert.AreEqual(customer_code, apiClient.Parameters["customer"]);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<CustomerTotals>>));
			Assert.AreEqual(apiClient.Data.CustomerTotals.balance,
				(result as OkNegotiatedContentResult<Task<CustomerTotals>>)?.Content?.Result?.balance);
		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void CustomerTotalsNotAuthorized()
		{
			var customer_code = "c000";
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = "xxx"}}
			};
			apiClient.Data.CustomerTotals = new CustomerTotals
			{
				balance = 100,
				creditLimit = 1000,
				numOfInvoices = 10,
				orderTotal = 10000
			};
			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			controller.Request = request;
			var result = controller.getCustomerTotals(customer_code, DateTime.MinValue, DateTime.MaxValue, "").Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void CustomerTotalsBranchAdmin()
		{
			var customer_code = "c2";
			unitOfWork.Data = Utils.CreateAdminAndUser();
			apiClient.Data.CustomerTotals = new CustomerTotals();
			unitOfWork.Data.Customers = new List<Customer>
			{
				new Customer {code = "c1"},
				new Customer {code = "c2", invoice_customer = "c3"},
				new Customer { code = "c3"}
			};
			unitOfWork.Data.Users.Add(new User {id=3, Roles = new List<Role>{new Role {id = Role.BranchAdmin}}, customer_code = "c3"});
			
			//Admin can access all data
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewInvoiceHistory}
			};
			cache.Set($"permissions_1", permissions , null);
			var result = controller.getCustomerTotals(customer_code, DateTime.MinValue, DateTime.MaxValue, "").Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<CustomerTotals>>));

			//User can't access other customer code
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			Assert.IsInstanceOfType(controller.getCustomerTotals("c1", null, null, null).Result, typeof(UnauthorizedResult));

			//Branch admin should access its users' code
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			cache.Set($"permissions_3", permissions , null);
			result = controller.getCustomerTotals("c3", DateTime.MinValue, DateTime.MaxValue, "").Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<CustomerTotals>>));
			result = controller.getCustomerTotals("c2", DateTime.MinValue, DateTime.MaxValue, "").Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<CustomerTotals>>));

			Assert.IsInstanceOfType(controller.getCustomerTotals("c1", null, null, null).Result, typeof(UnauthorizedResult));
		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void CustomerTotalsAdmin()
		{
			var customer_code = "c000";
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = "xxx", Roles = new List<Role>{new Role {id = Role.Admin}}}}
			};
			apiClient.Data.CustomerTotals = new CustomerTotals
			{
				balance = 100,
				creditLimit = 1000,
				numOfInvoices = 10,
				orderTotal = 10000
			};
			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewInvoiceHistory}
			};
			cache.Set($"permissions_1", permissions , null);
			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			controller.Request = request;
			var result = controller.getCustomerTotals(customer_code, DateTime.MinValue, DateTime.MaxValue, "").Result;
			Assert.IsNotNull(result);
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));
		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void CustomerTotalsPermissions()
		{
			var customer_code = "c1";
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = "c1", Roles = new List<Role>
				{
					new Role {id=Role.User}
				}}}
			};
			cache.Set($"permissions_1", new List<Permission>(), null);
			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			controller.Request = request;
			var result = controller.getCustomerTotals(customer_code, DateTime.MinValue, DateTime.MaxValue, "").Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewInvoiceHistory}
			};
			cache.Set($"permissions_1", permissions , null);
			result = controller.getCustomerTotals(customer_code, DateTime.MinValue, DateTime.MaxValue, "").Result;
			Assert.IsNotNull(result);
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));
		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void GetCustomer()
		{
			var customer_code = "c000";
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = "xxx", Roles = new List<Role>{new Role {id = Role.Admin}}}},
				Customers = new List<Customer> {new Customer {code = customer_code}}
			};
			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			controller.Request = request;
			var result = controller.getCustomer(customer_code);
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Customer>));

		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void GetInvoices()
		{
			var customer_code = "c000";
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = customer_code, 
					Roles = new List<Role>{new Role {id = Role.BranchAdmin}}}},
				Customers = new List<Customer> {new Customer {code = customer_code}}
			};

			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewInvoiceHistory}
			};
			cache.Set($"permissions_1", permissions , null);

			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			controller.Request = request;
			DateTime? dateFrom = DateTime.Today.AddDays(-2), dateTo = DateTime.Today;
			int recFrom = 1, recTo = 3;
			SortDirection direction = SortDirection.Asc;
			string sortBy = "sort", searchText = "search";

			apiClient.Data.Invoices = new List<Invoice>
			{
				new Invoice {amount = 1000},
				new Invoice {amount =  2000}
			};

			var result = controller.getInvoices(customer_code, dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText ).Result;
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
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<List<Invoice>>>));
			Assert.AreEqual(apiClient.Data.Invoices.Count, (result as OkNegotiatedContentResult<Task<List<Invoice>>>)?.Content?.Result?.Count );

			//Test unauthorized
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = "xxx", Roles = new List<Role>{new Role {id = Role.BranchAdmin}}}},
				Customers = new List<Customer> {new Customer {code = customer_code}}
			};
			result = controller.getInvoices(customer_code, dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText ).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

			//Branch admin
			unitOfWork.Data = Utils.CreateAdminAndUser();
			apiClient.Data.Invoices = new List<Invoice>();
			unitOfWork.Data.Customers = new List<Customer>
			{
				new Customer {code = "c1"},
				new Customer {code = "c2", invoice_customer = "c3"},
				new Customer { code = "c3"}
			};
			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "3");
			unitOfWork.Data.Users.Add(new User {id=3, Roles = new List<Role>{new Role {id = Role.BranchAdmin}}, customer_code = "c3"});
			
			cache.Set($"permissions_3", permissions , null);
			result = controller.getInvoices("c3", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<List<Invoice>>>));
			result = controller.getInvoices("c2", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<List<Invoice>>>));

			Assert.IsInstanceOfType(controller.getInvoices("c1", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result, typeof(UnauthorizedResult));

			//Permissions - regular user
			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			result = controller.getInvoices("c2", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
			
			unitOfWork.Data.Users.FirstOrDefault(u=>u.id == 2).Permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewInvoiceHistory}
			};
			result = controller.getInvoices("c2", dateFrom, dateTo, recFrom, recTo, sortBy, direction, searchText).Result;
			Assert.IsNotNull(result);
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));

		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void InvoicePdfs()
		{
			var context = new HttpContext(
				new HttpRequest("", "http://tempuri.org", ""),
				new HttpResponse(new StringWriter())
			);
			HttpContext.Current = context;
			var result = controller.InvoicePdfs("1,2");
			Assert.AreEqual("/Pdf/Invoices", pdfManager.Document.ImportFromUrlBaseUrl);
			Assert.IsTrue(pdfManager.Document.ImportFromUrlParameters.ContainsKey("order_nos"));
			Assert.IsInstanceOfType(result, typeof(byte[]));

			result = controller.InvoicePdf("1");
			Assert.AreEqual("/Pdf/Invoices", pdfManager.Document.ImportFromUrlBaseUrl);
			Assert.IsTrue(pdfManager.Document.ImportFromUrlParameters.ContainsKey("order_nos"));
			Assert.IsTrue(pdfManager.Document.ImportFromUrlParameters.ContainsKey("pdfKey"));
			Assert.IsInstanceOfType(result, typeof(byte[]));
		}

		[TestMethod, TestCategory("InvoiceHistoryController")]
		public void CheckPod()
		{
			var customer_code = "c000";
			unitOfWork.Data = new MockData
			{
				Users = new List<User> {new User {id = 1, name = "Test", customer_code = customer_code, Roles = new List<Role>{new Role {id = Role.BranchAdmin}}}},
				Customers = new List<Customer> {new Customer {code = customer_code}}
			};
			request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			controller.Request = request;
			webClient.Result = "No Tracking Information";
			var result = controller.CheckPod(string.Empty, String.Empty, String.Empty, string.Empty);
			Assert.AreEqual(false, result);
			Assert.IsFalse(string.IsNullOrEmpty(mailHelper.Data.Subject));
		}
	}
}
