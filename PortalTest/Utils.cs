using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Model;

namespace PortalTest
{
	public class Utils
	{
		public static void AssertRequestMessageAndStatus(object result, HttpStatusCode code)
		{
			Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
			var message = result as HttpResponseMessage;
			Assert.AreEqual(code, message?.StatusCode);
		}

		public static string ReadStringFromMessage(HttpResponseMessage message)
		{
			return message.Content.ReadAsStringAsync().Result;
		}

		public static void ParseUrl(string url, out string baseUrl, out Dictionary<string, string> parameters)
		{
			var uri = new Uri(url);
			baseUrl = uri.AbsolutePath;
			var query = uri.Query;
			if (query.FirstOrDefault() == '?')
				query = query.Substring(1);
			parameters = query.Split('&').ToDictionary(s => s.Split('=')[0], s => HttpUtility.UrlDecode(s.Split('=')[1]));
		}

		public static Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStreamWithTimeout();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public static HttpContext GetDummyHttpContext()
		{
			var context = new HttpContext(
				new HttpRequest("", "http://tempuri.org", ""),
				new HttpResponse(new StringWriter())
			);
			return context;
		}

		public static void TestCollection(object collection, int expectedCount, string propName = null, int index = 0, object valueToCompareTo = null)
		{
			var collProp = collection.GetType();
			Assert.IsNotNull(collection);
			Assert.IsNotNull(collProp.GetInterfaces().FirstOrDefault(i => i == typeof(IEnumerable)));
			
			var castCollection = (collection as IEnumerable).Cast<object>().ToList();
			Assert.AreEqual(expectedCount, castCollection.Count);

			if (propName != null)
			{
				Assert.IsTrue(index < castCollection.Count);
				var elem = castCollection[index];
				var prop = elem.GetType().GetProperty(propName);
				Assert.IsNotNull(prop);
				Assert.AreEqual(valueToCompareTo, prop.GetValue(elem));
			}
		}

		public static MockData CreateAdminAndUser()
		{
			return new MockData
			{
				Users = new List<User>
				{
					new User
					{
						id = 1,
						email = "admin",
						customer_code = "c1",
						Roles = new List<Role>
						{
							new Role {id = Role.Admin}
						}
					},
					new User
					{
						id = 2,
						email = "user",
						username = "user",
						customer_code = "c2",
						Roles = new List<Role>
						{
							new Role {id = Role.User}
						}
					}

				}
			};
		}
	}
}
