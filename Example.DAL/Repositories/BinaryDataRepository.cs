using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Example.Core.Infrastructure;
using Example.DAL.Entities;
using Example.DAL.Repositories.Abstract;
using Example.DAL.Repositories.Base;

namespace Example.DAL.Repositories
{
    public interface IBinaryDataRepository : IIntRepository<BinaryData>
    {
        Task<int> StreamBlobToServer(Stream stream, int? id);
        Task RemoveByIdAsync(int id);
        void RemoveById(int id);
    }

    public class BinaryDataRepository : IntRepository<BinaryData>, IBinaryDataRepository
    {
        private readonly IDbContextProvider _dbContextProvider;

        public BinaryDataRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<int> StreamBlobToServer(Stream stream, int? id = null)
        {
            if (id == null)
            {
                var bdata = new BinaryData {Data = new byte[] {0}};
                (_dbContextProvider.Context as ExampleDbContext).Blobs.Add(bdata);
                await _dbContextProvider.Context.SaveChangesAsync();

                id = bdata.Id;
            }
            
            var blob = new VarbinaryStream((SqlConnection) _dbContextProvider.Context.Database.Connection, "BinaryData", "Data", "Id", id.Value);
            await stream.CopyToAsync(blob);

            return id.Value;
        }

        public async Task RemoveByIdAsync(int id)
        {
            await _dbContextProvider.Context.Database.ExecuteSqlCommandAsync(@"
                    DELETE FROM [dbo].[BinaryData]
                    WHERE Id = @id;
                ", new SqlParameter("@id", id));
        }

        public void RemoveById(int id)
        {
            _dbContextProvider.Context.Database.ExecuteSqlCommand(@"
                    DELETE FROM [dbo].[BinaryData]
                    WHERE Id = @id;
                ", new SqlParameter("@id", id));
        }
    }
}
