using System;
using Example.DAL.Entities;

namespace Example.Services.Models
{
    public class ExampleMessage
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public ExampleUser Author { get; set; }

        public int TopicId { get; set; }

        public ExampleTopic Topic { get; set; }

        public ExampleMessage()
        {
            
        }

        public ExampleMessage(Message entity)
        {
            Id = entity.Id;
            Text = entity.Text;
            CreatedDate = entity.CreatedDate;
            ModifiedDate = entity.ModifiedDate;
            Author = entity.Author;
            TopicId = entity.TopicId;
            Topic = new ExampleTopic(entity.Topic);
        }

        public ExampleMessage(Message entity, ExampleTopic topic)
        {
            Id = entity.Id;
            Text = entity.Text;
            CreatedDate = entity.CreatedDate;
            ModifiedDate = entity.ModifiedDate;
            Author = entity.Author;
            TopicId = topic.Id;
            Topic = topic;
        }
    }
}
