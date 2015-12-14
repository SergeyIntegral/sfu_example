using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Example.Core.Consts;
using Example.Services.Context;
using Example.Services.Models;
using Example.Services.Services;

namespace Example.Web.Controllers
{
    [System.Web.Mvc.Authorize(Roles = UserRoles.Administrator + ", " + UserRoles.Moderator)]
	public class SectionController : Controller
	{
		private readonly ISectionService _sectionService;

		public SectionController(ISectionService service)
		{
			_sectionService = service;
		}

        public async Task<ActionResult> Create(int? id)
        {
            if (id == null || id == default(int))
            {
                return View(new ExampleSection());
            }

            var section = await _sectionService.FindByIdAsync(id.Value);
            if (section == null)
            {
                return HttpNotFound();
            }

            var newSection = new ExampleSection
            {
                ParentSectionId = id,
                ParentSection = new ExampleSection(section, false)
            };

            return View(newSection);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create([FromBody]ExampleSection model)
        {
            try
            {
                var id = _sectionService.Add(model);
                if (id == null)
                {
                    return View(model);
                }
                return RedirectToAction("Index", "Forum", new { id = id });
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        public ActionResult Edit(int id)
		{
			var model = _sectionService.Find(id);
		    if (model == null)
		    {
		        return HttpNotFound();
		    }
			return View(model);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult Edit([FromBody] ExampleSection model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_sectionService.Save(model);
					return RedirectToAction("Index", "Forum", new { id = model.Id });
				}
				catch (Exception exception)
				{
                    ExampleContext.Log.Error(exception);
					return View(model);
				}
			}
			return RedirectToAction("Index", "Forum", new { id = model.Id });
		}

		[System.Web.Mvc.HttpPost]
		public JsonResult Delete([FromBody]ExampleSection model)
		{
			try
			{
				//_sectionService.Remove(model.Id);
				return Json(new { state = true });
			}
			catch (Exception exception)
            {
                ExampleContext.Log.Error("SectionController.Delete", exception);
                return Json(new { state = false, message = "Error while deleting section." });
            }
		}
    }
}