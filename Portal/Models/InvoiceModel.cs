using C2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class InvoiceModel
    {
	    public static int rowsPerPageFull = 25;
	    public static int rowsPerPageFooter = 19;
        public Order Order { get; set; }        
		
    }

	public class InvoiceTableModel
	{
		public int From { get; set; }
		public Order Order { get; set; }
	}
    

}