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
	public class opheadm
	{
		public opheadm(string order_no = " ", string alpha = " ", string customer = " ", string address1 = " ",
			string address2 = " ", string address3 = " ", string address4 = " ", string address5 = " ",
			string address6 = " ", string town_city = " ", string county = " ", string state_region = " ",
			string iso_country_code = " ", string country = " ",
			string invoice_customer = " ", string statement_customer = " ", string group_customer = " ",
			string date_entered = null, string date_received = null, string date_required = null,
			string date_despatched = null, string invoice_due_date = null, string customer_order_no = " ",
			string territory = " ", string className = " ", string region = " ",
			string cust_disc_code = " ", string customer_vat_code = " ", string credit_category = " ",
			string status = " ", string hold_indicator = " ", string ack_sent_indicator = " ",
			string invoice_date = null, string invoice_no = " ", string indicators1 = " ", string indicators2 = " ",
			string price_list = " ", double order_discount = 0, double credit_limit = 0,
			double settlement_discoun = 0, double hdcomm_unused = 0, double spare1 = 0, double spare2 = 0,
			int settlement_days = 0, string labels = " ", string warehouse = " ",
			string wo_order_flag = " ", string pick_list = " ", string vehicle_reference = " ",
			string spare_field = " ", string consolidation_flag = " ", string transaction_anals1 = " ",
			string transaction_anals2 = " ", string transaction_anals3 = " ", string back_to_back_ind = " ",
			string vat_method = " ", string po_allocated_flag = " ", string quote_proforma = " ",
			string pricing_date = null, string consol_invoice_no = " ", string comment_only_flag = " ",
			string direct_debit_inv = " ", string invoice_required = " ", string vat_type = " ", string lang = " ",
			string delivery_method = " ", string carrier_code = " ", string payment_method = " ",
			string customer_class = " ", string sales_type = " ", double lower_value = 0, string payment_date_1 = " ",
			string payment_date_2 = " ", string payment_date_3 = " ", string court_charges = " ", string shipno = " ",
			string date_promised = null, string from_indicator = " ", string from_reference = " ",
			string sl_batch = " ", string sl_period = " ", string sl_year = " ", string payer = " ",
			string responsibility = " ", string schedule_number = " ", string to_reference = " ",
			string payment_terms_desc = " ", string shipping_note_ind = " ", string delivery_reason = " ",
			string shipper_code1 = " ", string shipper_code2 = " ", string shipper_code3 = " ",
			string shipping_text = " ", string appearance = " ", int total_packages = 0, double total_ship_weight = 0,
			string shipping_site = " ", string sett_category = " ", int settlement_days2 = 0,
			int settlement_days3 = 0, int settlement_days4 = 0, double settlement_disc2 = 0,
			double settlement_disc3 = 0, double settlement_disc4 = 0, string short_name = " ",
			string shipping_dept = " ",
			string effective_date = null, string period = " ", string slyear = " ", double nett_value = 0,
			double nett_base = 0, double gross_value = 0, double gross_base = 0, double order_cost = 0,
			double cost_base = 0, double discount_base = 0, string credit_point = " ", string credit_point_x = " ",
			byte[] rowstamp = null, int rowid = 0)
		{
			this.order_no = order_no;
			this.alpha = alpha;
			this.customer = customer;
			this.address1 = address1;
			this.address2 = address2;
			this.address3 = address3;
			this.address4 = address4;
			this.address5 = address5;
			this.address6 = address6;
			this.town_city = town_city;
			this.county = county;
			this.state_region = state_region;
			this.iso_country_code = iso_country_code;
			this.country = country;
			this.invoice_customer = invoice_customer;
			this.statement_customer = statement_customer;
			this.group_customer = group_customer;
			this.date_entered = date_entered;
			this.date_received = date_received;
			this.date_required = date_required;
			this.date_despatched = date_despatched;
			this.invoice_due_date = invoice_due_date;
			this.customer_order_no = customer_order_no;
			this.territory = territory;
			this.className = className;
			this.region = region;
			this.cust_disc_code = cust_disc_code;
			this.customer_vat_code = customer_vat_code;
			this.credit_category = credit_category;
			this.status = status;
			this.hold_indicator = hold_indicator;
			this.ack_sent_indicator = ack_sent_indicator;
			this.invoice_date = invoice_date;
			this.invoice_no = invoice_no;
			this.indicators1 = indicators1;
			this.indicators2 = indicators2;
			this.price_list = price_list;
			this.order_discount = order_discount;
			this.credit_limit = credit_limit;
			this.settlement_discoun = settlement_discoun;
			this.hdcomm_unused = hdcomm_unused;
			this.spare1 = spare1;
			this.spare2 = spare2;
			this.settlement_days = settlement_days;
			this.labels = labels;
			this.warehouse = warehouse;
			this.wo_order_flag = wo_order_flag;
			this.pick_list = pick_list;
			this.vehicle_reference = vehicle_reference;
			this.spare_field = spare_field;
			this.consolidation_flag = consolidation_flag;
			this.transaction_anals1 = transaction_anals1;
			this.transaction_anals2 = transaction_anals2;
			this.transaction_anals3 = transaction_anals3;
			this.back_to_back_ind = back_to_back_ind;
			this.vat_method = vat_method;
			this.po_allocated_flag = po_allocated_flag;
			this.quote_proforma = quote_proforma;
			this.pricing_date = pricing_date;
			this.consol_invoice_no = consol_invoice_no;
			this.comment_only_flag = comment_only_flag;
			this.direct_debit_inv = direct_debit_inv;
			this.invoice_required = invoice_required;
			this.vat_type = vat_type;
			this.lang = lang;
			this.delivery_method = delivery_method;
			this.carrier_code = carrier_code;
			this.payment_method = payment_method;
			this.customer_class = customer_class;
			this.sales_type = sales_type;
			this.lower_value = lower_value;
			this.payment_date_1 = payment_date_1;
			this.payment_date_2 = payment_date_2;
			this.payment_date_3 = payment_date_3;
			this.court_charges = court_charges;
			this.shipno = shipno;
			this.date_promised = date_promised;
			this.from_indicator = from_indicator;
			this.from_reference = from_reference;
			this.sl_batch = sl_batch;
			this.sl_period = sl_period;
			this.sl_year = sl_year;
			this.payer = payer;
			this.responsibility = responsibility;
			this.schedule_number = schedule_number;
			this.to_reference = to_reference;
			this.payment_terms_desc = payment_terms_desc;
			this.shipping_note_ind = shipping_note_ind;
			this.delivery_reason = delivery_reason;
			this.shipper_code1 = shipper_code1;
			this.shipper_code2 = shipper_code2;
			this.shipper_code3 = shipper_code3;
			this.shipping_text = shipping_text;
			this.appearance = appearance;
			this.total_packages = total_packages;
			this.total_ship_weight = total_ship_weight;
			this.shipping_site = shipping_site;
			this.sett_category = sett_category;
			this.settlement_days2 = settlement_days2;
			this.settlement_days3 = settlement_days3;
			this.settlement_days4 = settlement_days4;
			this.settlement_disc2 = settlement_disc2;
			this.settlement_disc3 = settlement_disc3;
			this.settlement_disc4 = settlement_disc4;
			this.short_name = short_name;
			this.shipping_dept = shipping_dept;
			this.effective_date = effective_date;
			this.period = period;
			this.slyear = slyear;
			this.nett_value = nett_value;
			this.nett_base = nett_base;
			this.gross_value = gross_value;
			this.gross_base = gross_base;
			this.order_cost = order_cost;
			this.cost_base = cost_base;
			this.discount_base = discount_base;
			this.credit_point = credit_point;
			this.credit_point_x = credit_point_x;
			this.rowstamp = rowstamp;
			this.rowid = rowid;
		}

		[Required][MaxLength(10)] public string order_no { get; set; }

		[Required] [MaxLength(8)] public string alpha { get; set; }

		[Required] [MaxLength(8)] public string customer { get; set; }

		[Required] [MaxLength(64)] public string address1 { get; set; }

		[Required] [MaxLength(64)] public string address2 { get; set; }

		[Required] [MaxLength(64)] public string address3 { get; set; }

		[Required] [MaxLength(64)] public string address4 { get; set; }

		[Required] [MaxLength(64)] public string address5 { get; set; }

		[Required] [MaxLength(64)] public string address6 { get; set; }

		[Required] [MaxLength(64)] public string town_city { get; set; }

		[Required] [MaxLength(64)] public string county { get; set; }

		[Required] [MaxLength(64)] public string state_region { get; set; }

		[Required] [MaxLength(2)] public string iso_country_code { get; set; }

		[Required] [MaxLength(64)] public string country { get; set; }

		[Required] [MaxLength(8)] public string invoice_customer { get; set; }

		[Required] [MaxLength(8)] public string statement_customer { get; set; }

		[Required] [MaxLength(8)] public string group_customer { get; set; }

		[MaxLength(29)] public string date_entered { get; set; }

		[MaxLength(29)] public string date_received { get; set; }

		[MaxLength(29)] public string date_required { get; set; }

		[MaxLength(29)] public string date_despatched { get; set; }

		[MaxLength(29)] public string invoice_due_date { get; set; }

		[Required] [MaxLength(20)] public string customer_order_no { get; set; }

		[Required] [MaxLength(6)] public string territory { get; set; }

		[Required] [MaxLength(6)] public string className { get; set; }

		[Required] [MaxLength(6)] public string region { get; set; }

		[Required] [MaxLength(4)] public string cust_disc_code { get; set; }

		[Required] [MaxLength(1)] public string customer_vat_code { get; set; }

		[Required] [MaxLength(1)] public string credit_category { get; set; }

		[Required] [MaxLength(1)] public string status { get; set; }

		[Required] [MaxLength(1)] public string hold_indicator { get; set; }

		[Required] [MaxLength(1)] public string ack_sent_indicator { get; set; }

		[MaxLength(29)] public string invoice_date { get; set; }

		[Required] [MaxLength(10)] public string invoice_no { get; set; }

		[Required] [MaxLength(1)] public string indicators1 { get; set; }

		[Required] [MaxLength(1)] public string indicators2 { get; set; }

		[Required] [MaxLength(3)] public string price_list { get; set; }

		[Required] public double order_discount { get; set; }

		[Required] public double credit_limit { get; set; }

		[Required] public double settlement_discoun { get; set; }

		[Required] public double hdcomm_unused { get; set; }

		[Required] public double spare1 { get; set; }

		[Required] public double spare2 { get; set; }

		[Required] public int settlement_days { get; set; }

		[Required] [MaxLength(2)] public string labels { get; set; }

		[Required] [MaxLength(2)] public string warehouse { get; set; }

		[Required] [MaxLength(1)] public string wo_order_flag { get; set; }

		[Required] [MaxLength(6)] public string pick_list { get; set; }

		[Required] [MaxLength(10)] public string vehicle_reference { get; set; }

		[Required] [MaxLength(18)] public string spare_field { get; set; }

		[Required] [MaxLength(1)] public string consolidation_flag { get; set; }

		[Required] [MaxLength(10)] public string transaction_anals1 { get; set; }

		[Required] [MaxLength(10)] public string transaction_anals2 { get; set; }

		[Required] [MaxLength(10)] public string transaction_anals3 { get; set; }

		[Required] [MaxLength(1)] public string back_to_back_ind { get; set; }

		[Required] [MaxLength(1)] public string vat_method { get; set; }

		[Required] [MaxLength(1)] public string po_allocated_flag { get; set; }

		[Required] [MaxLength(1)] public string quote_proforma { get; set; }

		[MaxLength(29)] public string pricing_date { get; set; }

		[Required] [MaxLength(10)] public string consol_invoice_no { get; set; }

		[Required] [MaxLength(1)] public string comment_only_flag { get; set; }

		[Required] [MaxLength(1)] public string direct_debit_inv { get; set; }

		[Required] [MaxLength(1)] public string invoice_required { get; set; }

		[Required] [MaxLength(10)] public string vat_type { get; set; }

		[Required] [MaxLength(10)] public string lang { get; set; }

		[Required] [MaxLength(10)] public string delivery_method { get; set; }

		[Required] [MaxLength(10)] public string carrier_code { get; set; }

		[Required] [MaxLength(2)] public string payment_method { get; set; }

		[Required] [MaxLength(8)] public string customer_class { get; set; }

		[Required] [MaxLength(2)] public string sales_type { get; set; }

		[Required] public double lower_value { get; set; }

		[Required] [MaxLength(2)] public string payment_date_1 { get; set; }

		[Required] [MaxLength(2)] public string payment_date_2 { get; set; }

		[Required] [MaxLength(2)] public string payment_date_3 { get; set; }

		[Required] [MaxLength(1)] public string court_charges { get; set; }

		[Required] [MaxLength(10)] public string shipno { get; set; }

		[MaxLength(29)] public string date_promised { get; set; }

		[Required] [MaxLength(1)] public string from_indicator { get; set; }

		[Required] [MaxLength(10)] public string from_reference { get; set; }

		[Required] [MaxLength(6)] public string sl_batch { get; set; }

		[Required] [MaxLength(2)] public string sl_period { get; set; }

		[Required] [MaxLength(2)] public string sl_year { get; set; }

		[Required] [MaxLength(10)] public string payer { get; set; }

		[Required] [MaxLength(10)] public string responsibility { get; set; }

		[Required] [MaxLength(15)] public string schedule_number { get; set; }

		[Required] [MaxLength(10)] public string to_reference { get; set; }

		[Required] [MaxLength(20)] public string payment_terms_desc { get; set; }

		[Required] [MaxLength(1)] public string shipping_note_ind { get; set; }

		[Required] [MaxLength(10)] public string delivery_reason { get; set; }

		[Required] [MaxLength(10)] public string shipper_code1 { get; set; }

		[Required] [MaxLength(10)] public string shipper_code2 { get; set; }

		[Required] [MaxLength(10)] public string shipper_code3 { get; set; }

		[Required] [MaxLength(60)] public string shipping_text { get; set; }

		[Required] [MaxLength(60)] public string appearance { get; set; }

		[Required] public int total_packages { get; set; }

		[Required] public double total_ship_weight { get; set; }

		[Required] [MaxLength(2)] public string shipping_site { get; set; }

		[Required] [MaxLength(2)] public string sett_category { get; set; }

		[Required] public int settlement_days2 { get; set; }

		[Required] public int settlement_days3 { get; set; }

		[Required] public int settlement_days4 { get; set; }

		[Required] public double settlement_disc2 { get; set; }

		[Required] public double settlement_disc3 { get; set; }

		[Required] public double settlement_disc4 { get; set; }

		[Required] [MaxLength(3)] public string short_name { get; set; }

		[Required] [MaxLength(2)] public string shipping_dept { get; set; }

		[MaxLength(29)] public string effective_date { get; set; }

		[Required] [MaxLength(2)] public string period { get; set; }

		[Required] [MaxLength(4)] public string slyear { get; set; }

		[Required] public double nett_value { get; set; }

		[Required] public double nett_base { get; set; }

		[Required] public double gross_value { get; set; }

		[Required] public double gross_base { get; set; }

		[Required] public double order_cost { get; set; }

		[Required] public double cost_base { get; set; }

		[Required] public double discount_base { get; set; }

		[Required] [MaxLength(8)] public string credit_point { get; set; }

		[Required] [MaxLength(1)] public string credit_point_x { get; set; }

		[Required] public byte[] rowstamp { get; set; }

		[Required] public int rowid { get; set; }

		public static void Insert(opheadm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[opheadm](
			[order_no],[alpha],[customer],[address1],[address2],[address3],[address4],[address5],[address6],[town_city],[county],[state_region],[iso_country_code],[country],[invoice_customer],[statement_customer],[group_customer],[date_entered],[date_received],[date_required],[date_despatched],[invoice_due_date],[customer_order_no],[territory],[class],[region],[cust_disc_code],[customer_vat_code],[credit_category],[status],[hold_indicator],[ack_sent_indicator],[invoice_date],[invoice_no],[indicators1],[indicators2],[price_list],[order_discount],[credit_limit],[settlement_discoun],[hdcomm_unused],[spare1],[spare2],[settlement_days],[labels],[warehouse],[wo_order_flag],[pick_list],[vehicle_reference],[spare_field],[consolidation_flag],[transaction_anals1],[transaction_anals2],[transaction_anals3],[back_to_back_ind],[vat_method],[po_allocated_flag],[quote_proforma],[pricing_date],[consol_invoice_no],[comment_only_flag],[direct_debit_inv],[invoice_required],[vat_type],[lang],[delivery_method],[carrier_code],[payment_method],[customer_class],[sales_type],[lower_value],[payment_date_1],[payment_date_2],[payment_date_3],[court_charges],[shipno],[date_promised],[from_indicator],[from_reference],[sl_batch],[sl_period],[sl_year],[payer],[responsibility],[schedule_number],[to_reference],[payment_terms_desc],[shipping_note_ind],[delivery_reason],[shipper_code1],[shipper_code2],[shipper_code3],[shipping_text],[appearance],[total_packages],[total_ship_weight],[shipping_site],[sett_category],[settlement_days2],[settlement_days3],[settlement_days4],[settlement_disc2],[settlement_disc3],[settlement_disc4],[short_name],[shipping_dept],[effective_date],[period],[slyear],[nett_value],[nett_base],[gross_value],[gross_base],[order_cost],[cost_base],[discount_base],[credit_point],[credit_point_x],[rowstamp],[rowid])
			 VALUES (@order_no,@alpha,@customer,@address1,@address2,@address3,@address4,@address5,@address6,@town_city,@county,@state_region,@iso_country_code,@country,@invoice_customer,@statement_customer,@group_customer,@date_entered,@date_received,@date_required,@date_despatched,@invoice_due_date,@customer_order_no,@territory,@class,@region,@cust_disc_code,@customer_vat_code,@credit_category,@status,@hold_indicator,@ack_sent_indicator,@invoice_date,@invoice_no,@indicators1,@indicators2,@price_list,@order_discount,@credit_limit,@settlement_discoun,@hdcomm_unused,@spare1,@spare2,@settlement_days,@labels,@warehouse,@wo_order_flag,@pick_list,@vehicle_reference,@spare_field,@consolidation_flag,@transaction_anals1,@transaction_anals2,@transaction_anals3,@back_to_back_ind,@vat_method,@po_allocated_flag,@quote_proforma,@pricing_date,@consol_invoice_no,@comment_only_flag,@direct_debit_inv,@invoice_required,@vat_type,@lang,@delivery_method,@carrier_code,@payment_method,@customer_class,@sales_type,@lower_value,@payment_date_1,@payment_date_2,@payment_date_3,@court_charges,@shipno,@date_promised,@from_indicator,@from_reference,@sl_batch,@sl_period,@sl_year,@payer,@responsibility,@schedule_number,@to_reference,@payment_terms_desc,@shipping_note_ind,@delivery_reason,@shipper_code1,@shipper_code2,@shipper_code3,@shipping_text,@appearance,@total_packages,@total_ship_weight,@shipping_site,@sett_category,@settlement_days2,@settlement_days3,@settlement_days4,@settlement_disc2,@settlement_disc3,@settlement_disc4,@short_name,@shipping_dept,@effective_date,@period,@slyear,@nett_value,@nett_base,@gross_value,@gross_base,@order_cost,@cost_base,@discount_base,@credit_point,@credit_point_x,@rowstamp,@rowid)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@order_no", data.order_no);
				cmd.Parameters.AddWithValue("@alpha", data.alpha);
				cmd.Parameters.AddWithValue("@customer", data.customer);
				cmd.Parameters.AddWithValue("@address1", data.address1);
				cmd.Parameters.AddWithValue("@address2", data.address2);
				cmd.Parameters.AddWithValue("@address3", data.address3);
				cmd.Parameters.AddWithValue("@address4", data.address4);
				cmd.Parameters.AddWithValue("@address5", data.address5);
				cmd.Parameters.AddWithValue("@address6", data.address6);
				cmd.Parameters.AddWithValue("@town_city", data.town_city);
				cmd.Parameters.AddWithValue("@county", data.county);
				cmd.Parameters.AddWithValue("@state_region", data.state_region);
				cmd.Parameters.AddWithValue("@iso_country_code", data.iso_country_code);
				cmd.Parameters.AddWithValue("@country", data.country);
				cmd.Parameters.AddWithValue("@invoice_customer", data.invoice_customer);
				cmd.Parameters.AddWithValue("@statement_customer", data.statement_customer);
				cmd.Parameters.AddWithValue("@group_customer", data.group_customer);
				cmd.Parameters.AddWithValue("@date_entered", (object) data.date_entered ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@date_received", (object) data.date_received ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@date_required", (object) data.date_required ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@date_despatched", (object) data.date_despatched ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@invoice_due_date", (object) data.invoice_due_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@customer_order_no", data.customer_order_no);
				cmd.Parameters.AddWithValue("@territory", data.territory);
				cmd.Parameters.AddWithValue("@class", data.className);
				cmd.Parameters.AddWithValue("@region", data.region);
				cmd.Parameters.AddWithValue("@cust_disc_code", data.cust_disc_code);
				cmd.Parameters.AddWithValue("@customer_vat_code", data.customer_vat_code);
				cmd.Parameters.AddWithValue("@credit_category", data.credit_category);
				cmd.Parameters.AddWithValue("@status", data.status);
				cmd.Parameters.AddWithValue("@hold_indicator", data.hold_indicator);
				cmd.Parameters.AddWithValue("@ack_sent_indicator", data.ack_sent_indicator);
				cmd.Parameters.AddWithValue("@invoice_date", (object) data.invoice_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@invoice_no", data.invoice_no);
				cmd.Parameters.AddWithValue("@indicators1", data.indicators1);
				cmd.Parameters.AddWithValue("@indicators2", data.indicators2);
				cmd.Parameters.AddWithValue("@price_list", data.price_list);
				cmd.Parameters.AddWithValue("@order_discount", data.order_discount);
				cmd.Parameters.AddWithValue("@credit_limit", data.credit_limit);
				cmd.Parameters.AddWithValue("@settlement_discoun", data.settlement_discoun);
				cmd.Parameters.AddWithValue("@hdcomm_unused", data.hdcomm_unused);
				cmd.Parameters.AddWithValue("@spare1", data.spare1);
				cmd.Parameters.AddWithValue("@spare2", data.spare2);
				cmd.Parameters.AddWithValue("@settlement_days", data.settlement_days);
				cmd.Parameters.AddWithValue("@labels", data.labels);
				cmd.Parameters.AddWithValue("@warehouse", data.warehouse);
				cmd.Parameters.AddWithValue("@wo_order_flag", data.wo_order_flag);
				cmd.Parameters.AddWithValue("@pick_list", data.pick_list);
				cmd.Parameters.AddWithValue("@vehicle_reference", data.vehicle_reference);
				cmd.Parameters.AddWithValue("@spare_field", data.spare_field);
				cmd.Parameters.AddWithValue("@consolidation_flag", data.consolidation_flag);
				cmd.Parameters.AddWithValue("@transaction_anals1", data.transaction_anals1);
				cmd.Parameters.AddWithValue("@transaction_anals2", data.transaction_anals2);
				cmd.Parameters.AddWithValue("@transaction_anals3", data.transaction_anals3);
				cmd.Parameters.AddWithValue("@back_to_back_ind", data.back_to_back_ind);
				cmd.Parameters.AddWithValue("@vat_method", data.vat_method);
				cmd.Parameters.AddWithValue("@po_allocated_flag", data.po_allocated_flag);
				cmd.Parameters.AddWithValue("@quote_proforma", data.quote_proforma);
				cmd.Parameters.AddWithValue("@pricing_date", (object) data.pricing_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@consol_invoice_no", data.consol_invoice_no);
				cmd.Parameters.AddWithValue("@comment_only_flag", data.comment_only_flag);
				cmd.Parameters.AddWithValue("@direct_debit_inv", data.direct_debit_inv);
				cmd.Parameters.AddWithValue("@invoice_required", data.invoice_required);
				cmd.Parameters.AddWithValue("@vat_type", data.vat_type);
				cmd.Parameters.AddWithValue("@lang", data.lang);
				cmd.Parameters.AddWithValue("@delivery_method", data.delivery_method);
				cmd.Parameters.AddWithValue("@carrier_code", data.carrier_code);
				cmd.Parameters.AddWithValue("@payment_method", data.payment_method);
				cmd.Parameters.AddWithValue("@customer_class", data.customer_class);
				cmd.Parameters.AddWithValue("@sales_type", data.sales_type);
				cmd.Parameters.AddWithValue("@lower_value", data.lower_value);
				cmd.Parameters.AddWithValue("@payment_date_1", data.payment_date_1);
				cmd.Parameters.AddWithValue("@payment_date_2", data.payment_date_2);
				cmd.Parameters.AddWithValue("@payment_date_3", data.payment_date_3);
				cmd.Parameters.AddWithValue("@court_charges", data.court_charges);
				cmd.Parameters.AddWithValue("@shipno", data.shipno);
				cmd.Parameters.AddWithValue("@date_promised", (object) data.date_promised ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@from_indicator", data.from_indicator);
				cmd.Parameters.AddWithValue("@from_reference", data.from_reference);
				cmd.Parameters.AddWithValue("@sl_batch", data.sl_batch);
				cmd.Parameters.AddWithValue("@sl_period", data.sl_period);
				cmd.Parameters.AddWithValue("@sl_year", data.sl_year);
				cmd.Parameters.AddWithValue("@payer", data.payer);
				cmd.Parameters.AddWithValue("@responsibility", data.responsibility);
				cmd.Parameters.AddWithValue("@schedule_number", data.schedule_number);
				cmd.Parameters.AddWithValue("@to_reference", data.to_reference);
				cmd.Parameters.AddWithValue("@payment_terms_desc", data.payment_terms_desc);
				cmd.Parameters.AddWithValue("@shipping_note_ind", data.shipping_note_ind);
				cmd.Parameters.AddWithValue("@delivery_reason", data.delivery_reason);
				cmd.Parameters.AddWithValue("@shipper_code1", data.shipper_code1);
				cmd.Parameters.AddWithValue("@shipper_code2", data.shipper_code2);
				cmd.Parameters.AddWithValue("@shipper_code3", data.shipper_code3);
				cmd.Parameters.AddWithValue("@shipping_text", data.shipping_text);
				cmd.Parameters.AddWithValue("@appearance", data.appearance);
				cmd.Parameters.AddWithValue("@total_packages", data.total_packages);
				cmd.Parameters.AddWithValue("@total_ship_weight", data.total_ship_weight);
				cmd.Parameters.AddWithValue("@shipping_site", data.shipping_site);
				cmd.Parameters.AddWithValue("@sett_category", data.sett_category);
				cmd.Parameters.AddWithValue("@settlement_days2", data.settlement_days2);
				cmd.Parameters.AddWithValue("@settlement_days3", data.settlement_days3);
				cmd.Parameters.AddWithValue("@settlement_days4", data.settlement_days4);
				cmd.Parameters.AddWithValue("@settlement_disc2", data.settlement_disc2);
				cmd.Parameters.AddWithValue("@settlement_disc3", data.settlement_disc3);
				cmd.Parameters.AddWithValue("@settlement_disc4", data.settlement_disc4);
				cmd.Parameters.AddWithValue("@short_name", data.short_name);
				cmd.Parameters.AddWithValue("@shipping_dept", data.shipping_dept);
				cmd.Parameters.AddWithValue("@effective_date", (object) data.effective_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@period", data.period);
				cmd.Parameters.AddWithValue("@slyear", data.slyear);
				cmd.Parameters.AddWithValue("@nett_value", data.nett_value);
				cmd.Parameters.AddWithValue("@nett_base", data.nett_base);
				cmd.Parameters.AddWithValue("@gross_value", data.gross_value);
				cmd.Parameters.AddWithValue("@gross_base", data.gross_base);
				cmd.Parameters.AddWithValue("@order_cost", data.order_cost);
				cmd.Parameters.AddWithValue("@cost_base", data.cost_base);
				cmd.Parameters.AddWithValue("@discount_base", data.discount_base);
				cmd.Parameters.AddWithValue("@credit_point", data.credit_point);
				cmd.Parameters.AddWithValue("@credit_point_x", data.credit_point_x);
				cmd.Parameters.AddWithValue("@rowstamp",  data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.ExecuteNonQuery();
			}
		}

	}

}
