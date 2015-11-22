using Example.DAL.Entities;
using Example.DAL.Repositories.Abstract;
using Example.DAL.Repositories.Base;

namespace Example.DAL.Repositories
{
    public interface ISectionRepository : IIntRepository<Section>
    {
        
    }

    public class SectionRepository : IntRepository<Section>, ISectionRepository
    {
        public SectionRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
