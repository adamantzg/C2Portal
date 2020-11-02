using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class OrdersBase:  BaseDataClass
	{
		protected override void CleanTables(SqlConnection conn)
		{
			new SqlCommand("DELETE FROM [dbo].[mpcportalprodlist]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[opheadm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[opdetm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[opaudm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[podetm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[poheadm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[cevatm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[slcustm]", conn).ExecuteNonQuery();
			
		}
	}

	public class OrdersData : OrdersBase
	{
		public OrdersData(IList<Mpcportalprodlist> mpcData = null, IList<opheadm> headers = null, IList<opdetm> details = null, IList<opaudm> audits = null,
			IList<poheadm> poHeaders = null, IList<podetm> poDetails = null, IList<cevatm> cevatm = null, IList<slcustm> customers = null)
		{
			Initialize(mpcData, headers, details, audits, poHeaders, poDetails, cevatm, customers);
		}

		private void Initialize(IList<Mpcportalprodlist> mpcData, IList<opheadm> headers, IList<opdetm> details, IList<opaudm> audits, 
			IList<poheadm> poHeaders = null, IList<podetm> poDetails = null, IList<cevatm> cevatmList = null, IList<slcustm> customers = null)
		{
			using (var conn = GetAndOpenConnection())
			{
				CleanTables(conn);

				if (mpcData != null)
				{
					foreach (var mpc in mpcData)
					{
						Mpcportalprodlist.Insert(mpc, conn);
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

				if (audits != null)
				{
					foreach(var a in audits)
						opaudm.Insert(a, conn);
				}

				if (poHeaders != null)
				{
					foreach(var h in poHeaders)
						poheadm.Insert(h, conn);
				}

				if (poDetails != null)
				{
					foreach (var d in poDetails)
					{
						podetm.Insert(d, conn);
					}
				}

				if (cevatmList != null)
				{
					foreach (var c in cevatmList)
					{
						cevatm.Insert(c, conn);
					}
				}

				if (customers != null)
				{
					foreach (var c in customers)
					{
						slcustm.Insert(c, conn);
					}
				}

			}
		}
	}
}
