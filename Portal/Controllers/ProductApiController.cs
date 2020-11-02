using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Portal.JWT;
using System.Net;
using C2.Model;
using Portal.Model;

namespace Portal.Controllers
{
	[Authorize]
	[RoutePrefix("api/product")]
	public class ProductApiController : BaseApiController
	{
		public ProductApiController(IUnitOfWork unitOfWork, IMailHelper mailHelper, IApiClient apiClient, IMyJwtDecoder jwtDecoder, ICache cache) : 
			base(unitOfWork, mailHelper, apiClient, jwtDecoder, cache)
		{
		}

		[Route("search")]
		[HttpGet]
		public object ProductSearch(string code)
		{
			var param = HttpUtility.UrlDecode(code);
			return uow.ProductRepository.Get(p => p.code.Contains(param) || p.name.Contains(param)).Select(GetUIObject).ToList();
		}

		[Route("get")]
		[HttpGet]
		public object GetProduct(string code)
		{
			var param = HttpUtility.UrlDecode(code);
			var product = uow.ProductRepository.Get(p => p.code == param).FirstOrDefault();
			if (product == null)
				return BadRequest("No product with that code");
			return GetUIObject(product);
		}
        

        public static object GetUIObject(Model.Product p)
		{
			return new
			{
				p.code,
				p.description,
				p.id,
				p.name,
				combined_name = p.code + " " + p.name
			};
		}

		[Route("getFreeStock")]
		[HttpGet]
		public async Task<IHttpActionResult> getFreeStock(string code)
		{
			var response = await apiClient.GetAsync($"{Properties.Settings.Default.apiUrl}/api/getFreeStock?code={code}");
			return Ok(response.Content.ReadAsAsync<StockData>());
			//return await response.Content.ReadAsAsync<double?>();
		}

		[Route("getPrice")]
		[HttpGet]
		public async Task<IHttpActionResult> getPrice(string customer, string code)
		{
			var user = GetCurrentUser();
			if (user == null || (user.customer_code != customer && !user.isAdmin && user.isInternal != true && !user.isBranchAdmin)
			                 || !user.HasPermission(PermissionId.ViewStockSearch)
			    )
				return Unauthorized();
			if (user.customer_code != customer && user.isBranchAdmin && !user.isAdmin)
			{
				var allowedCustomers = UserApiController.GetAllowedCustomersForBranchAdmin(uow, user.customer_code);
				if (!allowedCustomers.Select(c => c.code.Trim()).Contains(customer))
					return Unauthorized();
			}
			var response = await apiClient.GetAsync($"{Properties.Settings.Default.apiUrl}/api/getPrice?customer={customer}&product={code}");
			return Ok(response.Content.ReadAsAsync<object>());
			//return await response.Content.ReadAsAsync<object>();
			
		}

		[Route("getClearanceStock")]
		[HttpGet]
		public async Task<IHttpActionResult> getClearanceStock(string code, string description, string range, int recFrom, int recTo,
			string sortBy, SortDirection direction)
		{
			var response = await apiClient.GetAsync(
				$@"{Properties.Settings.Default.apiUrl}/api/getClearanceStock?productCode={code}&description={description}&range={range}&recFrom={recFrom}&recTo={recTo}&sortBy={sortBy}&direction={direction}");
			return Ok(response.Content.ReadAsAsync<List<ClearanceStock>>());
			//return await response.Content.ReadAsAsync<double?>();
		}

		[Route("getClearanceStockCount")]
		[HttpGet]
		public async Task<IHttpActionResult> getClearanceStockCount(string code, string description, string range)
		{
			var response = await apiClient.GetAsync(
				$@"{Properties.Settings.Default.apiUrl}/api/getClearanceStockCount?productCode={code}&description={description}&range={range}");
			return Ok(response.Content.ReadAsAsync<int?>());
			//return await response.Content.ReadAsAsync<double?>();
		}
	}
}