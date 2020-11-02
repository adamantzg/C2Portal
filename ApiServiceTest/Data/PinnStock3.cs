using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class PINNstock3 {

		public PINNstock3(string origin=" ",string warehouse=" ",string product=" ",string long_description=" ",double exvat_price=0,string Disc_Code=" ",string orig_analysis=" ",string remap_anal=" ",string prod_range=" ",string key_core_slow=" ",string abc_category=" ",string LCM_xref=" ",string salekey=" ",string Supplier=" ",
			string SuppCurr=" ",string AdminType=" ",string StockType=" ",double threemonthsales=0,double reorder_days=0,double economic_reorder_q=0,string bin_number=" ",
			double weight=0,string dimensions=" ",string finish=" ",string type=" ",double current_cost=0)
		{
			this.origin=origin;
			this.warehouse=warehouse;
			this.product=product;
			this.long_description=long_description;
			this.exvat_price=exvat_price;
			this.Disc_Code=Disc_Code;
			this.orig_analysis=orig_analysis;
			this.remap_anal=remap_anal;
			this.prod_range=prod_range;
			this.key_core_slow=key_core_slow;
			this.abc_category=abc_category;
			this.LCM_xref=LCM_xref;
			this.salekey=salekey;
			this.Supplier=Supplier;
			this.SuppCurr=SuppCurr;
			this.AdminType=AdminType;
			this.StockType=StockType;
			this.Threemonthsales=threemonthsales;
			this.reorder_days=reorder_days;
			this.economic_reorder_q=economic_reorder_q;
			this.bin_number=bin_number;
			this.weight=weight;
			this.dimensions=dimensions;
			this.finish=finish;
			this.type=type;
			this.current_cost=current_cost;
		}

		[Required]
		public string origin { get; set;}
 
		[Required]
		public string warehouse { get; set;}
 
		[Required]
		[MaxLength(20)]
		public string product { get; set;}
 
		[Required]
		[MaxLength(40)]
		public string long_description { get; set;}
 
		[Required]
		public double exvat_price { get; set;}
 
		[Required]
		[MaxLength(8)]
		public string Disc_Code { get; set;}
 
		[MaxLength(10)]
		public string orig_analysis { get; set;}
 
		public string remap_anal { get; set;}
 
		[Required]
		[MaxLength(10)]
		public string prod_range { get; set;}
 
		[MaxLength(10)]
		public string key_core_slow { get; set;}
 
		[Required]
		public string abc_category { get; set;}
 
		[Required]
		public string LCM_xref { get; set;}
 
		[Required]
		[MaxLength(10)]
		public string salekey { get; set;}
 
		[Required]
		public string Supplier { get; set;}
 
		[Required]
		public string SuppCurr { get; set;}
 
		[Required]
		public string AdminType { get; set;}
 
		public string StockType { get; set;}
 
		[Required]
		public double Threemonthsales { get; set;}
 
		[Required]
		public double reorder_days { get; set;}
 
		[Required]
		public double economic_reorder_q { get; set;}
 
		[Required]
		public string bin_number { get; set;}
 
		[Required]
		public double weight { get; set;}
 
		public string dimensions { get; set;}
 
		[Required]
		public string finish { get; set;}
 
		public string type { get; set;}
 
		[Required]
		public double current_cost { get; set;}

		public static void Insert(PINNstock3 data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [dbo].[PINN_stock3](
					origin,warehouse,product,long_description,exvat_price,Disc_Code,orig_analysis,remap_anal,prod_range,key_core_slow,abc_category,LCM_xref,salekey,
					Supplier,SuppCurr,AdminType,StockType,[3monthsales],reorder_days,economic_reorder_q,bin_number,weight,dimensions,finish,type,current_cost)
				VALUES (@origin,@warehouse,@product,@long_description,@exvat_price,@Disc_Code,@orig_analysis,@remap_anal,@prod_range,@key_core_slow,
				@abc_category,@LCM_xref,@salekey,@Supplier,@SuppCurr,@AdminType,@StockType,@3monthsales,@reorder_days,@economic_reorder_q,@bin_number,@weight,@dimensions,@finish,@type,@current_cost)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@origin",data.origin);
				cmd.Parameters.AddWithValue("@warehouse",data.warehouse);
				cmd.Parameters.AddWithValue("@product",data.product);
				cmd.Parameters.AddWithValue("@long_description",data.long_description);
				cmd.Parameters.AddWithValue("@exvat_price",data.exvat_price);
				cmd.Parameters.AddWithValue("@Disc_Code",data.Disc_Code);
				cmd.Parameters.AddWithValue("@orig_analysis",(object) data.orig_analysis ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@remap_anal",(object) data.remap_anal ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@prod_range",data.prod_range);
				cmd.Parameters.AddWithValue("@key_core_slow",(object) data.key_core_slow ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@abc_category",data.abc_category);
				cmd.Parameters.AddWithValue("@LCM_xref",data.LCM_xref);
				cmd.Parameters.AddWithValue("@salekey",data.salekey);
				cmd.Parameters.AddWithValue("@Supplier",data.Supplier);
				cmd.Parameters.AddWithValue("@SuppCurr",data.SuppCurr);
				cmd.Parameters.AddWithValue("@AdminType",data.AdminType);
				cmd.Parameters.AddWithValue("@StockType",(object) data.StockType ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@3monthsales",data.Threemonthsales);
				cmd.Parameters.AddWithValue("@reorder_days",data.reorder_days);
				cmd.Parameters.AddWithValue("@economic_reorder_q",data.economic_reorder_q);
				cmd.Parameters.AddWithValue("@bin_number",data.bin_number);
				cmd.Parameters.AddWithValue("@weight",data.weight);
				cmd.Parameters.AddWithValue("@dimensions",(object) data.dimensions ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@finish",data.finish);
				cmd.Parameters.AddWithValue("@type",(object) data.type ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@current_cost",data.current_cost);
				cmd.ExecuteNonQuery();
			}

			
		}
 
	}
}
