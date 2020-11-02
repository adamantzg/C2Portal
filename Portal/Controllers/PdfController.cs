
using ASPPDFLib;
using C2.Model;
using Portal.JWT;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Portal.Controllers
{
	
    public class PdfController : Controller
    {
	    private IApiClient apiClient;


	    public PdfController(IApiClient apiClient)
	    {
		    this.apiClient = apiClient;
	    }

        // GET: Pdf
        public ActionResult Index()
        {
            return View();
        }

		
   //     public async Task<ActionResult> Invoice(string order_no, string pdfKey)
   //     {
			//if(pdfKey == Properties.Settings.Default.pdfKey)
			//{
			//	var apiClient = new HttpClient();
			//	apiClient.DefaultRequestHeaders.Add("apiKey", Properties.Settings.Default.jwtKey);
			//	var response = await apiClient.GetAsync($"{Properties.Settings.Default.apiUrl}/api/getOrdersForInvoices?order_no={order_no}");
			//	var task = response.Content.ReadAsAsync<Order>();
			//	var order = task.Result;
			//	return View("Invoices" ,new List<Order> { order });
			//}
			//return null;
   //     }

		public async Task<ActionResult> Invoices(string order_nos, string pdfKey)
		{
			if (pdfKey == Properties.Settings.Default.pdfKey)
			{
				apiClient.AddDefaultRequestHeader("apiKey", Properties.Settings.Default.jwtKey);
				var response = await apiClient.GetAsync($"{Properties.Settings.Default.apiUrl}/api/getOrdersForInvoices?order_nos={order_nos}");
				var task = response.Content.ReadAsAsync<List<Order>>();				
				return View(task.Result);
			}
			return null;
		}
	}
}