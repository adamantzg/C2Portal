using System;
using System.Data.Entity;
using System.Net;
using AegisImplicitMail;
using ASPPDFLib;
using Portal.Controllers;
using Portal.JWT;
using Portal.Model;
using Unity;
using Unity.Injection;

namespace Portal
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
	        container.RegisterType<AccountApiController>();
	        container.RegisterType<UserApiController>();
	        container.RegisterType<InvoiceHistoryApiController>();
	        container.RegisterType<ProductApiController>();
	        container.RegisterType<HolidayApiController>();
	        container.RegisterType<OrderApiController>();
	        container.RegisterType<PdfController>();
	        container.RegisterType<IUnitOfWork, UnitOfWork>();
	        container.RegisterType<DbContext, Model.Model>(new InjectionConstructor("name=Model"));
	        container.RegisterType<IMailHelper, MailHelper>();
	        container.RegisterType<IApiClient, ApiClient>();
	        container.RegisterType<IWebClient, WebClient>();
	        container.RegisterType<IPdfManager, PdfManagerClass>();
	        container.RegisterType<IMyJwtDecoder, MyJwtDecoder>();
	        container.RegisterType<IEncryption, Encryption>();
	        var host = Properties.Settings.Default.SMTPServer;
	        var port = Properties.Settings.Default.SMTPServerPortNumber;
	        container.RegisterType<IMimeMailer, MimeMailer>(new InjectionConstructor(), new InjectionConstructor(host),
		        new InjectionConstructor(host,port), new InjectionConstructor(host, port, String.Empty, String.Empty, SslMode.None, AuthenticationType.PlainText) );
	        container.RegisterType<IRegistryReader, RegistryReader>();
	        container.RegisterType<ICache, Cache>();

	        /*container.RegisterType<IMimeMailer, MimeMailer>("hostConstructor", new InjectionConstructor(host));
	        container.RegisterType<IMimeMailer, MimeMailer>("hostPortConstructor", new InjectionConstructor(host,port));
	        container.RegisterType<IMimeMailer, MimeMailer>("multiParamConstructor", 
		        new InjectionConstructor(host, port, String.Empty, String.Empty, SslMode.None, AuthenticationType.PlainText));*/
        }
    }
}