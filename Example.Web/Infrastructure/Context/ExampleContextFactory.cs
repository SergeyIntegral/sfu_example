using System;
using System.Security.Principal;
using System.Web;
using Example.Services.Context;
using log4net;

namespace Example.Web.Infrastructure.Context
{
    internal class ExampleContextFactory : BaseExampleContextFactory
    {
        private const string ExampleContextKey = "MPRequestContext";

        private readonly Func<ExampleContext> _httpContextFunc;
        private readonly Func<IPrincipal> _userFunc;

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

            _userFunc = () => HttpContext.Current.User;

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
            return _userFunc();
        }
    }
}