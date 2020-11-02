using System;
using System.Collections.Generic;
using System.Web.Configuration;
using JWT;
using System.Linq;
using Portal.Model;

namespace Portal.JWT
{
	public class JwtManager
	{
		/// <summary>
		/// Create a Jwt with user information
		/// </summary>
		/// <param name="user"></param>
		/// <param name="dbUser"></param>
		/// <returns></returns>
		public static string CreateToken(User user, int tokenExpirationMinutes = 60)
		{

			var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var expiry = Math.Round((DateTime.UtcNow.AddMinutes(tokenExpirationMinutes) - unixEpoch).TotalSeconds);

			var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
			var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


			var payload = new Dictionary<string, object>
			{
				{"email", user.email},
				{"userId", user.id},
				{"role", string.Join(",",user.Roles.Select(r=>r.name))},
				{"sub", user.id},
				{"nbf", notBefore},
				{"iat", issuedAt}
			};

//#if !DEBUG
			payload.Add("exp", expiry);
//#endif

			var secret = Properties.Settings.Default.jwtKey; //secret key
			//dbUser = new { user.email, user.id };
			var token = JWTUtilities.Encode(payload, secret, JwtHashAlgorithm.HS256);
			return token;
		}
	}


}