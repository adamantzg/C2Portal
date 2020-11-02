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
	public class poheadm
	{
		public poheadm(string order_no=" ",string alpha=" ",string supplier=" ",string address1=" ",string address2=" ",string address3=" ",
			string address4=" ",string address5=" ",string address6=" ",string town_city=" ",string county=" ",string state_region=" ",string iso_country_code=" ",
			string country=" ",string date_entered=null,string date_required=null,string date_completed=null,string supplier_ref=" ",string currency=" ",string status=" ",
			string hold_indicator=" ",double exchange_rate=0,string parent_order=" ",string mrp_included=" ",string spare=" ",string nl_company=" ",string pl_company=" ",
			string committed_flag=" ",string committed_year=" ",string orig_req_no=" ",string amend_req_no=" ",string orig_paper_req_no=" ",string orig_req_id=" ",string tran_code1=" ",
			string tran_code2=" ",string tran_code3=" ",string memo1=" ",string memo2=" ",string last_grn_date=null,string last_inv_log_date=null,string vat_type=" ",string price_list=" ",
			string discount_category=" ",string buyer_id=" ",string delivery_address1=" ",string delivery_address2=" ",string delivery_address3=" ",string delivery_address4=" ",
			string delivery_address5=" ",string delivery_address6=" ",string del_town_city=" ",string del_county=" ",string del_state_region=" ",string del_iso_country_co=" ",
			string del_country=" ",string firm_planned=" ",byte[] rowstamp=null,int rowid=0)

		{
			this.order_no=order_no;
			this.alpha=alpha;
			this.supplier=supplier;
			this.address1=address1;
			this.address2=address2;
			this.address3=address3;
			this.address4=address4;
			this.address5=address5;
			this.address6=address6;
			this.town_city=town_city;
			this.county=county;
			this.state_region=state_region;
			this.iso_country_code=iso_country_code;
			this.country=country;
			this.date_entered=date_entered;
			this.date_required=date_required;
			this.date_completed=date_completed;
			this.supplier_ref=supplier_ref;
			this.currency=currency;
			this.status=status;
			this.hold_indicator=hold_indicator;
			this.exchange_rate=exchange_rate;
			this.parent_order=parent_order;
			this.mrp_included=mrp_included;
			this.spare=spare;
			this.nl_company=nl_company;
			this.pl_company=pl_company;
			this.committed_flag=committed_flag;
			this.committed_year=committed_year;
			this.orig_req_no=orig_req_no;
			this.amend_req_no=amend_req_no;
			this.orig_paper_req_no=orig_paper_req_no;
			this.orig_req_id=orig_req_id;
			this.tran_code1=tran_code1;
			this.tran_code2=tran_code2;
			this.tran_code3=tran_code3;
			this.memo1=memo1;
			this.memo2=memo2;
			this.last_grn_date=last_grn_date;
			this.last_inv_log_date=last_inv_log_date;
			this.vat_type=vat_type;
			this.price_list=price_list;
			this.discount_category=discount_category;
			this.buyer_id=buyer_id;
			this.delivery_address1=delivery_address1;
			this.delivery_address2=delivery_address2;
			this.delivery_address3=delivery_address3;
			this.delivery_address4=delivery_address4;
			this.delivery_address5=delivery_address5;
			this.delivery_address6=delivery_address6;
			this.del_town_city=del_town_city;
			this.del_county=del_county;
			this.del_state_region=del_state_region;
			this.del_iso_country_co=del_iso_country_co;
			this.del_country=del_country;
			this.firm_planned=firm_planned;
			this.rowstamp=rowstamp;
			this.rowid=rowid;
		}

		[Required]
		[MaxLength(10)]
		public string order_no { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public string alpha { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public string supplier { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string address1 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string address2 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string address3 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string address4 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string address5 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string address6 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string town_city { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string county { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string state_region { get; set;}
		 
		[Required]
		[MaxLength(2)]
		public string iso_country_code { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string country { get; set;}
		 
		[MaxLength(29)]
		public string date_entered { get; set;}
		 
		[MaxLength(29)]
		public string date_required { get; set;}
		 
		[MaxLength(29)]
		public string date_completed { get; set;}
		 
		[Required]
		[MaxLength(20)]
		public string supplier_ref { get; set;}
		 
		[Required]
		[MaxLength(3)]
		public string currency { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string status { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string hold_indicator { get; set;}
		 
		[Required]
		public double exchange_rate { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string parent_order { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string mrp_included { get; set;}
		 
		[Required]
		[MaxLength(14)]
		public string spare { get; set;}
		 
		[Required]
		[MaxLength(16)]
		public string nl_company { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public string pl_company { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string committed_flag { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string committed_year { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string orig_req_no { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string amend_req_no { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string orig_paper_req_no { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public string orig_req_id { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string tran_code1 { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string tran_code2 { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string tran_code3 { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string memo1 { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string memo2 { get; set;}
		 
		[MaxLength(29)]
		public string last_grn_date { get; set;}
		 
		[MaxLength(29)]
		public string last_inv_log_date { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string vat_type { get; set;}
		 
		[Required]
		[MaxLength(3)]
		public string price_list { get; set;}
		 
		[Required]
		[MaxLength(4)]
		public string discount_category { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public string buyer_id { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string delivery_address1 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string delivery_address2 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string delivery_address3 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string delivery_address4 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string delivery_address5 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string delivery_address6 { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string del_town_city { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string del_county { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string del_state_region { get; set;}
		 
		[Required]
		[MaxLength(2)]
		public string del_iso_country_co { get; set;}
		 
		[Required]
		[MaxLength(64)]
		public string del_country { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string firm_planned { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public byte[] rowstamp { get; set;}
		 
		[Required]
		public int rowid { get; set;}

		public static void Insert(poheadm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[poheadm](
						[order_no],[alpha],[supplier],[address1],[address2],[address3],[address4],[address5],[address6],[town_city],[county],[state_region],[iso_country_code],[country],[date_entered],[date_required],[date_completed],[supplier_ref],[currency],[status],[hold_indicator],[exchange_rate],[parent_order],[mrp_included],[spare],[nl_company],[pl_company],[committed_flag],[committed_year],[orig_req_no],[amend_req_no],[orig_paper_req_no],[orig_req_id],[tran_code1],[tran_code2],[tran_code3],[memo1],[memo2],[last_grn_date],[last_inv_log_date],[vat_type],[price_list],[discount_category],[buyer_id],[delivery_address1],[delivery_address2],[delivery_address3],[delivery_address4],[delivery_address5],[delivery_address6],[del_town_city],[del_county],[del_state_region],[del_iso_country_co],[del_country],[firm_planned],[rowstamp],[rowid])
						 VALUES (@order_no,@alpha,@supplier,@address1,@address2,@address3,@address4,@address5,@address6,@town_city,@county,@state_region,@iso_country_code,@country,@date_entered,@date_required,@date_completed,@supplier_ref,@currency,@status,@hold_indicator,@exchange_rate,@parent_order,@mrp_included,@spare,@nl_company,@pl_company,@committed_flag,@committed_year,@orig_req_no,@amend_req_no,@orig_paper_req_no,@orig_req_id,@tran_code1,@tran_code2,@tran_code3,@memo1,@memo2,@last_grn_date,@last_inv_log_date,@vat_type,@price_list,@discount_category,@buyer_id,@delivery_address1,@delivery_address2,@delivery_address3,@delivery_address4,@delivery_address5,@delivery_address6,@del_town_city,@del_county,@del_state_region,@del_iso_country_co,@del_country,@firm_planned,@rowstamp,@rowid)
						";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@order_no",data.order_no);
				cmd.Parameters.AddWithValue("@alpha",data.alpha);
				cmd.Parameters.AddWithValue("@supplier",data.supplier);
				cmd.Parameters.AddWithValue("@address1",data.address1);
				cmd.Parameters.AddWithValue("@address2",data.address2);
				cmd.Parameters.AddWithValue("@address3",data.address3);
				cmd.Parameters.AddWithValue("@address4",data.address4);
				cmd.Parameters.AddWithValue("@address5",data.address5);
				cmd.Parameters.AddWithValue("@address6",data.address6);
				cmd.Parameters.AddWithValue("@town_city",data.town_city);
				cmd.Parameters.AddWithValue("@county",data.county);
				cmd.Parameters.AddWithValue("@state_region",data.state_region);
				cmd.Parameters.AddWithValue("@iso_country_code",data.iso_country_code);
				cmd.Parameters.AddWithValue("@country",data.country);
				cmd.Parameters.AddWithValue("@date_entered",(object) data.date_entered ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@date_required",(object) data.date_required ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@date_completed",(object) data.date_completed ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@supplier_ref",data.supplier_ref);
				cmd.Parameters.AddWithValue("@currency",data.currency);
				cmd.Parameters.AddWithValue("@status",data.status);
				cmd.Parameters.AddWithValue("@hold_indicator",data.hold_indicator);
				cmd.Parameters.AddWithValue("@exchange_rate",data.exchange_rate);
				cmd.Parameters.AddWithValue("@parent_order",data.parent_order);
				cmd.Parameters.AddWithValue("@mrp_included",data.mrp_included);
				cmd.Parameters.AddWithValue("@spare",data.spare);
				cmd.Parameters.AddWithValue("@nl_company",data.nl_company);
				cmd.Parameters.AddWithValue("@pl_company",data.pl_company);
				cmd.Parameters.AddWithValue("@committed_flag",data.committed_flag);
				cmd.Parameters.AddWithValue("@committed_year",data.committed_year);
				cmd.Parameters.AddWithValue("@orig_req_no",data.orig_req_no);
				cmd.Parameters.AddWithValue("@amend_req_no",data.amend_req_no);
				cmd.Parameters.AddWithValue("@orig_paper_req_no",data.orig_paper_req_no);
				cmd.Parameters.AddWithValue("@orig_req_id",data.orig_req_id);
				cmd.Parameters.AddWithValue("@tran_code1",data.tran_code1);
				cmd.Parameters.AddWithValue("@tran_code2",data.tran_code2);
				cmd.Parameters.AddWithValue("@tran_code3",data.tran_code3);
				cmd.Parameters.AddWithValue("@memo1",data.memo1);
				cmd.Parameters.AddWithValue("@memo2",data.memo2);
				cmd.Parameters.AddWithValue("@last_grn_date",(object) data.last_grn_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@last_inv_log_date",(object) data.last_inv_log_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@vat_type",data.vat_type);
				cmd.Parameters.AddWithValue("@price_list",data.price_list);
				cmd.Parameters.AddWithValue("@discount_category",data.discount_category);
				cmd.Parameters.AddWithValue("@buyer_id",data.buyer_id);
				cmd.Parameters.AddWithValue("@delivery_address1",data.delivery_address1);
				cmd.Parameters.AddWithValue("@delivery_address2",data.delivery_address2);
				cmd.Parameters.AddWithValue("@delivery_address3",data.delivery_address3);
				cmd.Parameters.AddWithValue("@delivery_address4",data.delivery_address4);
				cmd.Parameters.AddWithValue("@delivery_address5",data.delivery_address5);
				cmd.Parameters.AddWithValue("@delivery_address6",data.delivery_address6);
				cmd.Parameters.AddWithValue("@del_town_city",data.del_town_city);
				cmd.Parameters.AddWithValue("@del_county",data.del_county);
				cmd.Parameters.AddWithValue("@del_state_region",data.del_state_region);
				cmd.Parameters.AddWithValue("@del_iso_country_co",data.del_iso_country_co);
				cmd.Parameters.AddWithValue("@del_country",data.del_country);
				cmd.Parameters.AddWithValue("@firm_planned",data.firm_planned);
				cmd.Parameters.AddWithValue("@rowstamp",(object) data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid",data.rowid);
				cmd.ExecuteNonQuery();
			}
		}
	}

	public class podetm
	{
		public podetm(string order_no=" ",string order_line_no=" ",string line_type=" ",string warehouse=" ",string product=" ",
			string description=" ",string unit_code=" ",string long_description=" ",string nl_category=" ",DateTime? date_required=null,string status=" ",
			DateTime? date_completed=null,string serial_indic=" ",string delivery_no1=" ",string delivery_no2=" ",string delivery_no3=" ",string vat_code=" ",
			double discount=0,double qty_ordered=0,double qty_received=0,double qty_invoiced=0,double weight=0,double local_expect_cost=0,double local_total_expect=0,
			double local_total_actual=0,double foreign_exp_cost=0,double foreign_total_exp=0,double foreign_total_act=0,double alloc_expect_cost=0,double unalloc_qty=0,
			double qty_issued=0,string mrp_flag=" ",double orig_qty=0,string spare=" ",string stock_unit_dp=" ",string trans_unit_dp=" ",string unit_group=" ",string unit_operand=" ",
			double unit_factor=0,double cost_per_unit=0,double tran_orig_orderq=0,double tran_order_qty=0,double tran_recd_qty=0,double tran_inv_qty=0,double tran_exp_cost=0,
			double tran_exp_fcost=0,string level_of_detail=" ",string tran_code1=" ",string tran_code2=" ",string tran_code3=" ",string grn_required=" ",string log_inv_by_qty=" ",
			string new_rec_this_run=" ",double inv_value_logged=0,double inv_value_posted=0,double inv_qty_to_date=0,string new_vat_code=" ",string goods_vat_ind=" ",
			string value_disc_flag=" ",string discount_code=" ",string pricing_unit=" ",string pricing_unit_op=" ",double pricing_unit_fact=0,double discount_tran_u=0,
			double list_cost_stock_u=0,double list_cost_tran_u=0,double list_cost_price_u=0,double cost_price_u=0,double discount_price_u=0,double qty_returned=0,double tran_qty_returned=0,
			DateTime? ship_date=null,string bundle_line_ref=" ",string val_match_tol=" ",byte[] rowstamp= null,int rowid=0)
		{
			this.order_no=order_no;
			this.order_line_no=order_line_no;
			this.line_type=line_type;
			this.warehouse=warehouse;
			this.product=product;
			this.description=description;
			this.unit_code=unit_code;
			this.long_description=long_description;
			this.nl_category=nl_category;
			this.date_required=date_required;
			this.status=status;
			this.date_completed=date_completed;
			this.serial_indic=serial_indic;
			this.delivery_no1=delivery_no1;
			this.delivery_no2=delivery_no2;
			this.delivery_no3=delivery_no3;
			this.vat_code=vat_code;
			this.discount=discount;
			this.qty_ordered=qty_ordered;
			this.qty_received=qty_received;
			this.qty_invoiced=qty_invoiced;
			this.weight=weight;
			this.local_expect_cost=local_expect_cost;
			this.local_total_expect=local_total_expect;
			this.local_total_actual=local_total_actual;
			this.foreign_exp_cost=foreign_exp_cost;
			this.foreign_total_exp=foreign_total_exp;
			this.foreign_total_act=foreign_total_act;
			this.alloc_expect_cost=alloc_expect_cost;
			this.unalloc_qty=unalloc_qty;
			this.qty_issued=qty_issued;
			this.mrp_flag=mrp_flag;
			this.orig_qty=orig_qty;
			this.spare=spare;
			this.stock_unit_dp=stock_unit_dp;
			this.trans_unit_dp=trans_unit_dp;
			this.unit_group=unit_group;
			this.unit_operand=unit_operand;
			this.unit_factor=unit_factor;
			this.cost_per_unit=cost_per_unit;
			this.tran_orig_orderq=tran_orig_orderq;
			this.tran_order_qty=tran_order_qty;
			this.tran_recd_qty=tran_recd_qty;
			this.tran_inv_qty=tran_inv_qty;
			this.tran_exp_cost=tran_exp_cost;
			this.tran_exp_fcost=tran_exp_fcost;
			this.level_of_detail=level_of_detail;
			this.tran_code1=tran_code1;
			this.tran_code2=tran_code2;
			this.tran_code3=tran_code3;
			this.grn_required=grn_required;
			this.log_inv_by_qty=log_inv_by_qty;
			this.new_rec_this_run=new_rec_this_run;
			this.inv_value_logged=inv_value_logged;
			this.inv_value_posted=inv_value_posted;
			this.inv_qty_to_date=inv_qty_to_date;
			this.new_vat_code=new_vat_code;
			this.goods_vat_ind=goods_vat_ind;
			this.value_disc_flag=value_disc_flag;
			this.discount_code=discount_code;
			this.pricing_unit=pricing_unit;
			this.pricing_unit_op=pricing_unit_op;
			this.pricing_unit_fact=pricing_unit_fact;
			this.discount_tran_u=discount_tran_u;
			this.list_cost_stock_u=list_cost_stock_u;
			this.list_cost_tran_u=list_cost_tran_u;
			this.list_cost_price_u=list_cost_price_u;
			this.cost_price_u=cost_price_u;
			this.discount_price_u=discount_price_u;
			this.qty_returned=qty_returned;
			this.tran_qty_returned=tran_qty_returned;
			this.ship_date=ship_date;
			this.bundle_line_ref=bundle_line_ref;
			this.val_match_tol=val_match_tol;
			this.rowstamp=rowstamp;
			this.rowid=rowid;
		}

		[Required]
		[MaxLength(10)]
		public string order_no { get; set;}
		 
		[Required]
		[MaxLength(4)]
		public string order_line_no { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string line_type { get; set;}
		 
		[Required]
		[MaxLength(2)]
		public string warehouse { get; set;}
		 
		[Required]
		[MaxLength(20)]
		public string product { get; set;}
		 
		[Required]
		[MaxLength(20)]
		public string description { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string unit_code { get; set;}
		 
		[Required]
		[MaxLength(40)]
		public string long_description { get; set;}
		 
		[Required]
		[MaxLength(3)]
		public string nl_category { get; set;}
		 
		public DateTime? date_required { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string status { get; set;}
		 
		public DateTime? date_completed { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string serial_indic { get; set;}
		 
		[Required]
		[MaxLength(20)]
		public string delivery_no1 { get; set;}
		 
		[Required]
		[MaxLength(20)]
		public string delivery_no2 { get; set;}
		 
		[Required]
		[MaxLength(20)]
		public string delivery_no3 { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string vat_code { get; set;}
		 
		[Required]
		public double discount { get; set;}
		 
		[Required]
		public double qty_ordered { get; set;}
		 
		[Required]
		public double qty_received { get; set;}
		 
		[Required]
		public double qty_invoiced { get; set;}
		 
		[Required]
		public double weight { get; set;}
		 
		[Required]
		public double local_expect_cost { get; set;}
		 
		[Required]
		public double local_total_expect { get; set;}
		 
		[Required]
		public double local_total_actual { get; set;}
		 
		[Required]
		public double foreign_exp_cost { get; set;}
		 
		[Required]
		public double foreign_total_exp { get; set;}
		 
		[Required]
		public double foreign_total_act { get; set;}
		 
		[Required]
		public double alloc_expect_cost { get; set;}
		 
		[Required]
		public double unalloc_qty { get; set;}
		 
		[Required]
		public double qty_issued { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string mrp_flag { get; set;}
		 
		[Required]
		public double orig_qty { get; set;}
		 
		[Required]
		[MaxLength(20)]
		public string spare { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string stock_unit_dp { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string trans_unit_dp { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string unit_group { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string unit_operand { get; set;}
		 
		[Required]
		public double unit_factor { get; set;}
		 
		[Required]
		public double cost_per_unit { get; set;}
		 
		[Required]
		public double tran_orig_orderq { get; set;}
		 
		[Required]
		public double tran_order_qty { get; set;}
		 
		[Required]
		public double tran_recd_qty { get; set;}
		 
		[Required]
		public double tran_inv_qty { get; set;}
		 
		[Required]
		public double tran_exp_cost { get; set;}
		 
		[Required]
		public double tran_exp_fcost { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string level_of_detail { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string tran_code1 { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string tran_code2 { get; set;}
		 
		[Required]
		[MaxLength(10)]
		public string tran_code3 { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string grn_required { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string log_inv_by_qty { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string new_rec_this_run { get; set;}
		 
		[Required]
		public double inv_value_logged { get; set;}
		 
		[Required]
		public double inv_value_posted { get; set;}
		 
		[Required]
		public double inv_qty_to_date { get; set;}
		 
		[Required]
		[MaxLength(3)]
		public string new_vat_code { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string goods_vat_ind { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string value_disc_flag { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public string discount_code { get; set;}
		 
		[Required]
		[MaxLength(6)]
		public string pricing_unit { get; set;}
		 
		[Required]
		[MaxLength(1)]
		public string pricing_unit_op { get; set;}
		 
		[Required]
		public double pricing_unit_fact { get; set;}
		 
		[Required]
		public double discount_tran_u { get; set;}
		 
		[Required]
		public double list_cost_stock_u { get; set;}
		 
		[Required]
		public double list_cost_tran_u { get; set;}
		 
		[Required]
		public double list_cost_price_u { get; set;}
		 
		[Required]
		public double cost_price_u { get; set;}
		 
		[Required]
		public double discount_price_u { get; set;}
		 
		[Required]
		public double qty_returned { get; set;}
		 
		[Required]
		public double tran_qty_returned { get; set;}
		 
		public DateTime? ship_date { get; set;}
		 
		[Required]
		[MaxLength(5)]
		public string bundle_line_ref { get; set;}
		 
		[Required]
		[MaxLength(30)]
		public string val_match_tol { get; set;}
		 
		[Required]
		[MaxLength(8)]
		public byte[] rowstamp { get; set;}
		 
		[Required]
		public int rowid { get; set;}

		public static void Insert(podetm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[podetm](
					[order_no],[order_line_no],[line_type],[warehouse],[product],[description],[unit_code],[long_description],[nl_category],[date_required],[status],[date_completed],[serial_indic],[delivery_no1],[delivery_no2],[delivery_no3],[vat_code],[discount],[qty_ordered],[qty_received],[qty_invoiced],[weight],[local_expect_cost],[local_total_expect],[local_total_actual],[foreign_exp_cost],[foreign_total_exp],[foreign_total_act],[alloc_expect_cost],[unalloc_qty],[qty_issued],[mrp_flag],[orig_qty],[spare],[stock_unit_dp],[trans_unit_dp],[unit_group],[unit_operand],[unit_factor],[cost_per_unit],[tran_orig_orderq],[tran_order_qty],[tran_recd_qty],[tran_inv_qty],[tran_exp_cost],[tran_exp_fcost],[level_of_detail],[tran_code1],[tran_code2],[tran_code3],[grn_required],[log_inv_by_qty],[new_rec_this_run],[inv_value_logged],[inv_value_posted],[inv_qty_to_date],[new_vat_code],[goods_vat_ind],[value_disc_flag],[discount_code],[pricing_unit],[pricing_unit_op],[pricing_unit_fact],[discount_tran_u],[list_cost_stock_u],[list_cost_tran_u],[list_cost_price_u],[cost_price_u],[discount_price_u],[qty_returned],[tran_qty_returned],[ship_date],[bundle_line_ref],[val_match_tol],[rowstamp],[rowid])
					 VALUES (@order_no,@order_line_no,@line_type,@warehouse,@product,@description,@unit_code,@long_description,@nl_category,@date_required,@status,@date_completed,@serial_indic,@delivery_no1,@delivery_no2,@delivery_no3,@vat_code,@discount,@qty_ordered,@qty_received,@qty_invoiced,@weight,@local_expect_cost,@local_total_expect,@local_total_actual,@foreign_exp_cost,@foreign_total_exp,@foreign_total_act,@alloc_expect_cost,@unalloc_qty,@qty_issued,@mrp_flag,@orig_qty,@spare,@stock_unit_dp,@trans_unit_dp,@unit_group,@unit_operand,@unit_factor,@cost_per_unit,@tran_orig_orderq,@tran_order_qty,@tran_recd_qty,@tran_inv_qty,@tran_exp_cost,@tran_exp_fcost,@level_of_detail,@tran_code1,@tran_code2,@tran_code3,@grn_required,@log_inv_by_qty,@new_rec_this_run,@inv_value_logged,@inv_value_posted,@inv_qty_to_date,@new_vat_code,@goods_vat_ind,@value_disc_flag,@discount_code,@pricing_unit,@pricing_unit_op,@pricing_unit_fact,@discount_tran_u,@list_cost_stock_u,@list_cost_tran_u,@list_cost_price_u,@cost_price_u,@discount_price_u,@qty_returned,@tran_qty_returned,@ship_date,@bundle_line_ref,@val_match_tol,@rowstamp,@rowid)
					";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@order_no",data.order_no);
				cmd.Parameters.AddWithValue("@order_line_no",data.order_line_no);
				cmd.Parameters.AddWithValue("@line_type",data.line_type);
				cmd.Parameters.AddWithValue("@warehouse",data.warehouse);
				cmd.Parameters.AddWithValue("@product",data.product);
				cmd.Parameters.AddWithValue("@description",data.description);
				cmd.Parameters.AddWithValue("@unit_code",data.unit_code);
				cmd.Parameters.AddWithValue("@long_description",data.long_description);
				cmd.Parameters.AddWithValue("@nl_category",data.nl_category);
				cmd.Parameters.AddWithValue("@date_required",(object) data.date_required ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@status",data.status);
				cmd.Parameters.AddWithValue("@date_completed",(object) data.date_completed ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@serial_indic",data.serial_indic);
				cmd.Parameters.AddWithValue("@delivery_no1",data.delivery_no1);
				cmd.Parameters.AddWithValue("@delivery_no2",data.delivery_no2);
				cmd.Parameters.AddWithValue("@delivery_no3",data.delivery_no3);
				cmd.Parameters.AddWithValue("@vat_code",data.vat_code);
				cmd.Parameters.AddWithValue("@discount",data.discount);
				cmd.Parameters.AddWithValue("@qty_ordered",data.qty_ordered);
				cmd.Parameters.AddWithValue("@qty_received",data.qty_received);
				cmd.Parameters.AddWithValue("@qty_invoiced",data.qty_invoiced);
				cmd.Parameters.AddWithValue("@weight",data.weight);
				cmd.Parameters.AddWithValue("@local_expect_cost",data.local_expect_cost);
				cmd.Parameters.AddWithValue("@local_total_expect",data.local_total_expect);
				cmd.Parameters.AddWithValue("@local_total_actual",data.local_total_actual);
				cmd.Parameters.AddWithValue("@foreign_exp_cost",data.foreign_exp_cost);
				cmd.Parameters.AddWithValue("@foreign_total_exp",data.foreign_total_exp);
				cmd.Parameters.AddWithValue("@foreign_total_act",data.foreign_total_act);
				cmd.Parameters.AddWithValue("@alloc_expect_cost",data.alloc_expect_cost);
				cmd.Parameters.AddWithValue("@unalloc_qty",data.unalloc_qty);
				cmd.Parameters.AddWithValue("@qty_issued",data.qty_issued);
				cmd.Parameters.AddWithValue("@mrp_flag",data.mrp_flag);
				cmd.Parameters.AddWithValue("@orig_qty",data.orig_qty);
				cmd.Parameters.AddWithValue("@spare",data.spare);
				cmd.Parameters.AddWithValue("@stock_unit_dp",data.stock_unit_dp);
				cmd.Parameters.AddWithValue("@trans_unit_dp",data.trans_unit_dp);
				cmd.Parameters.AddWithValue("@unit_group",data.unit_group);
				cmd.Parameters.AddWithValue("@unit_operand",data.unit_operand);
				cmd.Parameters.AddWithValue("@unit_factor",data.unit_factor);
				cmd.Parameters.AddWithValue("@cost_per_unit",data.cost_per_unit);
				cmd.Parameters.AddWithValue("@tran_orig_orderq",data.tran_orig_orderq);
				cmd.Parameters.AddWithValue("@tran_order_qty",data.tran_order_qty);
				cmd.Parameters.AddWithValue("@tran_recd_qty",data.tran_recd_qty);
				cmd.Parameters.AddWithValue("@tran_inv_qty",data.tran_inv_qty);
				cmd.Parameters.AddWithValue("@tran_exp_cost",data.tran_exp_cost);
				cmd.Parameters.AddWithValue("@tran_exp_fcost",data.tran_exp_fcost);
				cmd.Parameters.AddWithValue("@level_of_detail",data.level_of_detail);
				cmd.Parameters.AddWithValue("@tran_code1",data.tran_code1);
				cmd.Parameters.AddWithValue("@tran_code2",data.tran_code2);
				cmd.Parameters.AddWithValue("@tran_code3",data.tran_code3);
				cmd.Parameters.AddWithValue("@grn_required",data.grn_required);
				cmd.Parameters.AddWithValue("@log_inv_by_qty",data.log_inv_by_qty);
				cmd.Parameters.AddWithValue("@new_rec_this_run",data.new_rec_this_run);
				cmd.Parameters.AddWithValue("@inv_value_logged",data.inv_value_logged);
				cmd.Parameters.AddWithValue("@inv_value_posted",data.inv_value_posted);
				cmd.Parameters.AddWithValue("@inv_qty_to_date",data.inv_qty_to_date);
				cmd.Parameters.AddWithValue("@new_vat_code",data.new_vat_code);
				cmd.Parameters.AddWithValue("@goods_vat_ind",data.goods_vat_ind);
				cmd.Parameters.AddWithValue("@value_disc_flag",data.value_disc_flag);
				cmd.Parameters.AddWithValue("@discount_code",data.discount_code);
				cmd.Parameters.AddWithValue("@pricing_unit",data.pricing_unit);
				cmd.Parameters.AddWithValue("@pricing_unit_op",data.pricing_unit_op);
				cmd.Parameters.AddWithValue("@pricing_unit_fact",data.pricing_unit_fact);
				cmd.Parameters.AddWithValue("@discount_tran_u",data.discount_tran_u);
				cmd.Parameters.AddWithValue("@list_cost_stock_u",data.list_cost_stock_u);
				cmd.Parameters.AddWithValue("@list_cost_tran_u",data.list_cost_tran_u);
				cmd.Parameters.AddWithValue("@list_cost_price_u",data.list_cost_price_u);
				cmd.Parameters.AddWithValue("@cost_price_u",data.cost_price_u);
				cmd.Parameters.AddWithValue("@discount_price_u",data.discount_price_u);
				cmd.Parameters.AddWithValue("@qty_returned",data.qty_returned);
				cmd.Parameters.AddWithValue("@tran_qty_returned",data.tran_qty_returned);
				cmd.Parameters.AddWithValue("@ship_date",(object) data.ship_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@bundle_line_ref",data.bundle_line_ref);
				cmd.Parameters.AddWithValue("@val_match_tol",data.val_match_tol);
				cmd.Parameters.AddWithValue("@rowstamp",(object) data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid",data.rowid);
				cmd.ExecuteNonQuery();
			}
		}
	}
}
