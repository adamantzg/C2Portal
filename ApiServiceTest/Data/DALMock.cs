using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiService.Models;
using C2.Model;

namespace ApiServiceTest
{
	public class CustomerTotal
	{
		public double? orderTotal { get; set; }
		public double? balance { get; set; }
		public double? creditLimit { get; set; }
		public int? numOfInvoices { get; set; }
	}

	public class Parameters
	{
		public string Customer;
		public DateTime? DateFrom;
		public DateTime? DateTo;
		public string SearchText;
		public int RecFrom;
		public int RecTo;
		public string OrderByField;
		public SortDirection Direction;
		public string OrderNo;
		public bool GetOutOfStockShipmentDates;
		public string Product;
		public int MonthFrom;
		public int YearFrom;
		public int MonthTo;
		public int YearTo;
		public IList<string> ProductCodes;
	}

	public class DalMockData
	{
		public StockData StockData;
		public Dictionary<string, CustomerTotal> DictCustomerTotals = new Dictionary<string, CustomerTotal>();
		public List<Invoice> Invoices = new List<Invoice>();
		public Order Order;
		public List<OrderDetail> OrderDetails;
		public List<Order> Orders;
		public OrderTotals OrderTotals;
		public ProductPrices ProductPrices;
		public List<SalesData> SalesData;
		public List<StockData> StockDataList;
		public Customer Customer;
	}

	public class DALMock : IDal
	{
		
		public DALMock(DalMockData data = null)
		{
			Parameters = new Parameters();
			Data = data ?? new DalMockData();
		}

		public Customer GetCustomer(string customer)
		{
			Parameters.Customer = customer;
			return Data.Customer;
		}

		public Parameters Parameters { get; }
		public DalMockData Data { get; private set; }

		public void GetCustomerTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText, out double? orderTotal, out double? balance, out double? creditLimit, out int? numOfInvoices)
		{
			Parameters.Customer = customer;
			Parameters.DateFrom = dateFrom;
			Parameters.DateTo = dateTo;
			Parameters.SearchText = searchText;
			if (Data.DictCustomerTotals.ContainsKey(customer))
			{
				var customerTotal = Data.DictCustomerTotals[customer];
				orderTotal = customerTotal.orderTotal;
				balance = customerTotal.balance;
				creditLimit = customerTotal.creditLimit;
				numOfInvoices = customerTotal.numOfInvoices;
				return;
			}
			orderTotal = balance = creditLimit = 0;
			numOfInvoices = 0;
		}

		public StockData GetFreeStock(string productCode)
		{
			Parameters.Product = productCode;
			return Data.StockData;
		}

		public List<Invoice> GetInvoices(string customer, DateTime? dateFrom, DateTime? dateTo, int recFrom, int recTo, string orderByField, SortDirection direction, string searchText)
		{
			Parameters.Customer = customer;
			Parameters.DateFrom = dateFrom;
			Parameters.DateTo = dateTo;
			Parameters.SearchText = searchText;
			Parameters.RecFrom = recFrom;
			Parameters.RecTo = recTo;
			Parameters.OrderByField = orderByField;
			Parameters.Direction = direction;
			return Data.Invoices;
		}

		public Order GetOrderByCriteria(string order_no, string customer_code, bool getOutOfStockShipmentDates)
		{
			Parameters.Customer = customer_code;
			Parameters.OrderNo = order_no;
			Parameters.GetOutOfStockShipmentDates = getOutOfStockShipmentDates;
			return Data.Order;
		}

		public List<OrderDetail> GetOrderDetails(string order_no)
		{
			Parameters.OrderNo = order_no;
			return Data.OrderDetails;
		}

		public List<Order> GetOrders(string customer, DateTime? dateFrom, DateTime? dateTo, int recFrom, int recTo, string orderByField, SortDirection direction, string searchText)
		{
			Parameters.Customer = customer;
			Parameters.DateFrom = dateFrom;
			Parameters.DateTo = dateTo;
			Parameters.SearchText = searchText;
			Parameters.RecFrom = recFrom;
			Parameters.RecTo = recTo;
			Parameters.OrderByField = orderByField;
			Parameters.Direction = direction;
			return Data.Orders;
		}

		public List<Order> GetOrdersForInvoices(string order_nos)
		{
			Parameters.OrderNo = order_nos;
			return Data.Orders;
		}

		public OrderTotals GetOrderTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText)
		{
			Parameters.Customer = customer;
			Parameters.DateFrom = dateFrom;
			Parameters.DateTo = dateTo;
			Parameters.SearchText = searchText;
			return Data.OrderTotals;
		}

		public ProductPrices GetPrice(string customer, string product)
		{
			Parameters.Customer = customer;
			Parameters.Product = product;
			return Data.ProductPrices;
		}

		public List<SalesData> GetSalesData(int monthFrom, int yearFrom, int monthTo, int yearTo)
		{
			Parameters.MonthFrom = monthFrom;
			Parameters.YearFrom = yearFrom;
			Parameters.MonthTo = monthTo;
			Parameters.YearTo = yearTo;
			return Data.SalesData;
		}

		public List<StockData> GetStockData(IList<string> productCodes)
		{
			Parameters.ProductCodes = productCodes;
			return Data.StockDataList;
		}

		public void SetData(DalMockData data)
		{
			this.Data = data;
		}

		public void SetCustomerTotals(string customer, double? orderTotal = null, double? balance = null, double? creditLimit = null, int? numOfInvoices = null)
		{
			var customerTotal = new CustomerTotal();
			customerTotal.orderTotal = orderTotal;
			customerTotal.balance = balance;
			customerTotal.creditLimit = creditLimit;
			customerTotal.numOfInvoices = numOfInvoices;
			Data.DictCustomerTotals[customer] = customerTotal;
		}

		public void SetInvoicesData(List<Invoice> data)
		{
			Data.Invoices = data;
		}

		public void SetOrderData(Order o)
		{
			Data.Order = o;
		}

		public List<ClearanceStock> GetClearanceStock(int recFrom, int recTo, string orderByField, SortDirection direction, string product_code = null, string description = null, string range = null)
		{
			throw new NotImplementedException();
		}

		public int? GetClearanceStockCount(string product_code = null, string description = null, string range = null)
		{
			throw new NotImplementedException();
		}
	}
}
