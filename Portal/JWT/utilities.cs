using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.JWT
{
	public class JWTUtilities
	{
		public static string Decode(string token, string secret)
		{
			
			IJsonSerializer serializer = new JsonNetSerializer();
			IDateTimeProvider provider = new UtcDateTimeProvider();
			IJwtValidator validator = new JwtValidator(serializer, provider);
			IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
			IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

			return decoder.Decode(token, secret, verify: false);
			
		}

		public static string Encode(Dictionary<string, object> payload, string secret, JwtHashAlgorithm alg)
		{

			IJwtAlgorithm algorithm;
			switch (alg)
			{
				case JwtHashAlgorithm.HS256:
					algorithm = new HMACSHA256Algorithm();
					break;
				case JwtHashAlgorithm.HS384:
					algorithm = new HMACSHA384Algorithm();
					break;
				case JwtHashAlgorithm.HS512:
					algorithm = new HMACSHA512Algorithm();
					break;
				default:
					algorithm = new HMACSHA256Algorithm();
					break;
			}
			IJsonSerializer serializer = new JsonNetSerializer();
			IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
			IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

			return encoder.Encode(payload, secret);
		}
	}
}