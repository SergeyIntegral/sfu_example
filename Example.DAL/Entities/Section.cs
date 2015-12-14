using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Example.DAL.Entities.Abstract;

namespace Example.DAL.Entities
{
    public class Section : IntEntity, IDatesEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Section Parent { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Section> ChildSections { get; set; } 

        public virtual ICollection<Topic> Topics { get; set; } 
    }
}
