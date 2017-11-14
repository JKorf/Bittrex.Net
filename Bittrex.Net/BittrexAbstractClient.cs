using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;

namespace Bittrex.Net
{
    public abstract class BittrexAbstractClient: IDisposable
    {
        protected string apiKey;
        protected HMACSHA512 encryptor;
        internal Log log;

        protected BittrexAbstractClient()
        {
            log = new Log();

            if (BittrexDefaults.LogWriter != null)
                SetLogOutput(BittrexDefaults.LogWriter);

            if (BittrexDefaults.LogVerbosity != null)
                SetLogVerbosity(BittrexDefaults.LogVerbosity.Value);

            if (BittrexDefaults.ApiKey != null && BittrexDefaults.ApiSecret != null)
                SetApiCredentials(BittrexDefaults.ApiKey, BittrexDefaults.ApiSecret);
        }

        public void SetApiCredentials(string apiKey, string apiSecret)
        {
            SetApiKey(apiKey);
            SetApiSecret(apiSecret);
        }

        /// <summary>
        /// Sets the API Key. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        public void SetApiKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("Api key empty");

            this.apiKey = apiKey;
        }

        /// <summary>
        /// Sets the API Secret. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiSecret">The api secret</param>
        public void SetApiSecret(string apiSecret)
        {
            if (string.IsNullOrEmpty(apiSecret))
                throw new ArgumentException("Api secret empty");

            encryptor = new HMACSHA512(Encoding.UTF8.GetBytes(apiSecret));
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
            encryptor?.Dispose();
        }
    }
}
