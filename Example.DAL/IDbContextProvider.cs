using System.Data.Entity;
using System.Threading.Tasks;

namespace Example.DAL
{
    public interface IDbContextProvider
    {
        DbContext Context { get; }

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
