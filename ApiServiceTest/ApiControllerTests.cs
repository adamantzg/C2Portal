using System;
using System.Text;
using System.Collections.Generic;
using ApiService.Controllers;
using C2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiServiceTest
{
	/// <summary>
	/// Summary description for ApiControllerTests
	/// </summary>
	[TestClass]
	public class ApiControllerTests
	{
		private DALMock dal;
		private MainController apiController;

		[TestInitialize]
		public void TestInit()
		{
			dal = new DALMock();
			apiController = new MainController(dal);
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetFreeStock()
		{
			var product = "product";
			dal.SetData(new DalMockData{StockData = new StockData{product = product, quantity = 10}});
			var data = apiController.getFreeStock(product);
			Assert.AreEqual(product, dal.Parameters.Product);
			Assert.IsNotNull(data);
			Assert.AreEqual(10, data.quantity);
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetPrice()
		{
			var product = "product";
			dal.SetData(new DalMockData{ProductPrices = new ProductPrices {basePrice = 100, customerPrice = 50}});
			var data = apiController.getPrice("c",product);
			Assert.AreEqual(product, dal.Parameters.Product);
			Assert.AreEqual("c", dal.Parameters.Customer);
			Assert.IsNotNull(data);
			Assert.IsInstanceOfType(data, typeof(ProductPrices));
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetCustomerTotals()
		{
			var customer = "customer";
			dal.SetCustomerTotals(customer, 100.0);
			var data = apiController.getCustomerTotals(customer, DateTime.Today, DateTime.Today, "search");
			Assert.IsNotNull(data);
			Assert.AreEqual(customer, dal.Parameters.Customer);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateFrom);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateTo);
			Assert.AreEqual("search", dal.Parameters.SearchText);
			var prop = Utilities.GetProperty(data, "orderTotal");
			Assert.IsNotNull(prop);
			Assert.AreEqual(100.0, prop.GetValue(data));
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetInvoices()
		{
			var testData = new List<Invoice>
			{
				new Invoice(),
				new Invoice()
			};
			
			DateTime dateFrom, dateTo;
			dateFrom = dateTo = DateTime.Today;
			var searchText = "search";
			var customer = "c";
			var sortBy = "sort";
			var sort = SortDirection.Asc;
			dal.SetInvoicesData(testData);
			var data = apiController.getInvoices(customer, dateFrom, dateTo,0,0,sortBy, sort, searchText);
			Assert.IsNotNull(data);
			Assert.AreEqual(customer, dal.Parameters.Customer);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateFrom);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateTo);
			Assert.AreEqual(searchText, dal.Parameters.SearchText);
			Assert.AreEqual(sortBy, dal.Parameters.OrderByField);
			Assert.AreEqual(sort, dal.Parameters.Direction);
			Assert.AreEqual(0, dal.Parameters.RecFrom);
			Assert.AreEqual(0, dal.Parameters.RecTo);
			Assert.IsInstanceOfType(data, typeof(List<Invoice>));
			Assert.AreEqual(2, (data as List<Invoice>)?.Count);
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetOrderByCriteria()
		{
			var order_no = "order";
			var customer = "cust";
			var testData = new Order{order_no = order_no};
			dal.SetOrderData(testData);
			var data = apiController.getOrderByCriteria(order_no,customer, false );
			Assert.IsNotNull(data);
			Assert.AreEqual(order_no, dal.Parameters.OrderNo);
			Assert.AreEqual(customer, dal.Parameters.Customer);
			Assert.AreEqual(false, dal.Parameters.GetOutOfStockShipmentDates);
			Assert.IsInstanceOfType(data, typeof(Order));
			Assert.AreEqual(order_no, (data as Order)?.order_no);
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetCustomer()
		{
			var customer = "cust";
			dal.SetData(new DalMockData {Customer = new Customer {customer = customer}});
			var data = apiController.getSlCustomer(customer);
			Assert.IsNotNull(data);
			Assert.AreEqual(customer, dal.Parameters.Customer);
			Assert.IsInstanceOfType(data, typeof(Customer));
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetOrdersForInvoices()
		{
			var order_nos = "order";
			dal.SetData(new DalMockData{Orders = new List<Order>
			{
				new Order{order_no = "1"},
				new Order{order_no = "2"}
			}});
			var data = apiController.getOrderForInvoices(order_nos);
			Assert.IsNotNull(data);
			Assert.AreEqual(order_nos, dal.Parameters.OrderNo);
			Assert.IsInstanceOfType(data, typeof(List<Order>));
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetSalesData()
		{
			dal.SetData(new DalMockData{SalesData = new List<SalesData>
			{
				new SalesData{month = 1},
				new SalesData{month = 2}
			}});
			int monthFrom, monthTo, yearFrom, yearTo;
			monthFrom = monthTo = yearTo = yearFrom = 1;
			var data = apiController.getSalesData(monthFrom, yearFrom, monthTo, yearTo);
			Assert.IsNotNull(data);
			Assert.AreEqual(monthFrom, dal.Parameters.MonthFrom);
			Assert.AreEqual(monthTo, dal.Parameters.MonthTo);
			Assert.AreEqual(yearFrom, dal.Parameters.YearFrom);
			Assert.AreEqual(yearTo, dal.Parameters.YearTo);
			Assert.IsInstanceOfType(data, typeof(List<SalesData>));
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetOrders()
		{
			var testData = new List<Order>
			{
				new Order{order_no = "1"},
				new Order {order_no = "2"}
			};
			
			DateTime dateFrom, dateTo;
			dateFrom = dateTo = DateTime.Today;
			var searchText = "search";
			var customer = "c";
			var sortBy = "sort";
			var sort = SortDirection.Asc;
			dal.SetData(new DalMockData {Orders = testData});
			var data = apiController.getOrders(customer, dateFrom, dateTo,0,0,sortBy, sort, searchText);
			Assert.IsNotNull(data);
			Assert.AreEqual(customer, dal.Parameters.Customer);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateFrom);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateTo);
			Assert.AreEqual(searchText, dal.Parameters.SearchText);
			Assert.AreEqual(sortBy, dal.Parameters.OrderByField);
			Assert.AreEqual(sort, dal.Parameters.Direction);
			Assert.AreEqual(0, dal.Parameters.RecFrom);
			Assert.AreEqual(0, dal.Parameters.RecTo);
			Assert.IsInstanceOfType(data, typeof(List<Order>));
			Assert.AreEqual(2, (data as List<Order>)?.Count);
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetOrderDetails()
		{
			var order_nos = "order";
			dal.SetData(new DalMockData{OrderDetails = new List<OrderDetail>
			{
				new OrderDetail{product = "1"},
				new OrderDetail{product = "2"}
			}});
			var data = apiController.getOrderDetails(order_nos);
			Assert.IsNotNull(data);
			Assert.AreEqual(order_nos, dal.Parameters.OrderNo);
			Assert.IsInstanceOfType(data, typeof(List<OrderDetail>));
		}

		[TestMethod, TestCategory("ApiController")]
		public void GetOrderTotals()
		{
			DateTime dateFrom, dateTo;
			dateFrom = dateTo = DateTime.Today;
			var searchText = "search";
			var customer = "c";
			dal.SetData(new DalMockData{OrderTotals = new OrderTotals
			{
				orderCount = 1
			}});
			var data = apiController.getOrderTotals(customer, dateFrom, dateTo, searchText);
			Assert.IsNotNull(data);
			Assert.AreEqual(customer, dal.Parameters.Customer);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateFrom);
			Assert.AreEqual(DateTime.Today, dal.Parameters.DateTo);
			Assert.AreEqual(searchText, dal.Parameters.SearchText);

			Assert.IsInstanceOfType(data, typeof(OrderTotals));
		}

	}
}
