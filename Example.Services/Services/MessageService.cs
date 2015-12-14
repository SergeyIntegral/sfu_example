using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.DAL.Repositories.Abstract;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface IMessageService : IIntService<Message>
    {
        
    }

    public class MessageService : IntService<Message>, IMessageService
    {
        public MessageService(IMessageRepository repository, IDbContextProvider provider) 
            : base(repository, provider)
        {
        }
    }
}
