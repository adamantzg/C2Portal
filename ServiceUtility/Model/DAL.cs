using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using NLog;

namespace ServiceUtility.Model
{
	public class DAL
	{
		static Logger Nlog = LogManager.GetCurrentClassLogger();

		public static List<order_header> GetOrders(DateTime? invoice_from)
		{
			var result = new List<order_header>();
			
			using (var conn = new SqlConnection(Properties.Settings.Default.connString))
			{
				conn.Open();
				conn.Query<order_header, order_line, order_header>(@"SELECT scheme.opheadm.rowid, scheme.opheadm.order_no, scheme.opheadm.alpha, scheme.opheadm.date_entered, 
												scheme.opheadm.invoice_date, scheme.opdetm.rowid, scheme.opdetm.order_line_no, 
												scheme.opdetm.line_type, scheme.opdetm.warehouse, scheme.opdetm.product, scheme.opdetm.val, 
												scheme.opdetm.cost_of_sale, scheme.opservm.product_group_a
										   FROM scheme.opheadm INNER JOIN
												scheme.opdetm ON scheme.opheadm.order_no = scheme.opdetm.order_no LEFT OUTER JOIN
												scheme.opservm ON scheme.opdetm.product = scheme.opservm.product
										   WHERE scheme.opheadm.invoice_date >= @date
											ORDER BY scheme.opheadm.order_no, scheme.opdetm.order_line_no",
						  (h, l) =>
						  {
							  var header = result.FirstOrDefault(head => head.order_no == h.order_no);
							  if (header == null)
							  {
								  header = h;
								  result.Add(header);
								  header.Lines = new List<order_line>();
							  }
							  header.Lines.Add(l);
							  return header;
						  }, new { date = invoice_from }, splitOn: "rowid");
				return result;
			}
		}

		public static int BulkInsertSalesData(List<sku_sales_data> data)
		{
			var numOfErrors = 0;
			using (var conn = new SqlConnection(Properties.Settings.Default.connString))
			{
				conn.Open();
				var cmd = new SqlCommand("INSERT INTO dbo.sku_sales_data(rowid, line_type, price,cost) VALUES(@rowid, @line_type, @price, @cost)", conn);
				cmd.Parameters.AddWithValue("@rowid", 0);
				cmd.Parameters.AddWithValue("@line_type", "");
				cmd.Parameters.AddWithValue("@price", 0);
				cmd.Parameters.AddWithValue("@cost", 0);

				foreach(var d in data)
				{
					try
					{
						cmd.Parameters[0].Value = d.rowid;
						cmd.Parameters[1].Value = d.line_type;
						cmd.Parameters[2].Value = d.price;
						cmd.Parameters[3].Value = d.cost;
						cmd.ExecuteNonQuery();
					}
					catch (SqlException ex)
					{
						numOfErrors++;
						Nlog.Log(LogLevel.Error, $"Error inserting row for {d.rowid}. Text: {ex.Message}");
					}
				}

			}
			return numOfErrors;
		}
	}
}
