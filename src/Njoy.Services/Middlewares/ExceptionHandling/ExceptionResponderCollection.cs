using Nensure;
using System;
using System.Collections.Generic;

namespace Njoy.Services
{
    public sealed class ExceptionResponderCollection
    {
        private Dictionary<Type, IExceptionResponder> Responders { get; } = new Dictionary<Type, IExceptionResponder>();

        /// <summary>
        /// Returns the responder for the given exception type or null if not registered.
        /// </summary>
        /// <param name="exceptionType">Type of the Exception to be handled.</param>
        /// <returns>Related responder or null.</returns>
        public IExceptionResponder this[Type exceptionType]
        {
            get
            {
                return Contains(exceptionType) ? Responders[exceptionType] : default(IExceptionResponder);
            }
        }

        public void Add<TException>(IExceptionResponder responder) where TException : Exception
        {
            Ensure.NotNull(responder);
            var type = typeof(TException);
            if (Contains(type))
            {
                throw new InvalidOperationException($"You cannot have multiple handlers for a specific type of exception: {type}");
            }

            Responders[type] = responder;
        }

        public bool Contains(Type exceptionType)
        {
            return Responders.ContainsKey(exceptionType);
        }
    }
}