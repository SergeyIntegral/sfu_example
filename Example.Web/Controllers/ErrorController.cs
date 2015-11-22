using System.Web.Mvc;
using Example.Web.Controllers.Abstract;

namespace Example.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 403 page
        /// </summary>
        /// <returns></returns>
        public ActionResult AccessDenied()
        {
            if (!ViewData.ContainsKey("BeforeErrorRawUrl"))
                ViewData["BeforeErrorRawUrl"] = Request.UrlReferrer;

            if (Response.StatusCode != 403)
                Response.StatusCode = 403;
            return View();
        }

        /// <summary>
        /// 404 page
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            if (!ViewData.ContainsKey("BeforeErrorRawUrl"))
                ViewData["BeforeErrorRawUrl"] = Request.UrlReferrer;

            if (Response.StatusCode != 404)
                Response.StatusCode = 404;
            return View();
        }

        /// <summary>
        /// 500 page
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerError()
        {
            return View();
        }
    }
}