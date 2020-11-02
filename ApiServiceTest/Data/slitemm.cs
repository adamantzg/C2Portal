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
	public class slitemm
	{

		public slitemm(string customer=" ",string item_no=" ",string refernce=" ",DateTime? dated=null,DateTime? due_date=null,string kind=" ",double amount=0,double currency_amount=0,
			double unall_amount=0,double unall_curr_amount=0,double vat_amount=0,string open_indicator=" ",string hold_indicator=" ",string currency=" ",double discount_amt=0,
			string disc_sett_date=null,string spare=" ",string analysis_codes1=" ",string analysis_codes2=" ",string analysis_codes3=" ",string username=" ",string userdate=null,
			string usertime=" ",string direct_debit=" ",string customer_and_date=" ",string payment_method=" ",string cp_status=" ",string sales_type=" ",string transaction_group=" ",
			double orig_base_value=0,double current_rate=0,double original_rate=0,string fixed_rate=" ",string currency_operator=" ",string currency_type=" ",string customer_order_num=" ",
			string short_name=" ",string effective_date=null,string period=" ",string slyear=" ",string sett_category=" ",double discount_value2=0,double discount_value3=0,
			double discount_value4=0,string disc_sett_date2=null,string disc_sett_date3=null,string disc_sett_date4=null,double total_charge=0,double total_for_charge=0,
			string control=" ",string order_customer=" ",string consolidated=" ",byte[] rowstamp= null)
		{
			this.customer=customer;
			this.item_no=item_no;
			this.refernce=refernce;
			this.dated=dated;
			this.due_date=due_date;
			this.kind=kind;
			this.amount=amount;
			this.currency_amount=currency_amount;
			this.unall_amount=unall_amount;
			this.unall_curr_amount=unall_curr_amount;
			this.vat_amount=vat_amount;
			this.open_indicator=open_indicator;
			this.hold_indicator=hold_indicator;
			this.currency=currency;
			this.discount_amt=discount_amt;
			this.disc_sett_date=disc_sett_date;
			this.spare=spare;
			this.analysis_codes1=analysis_codes1;
			this.analysis_codes2=analysis_codes2;
			this.analysis_codes3=analysis_codes3;
			this.username=username;
			this.userdate=userdate;
			this.usertime=usertime;
			this.direct_debit=direct_debit;
			this.customer_and_date=customer_and_date;
			this.payment_method=payment_method;
			this.cp_status=cp_status;
			this.sales_type=sales_type;
			this.transaction_group=transaction_group;
			this.orig_base_value=orig_base_value;
			this.current_rate=current_rate;
			this.original_rate=original_rate;
			this.fixed_rate=fixed_rate;
			this.currency_operator=currency_operator;
			this.currency_type=currency_type;
			this.customer_order_num=customer_order_num;
			this.short_name=short_name;
			this.effective_date=effective_date;
			this.period=period;
			this.slyear=slyear;
			this.sett_category=sett_category;
			this.discount_value2=discount_value2;
			this.discount_value3=discount_value3;
			this.discount_value4=discount_value4;
			this.disc_sett_date2=disc_sett_date2;
			this.disc_sett_date3=disc_sett_date3;
			this.disc_sett_date4=disc_sett_date4;
			this.total_charge=total_charge;
			this.total_for_charge=total_for_charge;
			this.control=control;
			this.order_customer=order_customer;
			this.consolidated=consolidated;
			this.rowstamp=rowstamp;
		}

		[Required] [MaxLength(8)] public string customer { get; set; }

		[Required] [MaxLength(10)] public string item_no { get; set; }

		[Required] [MaxLength(10)] public string refernce { get; set; }

		public DateTime? dated { get; set; }

		public DateTime? due_date { get; set; }

		[Required] [MaxLength(3)] public string kind { get; set; }

		[Required] public double amount { get; set; }

		[Required] public double currency_amount { get; set; }

		[Required] public double unall_amount { get; set; }

		[Required] public double unall_curr_amount { get; set; }

		[Required] public double vat_amount { get; set; }

		[Required] [MaxLength(1)] public string open_indicator { get; set; }

		[Required] [MaxLength(1)] public string hold_indicator { get; set; }

		[Required] [MaxLength(3)] public string currency { get; set; }

		[Required] public double discount_amt { get; set; }

		[MaxLength(29)] public string disc_sett_date { get; set; }

		[Required] [MaxLength(49)] public string spare { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes1 { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes2 { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes3 { get; set; }

		[Required] [MaxLength(10)] public string username { get; set; }

		[MaxLength(29)] public string userdate { get; set; }

		[Required] [MaxLength(8)] public string usertime { get; set; }

		[Required] [MaxLength(1)] public string direct_debit { get; set; }

		[Required] [MaxLength(16)] public string customer_and_date { get; set; }

		[Required] [MaxLength(2)] public string payment_method { get; set; }

		[Required] [MaxLength(1)] public string cp_status { get; set; }

		[Required] [MaxLength(2)] public string sales_type { get; set; }

		[Required] [MaxLength(10)] public string transaction_group { get; set; }

		[Required] public double orig_base_value { get; set; }

		[Required] public double current_rate { get; set; }

		[Required] public double original_rate { get; set; }

		[Required] [MaxLength(1)] public string fixed_rate { get; set; }

		[Required] [MaxLength(1)] public string currency_operator { get; set; }

		[Required] [MaxLength(2)] public string currency_type { get; set; }

		[Required] [MaxLength(20)] public string customer_order_num { get; set; }

		[Required] [MaxLength(3)] public string short_name { get; set; }

		[MaxLength(29)] public string effective_date { get; set; }

		[Required] [MaxLength(2)] public string period { get; set; }

		[Required] [MaxLength(4)] public string slyear { get; set; }

		[Required] [MaxLength(2)] public string sett_category { get; set; }

		[Required] public double discount_value2 { get; set; }

		[Required] public double discount_value3 { get; set; }

		[Required] public double discount_value4 { get; set; }

		[MaxLength(29)] public string disc_sett_date2 { get; set; }

		[MaxLength(29)] public string disc_sett_date3 { get; set; }

		[MaxLength(29)] public string disc_sett_date4 { get; set; }

		[Required] public double total_charge { get; set; }

		[Required] public double total_for_charge { get; set; }

		[Required] [MaxLength(16)] public string control { get; set; }

		[Required] [MaxLength(8)] public string order_customer { get; set; }

		[Required] [MaxLength(1)] public string consolidated { get; set; }

		[Required] public byte[] rowstamp { get; set; }


		public static void Insert(slitemm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[slitemm](
			[customer],[item_no],[refernce],[dated],[due_date],[kind],[amount],[currency_amount],[unall_amount],[unall_curr_amount],[vat_amount],[open_indicator],[hold_indicator],[currency],[discount_amt],[disc_sett_date],[spare],[analysis_codes1],[analysis_codes2],[analysis_codes3],[username],[userdate],[usertime],[direct_debit],[customer_and_date],[payment_method],[cp_status],[sales_type],[transaction_group],[orig_base_value],[current_rate],[original_rate],[fixed_rate],[currency_operator],[currency_type],[customer_order_num],[short_name],[effective_date],[period],[slyear],[sett_category],[discount_value2],[discount_value3],[discount_value4],[disc_sett_date2],[disc_sett_date3],[disc_sett_date4],[total_charge],[total_for_charge],[control],[order_customer],[consolidated],[rowstamp])
			 VALUES (@customer,@item_no,@refernce,@dated,@due_date,@kind,@amount,@currency_amount,@unall_amount,@unall_curr_amount,@vat_amount,@open_indicator,@hold_indicator,@currency,@discount_amt,@disc_sett_date,@spare,@analysis_codes1,@analysis_codes2,@analysis_codes3,@username,@userdate,@usertime,@direct_debit,@customer_and_date,@payment_method,@cp_status,@sales_type,@transaction_group,@orig_base_value,@current_rate,@original_rate,@fixed_rate,@currency_operator,@currency_type,@customer_order_num,@short_name,@effective_date,@period,@slyear,@sett_category,@discount_value2,@discount_value3,@discount_value4,@disc_sett_date2,@disc_sett_date3,@disc_sett_date4,@total_charge,@total_for_charge,@control,@order_customer,@consolidated,@rowstamp)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@customer", data.customer);
				cmd.Parameters.AddWithValue("@item_no", data.item_no);
				cmd.Parameters.AddWithValue("@refernce", data.refernce);
				cmd.Parameters.AddWithValue("@dated", (object) data.dated ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@due_date", (object) data.due_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@kind", data.kind);
				cmd.Parameters.AddWithValue("@amount", data.amount);
				cmd.Parameters.AddWithValue("@currency_amount", data.currency_amount);
				cmd.Parameters.AddWithValue("@unall_amount", data.unall_amount);
				cmd.Parameters.AddWithValue("@unall_curr_amount", data.unall_curr_amount);
				cmd.Parameters.AddWithValue("@vat_amount", data.vat_amount);
				cmd.Parameters.AddWithValue("@open_indicator", data.open_indicator);
				cmd.Parameters.AddWithValue("@hold_indicator", data.hold_indicator);
				cmd.Parameters.AddWithValue("@currency", data.currency);
				cmd.Parameters.AddWithValue("@discount_amt", data.discount_amt);
				cmd.Parameters.AddWithValue("@disc_sett_date", (object) data.disc_sett_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@spare", data.spare);
				cmd.Parameters.AddWithValue("@analysis_codes1", data.analysis_codes1);
				cmd.Parameters.AddWithValue("@analysis_codes2", data.analysis_codes2);
				cmd.Parameters.AddWithValue("@analysis_codes3", data.analysis_codes3);
				cmd.Parameters.AddWithValue("@username", data.username);
				cmd.Parameters.AddWithValue("@userdate", (object) data.userdate ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@usertime", data.usertime);
				cmd.Parameters.AddWithValue("@direct_debit", data.direct_debit);
				cmd.Parameters.AddWithValue("@customer_and_date", data.customer_and_date);
				cmd.Parameters.AddWithValue("@payment_method", data.payment_method);
				cmd.Parameters.AddWithValue("@cp_status", data.cp_status);
				cmd.Parameters.AddWithValue("@sales_type", data.sales_type);
				cmd.Parameters.AddWithValue("@transaction_group", data.transaction_group);
				cmd.Parameters.AddWithValue("@orig_base_value", data.orig_base_value);
				cmd.Parameters.AddWithValue("@current_rate", data.current_rate);
				cmd.Parameters.AddWithValue("@original_rate", data.original_rate);
				cmd.Parameters.AddWithValue("@fixed_rate", data.fixed_rate);
				cmd.Parameters.AddWithValue("@currency_operator", data.currency_operator);
				cmd.Parameters.AddWithValue("@currency_type", data.currency_type);
				cmd.Parameters.AddWithValue("@customer_order_num", data.customer_order_num);
				cmd.Parameters.AddWithValue("@short_name", data.short_name);
				cmd.Parameters.AddWithValue("@effective_date", (object) data.effective_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@period", data.period);
				cmd.Parameters.AddWithValue("@slyear", data.slyear);
				cmd.Parameters.AddWithValue("@sett_category", data.sett_category);
				cmd.Parameters.AddWithValue("@discount_value2", data.discount_value2);
				cmd.Parameters.AddWithValue("@discount_value3", data.discount_value3);
				cmd.Parameters.AddWithValue("@discount_value4", data.discount_value4);
				cmd.Parameters.AddWithValue("@disc_sett_date2", (object) data.disc_sett_date2 ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@disc_sett_date3", (object) data.disc_sett_date3 ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@disc_sett_date4", (object) data.disc_sett_date4 ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@total_charge", data.total_charge);
				cmd.Parameters.AddWithValue("@total_for_charge", data.total_for_charge);
				cmd.Parameters.AddWithValue("@control", data.control);
				cmd.Parameters.AddWithValue("@order_customer", data.order_customer);
				cmd.Parameters.AddWithValue("@consolidated", data.consolidated);
				cmd.Parameters.AddWithValue("@rowstamp", data.rowstamp ?? SqlBinary.Null);
				cmd.ExecuteNonQuery();
			}
		}
	}


}
