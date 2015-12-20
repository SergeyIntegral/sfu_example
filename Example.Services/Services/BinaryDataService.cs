using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface IBinaryDataService : IIntService<BinaryData>
    {
    }

    public class BinaryDataService : IntService<BinaryData>, IBinaryDataService
    {
        private readonly IBinaryDataRepository _binaryDataRepository;
        private readonly IDbContextProvider _provider;

        public BinaryDataService(IBinaryDataRepository binaryDataRepository, IDbContextProvider provider) 
             : base(binaryDataRepository, provider)
        {
            _binaryDataRepository = binaryDataRepository;
            _provider = provider;
        }
    }
}
