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
	public class opdscntm
	{
		public opdscntm(string cust_disc_code=" ",string slash1=" ",string prod_disc_code=" ",string slash2=" ",string price_list=" ",string discount_type=" ",
			string description=" ",double quantity_break1=0,double quantity_break2=0,double quantity_break3=0,double quantity_break4=0,double quantity_break5=0,
			double quantity_break6=0,double quantity_break7=0,double discount1=0,double discount2=0,double discount3=0,double discount4=0,double discount5=0,double discount6=0,
			double discount7=0,string unit_code=" ",string unit_group=" ",string vat_inclusive_flag=" ",byte[] rowstamp=null,int rowid=0)

		{
			this.cust_disc_code=cust_disc_code;
			this.slash1=slash1;
			this.prod_disc_code=prod_disc_code;
			this.slash2=slash2;
			this.price_list=price_list;
			this.discount_type=discount_type;
			this.description=description;
			this.quantity_break1=quantity_break1;
			this.quantity_break2=quantity_break2;
			this.quantity_break3=quantity_break3;
			this.quantity_break4=quantity_break4;
			this.quantity_break5=quantity_break5;
			this.quantity_break6=quantity_break6;
			this.quantity_break7=quantity_break7;
			this.discount1=discount1;
			this.discount2=discount2;
			this.discount3=discount3;
			this.discount4=discount4;
			this.discount5=discount5;
			this.discount6=discount6;
			this.discount7=discount7;
			this.unit_code=unit_code;
			this.unit_group=unit_group;
			this.vat_inclusive_flag=vat_inclusive_flag;
			this.rowstamp=rowstamp;
			this.rowid=rowid;
		}

		[Required] [MaxLength(4)] public string cust_disc_code { get; set; }

		[Required] [MaxLength(1)] public string slash1 { get; set; }

		[Required] [MaxLength(8)] public string prod_disc_code { get; set; }

		[Required] [MaxLength(1)] public string slash2 { get; set; }

		[Required] [MaxLength(3)] public string price_list { get; set; }

		[Required] [MaxLength(1)] public string discount_type { get; set; }

		[Required] [MaxLength(20)] public string description { get; set; }

		[Required] public double quantity_break1 { get; set; }

		[Required] public double quantity_break2 { get; set; }

		[Required] public double quantity_break3 { get; set; }

		[Required] public double quantity_break4 { get; set; }

		[Required] public double quantity_break5 { get; set; }

		[Required] public double quantity_break6 { get; set; }

		[Required] public double quantity_break7 { get; set; }

		[Required] public double discount1 { get; set; }

		[Required] public double discount2 { get; set; }

		[Required] public double discount3 { get; set; }

		[Required] public double discount4 { get; set; }

		[Required] public double discount5 { get; set; }

		[Required] public double discount6 { get; set; }

		[Required] public double discount7 { get; set; }

		[Required] [MaxLength(6)] public string unit_code { get; set; }

		[Required] [MaxLength(1)] public string unit_group { get; set; }

		[Required] [MaxLength(1)] public string vat_inclusive_flag { get; set; }

		public byte[] rowstamp { get; set; }

		[Required] public int rowid { get; set; }

		public static void Insert(opdscntm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[opdscntm](
			[cust_disc_code],[slash1],[prod_disc_code],[slash2],[price_list],[discount_type],[description],[quantity_break1],[quantity_break2],[quantity_break3],[quantity_break4],[quantity_break5],[quantity_break6],[quantity_break7],[discount1],[discount2],[discount3],[discount4],[discount5],[discount6],[discount7],[unit_code],[unit_group],[vat_inclusive_flag],[rowstamp],[rowid])
			 VALUES (@cust_disc_code,@slash1,@prod_disc_code,@slash2,@price_list,@discount_type,@description,@quantity_break1,@quantity_break2,@quantity_break3,@quantity_break4,@quantity_break5,@quantity_break6,@quantity_break7,@discount1,@discount2,@discount3,@discount4,@discount5,@discount6,@discount7,@unit_code,@unit_group,@vat_inclusive_flag,@rowstamp,@rowid)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@cust_disc_code", data.cust_disc_code);
				cmd.Parameters.AddWithValue("@slash1", data.slash1);
				cmd.Parameters.AddWithValue("@prod_disc_code", data.prod_disc_code);
				cmd.Parameters.AddWithValue("@slash2", data.slash2);
				cmd.Parameters.AddWithValue("@price_list", data.price_list);
				cmd.Parameters.AddWithValue("@discount_type", data.discount_type);
				cmd.Parameters.AddWithValue("@description", data.description);
				cmd.Parameters.AddWithValue("@quantity_break1", data.quantity_break1);
				cmd.Parameters.AddWithValue("@quantity_break2", data.quantity_break2);
				cmd.Parameters.AddWithValue("@quantity_break3", data.quantity_break3);
				cmd.Parameters.AddWithValue("@quantity_break4", data.quantity_break4);
				cmd.Parameters.AddWithValue("@quantity_break5", data.quantity_break5);
				cmd.Parameters.AddWithValue("@quantity_break6", data.quantity_break6);
				cmd.Parameters.AddWithValue("@quantity_break7", data.quantity_break7);
				cmd.Parameters.AddWithValue("@discount1", data.discount1);
				cmd.Parameters.AddWithValue("@discount2", data.discount2);
				cmd.Parameters.AddWithValue("@discount3", data.discount3);
				cmd.Parameters.AddWithValue("@discount4", data.discount4);
				cmd.Parameters.AddWithValue("@discount5", data.discount5);
				cmd.Parameters.AddWithValue("@discount6", data.discount6);
				cmd.Parameters.AddWithValue("@discount7", data.discount7);
				cmd.Parameters.AddWithValue("@unit_code", data.unit_code);
				cmd.Parameters.AddWithValue("@unit_group", data.unit_group);
				cmd.Parameters.AddWithValue("@vat_inclusive_flag", data.vat_inclusive_flag);
				cmd.Parameters.AddWithValue("@rowstamp", (object) data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.ExecuteNonQuery();
			}
		}

	}
}
