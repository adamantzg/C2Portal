using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest.Data
{
	public class SalesDataBase: BaseDataClass
	{
		protected override void CleanTables(SqlConnection conn)
		{
			new SqlCommand("DELETE FROM [dbo].[mporderbookgbp]", conn).ExecuteNonQuery();
			
		}
	}

	public class SalesDataTables : SalesDataBase, IDisposable
	{
		public SalesDataTables(IList<mporderbookgbp> mpoData = null)
		{
			Initialize(mpoData);
		}

		
		private void Initialize(IList<mporderbookgbp> mpoData = null)
		{
			using (var conn = GetAndOpenConnection())
			{
				CleanTables(conn);
				
				if (mpoData != null)
				{
					foreach (var d in mpoData)
					{
						mporderbookgbp.Insert(d, conn);
					}
				}
				
			}
		}

		
	}
}
