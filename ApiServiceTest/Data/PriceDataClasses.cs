using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class PriceBase: BaseDataClass
	{
		protected override void CleanTables(SqlConnection conn)
		{
			new SqlCommand("DELETE FROM [dbo].[mpcportalprodlist]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[oplistm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[slcustm]", conn).ExecuteNonQuery();
			new SqlCommand("DELETE FROM [scheme].[opdscntm]", conn).ExecuteNonQuery();
		}
	}

	public class PriceTablesData : PriceBase, IDisposable
	{
		public PriceTablesData(IList<slcustm> custData, IList<Mpcportalprodlist> mpcData,
			IList<oplistm> oplistData = null, IList<opdscntm> opdscData = null)
		{
			Initialize(custData, mpcData, oplistData, opdscData);
		}



		private void Initialize(IList<slcustm> custData, IList<Mpcportalprodlist> mpcData,
			IList<oplistm> oplistData = null, IList<opdscntm> opdscData = null)
		{
			using (var conn = GetAndOpenConnection())
			{
				CleanTables(conn);

				foreach (var mpc in mpcData)
				{
					Mpcportalprodlist.Insert(mpc, conn);
				}

				if (custData != null)
				{
					foreach (var c in custData)
					{
						slcustm.Insert(c, conn);
					}
				}

				if (oplistData != null)
				{
					foreach (var d in oplistData)
						oplistm.Insert(d, conn);
				}

				if (opdscData != null)
				{
					foreach (var d in opdscData)
					{
						opdscntm.Insert(d, conn);
					}
				}


			}
		}


		
	}
}
