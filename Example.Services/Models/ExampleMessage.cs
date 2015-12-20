using System;
using System.ComponentModel.DataAnnotations;
using Example.DAL.Entities;
using Example.Services.Context;

namespace Example.Services.Models
{
    public class ExampleMessage
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public ExampleUser Author { get; set; }

        [Required]
        public int TopicId { get; set; }

        public ExampleTopic Topic { get; set; }

        public bool IsMine { get; set; }

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
            IsMine = ExampleContext.Current.User.Id == entity.AuthorId;
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
            IsMine = ExampleContext.Current.User.Id == entity.AuthorId;
            TopicId = topic.Id;
            Topic = topic;
        }
    }
}
