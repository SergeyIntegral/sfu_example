using System;
using System.Collections.Generic;
using System.Linq;
using Example.Core.Consts;
using Example.DAL.Entities;

namespace Example.Services.Models
{
    public class ExampleTopic
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public TopicStatus Status { get; set; }

        public ExampleUser Author { get; set; }

        public int SectionId { get; set; }

        public ExampleSection Section { get; set; }

        public List<ExampleMessage> Messages { get; set; } 

        public ExampleTopic()
        {
            
        }

        public ExampleTopic(Topic entity)
        {
            if(entity == null)
                throw new ArgumentNullException("entity");

            Id = entity.Id;
            Title = entity.Title;
            Text = entity.Text;
            CreatedDate = entity.CreatedDate;
            ModifiedDate = entity.ModifiedDate;
            Status = entity.Status;
            Author = entity.Author;
            SectionId = entity.SectionId;
            Section = new ExampleSection(entity.Section, false);

            if (entity.Messages != null && entity.Messages.Any())
            {
                Messages = entity.Messages.Select(m => new ExampleMessage(m, this)).ToList();
            }
            else
            {
                Messages = new List<ExampleMessage>();
            }
        }

        public ExampleTopic(Topic entity, ExampleSection section)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Id = entity.Id;
            Title = entity.Title;
            Text = entity.Text;
            CreatedDate = entity.CreatedDate;
            ModifiedDate = entity.ModifiedDate;
            Status = entity.Status;
            Author = entity.Author;
            SectionId = entity.SectionId;
            Section = section;

            if (entity.Messages != null && entity.Messages.Any())
            {
                Messages = entity.Messages.Select(m => new ExampleMessage(m, this)).ToList();
            }
            else
            {
                Messages = new List<ExampleMessage>();
            }
        }
    }
}
