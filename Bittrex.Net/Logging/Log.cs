using System;
using System.IO;

namespace Bittrex.Net.Logging
{
    internal class Log
    {
#if NETSTANDARD
        public TextWriter TextWriter { get; internal set; } = new DebugTextWriter();
#else
        public TextWriter TextWriter { get; internal set; } = new TraceTextWriter();
#endif
        public LogVerbosity Level { get; internal set; } = LogVerbosity.Warning;

        public void Write(LogVerbosity logType, string message)
        {
            if ((int)logType >= (int)Level)
                TextWriter.WriteLine($"{DateTime.Now:hh:mm:ss:fff} | {logType} | {message}");
        }
    }

    public enum LogVerbosity
    {
        Debug,
        Warning,
        Error,
        None
    }
}
