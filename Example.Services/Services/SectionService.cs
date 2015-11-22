using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories.Abstract;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface ISectionService : IIntService<Section>
    {
        
    }

    public class SectionService : IntService<Section>, ISectionService
    {
        public SectionService(IIntRepository<Section> repository, IDbContextProvider provider) : base(repository, provider)
        {
        }
    }
}
