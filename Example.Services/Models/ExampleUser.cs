using System;
using System.Security.Principal;
using System.Web.Mvc;
using Example.DAL;
using Example.DAL.Entities;
using Example.Services.Managers;

namespace Example.Services.Models
{
    public class ExampleUser
    {
        private readonly ExampleUserManager _manager;
        public User Entity { get; set; }

        public ExampleUser(IPrincipal principal)
        {
            var provider = DependencyResolver.Current.GetService<IDbContextProvider>();
            _manager = new ExampleUserManager(provider);
            User user = _manager.FindByName(principal.Identity.Name);
            if (user == null)
            {
                throw new Exception("principal.Identity.Name: " + principal.Identity.Name);
            }

            InitExampleUser(user);
        }

        public ExampleUser(ExampleUserManager manager, User user)
        {
            _manager = manager;
            InitExampleUser(user);
        }

        private void InitExampleUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            Entity = user;
        }
    }
}
