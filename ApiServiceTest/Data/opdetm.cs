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
	public class opdetm
	{
		public opdetm(string order_no = " ", string order_line_no = " ", string line_type = " ", string warehouse = " ",
			string product = " ", string description = " ", string unit_of_sale = " ", string order_line_status = " ",
			string bin_number = " ", string long_description = " ", string old_vat_code = " ",
			string discount_code = " ", string nominal_category = " ", string serial_no_indic = " ",
			string product_group_a = " ", string product_group_b = " ", string product_group_c = " ",
			string discount_value_fla = " ", string original_order = " ", double order_qty = 0,
			double allocated_qty = 0, double despatched_qty = 0, double list_price = 0, double net_price = 0,
			double discount = 0, double val = 0, double margin_percent = 0, double cost_of_sale = 0, double weight = 0,
			string wo_make = " ", string vehicle_reference = " ", string spare = " ", string transaction_anals1 = " ",
			string transaction_anals2 = " ", string transaction_anals3 = " ", string sales_account = " ",
			string cos_stock_account = " ", string cos_cost_account = " ", string discount_account = " ",
			string vat_inclusive = " ", string unit_group = " ", string unit_operand = " ", double vat_amount = 0,
			double vat_rate = 0, double original_order_qty = 0, double unit_factor = 0, double price_per_unit = 0,
			string original_order_no = " ", string stock_unit_dp = " ", string transaction_unitdp = " ",
			double trans_orig_qty = 0, double trans_order_qty = 0, double trans_alloc_qty = 0,
			double trans_desp_qty = 0, double trans_list_price = 0, double trans_price = 0, double trans_discount = 0,
			double trans_cost = 0, string job_codes1 = " ", string job_codes2 = " ", string job_codes3 = " ",
			string job_codes4 = " ", string revenue_codes1 = " ", string revenue_codes2 = " ",
			string revenue_codes3 = " ", string revenue_codes4 = " ", string pricing_unit = " ",
			string pricing_unit_op = " ", double pricing_unit_fac = 0, double pricing_list_price = 0,
			double pricing_price = 0, double pricing_discount = 0, string vat_code_new = " ",
			string supplement_vatcode = " ", double supplement_vat_amt = 0, double supplement_vatrate = 0,
			string no_of_labels = " ", string label_type = " ", string bundle_line_ref = " ", string ship_date = null,
			string supplier_code = " ", string packaging = " ", string back_to_back_ind = " ",
			string transfer_order = " ", string pick_list = " ", double pick_quantity = 0, double ent_order_qty = 0,
			string pick_confirmed = " ", string confirmed_by = " ", string confirmed_date = null, string picker = " ",
			string delivery_number = " ", string driver = " ", string delivery_date = null, string van_number = " ",
			byte[] rowstamp = null, int rowid = 0)
		{
			this.order_no = order_no;
			this.order_line_no = order_line_no;
			this.line_type = line_type;
			this.warehouse = warehouse;
			this.product = product;
			this.description = description;
			this.unit_of_sale = unit_of_sale;
			this.order_line_status = order_line_status;
			this.bin_number = bin_number;
			this.long_description = long_description;
			this.old_vat_code = old_vat_code;
			this.discount_code = discount_code;
			this.nominal_category = nominal_category;
			this.serial_no_indic = serial_no_indic;
			this.product_group_a = product_group_a;
			this.product_group_b = product_group_b;
			this.product_group_c = product_group_c;
			this.discount_value_fla = discount_value_fla;
			this.original_order = original_order;
			this.order_qty = order_qty;
			this.allocated_qty = allocated_qty;
			this.despatched_qty = despatched_qty;
			this.list_price = list_price;
			this.net_price = net_price;
			this.discount = discount;
			this.val = val;
			this.margin_percent = margin_percent;
			this.cost_of_sale = cost_of_sale;
			this.weight = weight;
			this.wo_make = wo_make;
			this.vehicle_reference = vehicle_reference;
			this.spare = spare;
			this.transaction_anals1 = transaction_anals1;
			this.transaction_anals2 = transaction_anals2;
			this.transaction_anals3 = transaction_anals3;
			this.sales_account = sales_account;
			this.cos_stock_account = cos_stock_account;
			this.cos_cost_account = cos_cost_account;
			this.discount_account = discount_account;
			this.vat_inclusive = vat_inclusive;
			this.unit_group = unit_group;
			this.unit_operand = unit_operand;
			this.vat_amount = vat_amount;
			this.vat_rate = vat_rate;
			this.original_order_qty = original_order_qty;
			this.unit_factor = unit_factor;
			this.price_per_unit = price_per_unit;
			this.original_order_no = original_order_no;
			this.stock_unit_dp = stock_unit_dp;
			this.transaction_unitdp = transaction_unitdp;
			this.trans_orig_qty = trans_orig_qty;
			this.trans_order_qty = trans_order_qty;
			this.trans_alloc_qty = trans_alloc_qty;
			this.trans_desp_qty = trans_desp_qty;
			this.trans_list_price = trans_list_price;
			this.trans_price = trans_price;
			this.trans_discount = trans_discount;
			this.trans_cost = trans_cost;
			this.job_codes1 = job_codes1;
			this.job_codes2 = job_codes2;
			this.job_codes3 = job_codes3;
			this.job_codes4 = job_codes4;
			this.revenue_codes1 = revenue_codes1;
			this.revenue_codes2 = revenue_codes2;
			this.revenue_codes3 = revenue_codes3;
			this.revenue_codes4 = revenue_codes4;
			this.pricing_unit = pricing_unit;
			this.pricing_unit_op = pricing_unit_op;
			this.pricing_unit_fac = pricing_unit_fac;
			this.pricing_list_price = pricing_list_price;
			this.pricing_price = pricing_price;
			this.pricing_discount = pricing_discount;
			this.vat_code_new = vat_code_new;
			this.supplement_vatcode = supplement_vatcode;
			this.supplement_vat_amt = supplement_vat_amt;
			this.supplement_vatrate = supplement_vatrate;
			this.no_of_labels = no_of_labels;
			this.label_type = label_type;
			this.bundle_line_ref = bundle_line_ref;
			this.ship_date = ship_date;
			this.supplier_code = supplier_code;
			this.packaging = packaging;
			this.back_to_back_ind = back_to_back_ind;
			this.transfer_order = transfer_order;
			this.pick_list = pick_list;
			this.pick_quantity = pick_quantity;
			this.ent_order_qty = ent_order_qty;
			this.pick_confirmed = pick_confirmed;
			this.confirmed_by = confirmed_by;
			this.confirmed_date = confirmed_date;
			this.picker = picker;
			this.delivery_number = delivery_number;
			this.driver = driver;
			this.delivery_date = delivery_date;
			this.van_number = van_number;
			this.rowstamp = rowstamp;
			this.rowid = rowid;
		}

		[Required] [MaxLength(10)] public string order_no { get; set; }

		[Required] [MaxLength(4)] public string order_line_no { get; set; }

		[Required] [MaxLength(1)] public string line_type { get; set; }

		[Required] [MaxLength(2)] public string warehouse { get; set; }

		[Required] [MaxLength(20)] public string product { get; set; }

		[Required] [MaxLength(20)] public string description { get; set; }

		[Required] [MaxLength(10)] public string unit_of_sale { get; set; }

		[Required] [MaxLength(1)] public string order_line_status { get; set; }

		[Required] [MaxLength(10)] public string bin_number { get; set; }

		[Required] [MaxLength(40)] public string long_description { get; set; }

		[Required] [MaxLength(1)] public string old_vat_code { get; set; }

		[Required] [MaxLength(8)] public string discount_code { get; set; }

		[Required] [MaxLength(3)] public string nominal_category { get; set; }

		[Required] [MaxLength(1)] public string serial_no_indic { get; set; }

		[Required] [MaxLength(10)] public string product_group_a { get; set; }

		[Required] [MaxLength(10)] public string product_group_b { get; set; }

		[Required] [MaxLength(10)] public string product_group_c { get; set; }

		[Required] [MaxLength(1)] public string discount_value_fla { get; set; }

		[Required] [MaxLength(10)] public string original_order { get; set; }

		[Required] public double order_qty { get; set; }

		[Required] public double allocated_qty { get; set; }

		[Required] public double despatched_qty { get; set; }

		[Required] public double list_price { get; set; }

		[Required] public double net_price { get; set; }

		[Required] public double discount { get; set; }

		[Required] public double val { get; set; }

		[Required] public double margin_percent { get; set; }

		[Required] public double cost_of_sale { get; set; }

		[Required] public double weight { get; set; }

		[Required] [MaxLength(1)] public string wo_make { get; set; }

		[Required] [MaxLength(10)] public string vehicle_reference { get; set; }

		[Required] [MaxLength(34)] public string spare { get; set; }

		[Required] [MaxLength(10)] public string transaction_anals1 { get; set; }

		[Required] [MaxLength(10)] public string transaction_anals2 { get; set; }

		[Required] [MaxLength(10)] public string transaction_anals3 { get; set; }

		[Required] [MaxLength(16)] public string sales_account { get; set; }

		[Required] [MaxLength(16)] public string cos_stock_account { get; set; }

		[Required] [MaxLength(16)] public string cos_cost_account { get; set; }

		[Required] [MaxLength(16)] public string discount_account { get; set; }

		[Required] [MaxLength(1)] public string vat_inclusive { get; set; }

		[Required] [MaxLength(1)] public string unit_group { get; set; }

		[Required] [MaxLength(1)] public string unit_operand { get; set; }

		[Required] public double vat_amount { get; set; }

		[Required] public double vat_rate { get; set; }

		[Required] public double original_order_qty { get; set; }

		[Required] public double unit_factor { get; set; }

		[Required] public double price_per_unit { get; set; }

		[Required] [MaxLength(10)] public string original_order_no { get; set; }

		[Required] [MaxLength(1)] public string stock_unit_dp { get; set; }

		[Required] [MaxLength(1)] public string transaction_unitdp { get; set; }

		[Required] public double trans_orig_qty { get; set; }

		[Required] public double trans_order_qty { get; set; }

		[Required] public double trans_alloc_qty { get; set; }

		[Required] public double trans_desp_qty { get; set; }

		[Required] public double trans_list_price { get; set; }

		[Required] public double trans_price { get; set; }

		[Required] public double trans_discount { get; set; }

		[Required] public double trans_cost { get; set; }

		[Required] [MaxLength(10)] public string job_codes1 { get; set; }

		[Required] [MaxLength(10)] public string job_codes2 { get; set; }

		[Required] [MaxLength(10)] public string job_codes3 { get; set; }

		[Required] [MaxLength(10)] public string job_codes4 { get; set; }

		[Required] [MaxLength(10)] public string revenue_codes1 { get; set; }

		[Required] [MaxLength(10)] public string revenue_codes2 { get; set; }

		[Required] [MaxLength(10)] public string revenue_codes3 { get; set; }

		[Required] [MaxLength(10)] public string revenue_codes4 { get; set; }

		[Required] [MaxLength(6)] public string pricing_unit { get; set; }

		[Required] [MaxLength(1)] public string pricing_unit_op { get; set; }

		[Required] public double pricing_unit_fac { get; set; }

		[Required] public double pricing_list_price { get; set; }

		[Required] public double pricing_price { get; set; }

		[Required] public double pricing_discount { get; set; }

		[Required] [MaxLength(3)] public string vat_code_new { get; set; }

		[Required] [MaxLength(3)] public string supplement_vatcode { get; set; }

		[Required] public double supplement_vat_amt { get; set; }

		[Required] public double supplement_vatrate { get; set; }

		[Required] [MaxLength(5)] public string no_of_labels { get; set; }

		[Required] [MaxLength(1)] public string label_type { get; set; }

		[Required] [MaxLength(5)] public string bundle_line_ref { get; set; }

		[MaxLength(29)] public string ship_date { get; set; }

		[Required] [MaxLength(8)] public string supplier_code { get; set; }

		[Required] [MaxLength(10)] public string packaging { get; set; }

		[Required] [MaxLength(1)] public string back_to_back_ind { get; set; }

		[Required] [MaxLength(10)] public string transfer_order { get; set; }

		[Required] [MaxLength(6)] public string pick_list { get; set; }

		[Required] public double pick_quantity { get; set; }

		[Required] public double ent_order_qty { get; set; }

		[Required] [MaxLength(1)] public string pick_confirmed { get; set; }

		[Required] [MaxLength(10)] public string confirmed_by { get; set; }

		[MaxLength(29)] public string confirmed_date { get; set; }

		[Required] [MaxLength(15)] public string picker { get; set; }

		[Required] [MaxLength(20)] public string delivery_number { get; set; }

		[Required] [MaxLength(15)] public string driver { get; set; }

		[MaxLength(29)] public string delivery_date { get; set; }

		[Required] [MaxLength(10)] public string van_number { get; set; }

		public byte[] rowstamp { get; set; }

		[Required] public int rowid { get; set; }

		public static void Insert(opdetm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[opdetm](
[order_no],[order_line_no],[line_type],[warehouse],[product],[description],[unit_of_sale],[order_line_status],[bin_number],[long_description],[old_vat_code],[discount_code],[nominal_category],[serial_no_indic],[product_group_a],[product_group_b],[product_group_c],[discount_value_fla],[original_order],[order_qty],[allocated_qty],[despatched_qty],[list_price],[net_price],[discount],[val],[margin_percent],[cost_of_sale],[weight],[wo_make],[vehicle_reference],[spare],[transaction_anals1],[transaction_anals2],[transaction_anals3],[sales_account],[cos_stock_account],[cos_cost_account],[discount_account],[vat_inclusive],[unit_group],[unit_operand],[vat_amount],[vat_rate],[original_order_qty],[unit_factor],[price_per_unit],[original_order_no],[stock_unit_dp],[transaction_unitdp],[trans_orig_qty],[trans_order_qty],[trans_alloc_qty],[trans_desp_qty],[trans_list_price],[trans_price],[trans_discount],[trans_cost],[job_codes1],[job_codes2],[job_codes3],[job_codes4],[revenue_codes1],[revenue_codes2],[revenue_codes3],[revenue_codes4],[pricing_unit],[pricing_unit_op],[pricing_unit_fac],[pricing_list_price],[pricing_price],[pricing_discount],[vat_code_new],[supplement_vatcode],[supplement_vat_amt],[supplement_vatrate],[no_of_labels],[label_type],[bundle_line_ref],[ship_date],[supplier_code],[packaging],[back_to_back_ind],[transfer_order],[pick_list],[pick_quantity],[ent_order_qty],[pick_confirmed],[confirmed_by],[confirmed_date],[picker],[delivery_number],[driver],[delivery_date],[van_number],[rowstamp],[rowid])
 VALUES (@order_no,@order_line_no,@line_type,@warehouse,@product,@description,@unit_of_sale,@order_line_status,@bin_number,@long_description,@old_vat_code,@discount_code,@nominal_category,@serial_no_indic,@product_group_a,@product_group_b,@product_group_c,@discount_value_fla,@original_order,@order_qty,@allocated_qty,@despatched_qty,@list_price,@net_price,@discount,@val,@margin_percent,@cost_of_sale,@weight,@wo_make,@vehicle_reference,@spare,@transaction_anals1,@transaction_anals2,@transaction_anals3,@sales_account,@cos_stock_account,@cos_cost_account,@discount_account,@vat_inclusive,@unit_group,@unit_operand,@vat_amount,@vat_rate,@original_order_qty,@unit_factor,@price_per_unit,@original_order_no,@stock_unit_dp,@transaction_unitdp,@trans_orig_qty,@trans_order_qty,@trans_alloc_qty,@trans_desp_qty,@trans_list_price,@trans_price,@trans_discount,@trans_cost,@job_codes1,@job_codes2,@job_codes3,@job_codes4,@revenue_codes1,@revenue_codes2,@revenue_codes3,@revenue_codes4,@pricing_unit,@pricing_unit_op,@pricing_unit_fac,@pricing_list_price,@pricing_price,@pricing_discount,@vat_code_new,@supplement_vatcode,@supplement_vat_amt,@supplement_vatrate,@no_of_labels,@label_type,@bundle_line_ref,@ship_date,@supplier_code,@packaging,@back_to_back_ind,@transfer_order,@pick_list,@pick_quantity,@ent_order_qty,@pick_confirmed,@confirmed_by,@confirmed_date,@picker,@delivery_number,@driver,@delivery_date,@van_number,@rowstamp,@rowid)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@order_no", data.order_no);
				cmd.Parameters.AddWithValue("@order_line_no", data.order_line_no);
				cmd.Parameters.AddWithValue("@line_type", data.line_type);
				cmd.Parameters.AddWithValue("@warehouse", data.warehouse);
				cmd.Parameters.AddWithValue("@product", data.product);
				cmd.Parameters.AddWithValue("@description", data.description);
				cmd.Parameters.AddWithValue("@unit_of_sale", data.unit_of_sale);
				cmd.Parameters.AddWithValue("@order_line_status", data.order_line_status);
				cmd.Parameters.AddWithValue("@bin_number", data.bin_number);
				cmd.Parameters.AddWithValue("@long_description", data.long_description);
				cmd.Parameters.AddWithValue("@old_vat_code", data.old_vat_code);
				cmd.Parameters.AddWithValue("@discount_code", data.discount_code);
				cmd.Parameters.AddWithValue("@nominal_category", data.nominal_category);
				cmd.Parameters.AddWithValue("@serial_no_indic", data.serial_no_indic);
				cmd.Parameters.AddWithValue("@product_group_a", data.product_group_a);
				cmd.Parameters.AddWithValue("@product_group_b", data.product_group_b);
				cmd.Parameters.AddWithValue("@product_group_c", data.product_group_c);
				cmd.Parameters.AddWithValue("@discount_value_fla", data.discount_value_fla);
				cmd.Parameters.AddWithValue("@original_order", data.original_order);
				cmd.Parameters.AddWithValue("@order_qty", data.order_qty);
				cmd.Parameters.AddWithValue("@allocated_qty", data.allocated_qty);
				cmd.Parameters.AddWithValue("@despatched_qty", data.despatched_qty);
				cmd.Parameters.AddWithValue("@list_price", data.list_price);
				cmd.Parameters.AddWithValue("@net_price", data.net_price);
				cmd.Parameters.AddWithValue("@discount", data.discount);
				cmd.Parameters.AddWithValue("@val", data.val);
				cmd.Parameters.AddWithValue("@margin_percent", data.margin_percent);
				cmd.Parameters.AddWithValue("@cost_of_sale", data.cost_of_sale);
				cmd.Parameters.AddWithValue("@weight", data.weight);
				cmd.Parameters.AddWithValue("@wo_make", data.wo_make);
				cmd.Parameters.AddWithValue("@vehicle_reference", data.vehicle_reference);
				cmd.Parameters.AddWithValue("@spare", data.spare);
				cmd.Parameters.AddWithValue("@transaction_anals1", data.transaction_anals1);
				cmd.Parameters.AddWithValue("@transaction_anals2", data.transaction_anals2);
				cmd.Parameters.AddWithValue("@transaction_anals3", data.transaction_anals3);
				cmd.Parameters.AddWithValue("@sales_account", data.sales_account);
				cmd.Parameters.AddWithValue("@cos_stock_account", data.cos_stock_account);
				cmd.Parameters.AddWithValue("@cos_cost_account", data.cos_cost_account);
				cmd.Parameters.AddWithValue("@discount_account", data.discount_account);
				cmd.Parameters.AddWithValue("@vat_inclusive", data.vat_inclusive);
				cmd.Parameters.AddWithValue("@unit_group", data.unit_group);
				cmd.Parameters.AddWithValue("@unit_operand", data.unit_operand);
				cmd.Parameters.AddWithValue("@vat_amount", data.vat_amount);
				cmd.Parameters.AddWithValue("@vat_rate", data.vat_rate);
				cmd.Parameters.AddWithValue("@original_order_qty", data.original_order_qty);
				cmd.Parameters.AddWithValue("@unit_factor", data.unit_factor);
				cmd.Parameters.AddWithValue("@price_per_unit", data.price_per_unit);
				cmd.Parameters.AddWithValue("@original_order_no", data.original_order_no);
				cmd.Parameters.AddWithValue("@stock_unit_dp", data.stock_unit_dp);
				cmd.Parameters.AddWithValue("@transaction_unitdp", data.transaction_unitdp);
				cmd.Parameters.AddWithValue("@trans_orig_qty", data.trans_orig_qty);
				cmd.Parameters.AddWithValue("@trans_order_qty", data.trans_order_qty);
				cmd.Parameters.AddWithValue("@trans_alloc_qty", data.trans_alloc_qty);
				cmd.Parameters.AddWithValue("@trans_desp_qty", data.trans_desp_qty);
				cmd.Parameters.AddWithValue("@trans_list_price", data.trans_list_price);
				cmd.Parameters.AddWithValue("@trans_price", data.trans_price);
				cmd.Parameters.AddWithValue("@trans_discount", data.trans_discount);
				cmd.Parameters.AddWithValue("@trans_cost", data.trans_cost);
				cmd.Parameters.AddWithValue("@job_codes1", data.job_codes1);
				cmd.Parameters.AddWithValue("@job_codes2", data.job_codes2);
				cmd.Parameters.AddWithValue("@job_codes3", data.job_codes3);
				cmd.Parameters.AddWithValue("@job_codes4", data.job_codes4);
				cmd.Parameters.AddWithValue("@revenue_codes1", data.revenue_codes1);
				cmd.Parameters.AddWithValue("@revenue_codes2", data.revenue_codes2);
				cmd.Parameters.AddWithValue("@revenue_codes3", data.revenue_codes3);
				cmd.Parameters.AddWithValue("@revenue_codes4", data.revenue_codes4);
				cmd.Parameters.AddWithValue("@pricing_unit", data.pricing_unit);
				cmd.Parameters.AddWithValue("@pricing_unit_op", data.pricing_unit_op);
				cmd.Parameters.AddWithValue("@pricing_unit_fac", data.pricing_unit_fac);
				cmd.Parameters.AddWithValue("@pricing_list_price", data.pricing_list_price);
				cmd.Parameters.AddWithValue("@pricing_price", data.pricing_price);
				cmd.Parameters.AddWithValue("@pricing_discount", data.pricing_discount);
				cmd.Parameters.AddWithValue("@vat_code_new", data.vat_code_new);
				cmd.Parameters.AddWithValue("@supplement_vatcode", data.supplement_vatcode);
				cmd.Parameters.AddWithValue("@supplement_vat_amt", data.supplement_vat_amt);
				cmd.Parameters.AddWithValue("@supplement_vatrate", data.supplement_vatrate);
				cmd.Parameters.AddWithValue("@no_of_labels", data.no_of_labels);
				cmd.Parameters.AddWithValue("@label_type", data.label_type);
				cmd.Parameters.AddWithValue("@bundle_line_ref", data.bundle_line_ref);
				cmd.Parameters.AddWithValue("@ship_date", (object) data.ship_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@supplier_code", data.supplier_code);
				cmd.Parameters.AddWithValue("@packaging", data.packaging);
				cmd.Parameters.AddWithValue("@back_to_back_ind", data.back_to_back_ind);
				cmd.Parameters.AddWithValue("@transfer_order", data.transfer_order);
				cmd.Parameters.AddWithValue("@pick_list", data.pick_list);
				cmd.Parameters.AddWithValue("@pick_quantity", data.pick_quantity);
				cmd.Parameters.AddWithValue("@ent_order_qty", data.ent_order_qty);
				cmd.Parameters.AddWithValue("@pick_confirmed", data.pick_confirmed);
				cmd.Parameters.AddWithValue("@confirmed_by", data.confirmed_by);
				cmd.Parameters.AddWithValue("@confirmed_date", (object) data.confirmed_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@picker", data.picker);
				cmd.Parameters.AddWithValue("@delivery_number", data.delivery_number);
				cmd.Parameters.AddWithValue("@driver", data.driver);
				cmd.Parameters.AddWithValue("@delivery_date", (object) data.delivery_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@van_number", data.van_number);
				cmd.Parameters.AddWithValue("@rowstamp", (object) data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.ExecuteNonQuery();
			}
		}

	}
}
