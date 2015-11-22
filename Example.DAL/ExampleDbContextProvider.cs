using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Example.DAL
{
    public class ExampleDbContextProvider : IDbContextProvider
    {
        private readonly ExampleDbContext _dbContext = new ExampleDbContext();

        public DbContext Context { get { return _dbContext; } }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool _disposed;
        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }
    }
}
