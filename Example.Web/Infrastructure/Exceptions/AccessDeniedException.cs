using System;

namespace Example.Web.Infrastructure.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base()
        {
            
        }

        public AccessDeniedException(string message) : base(message)
        {

        }

        public AccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}