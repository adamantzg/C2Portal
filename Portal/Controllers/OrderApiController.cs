using ASPPDFLib;
using C2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Portal.JWT;
using Portal.Model;

namespace Portal.Controllers
{
	[Authorize]
	[RoutePrefix("api/order")]
	public class OrderApiController : BaseApiController
	{
		// GET api/<controller>
		public OrderApiController(IUnitOfWork unitOfWork, IMailHelper mailHelper, IApiClient apiClient, IMyJwtDecoder jwtDecoder, ICache cache) : 
			base(unitOfWork, mailHelper, apiClient, jwtDecoder, cache)
		{
		}

		[HttpGet]
		[Route("")]
		public async Task<IHttpActionResult> GetOrder(string order_no, string customer_code)
		{
			var user = GetCurrentUser();
			if (user == null || !user.HasPermission(PermissionId.ViewOrderHistory))
				return Unauthorized();
			if (!user.isAdmin && !user.isBranchAdmin)
				customer_code = user.customer_code;
			if (user.customer_code != customer_code && user.isBranchAdmin && !user.isAdmin)
			{
				var allowedCustomers = UserApiController.GetAllowedCustomersForBranchAdmin(uow, user.customer_code);
				if (!allowedCustomers.Select(c => c.code.Trim()).Contains(customer_code))
					customer_code = user.customer_code;
			}
			var response = await apiClient.GetAsync($"{Properties.Settings.Default.apiUrl}/api/getOrderByCriteria?order_no={order_no}&customer_code={customer_code}");
			return Ok(response.Content.ReadAsAsync<Order>());
		}

		[HttpGet]
		[Route("orders")]
		public async Task<IHttpActionResult> GetOrders(string customer, DateTime? dateFrom, DateTime? dateTo, int from, int to, string sortBy, SortDirection direction, string searchText = "")
		{
			var user = GetCurrentUser();
			if (user == null || (user.customer_code != customer && !user.isAdmin && !user.isBranchAdmin )
			                 || !user.HasPermission(PermissionId.ViewOrderHistory))
				return Unauthorized();
			if (user.customer_code != customer && user.isBranchAdmin && !user.isAdmin)
			{
				var allowedCustomers = UserApiController.GetAllowedCustomersForBranchAdmin(uow, user.customer_code);
				if (!allowedCustomers.Select(c => c.code.Trim()).Contains(customer))
					return Unauthorized();
			}
			if (sortBy == "statusText")
				sortBy = "status";
			var response = 
				await apiClient.GetAsync(
					$@"{Properties.Settings.Default.apiUrl}/api/getOrders?customer={customer}&dateFrom={dateFrom.ToIsoDate()}&dateTo={dateTo.ToIsoDate()}&from={from}&to={to}&sortBy={sortBy}&direction={direction}&searchText={searchText}");
			return Ok(response.Content.ReadAsAsync<List<Order>>());
		}

		[HttpGet]
		[Route("getOrderTotals")]
		public async Task<IHttpActionResult> GetOrderTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText = "")
		{
			var user = GetCurrentUser();
			if (user == null || (user.customer_code != customer && !user.isAdmin && !user.isBranchAdmin) 
			                 || !user.HasPermission(PermissionId.ViewOrderHistory))
				return Unauthorized();
			if (user.customer_code != customer && user.isBranchAdmin && !user.isAdmin)
			{
				var allowedCustomers = UserApiController.GetAllowedCustomersForBranchAdmin(uow, user.customer_code);
				if (!allowedCustomers.Select(c => c.code.Trim()).Contains(customer))
					return Unauthorized();
			}
			var response = 
				await apiClient.GetAsync(
					$@"{Properties.Settings.Default.apiUrl}/api/getOrderTotals?customer={customer}&dateFrom={dateFrom.ToIsoDate()}&dateTo={dateTo.ToIsoDate()}&searchText={searchText}");
			return Ok(response.Content.ReadAsAsync<OrderTotals>());
		}

		[HttpGet]
		[Route("details")]
		public async Task<IHttpActionResult> GetOrderDetails(string order_no)
		{
			var response = await apiClient.GetAsync($"{Properties.Settings.Default.apiUrl}/api/getOrderDetails?order_no={order_no}");
			return Ok(response.Content.ReadAsAsync<List<OrderDetail>>());
		}

		[Route("getSlCustomer")]
		public Model.Customer GetCustomer(string customer)
		{
			/*var response = await apiClient.GetAsync($"{Properties.Settings.Default.apiUrl}/api/getSlCustomer?customer={customer}");
			return Ok(response.Content.ReadAsAsync<Customer>());*/
			return uow.CustomerRepository.Get(c => c.code == customer).FirstOrDefault();
		}



		
	}
}