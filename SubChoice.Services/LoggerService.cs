using Serilog;
using SubChoice.Core.Interfaces.Services;
using SubChoice.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubChoice.Services
{
    public class LoggerService : ILoggerService
    {
        public LoggerService()
        {
        }

        public void LogInfo(string msg)
        {
            Log.Information(msg);
        }

        public void LogError(string msg)
        {
            Log.Error(msg);
        }

        public void LogFatal(string msg)
        {
            Log.Fatal(msg);
        }
    }
}
