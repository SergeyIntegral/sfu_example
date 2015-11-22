using System.Threading.Tasks;
using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface IBinaryDataService
    {
        Task RecomputeGeneration();
        Task RemoveByGeneration(int? generationForRemoving);
    }

    public class BinaryDataService : IntService<BinaryData>, IBinaryDataService
    {
        private readonly IBinaryDataRepository _binaryDataRepository;

        public BinaryDataService(IBinaryDataRepository binaryDataRepository, IDbContextProvider provider) 
             : base(binaryDataRepository, provider)
        {
            _binaryDataRepository = binaryDataRepository;
        }

        public async Task RecomputeGeneration()
        {
            await _binaryDataRepository.RecomputeGeneration();
        }

        public async Task RemoveByGeneration(int? generationForRemoving)
        {
            await _binaryDataRepository.RemoveByGeneration(generationForRemoving);
        }
    }
}
