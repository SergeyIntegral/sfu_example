using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Text;
using Example.Core.Consts;
using Example.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Example.DAL
{
    internal class DatabaseSeed
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

        public void SeedUsersAndRoles()
        {
            #region Roles

            var anonymousRole = new IdentityRole(UserRoles.Anonymous);
            var userRole = new IdentityRole(UserRoles.User);
            var moderatorRole = new IdentityRole(UserRoles.Moderator);
            var administratorRole = new IdentityRole(UserRoles.Administrator);

            _dbContext.Roles.AddOrUpdate(anonymousRole);
            _dbContext.Roles.AddOrUpdate(userRole);
            _dbContext.Roles.AddOrUpdate(moderatorRole);
            _dbContext.Roles.AddOrUpdate(administratorRole);

            _dbContext.SaveChanges();

            #endregion

            #region Users

            var hasher = new PasswordHasher();

            var administrator = new ExampleUser
            {
                Id = "Administrator",
                FullName = "Administrator",
                UserName = "Administrator",
                Email = "administrator@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            _dbContext.Users.AddOrUpdate(administrator);

            var moderator = new ExampleUser
            {
                Id = "Moderator",
                FullName = "Moderator",
                UserName = "moderator",
                Email = "moderator@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            _dbContext.Users.AddOrUpdate(moderator);

            var user1 = new ExampleUser
            {
                Id = "User1",
                FullName = "User One",
                UserName = "user1",
                Email = "user.one@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            _dbContext.Users.AddOrUpdate(user1);

            var user2 = new ExampleUser
            {
                Id = "User2",
                FullName = "User Two",
                UserName = "user2",
                Email = "user.two@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            _dbContext.Users.AddOrUpdate(user2);

            var user3 = new ExampleUser
            {
                Id = "User3",
                FullName = "User Three",
                UserName = "user3",
                Email = "user.three@localhost.net",
                PasswordHash = hasher.HashPassword("Password!1"),
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            _dbContext.Users.AddOrUpdate(user3);

            SaveChanges();

            #endregion


            _dbContext.UserRoles.AddOrUpdate(new IdentityUserRole {RoleId = administratorRole.Id, UserId = administrator.Id});
            _dbContext.UserRoles.AddOrUpdate(new IdentityUserRole {RoleId = moderatorRole.Id, UserId = moderator.Id});
            _dbContext.UserRoles.AddOrUpdate(new IdentityUserRole {RoleId = userRole.Id, UserId = user1.Id});
            _dbContext.UserRoles.AddOrUpdate(new IdentityUserRole {RoleId = userRole.Id, UserId = user2.Id});
            _dbContext.UserRoles.AddOrUpdate(new IdentityUserRole {RoleId = userRole.Id, UserId = user3.Id});

            SaveChanges();
        }
    }
}
