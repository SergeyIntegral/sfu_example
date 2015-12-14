using System;
using System.Security.Principal;
using Example.DAL.Entities;
using log4net;

namespace Example.Services.Context
{
    public class ExampleContext
    {
        public Uri Uri { get; set; }

        private static ILog _log;
        public static ILog Log
        {
            get
            {
                return _log;
            }
        }

        private ExampleUser _user { get; set; }
        public ExampleUser User
        {
            get
            {
                if (_user == null)
                {
                    if (Principal != null)
                    {
                        _user = Factory.GetUser(Principal);
                    }
                }
                return _user;
            }
        }

        private IPrincipal _principal;
        public IPrincipal Principal
        {
            get
            {
                if (_principal == null)
                {
                    var user = Factory.GetPrincipal();
                    if (user.Identity.IsAuthenticated)
                    {
                        _principal = user;
                    }
                }
                return _principal;
            }
        }

        private static BaseExampleContextFactory _factory;
        public static BaseExampleContextFactory Factory
        {
            get { return _factory; }
            set
            {
                if (_factory == null)
                {
                    _factory = value;
                    _log = value.GetLogger();
                }
            }
        }

        public static ExampleContext Current
        {
            get { return Factory.EnsureHttpContext(); }
        }
    }
}
