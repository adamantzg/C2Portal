using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using KellermanSoftware.CompareNetObjects;
using Portal.JWT;
using Portal.Model;


namespace Portal.Controllers
{
    public class HolidayApiController : BaseApiController
    {
        //PasswordRule rule = new PasswordRule(8, 1);

	    public HolidayApiController(IUnitOfWork unitOfWork,IMailHelper mailHelper, IApiClient apiClient, IMyJwtDecoder jwtDecoder, ICache cache) 
		    : base(unitOfWork, mailHelper, apiClient, jwtDecoder, cache)
	    {
	    }

	    [Route("api/admin/holidays")]
        [HttpGet]
        public object GetHolidays(int year)
        {

            var currUser = GetCurrentUser();
            return uow.HolidayRepository.Get(c => c.date.Year == year);

        }

        [Route("api/admin/insertHoliday")]
        [HttpPost]
        public object InsertHoliday(Holiday holiday)
        {
            var currUser = GetCurrentUser();

            if (!(currUser.isAdmin || currUser.isBranchAdmin))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            uow.HolidayRepository.Insert(holiday);
            uow.Save();
            return true;
            //return null;
        }
        [Route("api/admin/deleteHoliday")]
        [HttpPost]
        public object DeleteHoliday(Holiday holiday)
        {
            var currUser = GetCurrentUser();
            if (!(currUser.isAdmin || currUser.isBranchAdmin))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var test = holiday.date;
            var id = uow.HolidayRepository.Get(c => c.name == holiday.name && c.date == holiday.date).FirstOrDefault()?.id;
            if (id > 0)
                uow.HolidayRepository.DeleteByIds(new[] { id.Value });
            return true;
        }
        
    }

}