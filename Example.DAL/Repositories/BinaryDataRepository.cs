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
        Task RecomputeGeneration();
        Task RemoveByGeneration(int? generationForRemoving);
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

        public async Task RecomputeGeneration()
        {
            await _dbContextProvider.Context.Database.ExecuteSqlCommandAsync(@"
	                declare @i int
	                declare @binaryId int
	                declare @generation int
	                declare @newGeneration int
	                declare @update bit

	                declare @comments int
	                declare @docs int
	                declare @excels int
	                declare @messages int
	                declare @uploads int
	                declare @users int
	                declare @unconfirmed int

	                set @i = 0
	                while (@i < (select count(1) from dbo.[BinaryData]))
	                begin
		                select @binaryId = Id, @generation = Generation 
		                from dbo.[BinaryData]
		                order by Id
		                OFFSET @i ROWS
		                fetch next 1 ROWS ONLY
	
		                select @comments = count(1) from dbo.[Comment]
		                where ContentId = @binaryId

		                select @docs = count(1) from dbo.[DocumentSample]
		                where ContentId = @binaryId

		                select @excels = count(1) from dbo.[ExcelFile]
		                where ContentId = @binaryId
		
		                select @messages = count(1) from dbo.[SystemMessageFile]
		                where ContentId = @binaryId
		
		                select @uploads = count(1) from dbo.[UploadFile]
		                where ContentId = @binaryId
		
		                select @users = count(1) from dbo.[User]
		                where AvatarId = @binaryId
		
		                select @unconfirmed = count(1) from dbo.[UnconfirmedUser]
		                where AvatarId = @binaryId

		                set @generation = ISNULL(@generation, 0)
		                set @update = 1

		                if(@comments > 0 or @docs > 0 or @excels > 0 or @messages > 0 or @uploads > 0 or @users > 0 or @unconfirmed > 0)
		                begin
			                set @newGeneration = 0
			                if(@generation = @newGeneration)
				                set @update = 0
		                end
		                else
			                set @newGeneration = @generation + 1
		
		                if(@update = 1)
		                begin
			                update dbo.BinaryData
			                set Generation = @newGeneration
			                where Id = @binaryId
		                end

		                set @i = @i + 1
	                end
                ");
        }

        public async Task RemoveByGeneration(int? generationForRemoving)
        {
            await _dbContextProvider.Context.Database.ExecuteSqlCommandAsync(@"
                    DELETE FROM [dbo].[BinaryData]
                    WHERE Generation >= @generation;
                ", new SqlParameter("@generation", generationForRemoving));
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
