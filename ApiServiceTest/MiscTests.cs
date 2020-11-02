using System;
using C2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiServiceTest
{
	[TestClass]
	public class MiscTests
	{
		[TestMethod, TestCategory("C2 model")]
		public void StatusText()
		{
			var order = new Order();
			for (int i = 1; i <= 5; i++)
			{
				order.status = i.ToString();
				Assert.AreEqual("Received", order.statusText);
			}

			order.status = "6";
			Assert.AreEqual("In progress", order.statusText);
			order.status = "7";
			Assert.AreEqual("In progress", order.statusText);
			order.status = "8";
			Assert.AreEqual("Invoiced", order.statusText);
			order.status = "9";
			Assert.AreEqual("Cancelled/Deleted", order.statusText);
		}
	}
}
