using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using C2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal;
using Portal.Controllers;
using Portal.Model;

namespace PortalTest.Controllers
{
	[TestClass]
	public class PdfControllerTest
	{
		private ApiClientMock apiClient;

		[TestInitialize()]
		public void Init()
		{
			apiClient = new ApiClientMock();
		}

		[TestMethod, TestCategory("PdfController")]
		public void Invoices()
		{
			var controller = new PdfController(apiClient);
			var result = controller.Invoices("order", "xxx");
			Assert.IsNull(result.Result);

			var pdfKey = Portal.Properties.Settings.Default.pdfKey;
			result = controller.Invoices("order", pdfKey);
			Assert.IsTrue(apiClient.Parameters.ContainsKey("order_nos"));
			Assert.IsInstanceOfType(result.Result, typeof(ViewResult));
			Assert.IsInstanceOfType((result.Result as ViewResult).Model, typeof(List<Order>));
		}
	}
}
