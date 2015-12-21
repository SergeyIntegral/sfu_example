using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Example.DAL.Entities;
using Example.Services.Context;
using Example.Services.Managers;
using Example.Web.Models;

namespace Example.Web.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ProfileController : Controller
    {
        private readonly ExampleUserManager _userManager;

        public ProfileController(ExampleUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            if (ExampleContext.Current.User == null)
                return HttpNotFound();

            return View(ExampleContext.Current.User);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Avatar([FromBody]ExampleUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = ExampleContext.Current.User;
                if (model.Id == user.Id && model.Avatar != null)
                {
                    if (user.Avatar == null)
                    {
                        user.Avatar = new BinaryData();
                    }
                    using (var ms = new MemoryStream())
                    {
                        model.Avatar.InputStream.CopyTo(ms);
                        user.Avatar.Data = ms.GetBuffer();
                        user.Avatar.MimeType = model.Avatar.ContentType;
                    }
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new Exception();
                    }
                }    
            }

            return RedirectToAction("Index");
        }
    }
}