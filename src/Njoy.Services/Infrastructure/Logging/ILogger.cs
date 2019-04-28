using Microsoft.Extensions.Logging;
using System;

namespace Njoy.Services
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);

        void Log(LogLevel level, Exception exception, string message, params object[] args);
    }
}