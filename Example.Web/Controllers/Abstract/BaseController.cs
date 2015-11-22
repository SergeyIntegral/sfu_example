using System.Web.Mvc;
using Example.Web.Infrastructure.Filters;

namespace Example.Web.Controllers.Abstract
{
    [HandleAllError]
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
#if DEBUG
            ViewBag.Debug = true;
            ViewBag.Release = false;
#else
            ViewBag.Debug = false;
            ViewBag.Release = true;
#endif
        }
    }
}