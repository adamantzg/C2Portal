using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Dapper;
using C2.Model;


namespace ApiService.Models
{
    public class Dal : IDisposable, IDal
    {
	    //private string connString = Properties.Settings.Default.connString;

	    private SqlConnection conn = null;

		[ExcludeFromCodeCoverage]
	  //  public Dal()
	  //  {
			//CreateOpenConnection(Properties.Settings.Default.connString);
	  //  }

	    public Dal(IDbConnection conn)
	    {
			this.conn = (SqlConnection) conn;
	    }

	  //  private void CreateOpenConnection(string connString)
	  //  {
			//conn = new SqlConnection(connString);
			//conn.Open();
	  //  }

        public StockData GetFreeStock(string productCode)
        {
            var result = new StockData();
            
            var productRecord = conn.Query<Product>("SELECT * FROM dbo.mpcportalprodlist WITH (NOLOCK) WHERE product = @code", new { code = productCode }).FirstOrDefault();
            if (productRecord != null)
            {
                result.quantity = productRecord.free_stock;
                if (productRecord.key_core_slow?.Trim() == "MTO")
                    result.madeToOrder = true;
            }
	        result.quantity = productRecord?.free_stock; // conn.ExecuteScalar<double>("SELECT free_stock FROM dbo.mpcportalprodlist WHERE product = @code", new { code = productCode });
            var pinn_stock3_records =
	            conn.Query<string>(
		            "SELECT key_core_slow FROM dbo.PINN_stock3 WITH (NOLOCK) WHERE warehouse IN ('service', 'LV' , '00') AND product = @code ",
		            new {code = productCode}).ToList();
            /*result.discontinued = conn.ExecuteScalar<int?>("SELECT COUNT(*) FROM dbo.PINN_stock3 WHERE warehouse IN ('service', 'LV' , '00') AND key_core_slow LIKE 'X%' AND product = @code",
                new { code = productCode }) > 0;*/
            if (pinn_stock3_records.Any(r => r.StartsWith("X")))
            {
	            result.discontinued = CheckDiscontinued(pinn_stock3_records);
	            var thisYearDiscontinuation = "X" + (DateTime.Today.Year % 100);
	            result.expiresThisYear = pinn_stock3_records.All(x => x.StartsWith("X")) && pinn_stock3_records.Any(x => x.Trim() == thisYearDiscontinuation);
            }
            
            if (!result.madeToOrder)
            {
                result.madeToOrder = conn.ExecuteScalar<int?>("SELECT COUNT(product) FROM dbo.PINN_stock3 WITH (NOLOCK) WHERE warehouse IN ('service', 'LV' , '00') AND key_core_slow = 'MTO' AND product = @code",
                new { code = productCode }) > 0;
            }
            if (result.quantity <= 0)
            {
                //check ship date
                result.ship_date = GetStockData(new[] { productCode }).FirstOrDefault()?.ship_date;
            }
            
            return result;
        }

        public List<StockData> GetStockData(IList<string> productCodes)
        {
            
            return conn.Query<StockData>(
                  @"SELECT dbo.mpcportalprodlist.product, MIN(scheme.podetm.date_required) AS ship_date
							FROM scheme.poheadm WITH (NOLOCK) INNER JOIN scheme.podetm WITH (NOLOCK) ON scheme.podetm.order_no = scheme.poheadm.order_no 								
							RIGHT OUTER JOIN dbo.mpcportalprodlist WITH (NOLOCK) ON scheme.podetm.product = dbo.mpcportalprodlist.product
					WHERE       
					dbo.mpcportalprodlist.product IN @codes
					AND scheme.podetm.date_required > GETDATE() -1
					AND scheme.podetm.status = '' 
					AND scheme.poheadm.status NOT IN ('C','9')
					AND scheme.poheadm.order_no NOT LIKE 'S%'						
					AND (scheme.podetm.qty_ordered > 0) 
					AND (scheme.podetm.qty_received = 0) AND (scheme.podetm.warehouse IN ('LV', '00'))
					GROUP BY dbo.mpcportalprodlist.product 
					", new { codes = productCodes }).ToList();
            
        }

        public ProductPrices GetPrice(string customer, string product)
        {
            
            var objCust = conn.QueryFirstOrDefault("SELECT special_price, cust_disc_code FROM scheme.slcustm WITH (NOLOCK) WHERE customer = @customer", new { customer });
	        var result = new ProductPrices();
	        var objProd = conn.QueryFirstOrDefault<Product>("SELECT product, discount, exvat_price FROM dbo.[mpcportalprodlist] WITH (NOLOCK) WHERE product = @product",
		        new { product });
	        result.basePrice = objProd?.exvat_price;
            if (objCust != null)
            {
                if (!string.IsNullOrWhiteSpace(objCust.special_price))
                {
                    result.customerPrice = conn.ExecuteScalar<double?>("SELECT price FROM scheme.oplistm WITH (NOLOCK) WHERE price_list = @special_price AND product_code = @product",
                        new { objCust.special_price, product });
					if(result.customerPrice != null)
						return result;
                }
	            
                if (objProd != null)
                {
                    if (!string.IsNullOrWhiteSpace(objProd.discount))
                    {
                        var discount = conn.ExecuteScalar<double?>("SELECT discount1 FROM scheme.opdscntm WITH (NOLOCK) WHERE cust_disc_code = @cust_disc_code AND prod_disc_code = @prod_disc_code",
                            new { objCust.cust_disc_code, prod_disc_code = objProd.discount });
                        if (discount != null)
                        {
                            result.customerPrice = objProd.exvat_price * (100 + discount) / 100.0;
                            return result;
                        }
                    }

                    return result;
                }
            }
            
            return null;
        }

		public void GetCustomerTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText, out double? orderTotal, out double? balance, out double? creditLimit, out int? numOfInvoices)
		{
			
			var custRecord = conn.Query<Customer>("SELECT credit_limit FROM scheme.slcustm WITH (NOLOCK) WHERE customer = @customer", new { customer }).FirstOrDefault();
			creditLimit = custRecord?.credit_limit;

			var totalRecord = conn.Query<Customer>(@"SELECT customer as invoice_customer, SUM(value) AS order_balance
									FROM  dbo.mpcporderbalance WITH (NOLOCK) WHERE customer = @customer
									GROUP BY customer", new {customer}).FirstOrDefault();
			orderTotal = totalRecord?.order_balance;
			
			var dateCriteria = @" AND (@dateFrom IS NULL OR scheme.slitemm.dated >= @dateFrom)
						AND(@dateTo IS NULL OR scheme.slitemm.dated <= @dateTo)";
			
            balance = conn.ExecuteScalar<double?>($@"SELECT SUM(amount) as balance 
					FROM scheme.slitemm WITH (NOLOCK) 
					WHERE customer = @customer AND kind IN ('INV','CRN','CSH') 
					AND open_indicator = 'O'
					{dateCriteria}", new { customer, dateFrom, dateTo });
			if (string.IsNullOrEmpty(searchText))
				searchText = null;
			else
				searchText = "%" + searchText + "%";
            numOfInvoices = conn.ExecuteScalar<int?>($@"SELECT COUNT(*) 
				FROM scheme.slitemm WITH (NOLOCK) WHERE customer = @customer AND kind IN ('INV','CRN','CSH')
				{dateCriteria} {GetInvoiceSearchTextCriteriaSQL(searchText)}", new { customer, dateFrom, dateTo, searchText = searchText?.ToLower() });
			
        }

		public List<Invoice> GetInvoices(string customer, DateTime? dateFrom, DateTime? dateTo, int recFrom, int recTo, string orderByField, SortDirection direction, string searchText)
        {
            
            if (string.IsNullOrEmpty(orderByField))
                orderByField = "scheme.slitemm.dated";
            if (orderByField == "reference")
                orderByField = "refernce";
			if (string.IsNullOrEmpty(searchText))
				searchText = null;
			else
				searchText = "%" + searchText + "%";
        
            return conn.Query<Invoice>(String.Format($@"WITH results AS 
						(SELECT scheme.slitemm.dated,
								scheme.slitemm.kind,
								scheme.slitemm.refernce AS reference,
								scheme.slitemm.item_no,
								scheme.slitemm.due_date,
								scheme.slitemm.amount,
								scheme.slitemm.customer_order_num,
								scheme.slitemm.open_indicator, 
								scheme.opheadm.address6 as postcode,
								ROW_NUMBER() OVER (ORDER BY {orderByField} {direction.ToString()}) AS RowNum 
						FROM scheme.slitemm WITH (NOLOCK) LEFT OUTER JOIN scheme.opheadm WITH (NOLOCK) ON scheme.slitemm.refernce = scheme.opheadm.order_no
						WHERE scheme.slitemm.customer = @customer 
						AND scheme.slitemm.kind IN ('INV','CRN','CSH')
						AND (@dateFrom IS NULL OR scheme.slitemm.dated >= @dateFrom)
						AND (@dateTo IS NULL OR scheme.slitemm.dated <= @dateTo)
						{GetInvoiceSearchTextCriteriaSQL(searchText)}
						)							
						SELECT * FROM results WHERE RowNum BETWEEN @from AND @to"),
						new { customer, from = recFrom, to = recTo, dateFrom, dateTo, searchText = searchText?.ToLower() }).ToList();
            
        }

		private string GetInvoiceSearchTextCriteriaSQL(string searchText)
		{
			return string.IsNullOrEmpty(searchText) ? "" :
				" AND (@searchText IS NULL OR LOWER(scheme.slitemm.refernce) LIKE @searchText OR LOWER(scheme.slitemm.item_no) LIKE @searchText OR LOWER(scheme.slitemm.customer_order_num) LIKE @searchText)";
		}

        public Customer GetCustomer(string customer)
        {
            return conn.QueryFirstOrDefault<Customer>(@"
            SELECT  scheme.slcustm.customer, 
                    scheme.slcustm.alpha, 
                    scheme.slcustm.name, 
                    scheme.slcustm.analysis_codes1
            FROM scheme.slcustm WITH (NOLOCK) 
            WHERE (scheme.slcustm.customer = @customer)
            ", new { customer });

        }

        public Order GetOrderByCriteria(string order_no, string customer_code, bool getOutOfStockShipmentDates)
        {
            
            Order result = null;
			List<Order> orders = new List<Order>();
            if (string.IsNullOrEmpty(customer_code))
                customer_code = null;
            conn.Query<Order, OrderAudit, Order>(@" 
						SELECT  scheme.opheadm.order_no,
								scheme.opheadm.customer_order_no,
                                scheme.opheadm.customer,
                                scheme.opheadm.date_entered,
								scheme.opheadm.invoice_date,
								scheme.opheadm.address1,
								scheme.opheadm.address2,
								scheme.opheadm.address3,
								scheme.opheadm.address4,
								scheme.opheadm.address5,
								scheme.opheadm.address6,
								scheme.opheadm.county,
								scheme.opheadm.town_city,
								scheme.opheadm.status,                                    
								scheme.opaudm.audit_key,
								scheme.opaudm.audit_date, 
								scheme.opaudm.character_value
						FROM    scheme.opheadm WITH (NOLOCK) INNER JOIN
								scheme.opaudm WITH (NOLOCK) ON scheme.opheadm.order_no = scheme.opaudm.audit_key                                   
								where (scheme.opheadm.order_no = @order_no OR scheme.opheadm.customer_order_no = @order_no) 
								AND (@customer_code IS NULL OR scheme.opheadm.customer = @customer_code)
								ORDER BY scheme.opheadm.order_no",
                        (order, audit) =>
                        {
                            var or = orders.FirstOrDefault(o => o.order_no == order.order_no);
                            if (or == null)
                            {
                                orders.Add(order);
                                order.audits = new List<OrderAudit>();
                                or = order;
                            }
                            or.audits.Add(audit);
                            return or;
                        },
                        new { order_no, customer_code }, splitOn: "audit_key");
            if (orders.Count > 1)
            {
	            result = orders.FirstOrDefault(o => o.order_no.Trim() == order_no);
            }
            if (result == null)
	            result = orders.FirstOrDefault();
            if (result != null && getOutOfStockShipmentDates)
            {
                var outOfStockLines = conn.Query<OrderDetail>(
                    @"SELECT DISTINCT scheme.opdetm.product
					  FROM scheme.opdetm WITH (NOLOCK) INNER JOIN dbo.mpcportalprodlist WITH (NOLOCK) ON scheme.opdetm.product = dbo.mpcportalprodlist.product
					  WHERE scheme.opdetm.order_no = @order_no 
					  AND dbo.mpcportalprodlist.free_stock <= 0
					  AND LEFT(scheme.opdetm.warehouse,1) <> 'd'", new { order_no }).ToList();	//warehouse starts with d for cancelled lines
                result.productStockData = new List<StockData>();
                if (outOfStockLines.Count > 0)
                {
                    var stockData = GetStockData(outOfStockLines.Select(l => l.product).ToList());
                    result.productStockData = outOfStockLines.Select(l =>
                    new StockData { product = l.product, ship_date = stockData.FirstOrDefault(d => d.product == l.product)?.ship_date?.AddDays(1) }).ToList();	//add 1 day to ship_date
                }
            }

            return result;
        
        }

        public List<Order> GetOrdersForInvoices(string order_nos)
        {
           var orderList = order_nos.Split(',').ToList();
            List<Order> result = null;

            conn.Query<Order, OrderDetail, Customer, Order>(@" 
						SELECT  scheme.opheadm.order_no,
								scheme.opheadm.customer_order_no,
								scheme.opheadm.date_entered,
								scheme.opheadm.invoice_date,
								scheme.opheadm.invoice_no,
								scheme.opheadm.invoice_date,
								scheme.opheadm.invoice_customer,
								scheme.opheadm.address1,
								scheme.opheadm.address2,
								scheme.opheadm.address3,
								scheme.opheadm.address4,
								scheme.opheadm.address5,
								scheme.opheadm.address6,
								scheme.opheadm.county,
								scheme.opheadm.town_city,
								scheme.opdetm.product,
								scheme.opdetm.long_description,
								scheme.opdetm.despatched_qty,
								scheme.opdetm.list_price,
								scheme.opdetm.discount,
								scheme.opdetm.net_price,
								scheme.opdetm.val,
								scheme.opdetm.vat_code_new,
								scheme.opdetm.vat_amount,
								scheme.slcustm.customer,
								scheme.slcustm.name,
								scheme.slcustm.address1,
								scheme.slcustm.address2,
								scheme.slcustm.address3,
								scheme.slcustm.address4,
								scheme.slcustm.town_city,
								scheme.slcustm.county,
								scheme.slcustm.address6,
								scheme.slcustm.vat_type,
								scheme.slcustm.vat_reg_number,
								scheme.cevatm.percentage AS vat_percentage
						FROM    scheme.opheadm WITH (NOLOCK) INNER JOIN 
								scheme.opdetm WITH (NOLOCK) ON scheme.opheadm.order_no = scheme.opdetm.order_no INNER JOIN
								scheme.slcustm WITH (NOLOCK) ON scheme.opheadm.customer = scheme.slcustm.customer LEFT OUTER JOIN
								scheme.cevatm WITH (NOLOCK) ON scheme.slcustm.vat_type = scheme.cevatm.vat_code
								where scheme.opheadm.order_no IN @orderList AND COALESCE(scheme.opdetm.bundle_line_ref,'') = ''
						ORDER BY scheme.opheadm.order_no, scheme.opdetm.order_line_no",
                        (order, detail, orderCustomer) =>
                        {

                            if (result == null)
                                result = new List<Order>();
                            var o = result.FirstOrDefault(x => x.order_no == order.order_no);
                            if (o == null)
                            {
                                order.details = new List<OrderDetail>();
                                order.OrderCustomer = orderCustomer;
                                result.Add(order);
                                o = order;
                            }

                            o.details.Add(detail);
                            return o;
                        },
                        new { orderList }, splitOn: "product,customer").ToList();
            return result;
            
        }

        public List<SalesData> GetSalesData(int monthFrom, int yearFrom, int monthTo, int yearTo)
        {
            return conn.Query<SalesData>(@"SELECT mporderbookgbp.RTM as sector, mporderbookgbp.calmonth as month, mporderbookgbp.calyear as year, SUM(mporderbookgbp.valuegbp) as value
											FROM dbo.mporderbookgbp WITH (NOLOCK) 
											WHERE status < '9' AND calmonth >= @monthFrom AND calyear >= @yearFrom AND calmonth <= @monthTo AND calyear <= @yearTo
											GROUP BY RTM, calmonth, calyear
											ORDER BY RTM, calyear, calmonth", new { monthFrom, yearFrom, monthTo, yearTo }).ToList();
        
        }

	    public List<Order> GetOrders(string customer, DateTime? dateFrom, DateTime? dateTo, int recFrom,
		    int recTo, string orderByField, SortDirection direction, string searchText)
	    {
		    
		    if (string.IsNullOrEmpty(orderByField))
			    orderByField = "scheme.opheadm.date_entered";
		    
		    if (string.IsNullOrEmpty(searchText))
			    searchText = null;
		    else
			    searchText = "%" + searchText + "%";
		
		    return conn.Query<Order>(String.Format($@"WITH results AS 
						(SELECT scheme.opheadm.order_no,
								scheme.opheadm.date_entered,
								scheme.opheadm.date_required,
								scheme.opheadm.customer_order_no,
								scheme.opheadm.address1,scheme.opheadm.address2,scheme.opheadm.address3,scheme.opheadm.address4,scheme.opheadm.address5,scheme.opheadm.address6,
								scheme.opheadm.status,
								(SELECT MIN(CASE LEFT(scheme.opdetm.warehouse,1) WHEN 'd' THEN 1 ELSE 0 END) FROM scheme.opdetm WHERE scheme.opdetm.order_no = scheme.opheadm.order_no) planned,
								ROW_NUMBER() OVER (ORDER BY {orderByField} {direction.ToString()}) AS RowNum 
						FROM scheme.opheadm WITH (NOLOCK) 
						WHERE scheme.opheadm.customer = @customer 
						AND scheme.opheadm.status IN ('1','2','3','4','5','6','7','8')
						AND (@dateFrom IS NULL OR scheme.opheadm.date_entered >= @dateFrom)
						AND (@dateTo IS NULL OR scheme.opheadm.date_entered <= @dateTo)
						{GetOrderSearchTextCriteriaSql(searchText)}
						)							
						SELECT * FROM results WHERE RowNum BETWEEN @from AND @to"),
			    new { customer, from = recFrom, to = recTo, dateFrom, dateTo, searchText = searchText?.ToLower() }).ToList();
		    
	    }

	    private static string GetOrderSearchTextCriteriaSql(string searchText)
	    {
		    return string.IsNullOrEmpty(searchText) ? "" :
			    @" AND (@searchText IS NULL OR LOWER(scheme.opheadm.order_no) LIKE @searchText OR LOWER(scheme.opheadm.customer_order_no) LIKE @searchText)";
	    }

	    public List<OrderDetail> GetOrderDetails(string order_no)
	    {
		    return conn.Query<OrderDetail>(
			    @"SELECT product, order_qty, val, despatched_qty FROM scheme.opdetm WITH (NOLOCK) WHERE order_no = @order_no 
				AND order_qty > 0 AND LEN(LTRIM(RTRIM(COALESCE(product,'')))) > 0 ORDER BY order_line_no",
			    new {order_no}).ToList();
		    
	    }

	    public OrderTotals GetOrderTotals(string customer, DateTime? dateFrom, DateTime? dateTo, string searchText)
	    {
		        
		    if (string.IsNullOrEmpty(searchText))
			    searchText = null;
		    else
			    searchText = "%" + searchText + "%";
		
		    return conn.Query<OrderTotals>(String.Format($@"SELECT COUNT(*) AS orderCount
						FROM scheme.opheadm WITH (NOLOCK) 
						WHERE scheme.opheadm.customer = @customer 
						AND scheme.opheadm.status IN ('1','2','3','4','5','6','7','8')
						AND (@dateFrom IS NULL OR scheme.opheadm.date_entered >= @dateFrom)
						AND (@dateTo IS NULL OR scheme.opheadm.date_entered <= @dateTo)
						{GetOrderSearchTextCriteriaSql(searchText)}
						"),
			    new { customer, dateFrom, dateTo, searchText = searchText?.ToLower() }).FirstOrDefault();
	    }

	    public List<ClearanceStock> GetClearanceStock(int recFrom, int recTo,string orderByField, SortDirection direction,
		    string product_code = null, string description = null, string range = null)
	    {
		    return conn.Query<ClearanceStock>(
			    $@"WITH results AS 
						(SELECT [price_list],[product_code],[long_description],
								[range], exvat_price,[price],[freestock],
								ROW_NUMBER() OVER (ORDER BY {orderByField} {direction.ToString()}) AS RowNum
								FROM dbo.mpclearancestock WITH (NOLOCK) 
								WHERE
								(@product_code IS NULL OR product_code LIKE @product_code) AND
								(@description IS NULL OR long_description LIKE @description) AND
								(@range IS NULL OR range LIKE @range)
						)
						SELECT * FROM results WHERE RowNum BETWEEN @recFrom AND @recTo",
			    new
			    {
				    recFrom, 
				    recTo,
					product_code = !string.IsNullOrEmpty(product_code) ? $"%{product_code}%" : null,
				    description = !string.IsNullOrEmpty(description) ? $"%{description}%" : null,
				    range = !string.IsNullOrEmpty(range) ? $"%{range}%" : null
			    }
		    ).ToList();
	    }

	    public int? GetClearanceStockCount(string product_code = null, string description = null, string range = null)
	    {
		    return conn.ExecuteScalar<int?>(
			    @"SELECT COUNT(*)
					FROM dbo.mpclearancestock WITH (NOLOCK) 
					WHERE
					(@product_code IS NULL OR product_code LIKE @product_code) AND
					(@description IS NULL OR long_description LIKE @description) AND
					(@range IS NULL OR range LIKE @range)",
			    new
			    {
				    product_code = !string.IsNullOrEmpty(product_code) ? $"%{product_code}%" : null,
				    description = !string.IsNullOrEmpty(description) ? $"%{description}%" : null,
				    range = !string.IsNullOrEmpty(range) ? $"%{range}%" : null
			    });
	    }

	    public void Dispose()
		{
			if (conn != null)
			{
				if(conn.State == ConnectionState.Open)
					conn.Close();
			}
			conn = null;
		}

	    public bool CheckDiscontinued(IEnumerable<string> pinn_stock3_records)
	    {
		    var thisYear = DateTime.Today.Year % 100;
		    return pinn_stock3_records.All(x => x.StartsWith("X") && GetNumber(x.Trim().Substring(1)) < thisYear);
	    }

	    private int GetNumber(string s)
	    {
		    var parsed = int.TryParse(s, out int res);
		    return parsed ? res : 0;
	    }
	}

}