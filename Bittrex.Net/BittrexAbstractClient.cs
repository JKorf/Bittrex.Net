using System;
using System.IO;
using System.Text;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;

namespace Bittrex.Net
{
    public abstract class BittrexAbstractClient: IDisposable
    {
        internal Log log;

        protected string proxyHost;
        protected int proxyPort;

        protected BittrexAbstractClient()
        {
            log = new Log();
        }
        
        protected void Configure(BittrexOptions options)
        {
            if (options.LogWriter != null)
                log.TextWriter = options.LogWriter;

            log.Level = options.LogVerbosity;
            proxyHost = options.ProxyHost;
            proxyPort = options.ProxyPort;
        }

        /// <summary>
        /// Sets the verbosity of the log messages
        /// </summary>
        /// <param name="verbosity">Verbosity level</param>
        public void SetLogVerbosity(LogVerbosity verbosity)
        {
            log.Level = verbosity;
        }

        /// <summary>
        /// Sets the log output
        /// </summary>
        /// <param name="writer">The output writer</param>
        public void SetLogOutput(TextWriter writer)
        {
            log.TextWriter = writer;
        }

        protected BittrexApiResult<T> ThrowErrorMessage<T>(BittrexError error)
        {
            return ThrowErrorMessage<T>(error, null);
        }

        protected BittrexApiResult<T> ThrowErrorMessage<T>(BittrexError error, string extraInformation)
        {
            log.Write(LogVerbosity.Warning, $"Call failed: {error.ErrorMessage}");
            var result = (BittrexApiResult<T>)Activator.CreateInstance(typeof(BittrexApiResult<T>));
            result.Error = error;
            if (extraInformation != null)
                result.Error.ErrorMessage += Environment.NewLine + extraInformation;
            return result;
        }

        public virtual void Dispose()
        {
        }
    }
}
