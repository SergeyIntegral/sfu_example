using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Example.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Example.DAL.Repositories
{
    public class ExampleUserStore : IUserPasswordStore<User>, IUserStore<User>, IUserLockoutStore<User, string>
    {
        readonly ExampleDbContext _context;

        public ExampleUserStore(IDbContextProvider dbContextProvider)
        {
            _context = dbContextProvider.Context as ExampleDbContext;
        }

        public Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void Update(User user)
        {
            var record = _context.Users.Find(user.Id);
            if (record != null)
            {
                record.UserName = user.UserName;
                record.FullName = user.FullName;
                record.AccessFailedCount = user.AccessFailedCount;
                record.Email = user.Email;
                record.LockoutEnabled = user.LockoutEnabled;
                record.LockoutEndDateUtc = user.LockoutEndDateUtc;
                record.PasswordHash = user.PasswordHash;
                record.DateOfLastVisit = user.DateOfLastVisit;
                record.IsEndSession = user.IsEndSession;
                record.Phone = user.Phone;
                record.Avatar = user.Avatar;

                _context.SaveChanges();
            }
        }

        public Task UpdateAsync(User user)
        {
            Update(user);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(User user)
        {
            var deleted = _context.Users.Find(user.Id);
            _context.Users.Remove(deleted);
            return _context.SaveChangesAsync();
        }

        public User FindById(string userId)
        {
            return _context.Users.Single(u => u.Id == userId);
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public User FindByName(string userName)
        {
            //поиск пользователя по имени и почте
            return _context.Users.FirstOrDefault(u => 
                u.UserName == userName ||
                u.Email.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }


        public Task<User> FindByNameAsync(string userName)
        {
            //поиск пользователя по имени и почте
            return _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public Task<User> FindByIdAsync(Guid index)
        {
            //поиск пользователя по имени и почте
            return _context.Users.FirstOrDefaultAsync(u => u.Index == index);
        }

        public User FindByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<User> FindByEmails(params string[] emails)
        {
            var emailsList = (emails ?? new string[] { }).ToList();

            return _context.Users.Where(u => emailsList.Contains(u.Email)).ToList();
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            var record = _context.Users.Single(u => u.Id == user.Id);
            record.PasswordHash = passwordHash;

            return _context.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.Factory.StartNew(() =>
            {
                var record = _context.Users.Single(u => u.Id == user.Id);
                return record.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.Factory.StartNew(() =>
            {
                var record = _context.Users.Single(u => u.Id == user.Id);
                return string.IsNullOrWhiteSpace(record.PasswordHash);
            });
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(user.LockoutEndDateUtc ?? DateTimeOffset.MinValue);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            user.LockoutEndDateUtc = DateTimeOffset.MinValue;
            user.LockoutEnabled = false;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public List<User> GeAll()
        {
            return _context.Users.ToList();
        }

        public IQueryable<User> AsQueryable()
        {
            return _context.Users;
        }
    }
}
