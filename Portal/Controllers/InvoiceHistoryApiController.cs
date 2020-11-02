using ASPPDFLib;
using C2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MySql.Data.MySqlClient.Memcached;
using Portal.JWT;
using Portal.Model;

namespace Portal.Controllers
{
	[Authorize]
	[RoutePrefix("api/invoicehistory")]
	public class InvoiceHistoryApiController : BaseApiController
	{
		private IPdfManager pdfManager;
		private IWebClient webClient;

		public InvoiceHistoryApiController(IUnitOfWork unitOfWork, IMailHelper mailHelper, IApiClient apiClient, 
			IPdfManager pdfManager, IWebClient webClient, IMyJwtDecoder jwtDecoder, ICache cache) : 
			base(unitOfWork, mailHelper, apiClient, jwtDecoder, cache)
		{
			this.pdfManager = pdfManager;
			this.webClient = webClient;
		}

		[Route("getCustomerTotals")]
		[HttpGet]
		public async Task<IHttpActionResult> getCustomerTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText)
		{
			var user = GetCurrentUser();
			if (user == null || 
			    (user.customer_code != customer && !user.isBranchAdmin && !user.isAdmin)
			    || !user.HasPermission(PermissionId.ViewInvoiceHistory))
				return Unauthorized();
			if (user.customer_code != customer && user.isBranchAdmin && !user.isAdmin)
			{
				var allowedCustomers = UserApiController.GetAllowedCustomersForBranchAdmin(uow, user.customer_code);
				if (!allowedCustomers.Select(c => c.code.Trim()).Contains(customer))
					return Unauthorized();
			}
			var response = await apiClient.GetAsync(
				$@"{Properties.Settings.Default.apiUrl}/api/getCustomerTotals?customer={customer}&dateFrom={dateFrom.ToIsoDate()}&dateTo={dateTo.ToIsoDate()}&searchText={searchText}");
			return Ok(response.Content.ReadAsAsync<CustomerTotals>());
		}

		[Route("customer")]
		[HttpGet]
		public IHttpActionResult getCustomer(string code)
		{
			return Ok(uow.CustomerRepository.Get(c=>c.code == code).FirstOrDefault());
		}

		[Route("invoices")]
		[HttpGet]
		public async Task<IHttpActionResult> getInvoices(string customer, DateTime? dateFrom, DateTime? dateTo, int from, int to, 
			string sortBy, SortDirection direction, string searchText = "")
		{
			var user = GetCurrentUser();
			if (user == null || (user.customer_code != customer && !user.isBranchAdmin && !user.isAdmin)
			                 || !user.HasPermission(PermissionId.ViewInvoiceHistory))
				return Unauthorized();
			if (user.customer_code != customer && user.isBranchAdmin && !user.isAdmin)
			{
				var allowedCustomers = UserApiController.GetAllowedCustomersForBranchAdmin(uow, user.customer_code);
				if (!allowedCustomers.Select(c => c.code.Trim()).Contains(customer))
					return Unauthorized();
			}
			var response = 
				await apiClient.GetAsync(
				$@"{Properties.Settings.Default.apiUrl}/api/getInvoices?customer={customer}&dateFrom={dateFrom.ToIsoDate()}&dateTo={dateTo.ToIsoDate()}&from={from}&to={to}&sortBy={sortBy}&direction={direction}&searchText={searchText}");
			return Ok(response.Content.ReadAsAsync<List<Invoice>>());
		}

		[Route("invoicepdf")]
		[HttpPost]
		public object InvoicePdf(string order_no)
		{
			var doc = pdfManager.CreateDocument();
			doc.ImportFromUrl(Utilities.GetSiteUrl() + $"/Pdf/Invoices?order_nos={order_no}&pdfKey={Properties.Settings.Default.pdfKey}", "scale=0.78, leftmargin=12,rightmargin=12,topmargin=12, bottommargin=12,media=1");
			var s = doc.SaveToMemory();
			return (byte[])s;
		}

		[Route("invoicepdfs")]
		[HttpPost]
		public object InvoicePdfs(string order_nos)
		{
			var doc = pdfManager.CreateDocument();
			doc.ImportFromUrl(Utilities.GetSiteUrl() + $"/Pdf/Invoices?order_nos={order_nos}&pdfKey={Properties.Settings.Default.pdfKey}", "scale=0.78, leftmargin=12,rightmargin=12,topmargin=12, bottommargin=12,media=1");
			
			var s = doc.SaveToMemory();
			return (byte[])s;
		}

		[Route("checkpod")]
		[HttpGet]
		public object CheckPod(string order_no, string postcode, string customer, string customer_order_no)
		{
			//In production on real site it calls carrier web site for status
			var random = new Random();
			var num = random.Next(1, 3);

			return num == 2;

		}
	}

	public class CustomerTotals
	{
		public double? orderTotal { get; set; }
		public double? balance { get; set; }
		public double? creditLimit { get; set; }
		public int? numOfInvoices { get; set; }
	}
}