using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Example.Core.Consts;
using Example.DAL.Entities.Abstract;

namespace Example.DAL.Entities
{
    public class Topic : IntEntity, IDatesEntity
    {
        [Required]
        public string Title { get; set; }

        public string Text { get; set; }

        [Required]
        public TopicStatus Status { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public virtual User Author { get; set; }
        
        [Required]
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public int PictureId { get; set; }
        public virtual BinaryData Picture { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; } 
    }
}
