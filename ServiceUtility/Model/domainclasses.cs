using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceUtility.Model
{
	public class order_header
	{
		public int rowid { get; set; }
		public DateTime? date_entered { get; set; }
		public DateTime? invoice_date { get; set; }
		public string alpha { get; set; }
		public string order_no { get; set; }

		public List<order_line> Lines { get; set; }
	}

	public class order_line
	{
		public int rowid { get; set; }
		public string order_no { get; set; }
		public string order_line_no { get; set; }
		public string line_type { get; set; }
		public string warehouse { get; set; }
		public string product { get; set; }
		public double? order_qty { get; set; }
		public double? val { get; set; }
		public double? cost_of_sale { get; set; }

		public string product_group_a { get; set; }

		public order_header Header { get; set; }
	}

	public class sku_sales_data
	{
		public int rowid { get; set; }
		public string line_type { get; set; }
		public double? price { get; set; }
		public double? cost { get; set; }
	}
}
