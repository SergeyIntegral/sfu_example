using Example.DAL.Entities;
using Example.DAL.Repositories.Abstract;
using Example.DAL.Repositories.Base;

namespace Example.DAL.Repositories
{
    public interface ITopicRepository : IIntRepository<Topic>
    {
        
    }

    public class TopicRepository : IntRepository<Topic>, ITopicRepository
    {
        public TopicRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
