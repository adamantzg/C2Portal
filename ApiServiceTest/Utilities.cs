using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiServiceTest
{
	public class Utilities
	{
		public static PropertyInfo GetProperty(object obj, string propertyName)
		{
			return obj.GetType().GetProperty(propertyName);
		}

		
	}
}
