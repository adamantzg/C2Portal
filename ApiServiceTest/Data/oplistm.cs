using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class oplistm
	{
		public oplistm(string price_list = " ", string product_code = " ", string sequence_number = " ",
			string customer_code = " ", string currency_code = " ", string description = " ",
			double price = 0, double new_price = 0, string unit_code = " ", string unit_code_group = " ",
			string vat_inclusive_flag = " ", double unit_qty_per_price = 0,
			DateTime? price_start_date = null, DateTime? price_end_date = null, byte[] rowstamp = null, int rowid = 0)
		{
			this.price_list = price_list;
			this.product_code = product_code;
			this.sequence_number = sequence_number;
			this.customer_code = customer_code;
			this.currency_code = currency_code;
			this.description = description;
			this.price = price;
			this.new_price = new_price;
			this.unit_code = unit_code;
			this.unit_code_group = unit_code_group;
			this.vat_inclusive_flag = vat_inclusive_flag;
			this.unit_qty_per_price = unit_qty_per_price;
			this.price_start_date = price_start_date;
			this.price_end_date = price_end_date;
			this.rowstamp = rowstamp;
			this.rowid = rowid;
		}

		[Required] [MaxLength(3)] public string price_list { get; set; }

		[Required] [MaxLength(20)] public string product_code { get; set; }

		[Required] [MaxLength(2)] public string sequence_number { get; set; }

		[Required] [MaxLength(8)] public string customer_code { get; set; }

		[Required] [MaxLength(3)] public string currency_code { get; set; }

		[Required] [MaxLength(20)] public string description { get; set; }

		[Required] public double price { get; set; }

		[Required] public double new_price { get; set; }

		[Required] [MaxLength(6)] public string unit_code { get; set; }

		[Required] [MaxLength(1)] public string unit_code_group { get; set; }

		[Required] [MaxLength(1)] public string vat_inclusive_flag { get; set; }

		[Required] public double unit_qty_per_price { get; set; }

		public DateTime? price_start_date { get; set; }

		public DateTime? price_end_date { get; set; }

		public byte[] rowstamp { get; set; }

		[Required] public int rowid { get; set; }

		public static void Insert(oplistm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[oplistm](
				[price_list],[product_code],[sequence_number],[customer_code],[currency_code],[description],[price],[new_price],[unit_code],[unit_code_group],[vat_inclusive_flag],[unit_qty_per_price],[price_start_date],[price_end_date],[rowstamp],[rowid])
				 VALUES (@price_list,@product_code,@sequence_number,@customer_code,@currency_code,@description,@price,@new_price,@unit_code,@unit_code_group,@vat_inclusive_flag,@unit_qty_per_price,@price_start_date,@price_end_date,@rowstamp,@rowid)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@price_list", data.price_list);
				cmd.Parameters.AddWithValue("@product_code", data.product_code);
				cmd.Parameters.AddWithValue("@sequence_number", data.sequence_number);
				cmd.Parameters.AddWithValue("@customer_code", data.customer_code);
				cmd.Parameters.AddWithValue("@currency_code", data.currency_code);
				cmd.Parameters.AddWithValue("@description", data.description);
				cmd.Parameters.AddWithValue("@price", data.price);
				cmd.Parameters.AddWithValue("@new_price", data.new_price);
				cmd.Parameters.AddWithValue("@unit_code", data.unit_code);
				cmd.Parameters.AddWithValue("@unit_code_group", data.unit_code_group);
				cmd.Parameters.AddWithValue("@vat_inclusive_flag", data.vat_inclusive_flag);
				cmd.Parameters.AddWithValue("@unit_qty_per_price", data.unit_qty_per_price);
				cmd.Parameters.AddWithValue("@price_start_date", (object) data.price_start_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@price_end_date", (object) data.price_end_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@rowstamp", (object) data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.ExecuteNonQuery();
			}
		}
	}
}
