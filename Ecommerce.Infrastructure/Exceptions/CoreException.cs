using System;

namespace Ecommerce.Infrastructure.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException() : base() { }

        public CoreException(string message) : base(message)
        {
        }
    }
}
