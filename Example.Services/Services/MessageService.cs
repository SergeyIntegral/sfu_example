using System;
using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.Services.Context;
using Example.Services.Models;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface IMessageService : IIntService<Message>
    {
        void Remove(int id);
        int? Add(ExampleMessage model);
    }

    public class MessageService : IntService<Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IDbContextProvider _provider;

        public MessageService(IMessageRepository repository, IDbContextProvider provider) 
            : base(repository, provider)
        {
            _messageRepository = repository;
            _provider = provider;
        }

        public void Remove(int id)
        {
            var message = _messageRepository.FindById(id);
            if (message == null)
                throw new NullReferenceException();

            _messageRepository.Delete(message.Id);
            _provider.SaveChanges();
        }

        public int? Add(ExampleMessage model)
        {
            try
            {
                var entity = new Message
                {
                    Text = model.Text,
                    TopicId = model.TopicId,
                    AuthorId = model.Author.Id
                };
                var newEntity = _messageRepository.Insert(entity);
                _provider.SaveChanges();

                return newEntity.Id;
            }
            catch (Exception exception)
            {
                ExampleContext.Log.Error("MessageService.Add", exception);
                return null;
            }
        }
    }
}
