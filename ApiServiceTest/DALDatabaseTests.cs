using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using ApiService.Models;
using ApiServiceTest.Data;
using C2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;

namespace ApiServiceTest
{
	[TestClass]
	public class DALDatabaseTests
	{
		#region GetFreeStock
		[TestMethod, TestCategory("FreeStock")]
		public void ProductWithFreeStock10()
		{
			using (var data = new StockTablesData(new Mpcportalprodlist("WH", "Stock10", free_stock: 10)))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock("Stock10");
					Assert.IsNotNull(stockData);
					Assert.AreEqual(10, stockData.quantity);
					Assert.AreEqual(false, stockData.madeToOrder);
				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductWithKeyCoreSlowMTO()
		{
			var product = "KeyMTO";
			using (var data = new StockTablesData(new Mpcportalprodlist("WH", product, key_core_slow: "MTO", free_stock: 10)))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.AreEqual(true, stockData.madeToOrder);
				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductDiscontinuedBeforeThisYear()
		{
			var product = "DiscBefore";
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 10) },
				new List<PINNstock3> { new PINNstock3(warehouse:"LV", product:product, key_core_slow:"XX")}))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.AreEqual(true, stockData.discontinued);
					Assert.AreEqual(false, stockData.expiresThisYear);
				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductDiscontinuedThisYear()
		{
			var product = "DiscThisYear";
			var thisYearCode = "X" + DateTime.Now.ToString("yy");
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 10) },
				new List<PINNstock3> { new PINNstock3(warehouse:"LV", product:product, key_core_slow:"XX"), new PINNstock3(warehouse:"service", product:product, key_core_slow: thisYearCode)}))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.AreEqual(false, stockData.discontinued);
					Assert.AreEqual(true, stockData.expiresThisYear);
				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductDiscontinuedNextYear()
		{
			var product = "DiscNextYear";
			var nextYearCode = "X" + (DateTime.Now.Year+1 - 2000).ToString();
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 10) },
				new List<PINNstock3> { new PINNstock3(warehouse:"LV", product:product, key_core_slow:"XX"), 
					new PINNstock3(warehouse:"service", product:product, key_core_slow: nextYearCode)}))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.IsFalse(stockData.discontinued);
					Assert.IsFalse(stockData.expiresThisYear);
				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void CheckDiscontinuation()
		{
			using (var dal = GetDal())
			{
				var pinn3_stock = new[] {"XX", "X17", "AB"};
				Assert.IsFalse(dal.CheckDiscontinued(pinn3_stock));

				pinn3_stock = new[] {"XX", "X17"};
				Assert.IsTrue(dal.CheckDiscontinued(pinn3_stock));

				pinn3_stock = new[] {"XX", "X18   "};
				Assert.IsFalse(dal.CheckDiscontinued(pinn3_stock));

				pinn3_stock = new[] {"XX", "X19   "};
				Assert.IsFalse(dal.CheckDiscontinued(pinn3_stock));

			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductWithMTOInPinnStock()
		{
			var product = "MTOPinnStock";
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 10) },
				new List<PINNstock3> { new PINNstock3(warehouse:"LV", product:product, key_core_slow:"MTO"), new PINNstock3(warehouse:"service", product:product, key_core_slow: "K")}))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.AreEqual(true, stockData.madeToOrder);
					
				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductWithNoStockNoOrder()
		{
			var product = "NoStock";
			
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 0) }))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.IsNull(stockData.ship_date);

				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductWithNoStockNoOrderWithStatus()
		{
			var product = "NoStock";
			var order_no = "order";
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 0) },
				null,
				new List<poheadm> {new poheadm(status:"C", order_no: order_no)},
				new List<podetm> {new podetm(order_no, warehouse: "LV", product: product, ship_date: DateTime.Now)}))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.IsNull(stockData.ship_date);

				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductWithNoStockOrderNoStartsWithS()
		{
			var product = "NoStock";
			var order_no = "Sorder";
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 0) },
				null,
				new List<poheadm> {new poheadm(status:"O", order_no: order_no)},
				new List<podetm> {new podetm(order_no, warehouse: "LV", product: product, ship_date: DateTime.Now)}))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.IsNull(stockData.ship_date);

				}
			}
		}

		[TestMethod, TestCategory("FreeStock")]
		public void ProductWithNoStockHasShipDate()
		{
			var product = "NoStock";
			var order_no = "order";
			var date = DateTime.Today;
			using (var data = new StockTablesData(new List<Mpcportalprodlist> { new Mpcportalprodlist("WH", product, key_core_slow: "K", free_stock: 0) },
				null,
				new List<poheadm> {new poheadm(status:"O", order_no: order_no)},
				new List<podetm>
				{
					new podetm(order_no, qty_ordered: 2, warehouse: "LV", product: product, date_required: date.AddDays(2), status: ""),
					new podetm(order_no, qty_ordered: 4, warehouse: "00", product: product, date_required: date.AddDays(4), status: "")
				}))
			{
				using (var dal = GetDal())
				{
					var stockData = dal.GetFreeStock(product);
					Assert.IsNotNull(stockData);
					Assert.IsNotNull(stockData.ship_date);
					Assert.AreEqual(date.AddDays(2),stockData.ship_date);
				}
			}
		}


		#endregion

		#region GetPrice 

		[TestMethod, TestCategory("GetPrice")]
		public void GetPriceCustomerNoProduct()
		{
			var customerCode = "cust";
			var productCode = "product";

			using (new PriceTablesData(new List<slcustm> {new slcustm(customerCode)},
				new List<Mpcportalprodlist>{new Mpcportalprodlist(product: productCode, exvat_price: 80)}
				))
			{
				using (var dal = GetDal())
				{
					var pricesData = dal.GetPrice(customerCode, "XXX");
					Assert.IsNull(pricesData);
				}
			}
		}

		[TestMethod, TestCategory("GetPrice")]
		public void GetPriceCustomerHasSpecialPrice()
		{
			var customerCode = "cust";
			var productCode = "product";

			using (new PriceTablesData(new List<slcustm> {new slcustm(customerCode, special_price: "SP")},
				new List<Mpcportalprodlist>{new Mpcportalprodlist(product: productCode, exvat_price: 80)},
				new List<oplistm> {new oplistm(price_list: "SP", product_code:productCode, price: 70)}))
			{
				using (var dal = GetDal())
				{
					var pricesData = dal.GetPrice(customerCode, productCode);
					Assert.IsNotNull(pricesData);
					Assert.AreEqual(70, pricesData.customerPrice);
					Assert.AreEqual(80, pricesData.basePrice);
				}
			}
		}

		[TestMethod, TestCategory("GetPrice")]
		public void GetPriceCustomerHasSpecialPriceNoOpListRecord()
		{
			var customerCode = "cust";
			var productCode = "product";

			using (new PriceTablesData(new List<slcustm> {new slcustm(customerCode, special_price: "SP")},
				new List<Mpcportalprodlist>{new Mpcportalprodlist(product: productCode, exvat_price: 80)}
				))
			{
				using (var dal = GetDal())
				{
					var pricesData = dal.GetPrice(customerCode, productCode);
					Assert.IsNotNull(pricesData);
					Assert.IsNull(pricesData.customerPrice);
					Assert.AreEqual(80, pricesData.basePrice);
				}
			}
		}

		[TestMethod, TestCategory("GetPrice")]
		public void GetPriceCustomerHasDiscount()
		{
			var customerCode = "cust";
			var productCode = "product";

			using (new PriceTablesData(new List<slcustm> {new slcustm(customerCode, cust_disc_code: "CD")},
				new List<Mpcportalprodlist>{new Mpcportalprodlist(product: productCode, exvat_price: 100, discount: "PD")},
				null,
				new List<opdscntm>{new opdscntm(discount1: -40, cust_disc_code: "CD", prod_disc_code: "PD")}))
			{
				using (var dal = GetDal())
				{
					var pricesData = dal.GetPrice(customerCode, productCode);
					Assert.IsNotNull(pricesData);
					Assert.AreEqual(60, pricesData.customerPrice);
					Assert.AreEqual(100, pricesData.basePrice);
				}
			}
		}

		[TestMethod, TestCategory("GetPrice")]
		public void GetPriceCustomerHasDiscountNoOpDscRecord()
		{
			var customerCode = "cust";
			var productCode = "product";

			using (new PriceTablesData(new List<slcustm> {new slcustm(customerCode, cust_disc_code: "CD")},
				new List<Mpcportalprodlist>{new Mpcportalprodlist(product: productCode, exvat_price: 100, discount: "PD")}
				))
			{
				using (var dal = GetDal())
				{
					var pricesData = dal.GetPrice(customerCode, productCode);
					Assert.IsNotNull(pricesData);
					Assert.IsNull(pricesData.customerPrice);
					Assert.AreEqual(100, pricesData.basePrice);
				}
			}
		}

		[TestMethod, TestCategory("GetPrice")]
		public void GetPriceCustomerNoSpecialNoDiscount()
		{
			var customerCode = "cust";
			var productCode = "product";

			using (new PriceTablesData(new List<slcustm> {new slcustm(customerCode, cust_disc_code: "CD")},
				new List<Mpcportalprodlist>{new Mpcportalprodlist(product: productCode, exvat_price: 100, discount: "PD")}))
			{
				using (var dal = GetDal())
				{
					var pricesData = dal.GetPrice(customerCode, productCode);
					Assert.IsNotNull(pricesData);
					Assert.IsNull(pricesData.customerPrice);
					Assert.AreEqual(100, pricesData.basePrice);
				}
			}
		}

		[TestMethod, TestCategory("GetPrice")]
		public void GetPriceNoCustomer()
		{
			var customerCode = "cust";
			var productCode = "product";

			using (new PriceTablesData(new List<slcustm> {new slcustm("XXX")},
				new List<Mpcportalprodlist>{new Mpcportalprodlist(product: productCode, exvat_price: 80)}))
			{
				using (var dal = GetDal())
				{
					var pricesData = dal.GetPrice(customerCode, productCode);
					Assert.IsNull(pricesData);
				}
			}
		}

		#endregion

		#region CustomerTotals
		[TestMethod, TestCategory("CustomerTotals")]
		public void GetTotalsNoCriteria()
		{
			
			DateTime? dateFrom = null, dateTo = null;
			var searchText = string.Empty;
			var customerCode = "customer";
			var order_no = "order";
			var order_no_cn = "CN1";
			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode,credit_limit: 1000)},
				new List<slitemm>
				{
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100),
					new slitemm(customerCode, open_indicator: "O", kind: "CRN", amount: 100),
					new slitemm(customerCode, open_indicator: "O", kind: "CSH", amount: 100),
					new slitemm(customerCode, open_indicator: "C", kind: "CRN", amount: 100)
				},
				new List<opheadm>{new opheadm(order_no, customer: customerCode, status: "5"), new opheadm(order_no_cn, customer: customerCode, status: "4")},
				new List<opdetm>{ new opdetm(order_no, val:100), new opdetm(order_no, val: 300), new opdetm(order_no_cn, val: 50), new opdetm(order_no_cn, val:50)}
				))
			{
				using (var dal = GetDal())
				{
					dal.GetCustomerTotals(customerCode,dateFrom, dateTo, searchText, out double? orderTotal, out double? balance, out double? creditLimit, out int? numOfInvoices);
					Assert.AreEqual(1000, creditLimit);
					Assert.AreEqual(300, orderTotal); //100 + 300 - 2*50 (CN gets subtracted)
					Assert.AreEqual(300, balance); //100 + 100 + 100 (100 with open_indicator = C not counted)
					Assert.AreEqual(4, numOfInvoices);
				}
			}
		}

		[TestMethod, TestCategory("CustomerTotals")]
		public void GetTotalsSearchText()
		{
			
			DateTime? dateFrom = null, dateTo = null;
			var searchText = "cw";
			var customerCode = "customer";
			var order_no = "order";
			
			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode)},
				new List<slitemm>
				{
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100, refernce: "acwb"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100, item_no: "acwb"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100, customer_order_num: "acwb"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100)
				},
				new List<opheadm>{new opheadm(order_no, customer: customerCode, status: "5")},
				new List<opdetm>{ new opdetm(order_no, val:100), new opdetm(order_no, val: 300)}
			))
			{
				using (var dal = GetDal())
				{
					dal.GetCustomerTotals(customerCode,dateFrom, dateTo, searchText, out double? orderTotal, out double? balance, out double? creditLimit, out int? numOfInvoices);
					
					Assert.AreEqual(0, creditLimit);
					Assert.AreEqual(400, orderTotal); //100 + 300
					Assert.AreEqual(400, balance); //100 + 100 + 100 + 100
					Assert.AreEqual(3, numOfInvoices); //searchtext applied only here
				}
			}
		}

		[TestMethod, TestCategory("CustomerTotals")]
		public void GetTotalsDateCriteria()
		{
			
			DateTime? dateFrom = DateTime.Today, dateTo = DateTime.Today.AddDays(5);
			var searchText = string.Empty;
			var customerCode = "customer";
			var order_no = "order";

			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode)},
				new List<slitemm>
				{
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100, dated: dateFrom.Value.AddDays(1)),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100, dated: dateFrom.Value.AddDays(-1)),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100, dated: dateFrom.Value.AddDays(5)),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", amount: 100, dated: dateFrom.Value.AddDays(6))
				},
				new List<opheadm>{new opheadm(order_no, customer: customerCode, status: "5")},
				new List<opdetm>{ new opdetm(order_no, val:100), new opdetm(order_no, val: 300)}
			))
			{
				using (var dal = GetDal())
				{
					dal.GetCustomerTotals(customerCode,dateFrom, dateTo, searchText, out double? orderTotal, out double? balance, out double? creditLimit, out int? numOfInvoices);
					
					Assert.AreEqual(0, creditLimit);
					Assert.AreEqual(400, orderTotal); //100 + 300
					Assert.AreEqual(200, balance); //100 + 100 
					Assert.AreEqual(2, numOfInvoices); //searchtext applied only here
				}
			}
		}
		#endregion
		
		#region GetInvoices
		[TestMethod, TestCategory("GetInvoices")]
		public void GetInvoicesNoFilter()
		{
			
			DateTime? dateFrom = null, dateTo = null;
			int from = 1, to = 3;
			var searchText = string.Empty;
			var customerCode = "customer";
			var order_no = "order";
			string sortBy = "customer_order_num";
			SortDirection direction = SortDirection.Asc;

			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode)},
				new List<slitemm>
				{
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "A"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "B"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "C"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "D" )
				}
				
			))
			{
				using (var dal = GetDal())
				{
					var invoices = dal.GetInvoices(customerCode, dateFrom, dateTo, from, to, sortBy, direction, searchText);
					Assert.IsNotNull(invoices);
					Assert.AreEqual(3, invoices.Count);
					Assert.AreEqual("A", invoices[0].customer_order_num);
				}
			}
		}

		[TestMethod, TestCategory("GetInvoices")]
		public void GetInvoicesDefaultSort()
		{
			
			DateTime? dateFrom = null, dateTo = null;
			int from = 1, to = 3;
			var searchText = string.Empty;
			var customerCode = "customer";
			var order_no = "order";
			string sortBy = "";
			SortDirection direction = SortDirection.Asc;
			DateTime startDate = DateTime.Today;

			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode)},
				new List<slitemm>
				{
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "A", dated: startDate ),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "B", dated: startDate.AddDays(-1)),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "C", dated: startDate.AddDays(1)),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", customer_order_num: "D" , dated: startDate.AddDays(2))
				}
				
			))
			{
				using (var dal = GetDal())
				{
					var invoices = dal.GetInvoices(customerCode, dateFrom, dateTo, from, to, sortBy, direction, searchText);
					Assert.IsNotNull(invoices);
					Assert.AreEqual(3, invoices.Count);
					Assert.AreEqual("B", invoices[0].customer_order_num); //sorted by default by date
				}
			}
		}

		[TestMethod, TestCategory("GetInvoices")]
		public void GetInvoicesSortByReferenceDesc()
		{
			
			DateTime? dateFrom = null, dateTo = null;
			int from = 1, to = 3;
			var searchText = string.Empty;
			var customerCode = "customer";
			var order_no = "order";
			string sortBy = "reference";
			SortDirection direction = SortDirection.Desc;
			DateTime startDate = DateTime.Today;

			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode)},
				new List<slitemm>
				{
					new slitemm(customerCode, open_indicator: "O", kind: "INV", refernce: "A"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", refernce: "B"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", refernce: "C"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", refernce: "D")
				}
				
			))
			{
				using (var dal = GetDal())
				{
					var invoices = dal.GetInvoices(customerCode, dateFrom, dateTo, from, to, sortBy, direction, searchText);
					Assert.IsNotNull(invoices);
					Assert.AreEqual(3, invoices.Count);
					Assert.AreEqual("D", invoices[0].reference); //sorted by reference
				}
			}
		}

		[TestMethod, TestCategory("GetInvoices")]
		public void GetInvoicesDatesSearchText()
		{
			
			
			int from = 1, to = 10;
			var searchText = string.Empty;
			var customerCode = "customer";
			string sortBy = "";
			SortDirection direction = SortDirection.Asc;
			DateTime startDate = DateTime.Today;
			DateTime? dateFrom = startDate, dateTo = startDate.AddDays(3);

			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode)},
				new List<slitemm>
				{
					new slitemm(customerCode, open_indicator: "O", kind: "INV", dated: startDate, refernce: "AAA", customer_order_num: "BBB", item_no: "CCC"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", dated: startDate.AddHours(1), refernce: "BBB", customer_order_num: "AAA", item_no: "CCC"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", dated: startDate.AddHours(2), refernce: "BBB", customer_order_num: "BBB", item_no: "AAA"),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", dated: startDate.AddDays(2)),
					new slitemm(customerCode, open_indicator: "O", kind: "INV", dated: startDate.AddDays(-1)),
					new slitemm(customerCode, open_indicator: "O", kind: "XXX", dated: startDate.AddDays(1))
				}
				
			))
			{
				using (var dal = GetDal())
				{
					var invoices = dal.GetInvoices(customerCode, dateFrom, dateTo, from, to, sortBy, direction, searchText);
					Assert.IsNotNull(invoices);
					Assert.AreEqual(4, invoices.Count); //one filtered because of date, one because of kind
					Assert.AreEqual("AAA", invoices[0].reference); //sorted by date

					//searchtext
					searchText = "A";
					direction = SortDirection.Desc;
					invoices = dal.GetInvoices(customerCode, dateFrom, dateTo, from, to, sortBy, direction, searchText);
					Assert.IsNotNull(invoices);
					Assert.AreEqual(3, invoices.Count); //one filtered because of date, one because of kind, one because of search text
					Assert.AreEqual("BBB", invoices[0].reference); //sorted by date

				}
			}
		}

		[TestMethod, TestCategory("GetInvoices")]
		public void GetCustomer()
		{
			var customerCode = "customer";
			
			using (new CustomerTotalsData(new List<slcustm> {new slcustm(customerCode)}))
			{
				using (var dal = GetDal())
				{
					var customer = dal.GetCustomer(customerCode);
					Assert.IsNotNull(customer);
					customer = dal.GetCustomer("XXX");
					Assert.IsNull(customer);
				}
			}
		}

		#endregion

		#region Orders

		[TestMethod, TestCategory("GetOrderByCriteria")]
		public void GetOrderNoOutOfStockDates()
		{
			var customerCode = "customer";
			var order_no = "order";
			var order_no2 = "other";
			var auditDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
			using (new OrdersData(
					headers: new List<opheadm>
					{
						new opheadm(order_no, customer: customerCode), 
						new opheadm(order_no2, customer_order_no: order_no, customer: customerCode)
					},
					audits: new List<opaudm>
					{
						new opaudm(audit_key: order_no, audit_date: auditDate, character_value: "6" ), 
						new opaudm(audit_key: order_no),
						new opaudm(audit_key: order_no2)
					}
				))
			{
				using (var dal = GetDal())
				{
					var order = dal.GetOrderByCriteria(order_no, customerCode, false);
					Assert.IsNotNull(order);
					Assert.AreEqual(order_no, order.order_no);
					Assert.AreEqual(2, order.audits.Count);
					Assert.IsNotNull(order.audits.FirstOrDefault(a=>a.character_value == "6"));

					order = dal.GetOrderByCriteria(order_no2, customerCode, false);
					Assert.IsNotNull(order);
					Assert.AreEqual(order_no2, order.order_no);
					Assert.AreEqual(1, order.audits.Count);
				}
			}
		}

		[TestMethod, TestCategory("GetOrderByCriteria")]
		public void GetOrderNoAudits()
		{
			var customerCode = "customer";
			var order_no = "order";
			
			using (new OrdersData(
				headers: new List<opheadm>
				{
					new opheadm(order_no, customer: customerCode)
				}
			))
			{
				using (var dal = GetDal())
				{
					var order = dal.GetOrderByCriteria(order_no, customerCode, false);
					Assert.IsNull(order);
					
				}
			}
		}

		[TestMethod, TestCategory("GetOrderByCriteria")]
		public void GetOrderOutOfStockDates()
		{
			var customerCode = "customer";
			var order_no = "order";
			var order_no2 = "other";
			var productStock = "product1";
			var productNoStock = "product2";
			var date = DateTime.Today;
			using (new OrdersData(
				headers: new List<opheadm>
				{
					new opheadm(order_no, customer: customerCode)
				},
				audits: new List<opaudm>
				{
					new opaudm(audit_key: order_no)
				},
				details: new List<opdetm>
				{
					new opdetm(order_no, product: productStock),
					new opdetm(order_no, product: productNoStock)
				},
				poHeaders: new List<poheadm>
				{
					new poheadm("pohead1"),
					new poheadm("pohead2")
				},
				poDetails: new List<podetm>
				{
					new podetm("pohead1", status:"", product: productStock, date_required: date.AddDays(1), qty_ordered: 2, qty_received: 0, warehouse: "LV"),
					new podetm("pohead1", status: "", product: productNoStock, date_required: date.AddDays(2), qty_ordered: 2, qty_received: 0, warehouse: "00"),
					new podetm("pohead2", status: "", product: productNoStock, date_required: date.AddDays(1), qty_ordered: 2, qty_received: 0, warehouse: "LV")
				},
				mpcData: new List<Mpcportalprodlist>
				{
					new Mpcportalprodlist(product: productStock, free_stock: 10),
					new Mpcportalprodlist(product: productNoStock, free_stock: 0)
				}

			))
			{
				using (var dal = GetDal())
				{
					var order = dal.GetOrderByCriteria(order_no, customerCode, true);
					Assert.IsNotNull(order);
					Assert.AreEqual(order_no, order.order_no);
					Assert.AreEqual(1, order.audits.Count);
					Assert.IsNotNull(order.productStockData);
					Assert.AreEqual(1, order.productStockData.Count);

					//ship_date should be date +1 , but routine adds 1 day to ship_date from the table
					Assert.AreEqual(date.AddDays(2), order.productStockData.FirstOrDefault(p=>p.product == productNoStock)?.ship_date);

					//Check when no customer code
					order = dal.GetOrderByCriteria(order_no, string.Empty, true);
					Assert.IsNotNull(order);
					Assert.AreEqual(order_no, order.order_no);


				}
			}
		}

		

		[TestMethod, TestCategory("GetOrders")]
		public void GetOrdersForInvoices()
		{
			var customerCode = "cust";
			var customerCode2 = "cust2";
			var order_no = "order";
			var order_no2 = "order2";
			using (new OrdersData(
				headers: new List<opheadm>
				{
					new opheadm(order_no, customer: customerCode),
					new opheadm(order_no2, customer: customerCode2)
				},
				details: new List<opdetm>
				{
					new opdetm(order_no, product: "A"),
					new opdetm(order_no, product: "B"),
					new opdetm(order_no2, product: "A"),
					new opdetm(order_no2, product: "B", bundle_line_ref: "A")
				},
				cevatm: new List<cevatm>
				{
					new cevatm("A", percentage: 20)
				},
				customers: new List<slcustm>
				{
					new slcustm(customerCode, vat_type: "A"),
					new slcustm(customerCode2)
				}
			))
			{
				using (var dal = GetDal())
				{
					var orders = dal.GetOrdersForInvoices(string.Join(",", order_no, order_no2));
					Assert.IsNotNull(orders);
					Assert.AreEqual(2, orders.Count);
					Assert.AreEqual(2, orders.FirstOrDefault(o=>o.order_no == order_no)?.details.Count);
					Assert.AreEqual(1, orders.FirstOrDefault(o=>o.order_no == order_no2)?.details.Count);
					Assert.AreEqual(20, orders.FirstOrDefault(o=>o.order_no == order_no)?.OrderCustomer?.vat_percentage);
					Assert.IsNull(orders.FirstOrDefault(o=>o.order_no == order_no2)?.OrderCustomer?.vat_percentage);
					
				}
			}
		}

		[TestMethod, TestCategory("GetOrders")]
		public void GetOrderTotals()
		{
			var order_no = "order";
			var format = "yyyy-MM-dd";
			var date = DateTime.Today;
			var customer = "customer";
			
			using (new OrdersData(headers: new List<opheadm>
			{
				new opheadm(order_no, status: "1", date_entered: date.ToString(format), customer:customer, customer_order_no: "A"),
				new opheadm(order_no, status: "1", date_entered: date.AddDays(1).ToString(format), customer:customer, customer_order_no: "B"),
				new opheadm(order_no, status: "3", date_entered: date.AddDays(-1).ToString(format), customer:customer, customer_order_no: "C"),
				new opheadm(order_no, status: "C", date_entered: date.ToString(format), customer:customer, customer_order_no: "A"),
				new opheadm(order_no, status: "4", date_entered: date.AddDays(4).ToString(format), customer:customer, customer_order_no: "A"),
			}))
			{
				using (var dal = GetDal())
				{
					var totals = dal.GetOrderTotals(customer, null, null, string.Empty);
					Assert.IsNotNull(totals);
					Assert.AreEqual(4, totals.orderCount);
					totals = dal.GetOrderTotals(customer, date, date.AddDays(2), string.Empty);
					Assert.AreEqual(2, totals.orderCount);
					totals = dal.GetOrderTotals(customer, null, null, "A");
					Assert.AreEqual(2, totals.orderCount);

				}
			}
		}

		[TestMethod, TestCategory("GetOrders")]
		public void GetOrders()
		{
			
			var format = "yyyy-MM-dd";
			var date = DateTime.Today;
			var customer = "customer";
			
			using (new OrdersData(headers: new List<opheadm>
			{
				new opheadm("1", status: "1", date_entered: date.ToString(format), customer:customer, customer_order_no: "A"),
				new opheadm("2", status: "1", date_entered: date.AddDays(1).ToString(format), customer:customer, customer_order_no: "B")
			}, details: new List<opdetm>
			{
				new opdetm("1", warehouse: "LV", val: 100),
				new opdetm("1", warehouse: "d1", val: 20),
				new opdetm("2", warehouse: "d1", val: 20),
				new opdetm("2", warehouse:"d2", val: 30)
			}))
			{
				using (var dal = GetDal())
				{
					var orders = dal.GetOrders(customer, null, null, 1, 5, "order_no", SortDirection.Asc, "" );
					Assert.IsNotNull(orders);
					Assert.AreEqual(2, orders.Count);
					Assert.AreEqual(false, orders[0].planned);
					Assert.AreEqual(true, orders[1].planned);

					orders = dal.GetOrders(customer, date.AddDays(1), null, 1, 5, "", SortDirection.Asc, "" );
					Assert.IsNotNull(orders);
					Assert.AreEqual(1, orders.Count);

					orders = dal.GetOrders(customer, null, null, 1, 5, "", SortDirection.Asc, "B" );
					Assert.IsNotNull(orders);
					Assert.AreEqual(1, orders.Count);

				}
			}
		}

		[TestMethod, TestCategory("GetOrder")]
		public void GetOrderDetails()
		{
			var order_no = "order";
			
			using (new OrdersData(headers: new List<opheadm>
			{
				new opheadm(order_no)
			},
			details: new List<opdetm>
			{
				new opdetm(order_no, product: "prod1", order_qty: 0),
				new opdetm(order_no, product: "prod2", order_qty: 5, order_line_no: "2"),
				new opdetm(order_no, product: "prod3", order_qty: 3, order_line_no: "1"),
				new opdetm(order_no, product: "  ", order_qty: 2)
			}))
			{
				using (var dal = GetDal())
				{
					var details = dal.GetOrderDetails(order_no);
					Assert.IsNotNull(details);
					Assert.AreEqual(2, details.Count); // 1 filtered out because qty = 0, 1 because product code empty
					Assert.AreEqual(3, details[0].order_qty);	//sort by order_line_no
				}
			}	
		}
		#endregion

		#region SalesData

		[TestMethod, TestCategory("SalesData")]
		public void GetSalesData()
		{
			var date = DateTime.Today;
			using (new SalesDataTables(mpoData: new List<mporderbookgbp>
				{
					new mporderbookgbp(status: "6", RTM: "1",calmonth: 5, calyear: 2018, valuegbp: 100),
					new mporderbookgbp(status: "9", RTM: "1",calmonth: 6, calyear: 2018, valuegbp: 100),
					new mporderbookgbp(status: "6", RTM: "1",calmonth: 6, calyear: 2018, valuegbp: 100),
					new mporderbookgbp(status: "6", RTM: "1",calmonth: 6, calyear: 2018, valuegbp: 100),
					new mporderbookgbp(status: "6", RTM: "1",calmonth: 7, calyear: 2018, valuegbp: 100),
					new mporderbookgbp(status: "6", RTM: "2",calmonth: 6, calyear: 2018, valuegbp: 100),
					new mporderbookgbp(status: "6", RTM: "2",calmonth: 7, calyear: 2018, valuegbp: 100),
					new mporderbookgbp(status: "6", RTM: "1",calmonth: 9, calyear: 2018, valuegbp: 100)
				}))
			{
				using (var dal = GetDal())
				{
					var data = dal.GetSalesData(6, 2018, 8, 2018);
					Assert.IsNotNull(data);
					Assert.AreEqual(4, data.Count);	//sector 1 6,7 sector 2 6,7 (grouped by sector - RTM)
					Assert.AreEqual(200, data.FirstOrDefault(d=>d.month == 6 && d.sector == "1")?.value);
				}
			}	
		}
#endregion

		[TestMethod, TestCategory("Clearance")]
		public void ClearanceStockTest()
		{
			using (new ClearanceStockData(new List<mpclearancestock>
			{
				new mpclearancestock(product_code: "codea", long_description: "desc_aaa", range: "range_aaa", freestock:0),
				new mpclearancestock(product_code: "codeb", long_description: "desc_bbb", range: "range_bbb", freestock:0),
				new mpclearancestock(product_code: "codec", long_description: "desc_ccc", range: "range_ccc", freestock:0)
			}))
			{
				using (var dal = GetDal())
				{
					var data = dal.GetClearanceStock(1, 50, "product_code",SortDirection.Asc, "code");
					Assert.IsNotNull(data);
					Assert.AreEqual(3, data.Count);

					data = dal.GetClearanceStock(1, 50, "product_code",SortDirection.Asc, description: "aa");
					Assert.AreEqual(1, data.Count);

					data = dal.GetClearanceStock(1, 50, "product_code",SortDirection.Asc, range: "bb");
					Assert.AreEqual(1, data.Count);
				}
			}
		}

		private Dal GetDal()
		{
			return new Dal(new SqlConnection(Properties.Settings.Default.connString));
		}
	}
}
