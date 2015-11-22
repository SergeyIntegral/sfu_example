using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Text;
using Example.Core.Consts;
using Example.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Example.DAL
{
    internal partial class DatabaseSeed
    {
        private readonly ExampleDbContext _dbContext;

        public DatabaseSeed(ExampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(string.Format("Entity Validation Failed - errors follow:\n{0}", sb), ex);
            }
        }

        public void SeedUsers()
        {
            var hasher = new PasswordHasher();
           
            #region Users

            var administrator = new User
            {
                Id = "Administrator",
                FullName = "Administrator",
                UserName = "administrator@localhost.net",
                Email = "administrator@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                Role = UserRoles.Administrator,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };
            _dbContext.Users.AddOrUpdate(administrator);

            var moderator = new User
            {
                Id = "Moderator",
                FullName = "Moderator",
                UserName = "moderator@localhost.net",
                Email = "moderator@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                Role = UserRoles.Moderator,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };
            _dbContext.Users.AddOrUpdate(moderator);

            var user = new User
            {
                Id = "User",
                FullName = "User",
                UserName = "user@localhost.net",
                Email = "user@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                Role = UserRoles.User,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };
            _dbContext.Users.AddOrUpdate(user);

            SaveChanges();

            #endregion
        }
    }
}
