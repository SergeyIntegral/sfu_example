using System;
using System.Web.Http;
using System.Web.Mvc;
using Example.Core.Consts;
using Example.DAL;
using Example.Services.Context;
using Example.Services.Models;
using Example.Services.Services;

namespace Example.Web.Controllers
{
    [System.Web.Mvc.Authorize(Roles = UserRoles.Administrator + ", " + UserRoles.Moderator + ", " + UserRoles.User)]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IDbContextProvider _provider;

        public MessageController(IMessageService messageService, IDbContextProvider provider)
        {
            _messageService = messageService;
            _provider = provider;
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody] ExampleMessage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Author = ExampleContext.Current.User;
                    var messageId = _messageService.Add(model);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Unexpected error. Try again.");
                }
            }

            return RedirectToAction("Detail", "Topic", new { id = model.TopicId });
        }
    }
}