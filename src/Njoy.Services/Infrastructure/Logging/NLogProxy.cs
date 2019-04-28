using NLog;
using System;
using System.Collections.Generic;
using MsLog = Microsoft.Extensions.Logging;

namespace Njoy.Services
{
    public sealed class NLogProxy<T> : ILogger
    {
        private static readonly NLog.ILogger Logger = LogManager.GetLogger(typeof(T).FullName);

        private static readonly Dictionary<MsLog.LogLevel, LogLevel> LevelMapper = new Dictionary<MsLog.LogLevel, LogLevel>
        {
            {MsLog.LogLevel.None, LogLevel.Off },
            {MsLog.LogLevel.Trace, LogLevel.Trace },
            {MsLog.LogLevel.Debug, LogLevel.Debug },
            {MsLog.LogLevel.Information, LogLevel.Info },
            {MsLog.LogLevel.Warning, LogLevel.Warn },
            {MsLog.LogLevel.Error, LogLevel.Error },
            {MsLog.LogLevel.Critical, LogLevel.Fatal }
        };

        public void Log(MsLog.LogLevel level, string message)
        {
            Logger.Log(LevelMapper[level], message);
        }

        public void Log(MsLog.LogLevel level, Exception exception, string message, params object[] args)
        {
            Logger.Log(LevelMapper[level], exception, message, args);
        }
    }
}