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
	public class cevatm
	{
		public cevatm(string vat_code = " ", string description = " ", double percentage = 0,
			DateTime? vat_expiry_date = null, string new_vat_code = " ", string input_output = " ",
			double tolerance = 0, string control_acct = " ", string difference_acct = " ", string discount_acct = " ",
			string prepayment_acct = " ", string net_acct = " ", string contra_acct = " ", string spare = " ",
			string service_code = " ", string second_vat_code = " ", string unauth_sett_disc = " ",
			string auth_sett_disc = " ", byte[] rowstamp = null, int rowid = 0)
		{
			this.vat_code = vat_code;
			this.description = description;
			this.percentage = percentage;
			this.vat_expiry_date = vat_expiry_date;
			this.new_vat_code = new_vat_code;
			this.input_output = input_output;
			this.tolerance = tolerance;
			this.control_acct = control_acct;
			this.difference_acct = difference_acct;
			this.discount_acct = discount_acct;
			this.prepayment_acct = prepayment_acct;
			this.net_acct = net_acct;
			this.contra_acct = contra_acct;
			this.spare = spare;
			this.service_code = service_code;
			this.second_vat_code = second_vat_code;
			this.unauth_sett_disc = unauth_sett_disc;
			this.auth_sett_disc = auth_sett_disc;
			this.rowstamp = rowstamp;
			this.rowid = rowid;
		}

		[Required] [MaxLength(3)] public string vat_code { get; set; }

		[Required] [MaxLength(30)] public string description { get; set; }

		[Required] public double percentage { get; set; }

		public DateTime? vat_expiry_date { get; set; }

		[Required] [MaxLength(3)] public string new_vat_code { get; set; }

		[Required] [MaxLength(1)] public string input_output { get; set; }

		[Required] public double tolerance { get; set; }

		[Required] [MaxLength(16)] public string control_acct { get; set; }

		[Required] [MaxLength(16)] public string difference_acct { get; set; }

		[Required] [MaxLength(16)] public string discount_acct { get; set; }

		[Required] [MaxLength(16)] public string prepayment_acct { get; set; }

		[Required] [MaxLength(16)] public string net_acct { get; set; }

		[Required] [MaxLength(16)] public string contra_acct { get; set; }

		[Required] [MaxLength(20)] public string spare { get; set; }

		[Required] [MaxLength(20)] public string service_code { get; set; }

		[Required] [MaxLength(3)] public string second_vat_code { get; set; }

		[Required] [MaxLength(16)] public string unauth_sett_disc { get; set; }

		[Required] [MaxLength(16)] public string auth_sett_disc { get; set; }

		public byte[] rowstamp { get; set; }

		[Required] public int rowid { get; set; }

		public static void Insert(cevatm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[cevatm](
			[vat_code],[description],[percentage],[vat_expiry_date],[new_vat_code],[input_output],[tolerance],[control_acct],[difference_acct],[discount_acct],[prepayment_acct],[net_acct],[contra_acct],[spare],[service_code],[second_vat_code],[unauth_sett_disc],[auth_sett_disc],[rowstamp],[rowid])
			 VALUES (@vat_code,@description,@percentage,@vat_expiry_date,@new_vat_code,@input_output,@tolerance,@control_acct,@difference_acct,@discount_acct,@prepayment_acct,@net_acct,@contra_acct,@spare,@service_code,@second_vat_code,@unauth_sett_disc,@auth_sett_disc,@rowstamp,@rowid)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@vat_code", data.vat_code);
				cmd.Parameters.AddWithValue("@description", data.description);
				cmd.Parameters.AddWithValue("@percentage", data.percentage);
				cmd.Parameters.AddWithValue("@vat_expiry_date", (object) data.vat_expiry_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@new_vat_code", data.new_vat_code);
				cmd.Parameters.AddWithValue("@input_output", data.input_output);
				cmd.Parameters.AddWithValue("@tolerance", data.tolerance);
				cmd.Parameters.AddWithValue("@control_acct", data.control_acct);
				cmd.Parameters.AddWithValue("@difference_acct", data.difference_acct);
				cmd.Parameters.AddWithValue("@discount_acct", data.discount_acct);
				cmd.Parameters.AddWithValue("@prepayment_acct", data.prepayment_acct);
				cmd.Parameters.AddWithValue("@net_acct", data.net_acct);
				cmd.Parameters.AddWithValue("@contra_acct", data.contra_acct);
				cmd.Parameters.AddWithValue("@spare", data.spare);
				cmd.Parameters.AddWithValue("@service_code", data.service_code);
				cmd.Parameters.AddWithValue("@second_vat_code", data.second_vat_code);
				cmd.Parameters.AddWithValue("@unauth_sett_disc", data.unauth_sett_disc);
				cmd.Parameters.AddWithValue("@auth_sett_disc", data.auth_sett_disc);
				cmd.Parameters.AddWithValue("@rowstamp", (object) data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.ExecuteNonQuery();
			}
		}

	}
}
