using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal;
using Portal.Controllers;
using Portal.Model;

namespace PortalTest.Controllers
{
	[TestClass]
	public class HolidayApiControllerTests
	{
		private HolidayApiController controller;
		private UnitOfWorkMock unitOfWork;
		private ApiClientMock apiClient;
		
		[TestInitialize()]
		public void Init()
		{
			unitOfWork = new UnitOfWorkMock();
			apiClient = new ApiClientMock();

			controller = new HolidayApiController(unitOfWork, new MailHelper(new MimeMailerMock(), new RegistryReaderMock()), 
				apiClient, new JwtDecoderMock(), new CacheMock());
			controller.Request = new HttpRequestMessage();
			unitOfWork.Data = Utils.CreateAdminAndUser();
			unitOfWork.Data.Holidays = new List<Holiday>
				{
					new Holiday {id = 1, date = DateTime.Today, name="holiday1"},
					new Holiday {id = 2, date = DateTime.Today.AddDays(2), name="holiday2"},
					new Holiday {id = 3, date = DateTime.Today.AddYears(-1), name="holiday3"}
				};
		}

		[TestMethod, TestCategory("HolidayApiController")]
		public void GetHolidays()
		{
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			var result = controller.GetHolidays(DateTime.Today.Year);
			Utils.TestCollection(result,2);
		}

		[TestMethod, TestCategory("HolidayApiController")]
		public void InsertHoliday()
		{
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			var holiday = new Holiday {date = DateTime.Today.AddDays(1), name = "test"};
			var result = controller.InsertHoliday(holiday);
			Assert.AreEqual(4, unitOfWork.Data.Holidays.Count);
			Assert.IsTrue(unitOfWork.Saved);

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			result = controller.InsertHoliday(holiday);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.Unauthorized);
			
		}

		[TestMethod, TestCategory("HolidayApiController")]
		public void DeleteHoliday()
		{
			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "1");
			
			var holiday = new Holiday {date = DateTime.Today, name = "holiday1"};
			var countBefore = unitOfWork.Data.Holidays.Count;
			var result = controller.DeleteHoliday(holiday);
			Assert.AreEqual(countBefore-1, unitOfWork.Data.Holidays.Count);
			Assert.AreEqual(true, result);

			controller.Request.Headers.Authorization = new AuthenticationHeaderValue("jwt", "2");
			result = controller.InsertHoliday(holiday);
			Utils.AssertRequestMessageAndStatus(result, HttpStatusCode.Unauthorized);
			
		}
	}
}
