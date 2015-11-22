using System;

namespace Example.DAL.Entities.Abstract
{
    public interface IDatesEntity
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}
