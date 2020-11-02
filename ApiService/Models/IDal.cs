using System;
using System.Collections.Generic;
using C2.Model;

namespace ApiService.Models
{
	public interface IDal
	{
		StockData GetFreeStock(string productCode);
		List<StockData> GetStockData(IList<string> productCodes);
		ProductPrices GetPrice(string customer, string product);
		void GetCustomerTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText, out double? orderTotal, out double? balance, out double? creditLimit, out int? numOfInvoices);
		List<Invoice> GetInvoices(string customer, DateTime? dateFrom, DateTime? dateTo, int recFrom, int recTo, string orderByField, SortDirection direction, string searchText);
		Customer GetCustomer(string customer);
		Order GetOrderByCriteria(string order_no, string customer_code, bool getOutOfStockShipmentDates);
		List<Order> GetOrdersForInvoices(string order_nos);
		List<SalesData> GetSalesData(int monthFrom, int yearFrom, int monthTo, int yearTo);

		List<Order> GetOrders(string customer, DateTime? dateFrom, DateTime? dateTo, int recFrom,
			int recTo, string orderByField, SortDirection direction, string searchText);

		List<OrderDetail> GetOrderDetails(string order_no);
		OrderTotals GetOrderTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText);

		List<ClearanceStock> GetClearanceStock(int recFrom, int recTo,string orderByField, SortDirection direction,
			string product_code = null, string description = null, string range = null);

		int? GetClearanceStockCount(string product_code = null, string description = null, string range = null);
	}
}