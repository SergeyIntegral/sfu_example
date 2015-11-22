using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories.Abstract;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface ITopicService : IIntService<Topic>
    {
        
    }

    public class TopicService : IntService<Topic>, ITopicService
    {
        public TopicService(IIntRepository<Topic> repository, IDbContextProvider provider) : base(repository, provider)
        {
        }
    }
}
