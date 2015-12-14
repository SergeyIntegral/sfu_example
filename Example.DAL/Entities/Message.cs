using System;
using System.ComponentModel.DataAnnotations;
using Example.DAL.Entities.Abstract;

namespace Example.DAL.Entities
{
    public class Message : IntEntity, IDatesEntity
    {
        [Required]
        public string AuthorId { get; set; }
        public virtual ExampleUser Author { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
