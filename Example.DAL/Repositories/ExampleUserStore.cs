using System;
using System.Linq;
using Example.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Example.DAL.Repositories
{
    public class ExampleUserStore : UserStore<ExampleUser>
    {
        private readonly IDbContextProvider _provider;
        private readonly ExampleDbContext _context;


        public ExampleUserStore(IDbContextProvider provider)
            : base(provider.Context)
        {
            _provider = provider;
            _context = provider.Context as ExampleDbContext;
        }

        public ExampleUser FindByUsernameOrEmail(string usernameOrEmail)
        {
            return _context.Users.FirstOrDefault(u =>
                        u.UserName.Equals(usernameOrEmail) ||
                        u.Email.Equals(usernameOrEmail, StringComparison.OrdinalIgnoreCase));
        }
    }
}
