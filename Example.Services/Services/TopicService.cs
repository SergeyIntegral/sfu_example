using System;
using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.DAL.Repositories.Abstract;
using Example.Services.Models;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface ITopicService : IIntService<Topic>
    {
        int? Add(ExampleTopic model);
    }

    public class TopicService : IntService<Topic>, ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IDbContextProvider _provider;

        public TopicService(ITopicRepository repository, IDbContextProvider provider) : base(repository, provider)
        {
            _topicRepository = repository;
            _provider = provider;
        }

        public int? Add(ExampleTopic model)
        {
            try
            {
                var entity = new Topic
                {
                    Title = model.Title,
                    Status = model.Status,
                    SectionId = model.SectionId,
                    AuthorId = model.Author.Id
                };
                var newEntity = _topicRepository.Insert(entity);
                _provider.SaveChanges();

                return newEntity.Id;
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}
