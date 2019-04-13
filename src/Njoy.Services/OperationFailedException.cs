using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

namespace Njoy.Services
{
    [Serializable]
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string operation)
            : base(BuildOperationString(operation))
        {
        }

        public OperationFailedException(string operation, string message)
            : base($"{BuildOperationString(operation)} {message}")
        {
        }

        public OperationFailedException(string operation, params object[] args)
            : base($"{BuildOperationString(operation)} {JsonConvert.SerializeObject(args)}")
        {
        }

        public OperationFailedException(string operation, Exception inner) :
            base(BuildOperationString(operation), inner)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string BuildOperationString(string operation)
        {
            return $"Operation {operation} failed.";
        }
    }
}