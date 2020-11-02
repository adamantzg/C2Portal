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
	public class slcustm
	{
		public slcustm(string customer = " ", string alpha = " ", string name = " ", string address1 = " ",
			string address2 = " ", string address3 = " ", string address4 = " ", string address5 = " ",
			string town_city = " ", string county = " ", string state_region = " ", string iso_country_code = " ",
			string country = " ", string credit_category = " ", string export_indicator = " ",
			string cust_disc_code = " ", string currency = " ", string territory = " ", string className = " ",
			string region = " ", string invoice_customer = " ", string statement_customer = " ",
			string group_customer = " ", DateTime? date_last_issue = null, DateTime? date_created = null,
			string analysis_codes1 = " ", string analysis_codes2 = " ", string analysis_codes3 = " ",
			string analysis_codes4 = " ", string analysis_codes5 = " ", string reminder_cat = " ",
			string settlement_code = " ", string sett_days_code = " ", string price_list = " ",
			string letter_code = " ",
			string balance_fwd = " ", double credit_limit = 0, double ytd_sales = 0, double ytd_cost_of_sales = 0,
			double cumulative_sales = 0, double order_balance = 0, string sales_nl_cat = " ",
			string special_price = " ", string vat_registration = " ", string direct_debit = " ",
			string invoices_printed = " ", string consolidated_inv = " ", string comment_only_inv = " ",
			string bank_account_no = " ", string bank_sort_code = " ", string bank_name = " ",
			string bank_address1 = " ", string bank_address2 = " ", string bank_address3 = " ",
			string bank_address4 = " ",
			string analysis_code_6 = " ", string produce_statements = " ", string edi_customer = " ",
			string vat_type = " ", string lang = " ", string delivery_method = " ", string carrier = " ",
			string vat_reg_number = " ", string vat_exe_number = " ", string paydays1 = " ", string paydays2 = " ",
			string paydays3 = " ", string bank_branch_code = " ", string print_cp_with_stat = " ",
			string payment_method = " ", string customer_class = " ", string sales_type = " ",
			double cp_lower_value = 0, string address6 = " ", string fax = " ", string telex = " ", string btx = " ",
			string cp_charge = " ", string control_digit = " ", string payer = " ", string responsibility = " ",
			string despatch_held = " ", string credit_controller = " ", string reminder_letters = " ",
			int severity_days1 = 0, int severity_days2 = 0, int severity_days3 = 0, int severity_days4 = 0,
			int severity_days5 = 0, int severity_days6 = 0, string delivery_reason = " ", string shipper_code1 = " ",
			string shipper_code2 = " ", string shipper_code3 = " ", string shipping_note_ind = " ",
			string account_type = " ", string admin_fee = " ", string intrest_rate = " ", string iban = " ",
			string bic = " ",
			string email = " ", string transaction_email = " ", double credit_limit_safe = 0, byte[] rowstamp = null,
			int rowid = 0)
		{
			this.customer = customer;
			this.alpha = alpha;
			this.name = name;
			this.address1 = address1;
			this.address2 = address2;
			this.address3 = address3;
			this.address4 = address4;
			this.address5 = address5;
			this.town_city = town_city;
			this.county = county;
			this.state_region = state_region;
			this.iso_country_code = iso_country_code;
			this.country = country;
			this.credit_category = credit_category;
			this.export_indicator = export_indicator;
			this.cust_disc_code = cust_disc_code;
			this.currency = currency;
			this.territory = territory;
			this.className = className;
			this.region = region;
			this.invoice_customer = invoice_customer;
			this.statement_customer = statement_customer;
			this.group_customer = group_customer;
			this.date_last_issue = date_last_issue;
			this.date_created = date_created;
			this.analysis_codes1 = analysis_codes1;
			this.analysis_codes2 = analysis_codes2;
			this.analysis_codes3 = analysis_codes3;
			this.analysis_codes4 = analysis_codes4;
			this.analysis_codes5 = analysis_codes5;
			this.reminder_cat = reminder_cat;
			this.settlement_code = settlement_code;
			this.sett_days_code = sett_days_code;
			this.price_list = price_list;
			this.letter_code = letter_code;
			this.balance_fwd = balance_fwd;
			this.credit_limit = credit_limit;
			this.ytd_sales = ytd_sales;
			this.ytd_cost_of_sales = ytd_cost_of_sales;
			this.cumulative_sales = cumulative_sales;
			this.order_balance = order_balance;
			this.sales_nl_cat = sales_nl_cat;
			this.special_price = special_price;
			this.vat_registration = vat_registration;
			this.direct_debit = direct_debit;
			this.invoices_printed = invoices_printed;
			this.consolidated_inv = consolidated_inv;
			this.comment_only_inv = comment_only_inv;
			this.bank_account_no = bank_account_no;
			this.bank_sort_code = bank_sort_code;
			this.bank_name = bank_name;
			this.bank_address1 = bank_address1;
			this.bank_address2 = bank_address2;
			this.bank_address3 = bank_address3;
			this.bank_address4 = bank_address4;
			this.analysis_code_6 = analysis_code_6;
			this.produce_statements = produce_statements;
			this.edi_customer = edi_customer;
			this.vat_type = vat_type;
			this.lang = lang;
			this.delivery_method = delivery_method;
			this.carrier = carrier;
			this.vat_reg_number = vat_reg_number;
			this.vat_exe_number = vat_exe_number;
			this.paydays1 = paydays1;
			this.paydays2 = paydays2;
			this.paydays3 = paydays3;
			this.bank_branch_code = bank_branch_code;
			this.print_cp_with_stat = print_cp_with_stat;
			this.payment_method = payment_method;
			this.customer_class = customer_class;
			this.sales_type = sales_type;
			this.cp_lower_value = cp_lower_value;
			this.address6 = address6;
			this.fax = fax;
			this.telex = telex;
			this.btx = btx;
			this.cp_charge = cp_charge;
			this.control_digit = control_digit;
			this.payer = payer;
			this.responsibility = responsibility;
			this.despatch_held = despatch_held;
			this.credit_controller = credit_controller;
			this.reminder_letters = reminder_letters;
			this.severity_days1 = severity_days1;
			this.severity_days2 = severity_days2;
			this.severity_days3 = severity_days3;
			this.severity_days4 = severity_days4;
			this.severity_days5 = severity_days5;
			this.severity_days6 = severity_days6;
			this.delivery_reason = delivery_reason;
			this.shipper_code1 = shipper_code1;
			this.shipper_code2 = shipper_code2;
			this.shipper_code3 = shipper_code3;
			this.shipping_note_ind = shipping_note_ind;
			this.account_type = account_type;
			this.admin_fee = admin_fee;
			this.intrest_rate = intrest_rate;
			this.iban = iban;
			this.bic = bic;
			this.email = email;
			this.transaction_email = transaction_email;
			this.credit_limit_safe = credit_limit_safe;
			this.rowstamp = rowstamp;
			this.rowid = rowid;
		}

		[Required] [MaxLength(8)] public string customer { get; set; }

		[Required] [MaxLength(8)] public string alpha { get; set; }

		[Required] [MaxLength(32)] public string name { get; set; }

		[Required] [MaxLength(64)] public string address1 { get; set; }

		[Required] [MaxLength(64)] public string address2 { get; set; }

		[Required] [MaxLength(64)] public string address3 { get; set; }

		[Required] [MaxLength(64)] public string address4 { get; set; }

		[Required] [MaxLength(64)] public string address5 { get; set; }

		[Required] [MaxLength(64)] public string town_city { get; set; }

		[Required] [MaxLength(64)] public string county { get; set; }

		[Required] [MaxLength(64)] public string state_region { get; set; }

		[Required] [MaxLength(2)] public string iso_country_code { get; set; }

		[Required] [MaxLength(64)] public string country { get; set; }

		[Required] [MaxLength(1)] public string credit_category { get; set; }

		[Required] [MaxLength(1)] public string export_indicator { get; set; }

		[Required] [MaxLength(4)] public string cust_disc_code { get; set; }

		[Required] [MaxLength(3)] public string currency { get; set; }

		[Required] [MaxLength(6)] public string territory { get; set; }

		[Required] [MaxLength(6)] public string className { get; set; }

		[Required] [MaxLength(6)] public string region { get; set; }

		[Required] [MaxLength(8)] public string invoice_customer { get; set; }

		[Required] [MaxLength(8)] public string statement_customer { get; set; }

		[Required] [MaxLength(8)] public string group_customer { get; set; }

		public DateTime? date_last_issue { get; set; }

		public DateTime? date_created { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes1 { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes2 { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes3 { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes4 { get; set; }

		[Required] [MaxLength(10)] public string analysis_codes5 { get; set; }

		[Required] [MaxLength(3)] public string reminder_cat { get; set; }

		[Required] [MaxLength(2)] public string settlement_code { get; set; }

		[Required] [MaxLength(1)] public string sett_days_code { get; set; }

		[Required] [MaxLength(3)] public string price_list { get; set; }

		[Required] [MaxLength(3)] public string letter_code { get; set; }

		[Required] [MaxLength(1)] public string balance_fwd { get; set; }

		[Required] public double credit_limit { get; set; }

		[Required] public double ytd_sales { get; set; }

		[Required] public double ytd_cost_of_sales { get; set; }

		[Required] public double cumulative_sales { get; set; }

		[Required] public double order_balance { get; set; }

		[Required] [MaxLength(3)] public string sales_nl_cat { get; set; }

		[Required] [MaxLength(3)] public string special_price { get; set; }

		[Required] [MaxLength(14)] public string vat_registration { get; set; }

		[Required] [MaxLength(1)] public string direct_debit { get; set; }

		[Required] [MaxLength(1)] public string invoices_printed { get; set; }

		[Required] [MaxLength(1)] public string consolidated_inv { get; set; }

		[Required] [MaxLength(1)] public string comment_only_inv { get; set; }

		[Required] [MaxLength(20)] public string bank_account_no { get; set; }

		[Required] [MaxLength(8)] public string bank_sort_code { get; set; }

		[Required] [MaxLength(20)] public string bank_name { get; set; }

		[Required] [MaxLength(30)] public string bank_address1 { get; set; }

		[Required] [MaxLength(30)] public string bank_address2 { get; set; }

		[Required] [MaxLength(30)] public string bank_address3 { get; set; }

		[Required] [MaxLength(30)] public string bank_address4 { get; set; }

		[Required] [MaxLength(10)] public string analysis_code_6 { get; set; }

		[Required] [MaxLength(1)] public string produce_statements { get; set; }

		[Required] [MaxLength(1)] public string edi_customer { get; set; }

		[Required] [MaxLength(10)] public string vat_type { get; set; }

		[Required] [MaxLength(10)] public string lang { get; set; }

		[Required] [MaxLength(10)] public string delivery_method { get; set; }

		[Required] [MaxLength(10)] public string carrier { get; set; }

		[Required] [MaxLength(16)] public string vat_reg_number { get; set; }

		[Required] [MaxLength(16)] public string vat_exe_number { get; set; }

		[Required] [MaxLength(2)] public string paydays1 { get; set; }

		[Required] [MaxLength(2)] public string paydays2 { get; set; }

		[Required] [MaxLength(2)] public string paydays3 { get; set; }

		[Required] [MaxLength(8)] public string bank_branch_code { get; set; }

		[Required] [MaxLength(1)] public string print_cp_with_stat { get; set; }

		[Required] [MaxLength(2)] public string payment_method { get; set; }

		[Required] [MaxLength(8)] public string customer_class { get; set; }

		[Required] [MaxLength(2)] public string sales_type { get; set; }

		[Required] public double cp_lower_value { get; set; }

		[Required] [MaxLength(64)] public string address6 { get; set; }

		[Required] [MaxLength(30)] public string fax { get; set; }

		[Required] [MaxLength(30)] public string telex { get; set; }

		[Required] [MaxLength(30)] public string btx { get; set; }

		[Required] [MaxLength(1)] public string cp_charge { get; set; }

		[Required] [MaxLength(2)] public string control_digit { get; set; }

		[Required] [MaxLength(10)] public string payer { get; set; }

		[Required] [MaxLength(10)] public string responsibility { get; set; }

		[Required] [MaxLength(1)] public string despatch_held { get; set; }

		[Required] [MaxLength(3)] public string credit_controller { get; set; }

		[Required] [MaxLength(1)] public string reminder_letters { get; set; }

		[Required] public int severity_days1 { get; set; }

		[Required] public int severity_days2 { get; set; }

		[Required] public int severity_days3 { get; set; }

		[Required] public int severity_days4 { get; set; }

		[Required] public int severity_days5 { get; set; }

		[Required] public int severity_days6 { get; set; }

		[Required] [MaxLength(10)] public string delivery_reason { get; set; }

		[Required] [MaxLength(10)] public string shipper_code1 { get; set; }

		[Required] [MaxLength(10)] public string shipper_code2 { get; set; }

		[Required] [MaxLength(10)] public string shipper_code3 { get; set; }

		[Required] [MaxLength(1)] public string shipping_note_ind { get; set; }

		[Required] [MaxLength(3)] public string account_type { get; set; }

		[Required] [MaxLength(20)] public string admin_fee { get; set; }

		[Required] [MaxLength(20)] public string intrest_rate { get; set; }

		[Required] [MaxLength(40)] public string iban { get; set; }

		[Required] [MaxLength(11)] public string bic { get; set; }

		[Required] [MaxLength(200)] public string email { get; set; }

		[Required] [MaxLength(1)] public string transaction_email { get; set; }

		[Required] public double credit_limit_safe { get; set; }

		[Required] [MaxLength(8)] public byte[] rowstamp { get; set; }

		[Required] public int rowid { get; set; }

		public static void Insert(slcustm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[slcustm](
			[customer],[alpha],[name],[address1],[address2],[address3],[address4],[address5],[town_city],[county],[state_region],[iso_country_code],[country],[credit_category],[export_indicator],[cust_disc_code],[currency],[territory],[class],[region],[invoice_customer],[statement_customer],[group_customer],[date_last_issue],[date_created],[analysis_codes1],[analysis_codes2],[analysis_codes3],[analysis_codes4],[analysis_codes5],[reminder_cat],[settlement_code],[sett_days_code],[price_list],[letter_code],[balance_fwd],[credit_limit],[ytd_sales],[ytd_cost_of_sales],[cumulative_sales],[order_balance],[sales_nl_cat],[special_price],[vat_registration],[direct_debit],[invoices_printed],[consolidated_inv],[comment_only_inv],[bank_account_no],[bank_sort_code],[bank_name],[bank_address1],[bank_address2],[bank_address3],[bank_address4],[analysis_code_6],[produce_statements],[edi_customer],[vat_type],[lang],[delivery_method],[carrier],[vat_reg_number],[vat_exe_number],[paydays1],[paydays2],[paydays3],[bank_branch_code],[print_cp_with_stat],[payment_method],[customer_class],[sales_type],[cp_lower_value],[address6],[fax],[telex],[btx],[cp_charge],[control_digit],[payer],[responsibility],[despatch_held],[credit_controller],[reminder_letters],[severity_days1],[severity_days2],[severity_days3],[severity_days4],[severity_days5],[severity_days6],[delivery_reason],[shipper_code1],[shipper_code2],[shipper_code3],[shipping_note_ind],[account_type],[admin_fee],[intrest_rate],[iban],[bic],[email],[transaction_email],[credit_limit_safe],[rowstamp],[rowid])
			 VALUES (@customer,@alpha,@name,@address1,@address2,@address3,@address4,@address5,@town_city,@county,@state_region,@iso_country_code,@country,@credit_category,@export_indicator,@cust_disc_code,@currency,@territory,@class,@region,@invoice_customer,@statement_customer,@group_customer,@date_last_issue,@date_created,@analysis_codes1,@analysis_codes2,@analysis_codes3,@analysis_codes4,@analysis_codes5,@reminder_cat,@settlement_code,@sett_days_code,@price_list,@letter_code,@balance_fwd,@credit_limit,@ytd_sales,@ytd_cost_of_sales,@cumulative_sales,@order_balance,@sales_nl_cat,@special_price,@vat_registration,@direct_debit,@invoices_printed,@consolidated_inv,@comment_only_inv,@bank_account_no,@bank_sort_code,@bank_name,@bank_address1,@bank_address2,@bank_address3,@bank_address4,@analysis_code_6,@produce_statements,@edi_customer,@vat_type,@lang,@delivery_method,@carrier,@vat_reg_number,@vat_exe_number,@paydays1,@paydays2,@paydays3,@bank_branch_code,@print_cp_with_stat,@payment_method,@customer_class,@sales_type,@cp_lower_value,@address6,@fax,@telex,@btx,@cp_charge,@control_digit,@payer,@responsibility,@despatch_held,@credit_controller,@reminder_letters,@severity_days1,@severity_days2,@severity_days3,@severity_days4,@severity_days5,@severity_days6,@delivery_reason,@shipper_code1,@shipper_code2,@shipper_code3,@shipping_note_ind,@account_type,@admin_fee,@intrest_rate,@iban,@bic,@email,@transaction_email,@credit_limit_safe,@rowstamp,@rowid)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@customer", data.customer);
				cmd.Parameters.AddWithValue("@alpha", data.alpha);
				cmd.Parameters.AddWithValue("@name", data.name);
				cmd.Parameters.AddWithValue("@address1", data.address1);
				cmd.Parameters.AddWithValue("@address2", data.address2);
				cmd.Parameters.AddWithValue("@address3", data.address3);
				cmd.Parameters.AddWithValue("@address4", data.address4);
				cmd.Parameters.AddWithValue("@address5", data.address5);
				cmd.Parameters.AddWithValue("@town_city", data.town_city);
				cmd.Parameters.AddWithValue("@county", data.county);
				cmd.Parameters.AddWithValue("@state_region", data.state_region);
				cmd.Parameters.AddWithValue("@iso_country_code", data.iso_country_code);
				cmd.Parameters.AddWithValue("@country", data.country);
				cmd.Parameters.AddWithValue("@credit_category", data.credit_category);
				cmd.Parameters.AddWithValue("@export_indicator", data.export_indicator);
				cmd.Parameters.AddWithValue("@cust_disc_code", data.cust_disc_code);
				cmd.Parameters.AddWithValue("@currency", data.currency);
				cmd.Parameters.AddWithValue("@territory", data.territory);
				cmd.Parameters.AddWithValue("@class", data.className);
				cmd.Parameters.AddWithValue("@region", data.region);
				cmd.Parameters.AddWithValue("@invoice_customer", data.invoice_customer);
				cmd.Parameters.AddWithValue("@statement_customer", data.statement_customer);
				cmd.Parameters.AddWithValue("@group_customer", data.group_customer);
				cmd.Parameters.AddWithValue("@date_last_issue", (object) data.date_last_issue ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@date_created", (object) data.date_created ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@analysis_codes1", data.analysis_codes1);
				cmd.Parameters.AddWithValue("@analysis_codes2", data.analysis_codes2);
				cmd.Parameters.AddWithValue("@analysis_codes3", data.analysis_codes3);
				cmd.Parameters.AddWithValue("@analysis_codes4", data.analysis_codes4);
				cmd.Parameters.AddWithValue("@analysis_codes5", data.analysis_codes5);
				cmd.Parameters.AddWithValue("@reminder_cat", data.reminder_cat);
				cmd.Parameters.AddWithValue("@settlement_code", data.settlement_code);
				cmd.Parameters.AddWithValue("@sett_days_code", data.sett_days_code);
				cmd.Parameters.AddWithValue("@price_list", data.price_list);
				cmd.Parameters.AddWithValue("@letter_code", data.letter_code);
				cmd.Parameters.AddWithValue("@balance_fwd", data.balance_fwd);
				cmd.Parameters.AddWithValue("@credit_limit", data.credit_limit);
				cmd.Parameters.AddWithValue("@ytd_sales", data.ytd_sales);
				cmd.Parameters.AddWithValue("@ytd_cost_of_sales", data.ytd_cost_of_sales);
				cmd.Parameters.AddWithValue("@cumulative_sales", data.cumulative_sales);
				cmd.Parameters.AddWithValue("@order_balance", data.order_balance);
				cmd.Parameters.AddWithValue("@sales_nl_cat", data.sales_nl_cat);
				cmd.Parameters.AddWithValue("@special_price", data.special_price);
				cmd.Parameters.AddWithValue("@vat_registration", data.vat_registration);
				cmd.Parameters.AddWithValue("@direct_debit", data.direct_debit);
				cmd.Parameters.AddWithValue("@invoices_printed", data.invoices_printed);
				cmd.Parameters.AddWithValue("@consolidated_inv", data.consolidated_inv);
				cmd.Parameters.AddWithValue("@comment_only_inv", data.comment_only_inv);
				cmd.Parameters.AddWithValue("@bank_account_no", data.bank_account_no);
				cmd.Parameters.AddWithValue("@bank_sort_code", data.bank_sort_code);
				cmd.Parameters.AddWithValue("@bank_name", data.bank_name);
				cmd.Parameters.AddWithValue("@bank_address1", data.bank_address1);
				cmd.Parameters.AddWithValue("@bank_address2", data.bank_address2);
				cmd.Parameters.AddWithValue("@bank_address3", data.bank_address3);
				cmd.Parameters.AddWithValue("@bank_address4", data.bank_address4);
				cmd.Parameters.AddWithValue("@analysis_code_6", data.analysis_code_6);
				cmd.Parameters.AddWithValue("@produce_statements", data.produce_statements);
				cmd.Parameters.AddWithValue("@edi_customer", data.edi_customer);
				cmd.Parameters.AddWithValue("@vat_type", data.vat_type);
				cmd.Parameters.AddWithValue("@lang", data.lang);
				cmd.Parameters.AddWithValue("@delivery_method", data.delivery_method);
				cmd.Parameters.AddWithValue("@carrier", data.carrier);
				cmd.Parameters.AddWithValue("@vat_reg_number", data.vat_reg_number);
				cmd.Parameters.AddWithValue("@vat_exe_number", data.vat_exe_number);
				cmd.Parameters.AddWithValue("@paydays1", data.paydays1);
				cmd.Parameters.AddWithValue("@paydays2", data.paydays2);
				cmd.Parameters.AddWithValue("@paydays3", data.paydays3);
				cmd.Parameters.AddWithValue("@bank_branch_code", data.bank_branch_code);
				cmd.Parameters.AddWithValue("@print_cp_with_stat", data.print_cp_with_stat);
				cmd.Parameters.AddWithValue("@payment_method", data.payment_method);
				cmd.Parameters.AddWithValue("@customer_class", data.customer_class);
				cmd.Parameters.AddWithValue("@sales_type", data.sales_type);
				cmd.Parameters.AddWithValue("@cp_lower_value", data.cp_lower_value);
				cmd.Parameters.AddWithValue("@address6", data.address6);
				cmd.Parameters.AddWithValue("@fax", data.fax);
				cmd.Parameters.AddWithValue("@telex", data.telex);
				cmd.Parameters.AddWithValue("@btx", data.btx);
				cmd.Parameters.AddWithValue("@cp_charge", data.cp_charge);
				cmd.Parameters.AddWithValue("@control_digit", data.control_digit);
				cmd.Parameters.AddWithValue("@payer", data.payer);
				cmd.Parameters.AddWithValue("@responsibility", data.responsibility);
				cmd.Parameters.AddWithValue("@despatch_held", data.despatch_held);
				cmd.Parameters.AddWithValue("@credit_controller", data.credit_controller);
				cmd.Parameters.AddWithValue("@reminder_letters", data.reminder_letters);
				cmd.Parameters.AddWithValue("@severity_days1", data.severity_days1);
				cmd.Parameters.AddWithValue("@severity_days2", data.severity_days2);
				cmd.Parameters.AddWithValue("@severity_days3", data.severity_days3);
				cmd.Parameters.AddWithValue("@severity_days4", data.severity_days4);
				cmd.Parameters.AddWithValue("@severity_days5", data.severity_days5);
				cmd.Parameters.AddWithValue("@severity_days6", data.severity_days6);
				cmd.Parameters.AddWithValue("@delivery_reason", data.delivery_reason);
				cmd.Parameters.AddWithValue("@shipper_code1", data.shipper_code1);
				cmd.Parameters.AddWithValue("@shipper_code2", data.shipper_code2);
				cmd.Parameters.AddWithValue("@shipper_code3", data.shipper_code3);
				cmd.Parameters.AddWithValue("@shipping_note_ind", data.shipping_note_ind);
				cmd.Parameters.AddWithValue("@account_type", data.account_type);
				cmd.Parameters.AddWithValue("@admin_fee", data.admin_fee);
				cmd.Parameters.AddWithValue("@intrest_rate", data.intrest_rate);
				cmd.Parameters.AddWithValue("@iban", data.iban);
				cmd.Parameters.AddWithValue("@bic", data.bic);
				cmd.Parameters.AddWithValue("@email", data.email);
				cmd.Parameters.AddWithValue("@transaction_email", data.transaction_email);
				cmd.Parameters.AddWithValue("@credit_limit_safe", data.credit_limit_safe);
				cmd.Parameters.AddWithValue("@rowstamp", data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.ExecuteNonQuery();
			}
		}
	}
}
