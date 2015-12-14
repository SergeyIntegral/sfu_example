using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Example.DAL.Entities;

namespace Example.Services.Models
{
    public class ExampleSection
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int? ParentSectionId { get; set; }

        public ExampleSection ParentSection { get; set; }

        public List<ExampleSection> ChildSections { get; set; }

        public List<ExampleTopic> Topics { get; set; }

        public ExampleSection()
        {

        }

        public ExampleSection(Section entity, bool childs)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            CreatedDate = entity.CreatedDate;
            ModifiedDate = entity.ModifiedDate;
            ParentSectionId = entity.ParentId;

            if (entity.ParentId != null && entity.Parent != null)
            {
                ParentSection = new ExampleSection(entity.Parent, false);
            }
            if (childs && entity.ChildSections != null && entity.ChildSections.Any())
            {
                ChildSections = entity.ChildSections.Select(s => new ExampleSection(s, false)).ToList();
            }
            else
            {
                ChildSections = new List<ExampleSection>();
            }
        }

        public ExampleSection(Section entity, ExampleSection parent, bool childs)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            CreatedDate = entity.CreatedDate;
            ModifiedDate = entity.ModifiedDate;

            if (parent != null)
            {
                ParentSectionId = parent.Id;
                ParentSection = parent;
            }
            if (childs && entity.ChildSections != null && entity.ChildSections.Any())
            {
                ChildSections = entity.ChildSections.Select(s => new ExampleSection(s, false)).ToList();
            }
            else
            {
                ChildSections = new List<ExampleSection>();
            }
        }
    }
}