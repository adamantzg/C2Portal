using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http.Results;
using C2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal;
using Portal.Controllers;
using Product = Portal.Model.Product;
using Customer = Portal.Model.Customer;
using Portal.Model;


namespace PortalTest.Controllers
{
	[TestClass]
	public class ProductApiControllerTests
	{
		private ProductApiController controller;
		private UnitOfWorkMock unitOfWork;
		private ApiClientMock apiClient;
		private CacheMock cache;
		
		[TestInitialize()]
		public void Init()
		{
			unitOfWork = new UnitOfWorkMock();
			apiClient = new ApiClientMock();
			cache = new CacheMock();
			controller = new ProductApiController(unitOfWork, new MailHelper(new MimeMailerMock(), new RegistryReaderMock()), apiClient, 
				new JwtDecoderMock(), cache);
			controller.Request = new HttpRequestMessage();
			
		}

		[TestMethod, TestCategory("ProductApiController")]
		public void ProductSearch()
		{
			unitOfWork.Data = new MockData
			{
				Products = new List<Product>
				{
					new Product
					{
						code = "code1",
						name = "prod 1"
					},
					new Product
					{
						code = "code2",
						name = "prod 2"
					},
					new Product
					{
						code = "ccc",
						name = "nnnn"
					}
				}
			};

			var result = controller.ProductSearch("prod");
			Utils.TestCollection(result, 2);

			result = controller.ProductSearch("prod%201");
			Utils.TestCollection(result, 1);
			
		}

		[TestMethod, TestCategory("ProductApiController")]
		public void GetProduct()
		{
			unitOfWork.Data = new MockData
			{
				Products = new List<Product>
				{
					new Product
					{
						code = "code 1",
						name = "prod 1"
					},
					new Product
					{
						code = "code2",
						name = "prod 2"
					},
					new Product
					{
						code = "ccc",
						name = "nnnn"
					}
				}
			};

			var result = controller.GetProduct("code 1");
			Assert.IsNotInstanceOfType(result, typeof(BadRequestErrorMessageResult));
			Assert.IsNotNull(result.GetType().GetProperty("code"));

			result = controller.GetProduct("code%201");
			Assert.IsNotInstanceOfType(result, typeof(BadRequestErrorMessageResult));
			Assert.IsNotNull(result.GetType().GetProperty("code"));

			result = controller.GetProduct("xxx");
			Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
			
		}

		[TestMethod, TestCategory("ProductApiController")]
		public void GetFreeStock()
		{
			var code = "code";
			var result = controller.getFreeStock(code).Result;
			Assert.IsTrue(apiClient.Parameters.ContainsKey("code"));
			Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Task<StockData>>));
		}

		[TestMethod, TestCategory("ProductApiController")]
		public void GetPrice()
		{
			var code = "code";
			unitOfWork.Data = Utils.CreateAdminAndUser();
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			var permissions = new List<Permission>
			{
				new Permission {id = (int) PermissionId.ViewStockSearch}
			};
			cache.Set($"permissions_2", permissions , null);
			var customer_code = "c1";
			var result = controller.getPrice(customer_code, code);
			Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));

			unitOfWork.Data.Users.FirstOrDefault(u => u.id == 2).isInternal = true;
			result = controller.getPrice(customer_code, code);
			Assert.IsNotInstanceOfType(result, typeof(UnauthorizedResult));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("customer"));
			Assert.IsTrue(apiClient.Parameters.ContainsKey("product"));
			Assert.IsInstanceOfType((result.Result as OkNegotiatedContentResult<Task<object>>)?.Content?.Result, typeof(ProductPrices));

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
			var res = controller.getPrice("c3", code).Result;
			Assert.IsNotNull(res);
			Assert.AreEqual("c3",apiClient.Parameters["customer"]);
			res = controller.getPrice("c2", code).Result;
			Assert.IsNotNull(res);
			Assert.IsNotInstanceOfType(res, typeof(UnauthorizedResult));
			Assert.AreEqual("c2",apiClient.Parameters["customer"]);

			res = controller.getPrice("c1", code).Result;
			Assert.IsNotNull(res);
			Assert.IsInstanceOfType(res, typeof(UnauthorizedResult));

			//user permissions
			cache.Remove("permissions_2");
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			res = controller.getPrice("c2", code).Result;
			Assert.IsNotNull(res);
			Assert.IsInstanceOfType(res, typeof(UnauthorizedResult));
		}
	}
}
