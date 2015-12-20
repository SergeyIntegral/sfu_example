using System;
using System.IO;
using System.Linq;
using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.Services.Context;
using Example.Services.Models;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface ITopicService : IIntService<Topic>
    {
        ExampleTopic Find(int id);
        void Save(ExampleTopic model);
        void Remove(int id);
        int? Add(ExampleTopic model);
    }

    public class TopicService : IntService<Topic>, ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IDbContextProvider _provider;

        public TopicService(ITopicRepository repository, IMessageRepository messageRepository, IDbContextProvider provider) : base(repository, provider)
        {
            _topicRepository = repository;
            _messageRepository = messageRepository;
            _provider = provider;
        }

        public ExampleTopic Find(int id)
        {
            var entity = _topicRepository.FindById(id);
            if (entity == null)
                return null;

            return new ExampleTopic(entity);
        }

        public void Save(ExampleTopic model)
        {
            var topic = _topicRepository.FindById(model.Id);
            if (topic == null)
                throw new NullReferenceException();

            topic.Title = model.Title;
            topic.Text = model.Text;
            topic.Status = model.Status;

            if (model.Picture != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    model.Picture.InputStream.CopyTo(ms);
                    if (topic.PictureId == null)
                    {
                        topic.Picture = new BinaryData();
                    }
                    topic.Picture.Data = ms.GetBuffer();
                    topic.Picture.MimeType = model.Picture.ContentType;
                }
            }

            _topicRepository.Update(topic);
            _provider.SaveChanges();
        }

        public void Remove(int id)
        {
            var topic = _topicRepository.FindById(id);
            if(topic == null)
                throw new NullReferenceException();

            if (topic.Messages != null)
            {
                while (topic.Messages.Any())
                {
                    _messageRepository.Delete(topic.Messages.First().Id);
                }
            }
            _topicRepository.Delete(topic.Id);
            _provider.SaveChanges();
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
                ExampleContext.Log.Error("TopicService.Add", exception);
                return null;
            }
        }
    }
}
