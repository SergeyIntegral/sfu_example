using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Example.Core.Consts;
using Example.Services.Context;
using Example.Services.Models;
using Example.Services.Services;

namespace Example.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly ITopicService _topicService;

        public TopicController(ISectionService sectionService, ITopicService topicService)
        {
            _sectionService = sectionService;
            _topicService = topicService;
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
        public ActionResult Create([FromBody]ExampleTopic model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Author = ExampleContext.Current.User;
                    var topicId = _topicService.Add(model);
                    if (topicId == null)
                    {
                        return View(model);
                    }
                    return RedirectToAction("Edit", "Topic", new {id = topicId.Value});
                }
                catch (Exception exception)
                {
                    ExampleContext.Log.Error("TopicController.Create", exception);
                    return View(model);
                }
            }

            return View(model);
        }
    }
}