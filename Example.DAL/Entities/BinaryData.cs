using System;
using Example.DAL.Entities.Abstract;

namespace Example.DAL.Entities
{
    public class BinaryData : IntEntity, IDatesEntity
    {
        public byte[] Data { get; set; }

        public int? Generation { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
