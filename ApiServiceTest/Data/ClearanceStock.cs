using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C2.Model;
using Dapper;

namespace ApiServiceTest.Data
{
	public class ClearanceStockData : BaseDataClass
	{
		protected override void CleanTables(SqlConnection conn)
		{
			conn.Execute("DELETE FROM dbo.mpclearancestock");
		}

		public ClearanceStockData(IList<mpclearancestock> data)
		{
			Initialize(data);
		}

		private void Initialize(IList<mpclearancestock> data)
		{
			using (var conn = GetAndOpenConnection())
			{
				CleanTables(conn);
				foreach(var r in data)
					mpclearancestock.Insert(r, conn);
			}
			
		}
	}
}
