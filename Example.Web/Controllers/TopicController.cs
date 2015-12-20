using System;
using System.Web.Http;
using System.Web.Mvc;
using Example.Core.Consts;
using Example.Services.Context;
using Example.Services.Models;
using Example.Services.Services;

namespace Example.Web.Controllers
{
    [System.Web.Mvc.Authorize(Roles = UserRoles.Administrator + ", " + UserRoles.Moderator + ", " + UserRoles.User)]
    public class TopicController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly ITopicService _topicService;

        public TopicController(ISectionService sectionService, ITopicService topicService)
        {
            _sectionService = sectionService;
            _topicService = topicService;
        }

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Detail(int id)
        {
            var model = _topicService.Find(id);
            if (model == null)
            {
                return HttpNotFound("Topic not found");
            }

            return View(model);
        }

        public ActionResult Create(int id)
        {
            var section = _sectionService.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }

            var topic = new ExampleTopic
            {
                Status = TopicStatus.Draft,
                SectionId = section.Id,
                Section = section
            };

            return View(topic);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody]ExampleTopic model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Status = TopicStatus.Draft;
                    model.Author = ExampleContext.Current.User;
                    var topicId = _topicService.Add(model);
                    if (topicId != null)
                    {
                        return RedirectToAction("Edit", "Topic", new { id = topicId.Value });
                    }

                    ModelState.AddModelError("SectionId", "SectionId is not valid");
                }
                catch (Exception exception)
                {
                    ExampleContext.Log.Error("TopicController.Create", exception);
                    ModelState.AddModelError(string.Empty, "Unexpected error. Try again.");
                }
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = _topicService.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromBody] ExampleTopic model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Status = User.IsInRole(UserRoles.Administrator) || User.IsInRole(UserRoles.Moderator)
                                 ? TopicStatus.Approved 
                                 : TopicStatus.NotApproved;
                    _topicService.Save(model);
                    return RedirectToAction("Detail", new { id = model.Id });
                }
                catch (Exception exception)
                {
                    ExampleContext.Log.Error(exception);
                    ModelState.AddModelError(string.Empty, "Unexpected error. Try again.");
                }
            }
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete([FromBody]ExampleTopic model)
        {
            try
            {
                _topicService.Remove(model.Id);
                return Json(new { state = true });
            }
            catch (Exception exception)
            {
                ExampleContext.Log.Error("TopicController.Delete", exception);
                return Json(new { state = false, message = "Error while deleting topic." });
            }
        }
    }
}