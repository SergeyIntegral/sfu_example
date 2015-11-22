using Example.DAL.Entities;
using Example.DAL.Repositories.Abstract;
using Example.DAL.Repositories.Base;

namespace Example.DAL.Repositories
{
    public interface IMessageRepository : IIntRepository<Message>
    {
        
    }

    public class MessageRepository : IntRepository<Message>, IMessageRepository
    {
        public MessageRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
