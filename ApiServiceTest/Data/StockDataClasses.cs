using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	/// <summary>
	/// Data for free stock tests
	/// </summary>
	public class StockBase: BaseDataClass
	{
		protected override void CleanTables(SqlConnection conn)
		{
			new SqlCommand("DELETE FROM [dbo].[mpcportalprodlist]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [dbo].[PINN_stock3]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[poheadm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[podetm]", conn).ExecuteNonQuery();
		}
	}

	public class StockTablesData : StockBase, IDisposable
	{
		public StockTablesData(IList<Mpcportalprodlist> mpcData, IList<PINNstock3> pinnStock3Data = null, IList<poheadm> poHeaders = null, IList<podetm> poDetails = null)
		{
			Initialize(mpcData, pinnStock3Data, poHeaders, poDetails);
		}

		public StockTablesData(Mpcportalprodlist mpc)
		{
			Initialize(new List<Mpcportalprodlist> {mpc});
		}

		private void Initialize(IList<Mpcportalprodlist> mpcData, IList<PINNstock3> pinnStock3Data = null, IList<poheadm> poHeaders = null, IList<podetm> poDetails = null)
		{
			using (var conn = GetAndOpenConnection())
			{
				CleanTables(conn);

				foreach (var mpc in mpcData)
				{
					Mpcportalprodlist.Insert(mpc, conn);
				}

				if (pinnStock3Data != null)
				{
					foreach (var pd in pinnStock3Data)
					{
						PINNstock3.Insert(pd, conn);
					}
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
				
				
			}
		}

		
	}

	

	

	

	
}
