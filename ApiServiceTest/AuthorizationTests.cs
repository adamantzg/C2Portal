using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http;
using ApiService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ApiServiceTest
{
	/// <summary>
	/// Summary description for MiscTests
	/// </summary>
	[TestClass]
	public class AuthorizationTests
	{
		public AuthorizationTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;
		private HttpActionContext actionContext;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		
		private void InitializeContext(List<KeyValuePair<string, string>> headers, bool isAnonymous)
		{
			var attributes = new Collection<AllowAnonymousAttribute>();
			if(isAnonymous)
				attributes.Add(new AllowAnonymousAttribute());

			var controllerDescriptor = new Mock<HttpControllerDescriptor>();
			controllerDescriptor.Setup(d=>d.GetCustomAttributes<AllowAnonymousAttribute>()).Returns(attributes);
			var request = new HttpRequestMessage();
			foreach (var h in headers)
			{
				request.Headers.Add(h.Key, h.Value);
			}
			var controllerContext = new HttpControllerContext
			{
				Request = request,
				ControllerDescriptor = controllerDescriptor.Object
			};

			var actionDescriptor = new Mock<HttpActionDescriptor>();
			actionDescriptor.Setup(d=>d.GetCustomAttributes<AllowAnonymousAttribute>()).Returns(attributes);

			actionContext = new HttpActionContext(controllerContext, actionDescriptor.Object);

		}
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod, TestCategory("Authorization")]
		public void FakeKey()
		{
			//First fake key
			var headers = new List<KeyValuePair<string, string>>{new KeyValuePair<string, string>("apiKey", "XXX")};
			InitializeContext(headers, false);
			
			var attr = new OnlyAuthorizedAttribute();
			attr.OnAuthorization(actionContext);
			Assert.AreEqual(HttpStatusCode.Unauthorized, actionContext.Response.StatusCode);
		}

		[TestMethod, TestCategory("Authorization")]
		public void RegularKey()
		{
			//Regular key
			var headers = new List<KeyValuePair<string, string>>{new KeyValuePair<string, string>("apiKey", Properties.Settings.Default.apiKey)};
			InitializeContext(headers, false);
			var attr = new OnlyAuthorizedAttribute();
			attr.OnAuthorization(actionContext);
			Assert.IsNull(actionContext.Response);
		}

		[TestMethod, TestCategory("Authorization")]
		public void Anonymous()
		{
			//Regular key
			var headers = new List<KeyValuePair<string, string>>{new KeyValuePair<string, string>("apiKey", "XXX")};
			InitializeContext(headers, true);
			var attr = new OnlyAuthorizedAttribute();
			attr.OnAuthorization(actionContext);
			Assert.IsNull(actionContext.Response);
		}
	}
}
