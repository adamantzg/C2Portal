using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class CustomerTotalsBase: BaseDataClass
	{
		protected override void CleanTables(SqlConnection conn)
		{
			new SqlCommand("DELETE FROM [scheme].[opheadm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[opdetm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[slitemm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[slcustm]", conn).ExecuteNonQuery();
		}
	}

	public class CustomerTotalsData : CustomerTotalsBase
	{
		public CustomerTotalsData(IList<slcustm> custData, IList<slitemm> slitemData = null,
			IList<opheadm> headers = null, IList<opdetm> details = null)
		{
			Initialize(custData, slitemData, headers, details);
		}



		private void Initialize(IList<slcustm> custData, IList<slitemm> slitemData,
			IList<opheadm> headers = null, IList<opdetm> details = null)
		{
			using (var conn = GetAndOpenConnection())
			{
				CleanTables(conn);

				if (slitemData != null)
				{
					foreach (var sd in slitemData)
					{
						slitemm.Insert(sd, conn);
					}
				}
				

				if (custData != null)
				{
					foreach (var c in custData)
					{
						slcustm.Insert(c, conn);
					}
				}

				if (headers != null)
				{
					foreach (var h in headers)
						opheadm.Insert(h, conn);
				}

				if (details != null)
				{
					foreach(var d in details)
						opdetm.Insert(d, conn);
				}
			}
		}
	}
}
