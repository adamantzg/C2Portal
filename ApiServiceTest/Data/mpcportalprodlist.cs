using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class Mpcportalprodlist {

		public Mpcportalprodlist(string warehouse=" ",string product=" ",string description=" ",string long_description=" ",string supersession=" ",
			string discount=" ",double exvat_price=0,string remap_anal=null,string prod_range=" ",string key_core_slow=null,double? free_stock=null)
		{
			this.warehouse=warehouse;
			this.product=product;
			this.description=description;
			this.long_description=long_description;
			this.supersession=supersession;
			this.discount=discount;
			this.exvat_price=exvat_price;
			this.remap_anal=remap_anal;
			this.prod_range=prod_range;
			this.key_core_slow=key_core_slow;
			this.free_stock=free_stock;
		}

		[Required]
		public string warehouse { get; set;}
 
		[Required]
		[MaxLength(20)]
		public string product { get; set;}
 
		[Required]
		[MaxLength(20)]
		public string description { get; set;}
 
		[Required]
		[MaxLength(40)]
		public string long_description { get; set;}
 
		[Required]
		public string supersession { get; set;}
 
		[Required]
		[MaxLength(8)]
		public string discount { get; set;}
 
		[Required]
		public double exvat_price { get; set;}
 
		public string remap_anal { get; set;}
 
		[Required]
		[MaxLength(10)]
		public string prod_range { get; set;}
 
		[MaxLength(10)]
		public string key_core_slow { get; set;}
 
		public double? free_stock { get; set;}

		public static void Insert(Mpcportalprodlist data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [dbo].[mpcportalprodlist](
						warehouse,product,description,long_description,supersession,discount,exvat_price,remap_anal,prod_range,key_core_slow,free_stock)
				VALUES (@warehouse,@product,@description,@long_description,@supersession,@discount,@exvat_price,@remap_anal,@prod_range,@key_core_slow,@free_stock)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@warehouse",data.warehouse);
				cmd.Parameters.AddWithValue("@product",data.product);
				cmd.Parameters.AddWithValue("@description",data.description);
				cmd.Parameters.AddWithValue("@long_description",data.long_description);
				cmd.Parameters.AddWithValue("@supersession",data.supersession);
				cmd.Parameters.AddWithValue("@discount",data.discount);
				cmd.Parameters.AddWithValue("@exvat_price",data.exvat_price);
				cmd.Parameters.AddWithValue("@remap_anal",(object) data.remap_anal ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@prod_range",data.prod_range);
				cmd.Parameters.AddWithValue("@key_core_slow",(object) data.key_core_slow ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@free_stock",(object) data.free_stock ?? DBNull.Value);
				cmd.ExecuteNonQuery();
			}
			
		}
 
	}
}
