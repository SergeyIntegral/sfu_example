using System.Web.Mvc;
using Example.Services.Services;

namespace Example.Web.Controllers
{
	public class ForumController : Controller
	{
		private readonly ISectionService _sectionService;

		public ForumController(ISectionService service)
		{
			_sectionService = service;
		}

		public ActionResult Index(int? id)
		{
		    ViewBag.IsRootSections = id != null;

		    var sections = id == null 
                         ? _sectionService.GetRootSections() 
                         : _sectionService.Find(id.Value);

		    return View(sections);
		}
    }
}