using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class mporderbookgbp
	{
		public mporderbookgbp(
			DateTime? date_entered = null, string order_no = " ", string customer = " ", string convcurr = " ",
			string warehouse = " ", string product = " ", double? value = null, string delivery_reason = " ",
			string appearance = " ", string analysis_codes1 = " ", string status = " ", string RTM = " ",
			string truewh = " ", DateTime? date_required = null, string CURRFWD = " ", string to_excl = null,
			string YYYMM_Ent = null, string Expr1 = null, int? yearreq = null, string bundle_line_ref = " ",
			DateTime? Date = null, int? Year = null, string period = " ", string slyear = " ", string remap_anal = null,
			string prod_range = " ", string yrrate = null, double? valuegbp = null, DateTime? invdate = null,
			int? realyear = null, int? calyear = null, int? calmonth = null, int rowid = 0, string description = " ",
			double despatched_qty = 0)
		{
			this.date_entered = date_entered;
			this.order_no = order_no;
			this.customer = customer;
			this.convcurr = convcurr;
			this.warehouse = warehouse;
			this.product = product;
			this.value = value;
			this.delivery_reason = delivery_reason;
			this.appearance = appearance;
			this.analysis_codes1 = analysis_codes1;
			this.status = status;
			this.RTM = RTM;
			this.truewh = truewh;
			this.date_required = date_required;
			this.CURRFWD = CURRFWD;
			this.to_excl = to_excl;
			this.YYYMM_Ent = YYYMM_Ent;
			this.Expr1 = Expr1;
			this.yearreq = yearreq;
			this.bundle_line_ref = bundle_line_ref;
			this.Date = Date;
			this.Year = Year;
			this.period = period;
			this.slyear = slyear;
			this.remap_anal = remap_anal;
			this.prod_range = prod_range;
			this.yrrate = yrrate;
			this.valuegbp = valuegbp;
			this.invdate = invdate;
			this.realyear = realyear;
			this.calyear = calyear;
			this.calmonth = calmonth;
			this.rowid = rowid;
			this.description = description;
			this.despatched_qty = despatched_qty;
		}

		public DateTime? date_entered { get; set; }

		[Required] [MaxLength(10)] public string order_no { get; set; }

		[Required] [MaxLength(8)] public string customer { get; set; }

		[Required] public string convcurr { get; set; }

		[Required] [MaxLength(2)] public string warehouse { get; set; }

		[Required] [MaxLength(20)] public string product { get; set; }

		public double? value { get; set; }

		[Required] [MaxLength(10)] public string delivery_reason { get; set; }

		[Required] [MaxLength(60)] public string appearance { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes1 { get; set; }

		[Required] [MaxLength(1)] public string status { get; set; }

		[Required] public string RTM { get; set; }

		[Required] public string truewh { get; set; }

		public DateTime? date_required { get; set; }

		[Required] public string CURRFWD { get; set; }

		public string to_excl { get; set; }

		public string YYYMM_Ent { get; set; }

		public string Expr1 { get; set; }

		public int? yearreq { get; set; }

		[Required] [MaxLength(5)] public string bundle_line_ref { get; set; }

		public DateTime? Date { get; set; }

		public int? Year { get; set; }

		[Required] [MaxLength(2)] public string period { get; set; }

		[Required] [MaxLength(4)] public string slyear { get; set; }

		public string remap_anal { get; set; }

		[Required] [MaxLength(10)] public string prod_range { get; set; }

		public string yrrate { get; set; }

		public double? valuegbp { get; set; }

		public DateTime? invdate { get; set; }

		public int? realyear { get; set; }

		public int? calyear { get; set; }

		public int? calmonth { get; set; }

		[Required] public int rowid { get; set; }

		[Required] [MaxLength(20)] public string description { get; set; }

		[Required] public double despatched_qty { get; set; }

		public static void Insert(mporderbookgbp data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [dbo].[mporderbookgbp](
				[date_entered],[order_no],[customer],[convcurr],[warehouse],[product],[value],[delivery_reason],[appearance],[analysis_codes1],[status],[RTM],[truewh],[date_required],[CURRFWD],[to_excl],[YYYMM_Ent],[Expr1],[yearreq],[bundle_line_ref],[Date],[Year],[period],[slyear],[remap_anal],[prod_range],[yrrate],[valuegbp],[invdate],[realyear],[calyear],[calmonth],[rowid],[description],[despatched_qty])
				 VALUES (@date_entered,@order_no,@customer,@convcurr,@warehouse,@product,@value,@delivery_reason,@appearance,@analysis_codes1,@status,@RTM,@truewh,@date_required,@CURRFWD,@to_excl,@YYYMM_Ent,@Expr1,@yearreq,@bundle_line_ref,@Date,@Year,@period,@slyear,@remap_anal,@prod_range,@yrrate,@valuegbp,@invdate,@realyear,@calyear,@calmonth,@rowid,@description,@despatched_qty)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@date_entered", (object) data.date_entered ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@order_no", data.order_no);
				cmd.Parameters.AddWithValue("@customer", data.customer);
				cmd.Parameters.AddWithValue("@convcurr", data.convcurr);
				cmd.Parameters.AddWithValue("@warehouse", data.warehouse);
				cmd.Parameters.AddWithValue("@product", data.product);
				cmd.Parameters.AddWithValue("@value", (object) data.value ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@delivery_reason", data.delivery_reason);
				cmd.Parameters.AddWithValue("@appearance", data.appearance);
				cmd.Parameters.AddWithValue("@analysis_codes1", data.analysis_codes1);
				cmd.Parameters.AddWithValue("@status", data.status);
				cmd.Parameters.AddWithValue("@RTM", data.RTM);
				cmd.Parameters.AddWithValue("@truewh", data.truewh);
				cmd.Parameters.AddWithValue("@date_required", (object) data.date_required ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@CURRFWD", data.CURRFWD);
				cmd.Parameters.AddWithValue("@to_excl", (object) data.to_excl ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@YYYMM_Ent", (object) data.YYYMM_Ent ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Expr1", (object) data.Expr1 ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@yearreq", (object) data.yearreq ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@bundle_line_ref", data.bundle_line_ref);
				cmd.Parameters.AddWithValue("@Date", (object) data.Date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Year", (object) data.Year ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@period", data.period);
				cmd.Parameters.AddWithValue("@slyear", data.slyear);
				cmd.Parameters.AddWithValue("@remap_anal", (object) data.remap_anal ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@prod_range", data.prod_range);
				cmd.Parameters.AddWithValue("@yrrate", (object) data.yrrate ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@valuegbp", (object) data.valuegbp ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@invdate", (object) data.invdate ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@realyear", (object) data.realyear ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@calyear", (object) data.calyear ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@calmonth", (object) data.calmonth ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.Parameters.AddWithValue("@description", data.description);
				cmd.Parameters.AddWithValue("@despatched_qty", data.despatched_qty);
				cmd.ExecuteNonQuery();
			}
		}
	}


}
