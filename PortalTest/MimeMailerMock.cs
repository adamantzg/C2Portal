using AegisImplicitMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PortalTest
{
	public class MimeMailerMock : IMimeMailer
	{
		public bool Sent { get; set; }
		public bool RaiseException { get; set; }

		public NetworkCredential Credentials { get; set; }
		public bool EnableImplicitSsl { get; set; }
		public int Timeout { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public AuthenticationType AuthenticationMode { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public MimeMailMessage MailMessage { get; set; }
		public SslMode SslType { get; set; }
		public bool DsnEnabled { get; }
		public bool ServerSupportsEai { get; }
		public bool SupportsTls { get; }
		public bool InCall { get; }

		public AbstractMailMessage GenerateMail(IMailAddress sender, List<IMailAddress> toAddresses, List<IMailAddress> ccAddresses, List<IMailAddress> bccAddresses,
			List<string> attachmentsList, string subject, string body)
		{
			throw new NotImplementedException();
		}

		public void Send(AbstractMailMessage message, SendCompletedEventHandler onSendCallBack = null)
		{
			Sent = true;
		}

		public void Send(string @from, string recipiant, string subject, string body)
		{
			throw new NotImplementedException();
		}

		public event SendCompletedEventHandler SendCompleted;
		public SslMode DetectSslMode()
		{
			throw new NotImplementedException();
		}

		public bool TestConnection()
		{
			throw new NotImplementedException();
		}

		public void SendMail(AbstractMailMessage message)
		{
			throw new NotImplementedException();
		}

		public void SendMailAsync(AbstractMailMessage message = null)
		{
			if(message is MimeMailMessage)
				MailMessage = message as MimeMailMessage;
			Sent = true;
			if(RaiseException)
				throw new Exception("mock exception");
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
