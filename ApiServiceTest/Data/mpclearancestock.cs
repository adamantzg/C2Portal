using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace ApiServiceTest.Data
{
	public class mpclearancestock {

		public mpclearancestock(
			string price_list=" ",string product_code=" ",string long_description=null,string range=null,
			double? exvat_price=null,double price=0,double? freestock=null)
		{
			this.price_list=price_list;
			this.product_code=product_code;
			this.long_description=long_description;
			this.range=range;
			this.exvat_price=exvat_price;
			this.price=price;
			this.freestock=freestock;
		}

		[Required]
		[MaxLength(3)]
		public string price_list { get; set;}
 
		[Required]
		[MaxLength(20)]
		public string product_code { get; set;}
 
		[MaxLength(40)]
		public string long_description { get; set;}
 
		[MaxLength(20)]
		public string range { get; set;}
 
		public double? exvat_price { get; set;}
 
		[Required]
		public double price { get; set;}
 
		public double? freestock { get; set;}

		public static void Insert(mpclearancestock data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [dbo].[mpclearancestock](
				[price_list],[product_code],[long_description],[range],[exvat_price],[price],[freestock])
				VALUES (@price_list,@product_code,@long_description,@range,@exvat_price,@price,@freestock)";
			conn.Execute(sql, data);
		}

 
	}
}
