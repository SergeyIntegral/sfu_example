using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Example.DAL.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Example.Services.Managers
{
    public class ExampleSignInManager : SignInManager<ExampleUser, string>
    {
        public ExampleSignInManager(ExampleUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ExampleUser user)
        {
            return user.GenerateUserIdentityAsync((ExampleUserManager)UserManager);
        }

        public static ExampleSignInManager Create(IdentityFactoryOptions<ExampleSignInManager> options, IOwinContext context)
        {
            return new ExampleSignInManager(context.GetUserManager<ExampleUserManager>(), context.Authentication);
        }
    }
}
