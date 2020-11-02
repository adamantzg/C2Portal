using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public abstract class BaseDataClass : IDisposable
	{
		protected SqlConnection GetAndOpenConnection()
		{
			var c = new SqlConnection(Properties.Settings.Default.connString);
			c.Open();
			return c;
		}

		public void Dispose()
		{
			using (var conn = GetAndOpenConnection())
			{
				CleanTables(conn);
			}
		}

		protected abstract void CleanTables(SqlConnection conn);
		
	}
}
