using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SpeechToText
{

    public class WebApiApplication : System.Web.HttpApplication
    {

        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            logger.Info("Application_BeginRequest");
        }
        protected void Application_Start()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", HttpRuntime.AppDomainAppPath + "DanelASR-d944489007a3.json");
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
