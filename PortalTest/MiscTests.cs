using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Portal;
using Portal.JWT;
using Portal.Model;

namespace PortalTest
{
	[TestClass]
	public class MiscTests
	{
		[TestMethod, TestCategory("Misc")]
		public void GetTokenFromRequest()
		{
			var request = new HttpRequestMessage();
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "token");
			var result = Utilities.GetTokenFromRequest(request);
			Assert.AreEqual("Bearer token", result);

			request.Headers.Authorization = null;
			request.RequestUri = new Uri("http://tempuri.org?Authorization=token");
			result = Utilities.GetTokenFromRequest(request);
			Assert.IsNotNull(result);
			Assert.AreEqual("token", result);
		}

		[TestMethod, TestCategory("Misc")]
		public void GetUserTime()
		{
			var dateTime = DateTime.Today.AddHours(DateTime.Now.Hour);
			var utcTime = dateTime.ToUniversalTime();

			var result = Utilities.GetUserTime(utcTime);
			Assert.AreEqual(dateTime, result);
		}

		[TestMethod, TestCategory("Misc")]
		public void DecodeToken()
		{
			var user = new User {id = 3, email = "email", Roles = new List<Role>
			{
				new Role{id = Role.User}
			}};
			var token = JwtManager.CreateToken(user);

			var decoder = new MyJwtDecoder();
			var id = decoder.GetUserIdFromToken(token);
			Assert.AreEqual(user.id, id);
		}

		[TestMethod, TestCategory("Misc")]
		public void MailHelper()
		{
			var mimeMailer = new MimeMailerMock();
			var registryReaderMock = new RegistryReaderMock();
			var mailHelper = new MailHelper(mimeMailer, registryReaderMock);

			var from = "from@test.com";
			var to = "to";
			var subject = "subject";
			var body = "body";

			var debugReceiver = "debugReceiver";
			registryReaderMock.CurrUserValues["HKEY_CURRENT_USER\\BB"] = new Dictionary<string, string>();
			registryReaderMock.CurrUserValues["HKEY_CURRENT_USER\\BB"]["DebugMailReceiver"] = debugReceiver;
			
#if DEBUG
			Assert.ThrowsException<Exception>(() => mailHelper.SendMail(from, to, subject, body));
#else
			Assert.ThrowsException<FormatException>(() => mailHelper.SendMail(from, to, subject, body));
#endif
			

			to = "test@gmail.com";
			var cc = "cc@gmail.com";
			var bcc = "bcc@gmail.com";
			debugReceiver = "debug@gmail.com";
			registryReaderMock.CurrUserValues["HKEY_CURRENT_USER\\BB"]["DebugMailReceiver"] = debugReceiver;
			mailHelper.SendMail(from, to, subject, body);

			var toAddress = mimeMailer.MailMessage?.To?.FirstOrDefault()?.Address;
#if DEBUG
			
			Assert.AreEqual(debugReceiver,toAddress);
#else
			Assert.AreEqual(to, toAddress);
#endif
			Assert.AreEqual(from, mimeMailer.MailMessage?.From?.Address);
			
			Assert.IsTrue(toAddress == to || toAddress == debugReceiver);
			Assert.AreEqual(subject, mimeMailer.MailMessage?.Subject);
			Assert.AreEqual(true,mimeMailer.MailMessage?.Body?.Contains(body));
			Assert.IsTrue(mimeMailer.Sent);

			//Throw exception
			mimeMailer.RaiseException = true;
			Assert.ThrowsException<Exception>(() => mailHelper.SendMail(from, to, subject, body));

			mimeMailer.RaiseException = false;
			var ms = new MemoryStream();
			mailHelper.SendMail(from, to, subject, body,cc, bcc, new[]{new Attachment(ms, new ContentType("text/text")) });
			Assert.AreEqual(1, mimeMailer.MailMessage?.Attachments?.Count);
#if DEBUG
			Assert.AreEqual(true, mimeMailer.MailMessage?.Body?.Contains("Original recipient"));
#endif
			to = "test@gmail.com, current@gmail.com";
			var currentUserMail = "current@gmail.com";
			mailHelper.SendMail(from, to, subject, body,cc, bcc, removeCurrentUser: true, currentUserEmail: currentUserMail);
#if DEBUG
			var originalRecipientsIndex = mimeMailer.MailMessage.Body.IndexOf("Original recipient");
			Assert.IsTrue(originalRecipientsIndex >= 0);
			var ccIndex = mimeMailer.MailMessage.Body.IndexOf("CC:", originalRecipientsIndex + 1);
			Assert.IsTrue(ccIndex >= 0);
			var toString = mimeMailer.MailMessage.Body.Substring(originalRecipientsIndex + 18,
				ccIndex - originalRecipientsIndex - 18);
			Assert.IsTrue(!toString.Contains(currentUserMail));
#else
			Assert.IsNull(mimeMailer.MailMessage.To.FirstOrDefault(a=>a.Address == currentUserMail));
#endif


		}

		[TestMethod, TestCategory("Misc")]
		public void Encrypter()
		{
			var encrypter = new Encryption();
			var text = "text";
			Assert.AreEqual(text, encrypter.UnProtect(encrypter.Protect(text)));
		}
		
		
	}
}
