using System.Security.Principal;
using Example.Services.Models;
using log4net;

namespace Example.Services.Context
{
    public abstract class BaseExampleContextFactory
    {
        public abstract ILog GetLogger();
        public abstract ExampleContext EnsureHttpContext();
        public abstract IPrincipal GetPrincipal();

        public virtual ExampleUser GetUser(IPrincipal principal)
        {
            return new ExampleUser(principal);
        }
    }
}
