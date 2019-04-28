using System;

namespace Njoy.Services
{
    public sealed class ExceptionContent
    {
        public Exception Exception { get; }

        public ExceptionContent(Exception exception)
        {
            Exception = exception;
        }
    }
}