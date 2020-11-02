using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ASPPDFLib;
using C2.Model;
using Portal;
using Portal.Controllers;
using Portal.JWT;

namespace PortalTest
{
	public class MailHelperData
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Cc { get; set; }
		public string Bcc { get; set; }
		public Attachment[] Attachments { get; set; }
		public string SmtpServer { get; set; }
		public string SmtpUserName { get; set; }
		public string SmtpPassword { get; set; }
		public int SmtpServerPortNumber { get; set; }
		public bool RemoveCurrentUser { get; set; }
		public string CurrentUserEmail { get; set; }
	}

	public class MailHelperMock : IMailHelper
	{
		public MailHelperData Data { get; private set; }

		public MailHelperMock()
		{
			Data = new MailHelperData();
		}

		public void ClearData()
		{
			Data = new MailHelperData();
		}

		public void SendMail(string @from, string to, string subject, string body, string cc = null, string bcc = null, 
			Attachment[] attachments = null, string smtpServer = null, string smtpUserName = null, string smtpPassword = null, 
			int smtpServerPortNumber = -1, bool removeCurrentUser = false, string currentUserEmail = null)
		{
			Data.From = @from;
			Data.To = to;
			Data.Subject = subject;
			Data.Body = body;
			Data.Cc = cc;
			Data.Bcc = bcc;
			Data.Attachments = attachments;
			Data.SmtpServer = smtpServer;
			Data.SmtpUserName = smtpUserName;
			Data.SmtpPassword = smtpPassword;
			Data.SmtpServerPortNumber = smtpServerPortNumber;
			Data.RemoveCurrentUser = removeCurrentUser;
			Data.CurrentUserEmail = currentUserEmail;
		}
		
	}

	public class ApiClientMock : IApiClient
	{
		public ApiClientMockData Data { get; set; } = new ApiClientMockData();
		public string BaseUrl { get; set; }
		public Dictionary<string, string> Parameters { get; set; }

		public void AddDefaultRequestHeader(string key, string value)
		{
			
		}

		public Task<HttpResponseMessage> GetAsync(string url)
		{
			var result = new HttpResponseMessage();
			ParseUrl(url);
			var tcs = new TaskCompletionSource<HttpResponseMessage>();
			tcs.SetResult(result);
			
			if (url.Contains("getCustomerTotals"))
			{
				result.Content = new ObjectContent(typeof(CustomerTotals), Data.CustomerTotals, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getInvoices"))
			{
				result.Content = new ObjectContent(typeof(List<Invoice>), Data.Invoices, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getOrders"))
			{
				result.Content = new ObjectContent(typeof(List<Order>), Data.Orders, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getOrderByCriteria"))
			{
				result.Content = new ObjectContent(typeof(Order), Data.Order, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getOrderTotals"))
			{
				result.Content = new ObjectContent(typeof(OrderTotals), Data.OrderTotals, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getOrderDetails"))
			{
				result.Content = new ObjectContent(typeof(List<OrderDetail>), Data.OrderDetails, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getFreeStock"))
			{
				result.Content = new ObjectContent(typeof(StockData), Data.StockData, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getPrice"))
			{
				result.Content = new ObjectContent(typeof(ProductPrices), Data.ProductPrices, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			if (url.Contains("getOrdersForInvoices"))
			{
				result.Content = new ObjectContent(typeof(List<Order>), Data.Orders, new JsonMediaTypeFormatter());
				return tcs.Task;
			}

			return null;
		}

		protected void ParseUrl(string url)
		{
			Utils.ParseUrl(url, out var baseUrl, out var parameters);
			BaseUrl = baseUrl;
			Parameters = parameters;
		}
	}

	public class ApiClientMockData
	{
		public ApiClientMockData()
		{
			Orders = new List<Order>();
			Invoices = new List<Invoice>();
			CustomerTotals = new CustomerTotals();
			Order = new Order();
			OrderTotals = new OrderTotals();
			OrderDetails = new List<OrderDetail>();
			StockData = new StockData();
			ProductPrices = new ProductPrices();
		}

		public List<Order> Orders { get; set; }
		public List<Invoice> Invoices { get; set; }
		public CustomerTotals CustomerTotals { get; set; }
		public Order Order { get; set; }
		public OrderTotals OrderTotals { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }
		public StockData StockData { get; set; }
		public ProductPrices ProductPrices { get; set; }
	}

	public class WebClientMock : IWebClient
	{
		public string Result { get; set; }

		public Stream OpenRead(string url)
		{
			return Utils.GenerateStreamFromString(Result);
		}
	}

	public class PdfManagerMock : IPdfManager
	{
		public PdfDocumentMock Document { get; private set; }

		public string Test()
		{
			throw new NotImplementedException();
		}

		public IPdfDocument CreateDocument(object ID)
		{
			Document = new PdfDocumentMock();
			return Document;
		}

		public void RunAsSystem()
		{
			throw new NotImplementedException();
		}

		public IPdfParam CreateParam(object ParamStr)
		{
			throw new NotImplementedException();
		}

		public IPdfDocument OpenDocument(string Path, object Password)
		{
			throw new NotImplementedException();
		}

		public string FormatDate(DateTime date, string Format)
		{
			throw new NotImplementedException();
		}

		public string FormatNumber(double Number, object Param)
		{
			throw new NotImplementedException();
		}

		public IPdfDocument OpenDocumentBinary(object Blob, object Password)
		{
			throw new NotImplementedException();
		}

		public void SendBinary(string Path, object ContentType, object DispHeader)
		{
			throw new NotImplementedException();
		}

		public string LoadTextFromFile(string Path)
		{
			throw new NotImplementedException();
		}

		public void LogonUser(string Domain, string UserID, string Password, object Flag)
		{
			throw new NotImplementedException();
		}

		public string InjectTextIntoFile(string Path, string Text, int Location)
		{
			throw new NotImplementedException();
		}

		public string Version => throw new NotImplementedException();

		public DateTime Expires => throw new NotImplementedException();

		public string RegKey { set => throw new NotImplementedException(); }
	}

	

	public class JwtDecoderMock : IMyJwtDecoder
	{
		public int? GetUserIdFromToken(string token)
		{
			int id;
			if (int.TryParse(token, out id))
				return id;
			return null;
		}
	}

	public class PdfDocumentMock : IPdfDocument
	{
		public string ImportFromUrlBaseUrl { get; set; }
		public Dictionary<string, string> ImportFromUrlParameters { get; set; } = new Dictionary<string, string>();
		public string ImportFromUrlOptions { get; set; }

		protected void ParseUrl(string url)
		{
			Utils.ParseUrl(url, out var baseUrl, out var parameters);
			ImportFromUrlBaseUrl = baseUrl;
			ImportFromUrlParameters = parameters;
		}

		public string Save(string Path, object Overwrite = null)
		{
			throw new NotImplementedException();
		}

		public void SaveHttp(string DispHeader, object ContentType = null)
		{
			throw new NotImplementedException();
		}

		public void Encrypt(object OwnerPassword = null, object UserPassword = null, object KeyLength = null,
			object Permissions = null)
		{
			throw new NotImplementedException();
		}

		public IPdfTable CreateTable(object Param)
		{
			throw new NotImplementedException();
		}

		public IPdfImage OpenImage(string Path, object Param = null)
		{
			throw new NotImplementedException();
		}

		public object SaveToMemory()
		{
			return new byte[0];
		}

		public IPdfDest CreateDest(IPdfPage Page, object Param = null)
		{
			throw new NotImplementedException();
		}

		public void SetViewerPrefs(object Param)
		{
			throw new NotImplementedException();
		}

		public IPdfGraphics CreateGraphics(object Param)
		{
			throw new NotImplementedException();
		}

		public IPdfAction CreateAction(object Param, string Value)
		{
			throw new NotImplementedException();
		}

		public IPdfAnnot Sign(object CryptoMsg, string Name, object Reason = null, object Location = null, object Param = null)
		{
			throw new NotImplementedException();
		}

		public IPdfSignature VerifySignature(object CryptoMsg)
		{
			throw new NotImplementedException();
		}

		public void AppendDocument(IPdfDocument Doc)
		{
			throw new NotImplementedException();
		}

		public IPdfImage OpenImageBinary(object Blob, object Param = null)
		{
			throw new NotImplementedException();
		}

		public IPdfDocument ExtractPages(object Param)
		{
			throw new NotImplementedException();
		}

		public void Close()
		{
			throw new NotImplementedException();
		}

		public void DebugDumpObject(int ObjNum, int GenNum, string Path)
		{
			throw new NotImplementedException();
		}

		public string ImportFromUrl(string url, object param = null, object Username = null, object Password = null)
		{
			ImportFromUrlOptions = param?.ToString();
			ParseUrl(url);
			return string.Empty;
		}

		public IPdfGraphics CreateGraphicsFromPage(IPdfDocument Doc, int PageIndex)
		{
			throw new NotImplementedException();
		}

		public IPdfColorSpace CreateColorSpace(string Name, object Param = null)
		{
			throw new NotImplementedException();
		}

		public IPdfFunction CreateFunction(object Param)
		{
			throw new NotImplementedException();
		}

		public void SendToPrinter(string Printer, object Param)
		{
			throw new NotImplementedException();
		}

		public IPdfImage OpenUrl(string Url, object Param = null, object Username = null, object Password = null)
		{
			throw new NotImplementedException();
		}

		public IPdfGState CreateGState(object Param)
		{
			throw new NotImplementedException();
		}

		public void AddTemplate(IPdfGraphics Graph, object Param = null)
		{
			throw new NotImplementedException();
		}

		public void ClearTemplates()
		{
			throw new NotImplementedException();
		}

		public void AddOutputIntent(string Condition, string ConditionID, string ProfilePath, int Colors)
		{
			throw new NotImplementedException();
		}

		public IPdfPages Pages { get; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Subject { get; set; }
		public string Creator { get; set; }
		public string Producer { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime ModDate { get; set; }
		public string ID { get; }
		public IPdfFonts Fonts { get; }
		public pdfPageLayout PageLayout { get; set; }
		public pdfPageMode PageMode { get; set; }
		public object OpenAction { get; set; }
		public string Keywords { get; set; }
		public string Version { get; set; }
		public IPdfOutline Outline { get; }
		public bool Encrypted { get; }
		public int KeyLength { get; }
		public int Permissions { get; }
		public string OriginalPath { get; }
		public string Path { get; }
		public string FileName { get; }
		public bool Mac { get; }
		public string OriginalFilename { get; }
		public bool UserPassword { get; }
		public bool OwnerPassword { get; }
		public IPdfForm Form { get; }
		public string OriginalVersion { get; }
		public string MetaData { get; set; }
		public bool OwnerPasswordUsed { get; }
		public bool CompressedXRef { get; set; }
		public IPdfParam ImportInfo { get; }
		public string SignatureInfo { get; }
	}

	
	public class MemoryStreamWithTimeout : MemoryStream
	{
		public override int ReadTimeout { get; set; }
	}

	public class EncrypterMock : IEncryption
	{
		public string Protect(string unprotectedText, string purpose = "email")
		{
			return unprotectedText;
		}

		public string UnProtect(string protectedText, string purpose = "email")
		{
			return protectedText;
		}
	}

	public class RegistryReaderMock : IRegistryReader
	{
		public Dictionary<string, Dictionary<string, string>> CurrUserValues { get; set; } = new Dictionary<string, Dictionary<string, string>>();
		public Dictionary<string, Dictionary<string, string>> HklmValues { get; set; } = new Dictionary<string, Dictionary<string, string>>();

		public object GetValue(string key, string value, object defaultValue = null)
		{
			if (CurrUserValues.ContainsKey(key) && CurrUserValues[key].ContainsKey(value))
				return CurrUserValues[key][value];
			return defaultValue;
		}

		public object GetHKLMValue(string subKey, string name, object defaultValue = null)
		{
			if (HklmValues.ContainsKey(subKey) && HklmValues[subKey].ContainsKey(name))
				return HklmValues[subKey][name];
			return defaultValue;
		}
	}

	public class CacheMock : ICache
	{
		Dictionary<string, object> data = new Dictionary<string, object>();

		public object Get(string key)
		{
			if (data.ContainsKey(key))
				return data[key];
			return null;
		}

		public void Set(string key, object value, DateTime? expiration)
		{
			data[key] = value;
		}

		public void Remove(string key)
		{
			data.Remove(key);
		}
	}
}
