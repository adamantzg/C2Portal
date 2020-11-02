using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;


namespace C2.Model
{
	[ExcludeFromCodeCoverage]
	public class Customer
	{
		public string customer { get; set; }
		public string special_price { get; set; }
		public string cust_disc_code { get; set; }
		public double? order_balance { get; set; }
		public double? credit_limit { get; set; }
		public int? numOfInvoices { get; set; }
		public string invoice_customer { get; set; }
		public string name { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string address3 { get; set; }
		public string address4 { get; set; }
		public string town_city { get; set; }
		public string county { get; set; }
		public string address6 { get; set; }
		public string vat_type { get; set; }
		public string vat_reg_number { get; set; }
		public double? vat_percentage { get; set; }
        public string analysis_codes1 { get; set; }
    }

	[ExcludeFromCodeCoverage]
	public class ProductPrices
	{
		public double? basePrice;
		public double? customerPrice;
	}

	[ExcludeFromCodeCoverage]
	public class Product
	{
		public string product { get; set; }
		public string discount { get; set; }
		public double? exvat_price { get; set; }
		public string key_core_slow { get; set; }
		public double? free_stock { get; set; }
	}

	[ExcludeFromCodeCoverage]
	public class Invoice
	{
		public string customer { get; set; }
		public DateTime? dated { get; set; }
		public string kind { get; set; }
		public string reference { get; set; }
		public string item_no { get; set; }
		public DateTime? due_date { get; set; }
		public double? amount { get; set; }
		public string customer_order_num { get; set; }
		public string open_indicator { get; set; }
		public string postcode { get; set; }

	}

	public class Order
	{
		public string order_no { get; set; }
		public string customer_order_no { get; set; }
        public string customer { get; set; }

        public DateTime? date_entered { get; set; }
		public DateTime? date_required { get; set; }
		public DateTime? invoice_date { get; set; }
		public string invoice_no { get; set; }		
		public string invoice_customer { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string address3 { get; set; }
		public string address4 { get; set; }
		public string address5 { get; set; }
		public string address6 { get; set; }
		public string town_city { get; set; }
		public string county { get; set; }
        public string status { get; set; }
        public List<OrderAudit> audits { get;set;}
		public List<OrderDetail> details { get; set; }
        public Customer OrderCustomer { get; set; }
        public List<StockData> productStockData { get; set; }
		public bool? planned { get; set; }

		public string statusText
		{
			get
			{
				switch (status)
				{
					case "1":
					case "2":
					case "3":
					case "4":
					case "5":
						return "Received";
					case "6":
					case "7":
						return "In progress";
					case "8":
						return "Invoiced";
					case "9":
						return "Cancelled/Deleted";
					default:
						return "";
				}
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public class OrderDetail
	{
		public string product { get; set; }
		public string long_description { get; set; }
		public double? despatched_qty { get; set; }
		public double? list_price { get; set; }
		public double? discount { get; set; }
		public double? net_price { get; set; }
		public double? order_qty { get; set; }
		public double? val { get; set; }
		public string vat_code_new { get; set; }
		public double? vat_amount { get;set;}
	}

	[ExcludeFromCodeCoverage]
	public class OrderAudit
	{
		public string audit_key { get; set; }
		public string character_value { get; set; }
		public DateTime? audit_date { get; set; }
	}

	[ExcludeFromCodeCoverage]
	public class OrderTotals
	{
		public int orderCount { get; set; }
	}

	[ExcludeFromCodeCoverage]
	public class StockData
	{
		public string product { get; set; }
		public double? quantity { get; set; }
		public DateTime? ship_date { get; set; }
		public bool discontinued { get; set; }
		public bool madeToOrder { get; set; }
		public bool expiresThisYear { get; set; }
	}

	[ExcludeFromCodeCoverage]
	public class SalesData
	{
		public string sector { get; set; }
		public int month { get; set; }
		public int year { get; set; }
		public double? value { get; set; }
	}

	[ExcludeFromCodeCoverage]
	public class ClearanceStock
	{
		public string product_code { get; set; }
		public string long_description { get; set; }
		public string range { get; set; }
		public double? exvat_price { get; set; }
		public double? price { get; set; }
		public int? freestock { get; set; }

		public double? discount
		{
			get
			{
				if (exvat_price > 0)
					return (price - exvat_price) / exvat_price;
				return 0;
			}
		}
	}

	
}