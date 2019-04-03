using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ASPJWTPractice.Utilities
{
    public class Logger : Interfaces.ILogger
    {
        public void LogDebug(string message)
        {
            Log.Debug(message);
            Log.CloseAndFlush();
        }

        public void LogError(string message)
        {
            Log.Error(message);
            Log.CloseAndFlush();
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
            Log.CloseAndFlush();
        }

        public void LogWarn(string message)
        {
            Log.Warning(message);
            Log.CloseAndFlush();
        }
    }
}
