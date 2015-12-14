using System;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Example.DAL.Entities;
using Example.Services.Context;
using Example.Services.Managers;
using log4net;
using Microsoft.AspNet.Identity;

namespace Example.Web.Infrastructure.Context
{
    internal class ExampleContextFactory : BaseExampleContextFactory
    {
        private const string ExampleContextKey = "MPRequestContext";

        private readonly Func<ExampleContext> _httpContextFunc;
        private readonly Func<IPrincipal> _principalFunc;
        private readonly Func<IPrincipal, ExampleUser> _userFunc; 

        public ExampleContextFactory()
        {
            #region HttpContext

            _httpContextFunc = () =>
            {
                ExampleContext mpContext;

                if (!HttpContext.Current.Items.Contains(ExampleContextKey))
                {
                    mpContext = new ExampleContext
                    {
                        Uri = HttpContext.Current.Request.Url,
                    };

                    HttpContext.Current.Items[ExampleContextKey] = mpContext;
                }
                else
                {
                    mpContext = HttpContext.Current.Items[ExampleContextKey] as ExampleContext;
                }

                return mpContext;
            };

            #endregion

            #region IPrincipal (HttpContext.Current.User)

            _principalFunc = () => HttpContext.Current.User;

            #endregion

            #region Current ExampleUser

            _userFunc = (principal) =>
            {
                var mngr = DependencyResolver.Current.GetService<ExampleUserManager>();
                return mngr.FindByNameOrEmail(principal.Identity.Name);
            };

            #endregion
        }

        public override ILog GetLogger()
        {
            return LogManager.GetLogger(typeof(MvcApplication));
        }

        public override ExampleContext EnsureHttpContext()
        {
            return _httpContextFunc();
        }

        public override IPrincipal GetPrincipal()
        {
            return _principalFunc();
        }

        public override ExampleUser GetUser(IPrincipal principal)
        {
            return _userFunc(principal);
        }
    }
}