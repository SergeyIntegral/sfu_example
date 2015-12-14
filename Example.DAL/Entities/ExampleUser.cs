using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Example.DAL.Entities.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Example.DAL.Entities
{
    public class ExampleUser : IdentityUser, IDatesEntity
    {
        public string FullName { get; set; }
        public virtual BinaryData Avatar { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ExampleUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        //public ExampleUser(IPrincipal principal)
        //{
        //    var provider = DependencyResolver.Current.GetService<IDbContextProvider>();
        //    _manager = new ExampleUserManager(provider);
        //    User user = _manager.FindByName(principal.Identity.Name);
        //    if (user == null)
        //    {
        //        throw new Exception("principal.Identity.Name: " + principal.Identity.Name);
        //    }

        //    InitExampleUser(user);
        //}

        //public ExampleUser(ExampleUserManager manager, User user)
        //{
        //    _manager = manager;
        //    InitExampleUser(user);
        //}

        //private void InitExampleUser(User user)
        //{
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException("user");
        //    }
        //    Entity = user;
        //}
    }
}
