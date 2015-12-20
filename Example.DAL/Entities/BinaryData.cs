using System;
using Example.DAL.Entities.Abstract;

namespace Example.DAL.Entities
{
    public class BinaryData : IntEntity, IDatesEntity
    {
        public byte[] Data { get; set; }

        public string MimeType { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
