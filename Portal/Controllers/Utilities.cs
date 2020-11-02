
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.Security;
using System.IO;
using System.Net.Http;

using System.Security.Claims;
using Portal.Models;


namespace Portal
{
    public class Utilities
    {
        
		static TimeZoneInfo TZInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        const string tokenKey = "Authorization";

		static object lockObj = new object();		        

        public static string GetTokenFromRequest(HttpRequestMessage request)
        {
            IEnumerable<string> values;
            string token = null;
            if (request.Headers.TryGetValues(tokenKey, out values))
                token = values.FirstOrDefault();
            if(token == null)
            {
                var qsDict = request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                if (qsDict.ContainsKey(tokenKey))
                    token = qsDict[tokenKey];
            }
            return token;
        }		        

        public static string GetSiteUrl()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }

		public static DateTime? GetUserTime(DateTime? utcTime)
		{
			if (utcTime == null)
				return null;
			return TimeZoneInfo.ConvertTimeFromUtc(utcTime.Value, TZInfo);
		}		
	    
	}

	public interface IEncryption
	{
		string Protect(string unprotectedText, string purpose = "email");
		string UnProtect(string protectedText, string purpose = "email");
	}

	public class Encryption : IEncryption
	{
		public string Protect(string unprotectedText, string purpose = "email")
		{
			var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedText);
			var protectedBytes = MachineKey.Protect(unprotectedBytes, purpose);
			var protectedText = Convert.ToBase64String(protectedBytes);
			return protectedText;
		}

		public string UnProtect(string protectedText, string purpose = "email")
		{
			var protectedBytes = Convert.FromBase64String(protectedText);
			var unprotectedBytes = MachineKey.Unprotect(protectedBytes, purpose);
			var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
			return unprotectedText;
		}
	}

	public static class Extensions
	{
		public static string ToIsoDate(this DateTime? date)
		{
			if (date == null)
				return string.Empty;
			return date.Value.ToString("yyyy-MM-dd HH:mm");
		}
	}
	
}
