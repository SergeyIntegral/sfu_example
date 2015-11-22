using System;
using System.Configuration;
using System.Threading.Tasks;
using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Microsoft.AspNet.Identity;

namespace Example.Services.Managers
{
    public class ExampleUserManager : UserManager<User>
    {
        public ExampleUserManager(IDbContextProvider provider)
            : base(new ExampleUserStore(provider))
        {
        }

        public ExampleUserManager(IUserStore<User> store)
            : base(store)
        {
            PasswordHasher = new PasswordHasher();

            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator
            {
                RequiredLength = int.Parse(ConfigurationManager.AppSettings["PasswordRequiredLength"] ?? "8"),
                RequireNonLetterOrDigit = bool.Parse(ConfigurationManager.AppSettings["PasswordRequireNonLetterOrDigit"] ?? "true"),
                RequireDigit = bool.Parse(ConfigurationManager.AppSettings["PasswordRequireDigit"] ?? "true"),
                RequireLowercase = bool.Parse(ConfigurationManager.AppSettings["PasswordRequireLowercase"] ?? "true"),
                RequireUppercase = bool.Parse(ConfigurationManager.AppSettings["PasswordRequireUppercase"] ?? "true")
            };
            PasswordValidator.ValidateAsync("");
            // Configure user lockout defaults
            UserLockoutEnabledByDefault = bool.Parse(ConfigurationManager.AppSettings["UserLockoutEnabledByDefault"] ?? "true");
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(double.Parse(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"] ?? "15"));
            MaxFailedAccessAttemptsBeforeLockout = int.Parse(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"] ?? "5");
        }

        public IdentityResult Create(User user, string password)
        {
            try
            {
                var hasher = new PasswordHasher();
                user.PasswordHash = hasher.HashPassword(password);
                //создание пользователя
                Create(user);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(e.Message);
            }
        }

        public IdentityResult Create(User user)
        {
            try
            {
                var store = (ExampleUserStore)Store;
                store.Create(user);

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(e.Message);
            }
        }

        //public IdentityResult Delete(User user)
        //{
        //    try
        //    {
        //        var store = (ExampleUserStore)Store;

        //        store.Delete(user);
        //        return IdentityResult.Success;
        //    }
        //    catch (Exception e)
        //    {
        //        return IdentityResult.Failed(e.Message);
        //    }
        //}

        public IdentityResult Update(User user)
        {
            try
            {
                var store = (ExampleUserStore)Store;
                store.Update(user);

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(e.Message);
            }
        }

        public override Task<User> FindAsync(string email, string password)
        {
            var taskInvoke = Task<User>.Factory.StartNew(() =>
            {
                var user = ((ExampleUserStore)Store).FindByEmailAsync(email).Result;

                if (user == null)
                    return null;

                PasswordVerificationResult result = PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return null;
                }
                return user;
            });
            return taskInvoke;
        }

        public User FindById(string userId)
        {
            return ((ExampleUserStore)Store).FindById(userId);
        }

        public async override Task<User> FindByIdAsync(string userId)
        {
            var user = await ((ExampleUserStore)Store).FindByIdAsync(userId);
            return user;
        }

        public async Task<User> FindByIdAsync(Guid index)
        {
            var user = await ((ExampleUserStore)Store).FindByIdAsync(index);
            return user;
        }

        public User FindByName(string userName)
        {
            return ((ExampleUserStore)Store).FindByName(userName);
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return ((ExampleUserStore)Store).FindByNameAsync(userName);
        }

        public User FindByEmail(string email)
        {
            return ((ExampleUserStore)Store).FindByEmail(email);
        }

        public override Task<User> FindByEmailAsync(string email)
        {
            return ((ExampleUserStore)Store).FindByEmailAsync(email);
        }

        public override Task<IdentityResult> UpdateAsync(User user)
        {
            var taskInvoke = Task<IdentityResult>.Factory.StartNew(() =>
            {
                try
                {
                    ((ExampleUserStore)Store).UpdateAsync(user);
                    return IdentityResult.Success;
                }
                catch (Exception error)
                {
                    return IdentityResult.Failed(error.Message);
                }
            });
            return taskInvoke;
        }

        public async Task<bool> IsLockedOutAsync(User user)
        {
            var store = ((ExampleUserStore)Store);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (!await store.GetLockoutEnabledAsync(user))
            {
                return false;
            }
            var lockoutTime = await store.GetLockoutEndDateAsync(user);
            return lockoutTime >= DateTimeOffset.UtcNow;
        }

        public async Task<bool> GetLockoutEnabledAsync(User user)
        {
            var store = ((ExampleUserStore)Store);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await store.GetLockoutEnabledAsync(user);
        }

        public async Task<int> GetAccessFailedCountAsync(User user)
        {
            var store = ((ExampleUserStore)Store);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await store.GetAccessFailedCountAsync(user);
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            var store = ((ExampleUserStore)Store);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await store.ResetAccessFailedCountAsync(user);
        }

        public async Task AccessFailedAsync(User user)
        {
            var store = ((ExampleUserStore)Store);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var lockoutTime = await store.GetLockoutEndDateAsync(user);
            var count = await store.GetAccessFailedCountAsync(user);
            if (await store.GetLockoutEnabledAsync(user) &&
                count >= MaxFailedAccessAttemptsBeforeLockout
                && lockoutTime <= DateTimeOffset.UtcNow)
            {
                await store.ResetAccessFailedCountAsync(user);
            }

            await store.IncrementAccessFailedCountAsync(user);
            if (await store.GetAccessFailedCountAsync(user) >= MaxFailedAccessAttemptsBeforeLockout)
            {
                await store.SetLockoutEnabledAsync(user, true);
                await store.SetLockoutEndDateAsync(user, DateTimeOffset.Now.Add(DefaultAccountLockoutTimeSpan));
            }
        }
    }
}
