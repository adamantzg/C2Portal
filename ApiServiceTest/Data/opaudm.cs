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
	public class opaudm
	{
		public opaudm(string audit_table = " ", string audit_key = " ", string sequence_number = " ",
			string audit_event = " ", string audit_column = " ", int data_type = 0, string character_value = " ",
			int integer_value = 0, double double_value = 0, string date_value = null, string ddmmyy_value = null,
			string audit_date = null, int audit_time = 0, string audit_user = " ", string audit_program = " ",
			byte[] rowstamp = null, int rowid = 0)
		{
			this.audit_table = audit_table;
			this.audit_key = audit_key;
			this.sequence_number = sequence_number;
			this.audit_event = audit_event;
			this.audit_column = audit_column;
			this.data_type = data_type;
			this.character_value = character_value;
			this.integer_value = integer_value;
			this.double_value = double_value;
			this.date_value = date_value;
			this.ddmmyy_value = ddmmyy_value;
			this.audit_date = audit_date;
			this.audit_time = audit_time;
			this.audit_user = audit_user;
			this.audit_program = audit_program;
			this.rowstamp = rowstamp;
			this.rowid = rowid;
		}

		[Required] [MaxLength(20)] public string audit_table { get; set; }

		[Required] [MaxLength(10)] public string audit_key { get; set; }

		[Required] [MaxLength(12)] public string sequence_number { get; set; }

		[Required] [MaxLength(1)] public string audit_event { get; set; }

		[Required] [MaxLength(20)] public string audit_column { get; set; }

		[Required] public int data_type { get; set; }

		[Required] [MaxLength(40)] public string character_value { get; set; }

		[Required] public int integer_value { get; set; }

		[Required] public double double_value { get; set; }

		[MaxLength(29)] public string date_value { get; set; }

		[MaxLength(29)] public string ddmmyy_value { get; set; }

		[MaxLength(29)] public string audit_date { get; set; }

		[Required] public int audit_time { get; set; }

		[Required] [MaxLength(8)] public string audit_user { get; set; }

		[Required] [MaxLength(32)] public string audit_program { get; set; }

		[Required] public byte[] rowstamp { get; set; }

		[Required] public int rowid { get; set; }

		public static void Insert(opaudm data, SqlConnection conn)
		{
			var sql = @"INSERT INTO [scheme].[opaudm](
			[audit_table],[audit_key],[sequence_number],[audit_event],[audit_column],[data_type],[character_value],[integer_value],[double_value],[date_value],[ddmmyy_value],[audit_date],[audit_time],[audit_user],[audit_program],[rowstamp],[rowid])
			 VALUES (@audit_table,@audit_key,@sequence_number,@audit_event,@audit_column,@data_type,@character_value,@integer_value,@double_value,@date_value,@ddmmyy_value,@audit_date,@audit_time,@audit_user,@audit_program,@rowstamp,@rowid)";
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@audit_table", data.audit_table);
				cmd.Parameters.AddWithValue("@audit_key", data.audit_key);
				cmd.Parameters.AddWithValue("@sequence_number", data.sequence_number);
				cmd.Parameters.AddWithValue("@audit_event", data.audit_event);
				cmd.Parameters.AddWithValue("@audit_column", data.audit_column);
				cmd.Parameters.AddWithValue("@data_type", data.data_type);
				cmd.Parameters.AddWithValue("@character_value", data.character_value);
				cmd.Parameters.AddWithValue("@integer_value", data.integer_value);
				cmd.Parameters.AddWithValue("@double_value", data.double_value);
				cmd.Parameters.AddWithValue("@date_value", (object) data.date_value ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@ddmmyy_value", (object) data.ddmmyy_value ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@audit_date", (object) data.audit_date ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@audit_time", data.audit_time);
				cmd.Parameters.AddWithValue("@audit_user", data.audit_user);
				cmd.Parameters.AddWithValue("@audit_program", data.audit_program);
				cmd.Parameters.AddWithValue("@rowstamp", data.rowstamp ?? SqlBinary.Null);
				cmd.Parameters.AddWithValue("@rowid", data.rowid);
				cmd.ExecuteNonQuery();
			}
		}

	}
}
