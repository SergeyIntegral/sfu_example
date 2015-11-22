using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Example.Services.Context;
using Example.Web.Infrastructure.Exceptions;

namespace Example.Web.Infrastructure.Filters
{
    public class HandleAllErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            Exception exception = filterContext.Exception;
            if (!ExceptionType.IsInstanceOfType(exception))
            {
                return;
            }

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var action = "Error";
            var statusCode = HttpStatusCode.InternalServerError;

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

                    default:
                        action = View;
                        break;
                }
            }
            filterContext.Result = new ViewResult
            {
                ViewName = action,
                MasterName = Master,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.ClearError();
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)statusCode;
            
            if (filterContext.HttpContext.Response.StatusCode == 500)
            {
                ExampleContext.Log.ErrorFormat("controller: {0}, action: {1}", controllerName, actionName);
                ExampleContext.Log.Error(exception);
            }

            // Certain versions of IIS will sometimes use their own error page when
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}