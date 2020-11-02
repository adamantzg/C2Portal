using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiService.Models;
using C2.Model;
using Dapper;

namespace ApiService.Controllers
{
	[OnlyAuthorized]
	public class MainController : ApiController
	{
		private IDal Dal;

		public MainController(IDal dal)
		{
			Dal = dal;
		}

		//public MainController()
		//{

		//}

		[Route("api/getFreeStock")]
		[HttpGet]
		public StockData getFreeStock(string code)
		{
			return Dal.GetFreeStock(code);
		}

		[Route("api/getPrice")]
		[HttpGet]
		public object getPrice(string customer, string product)
		{
			return Dal.GetPrice(customer, product);
		}

		[Route("api/getCustomerTotals")]
		[HttpGet]
		public object getCustomerTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText)
		{
			double? orderTotal, balance, creditLimit;
			int? numOfInvoices;
			Dal.GetCustomerTotals(customer,dateFrom, dateTo, searchText, out orderTotal, out balance, out creditLimit, out numOfInvoices);
			return new
			{
				orderTotal,
				balance,
				creditLimit,
				numOfInvoices
			};
		}
			
		

		[Route("api/getInvoices")]
		[HttpGet]
		public object getInvoices(string customer, DateTime? dateFrom, DateTime? dateTo, int from, int to, string sortBy, SortDirection direction, string searchText)
		{
			return Dal.GetInvoices(customer, dateFrom, dateTo, from, to, sortBy, direction, searchText);
		}

		[Route("api/getOrderByCriteria")]
		[HttpGet]
		public object getOrderByCriteria(string order_no, string customer_code, bool getOutOfStockShipmentDates = true)
		{
			return Dal.GetOrderByCriteria(order_no, customer_code, getOutOfStockShipmentDates);
		}

        [Route("api/getSlCustomer")]
        [HttpGet]
        public object getSlCustomer(string customer)
        {
            return Dal.GetCustomer(customer);
        }

        [Route("api/getOrdersForInvoices")]
		[HttpGet]
		public object getOrderForInvoices(string order_nos)
		{
			return Dal.GetOrdersForInvoices(order_nos);
		}

		[Route("api/getSalesData")]
		[HttpGet]
		public object getSalesData(int monthFrom, int yearFrom, int monthTo, int yearTo)
		{
			return Dal.GetSalesData(monthFrom, yearFrom, monthTo, yearTo);
		}

		[Route("api/getOrders")]
		[HttpGet]
		public object getOrders(string customer, DateTime? dateFrom, DateTime? dateTo, int from, int to, string sortBy, SortDirection direction, string searchText)
		{
			return Dal.GetOrders(customer, dateFrom, dateTo, from, to, sortBy, direction, searchText);
		}

		[Route("api/getOrderDetails")]
		[HttpGet]
		public object getOrderDetails(string order_no)
		{
			return Dal.GetOrderDetails(order_no);
		}

		[Route("api/getOrderTotals")]
		[HttpGet]
		public object getOrderTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText)
		{
			return Dal.GetOrderTotals(customer, dateFrom, dateTo, searchText);
		}

		[Route("api/getClearanceStock")]
		[HttpGet]
		public object getClearanceStock(string productCode, string description, string range, int recFrom, int recTo, 
			string sortBy, SortDirection direction)
		{
			return Dal.GetClearanceStock(recFrom, recTo, sortBy, direction,productCode, description, range);
		}

		[Route("api/getClearanceStockCount")]
		[HttpGet]
		public object getClearanceStockCount(string productCode, string description, string range)
		{
			return Dal.GetClearanceStockCount(productCode, description, range);
		}

	}
}
