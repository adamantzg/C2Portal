using System;
using System.Collections.Generic;
using System.Web.Configuration;
using JWT;
using Newtonsoft.Json;
using Portal.Models;

namespace Portal.JWT
{
	public interface IMyJwtDecoder
	{
		int? GetUserIdFromToken(string token);
	}

	/// <summary>
	/// A class to decode the authentication token 
	/// </summary>
	public class MyJwtDecoder : IMyJwtDecoder
	{
		/// <summary>
		/// Get the userid from the token if the token is not expired
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public int? GetUserIdFromToken(string token)
		{
			string key = Properties.Settings.Default.jwtKey;
			
			var decodedToken = JWTUtilities.Decode(token, key);
			var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(decodedToken);
			object userId, exp;
			data.TryGetValue("userId", out userId);
			data.TryGetValue("exp", out exp);
			if(exp != null)
			{
				var validTo = FromUnixTime(long.Parse(exp.ToString()));
				if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
				{
					return null;
				}
			}			
			return Convert.ToInt32(userId);
		}

		private static DateTime FromUnixTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTime);
		}
	}

}