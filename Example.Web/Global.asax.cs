using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Example.DAL;
using Example.Services.Context;
using Example.Web.Controllers;
using Example.Web.Infrastructure.Context;
using Example.Web.Infrastructure.Exceptions;
using Newtonsoft.Json.Serialization;

namespace Example.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ExampleContext.Factory = new ExampleContextFactory();
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HttpConfiguration config = GlobalConfiguration.Configuration;
            ((DefaultContractResolver)config.Formatters.JsonFormatter.SerializerSettings.ContractResolver).IgnoreSerializableAttribute = true;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ExampleDbContext, DAL.Migrations.Configuration>());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;

            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var currentController = " ";
            var currentAction = " ";

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var exception = Server.GetLastError();
            var controller = new ErrorController();
            var routeData = new RouteData();
            var action = "Error";
            var statusCode = HttpStatusCode.InternalServerError;

            if (exception.InnerException is AccessDeniedException)
            {
                exception = exception.InnerException;
            }

            HttpException httpException;
            if (exception is AccessDeniedException)
            {
                httpException = new HttpException((int)HttpStatusCode.Forbidden, exception.Message);
            }
            else
            {
                httpException = exception as HttpException;
            }
            if (httpException != null)
            {
                statusCode = (HttpStatusCode)httpException.GetHttpCode();

                if (httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
                    statusCode = HttpStatusCode.RequestEntityTooLarge;

                switch (statusCode)
                {
                    case HttpStatusCode.Forbidden:
                        action = "AccessDenied";
                        break;

                    case HttpStatusCode.NotFound:
                        action = "NotFound";
                        break;

                    case HttpStatusCode.InternalServerError:
                        action = "ServerError";
                        break;

                    case HttpStatusCode.RequestEntityTooLarge:
                        action = "RequestSizeExceededError";
                        break;

                    default:
                        action = "Error";
                        break;
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.TrySkipIisCustomErrors = true;
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            try
            {
                httpContext.Response.ContentType = "text/html; charset=UTF-8";
                if (string.Compare(currentController, "Error", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    controller.ViewData["BeforeErrorRawUrl"] = httpContext.Request.UrlReferrer;
                }
                else
                {
                    controller.ViewData["BeforeErrorRawUrl"] = httpContext.Request.RawUrl;
                }
                controller.ViewData.Model = new HandleErrorInfo(exception, currentController, currentAction);
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));

                if (statusCode == HttpStatusCode.InternalServerError && ExampleContext.Log != null)
                {
                    ExampleContext.Log.ErrorFormat("controller: {0}, action: {1}", controller, action);
                    ExampleContext.Log.Error(exception);
                }
            }
            catch (Exception ex)
            {
                Server.ClearError();
                Response.Redirect("/Error/NotFound");
                ExampleContext.Log.Error(ex);
            }
        }
    }
}
