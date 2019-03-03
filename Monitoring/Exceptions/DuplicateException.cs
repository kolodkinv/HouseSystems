using System;

namespace Monitoring.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string message)
            : base(message)
        { }
    }
}