using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;

namespace Bittrex.Net
{
    public abstract class BittrexAbstractClient
    {
        protected string apiKey;
        protected HMACSHA512 encryptor;
        internal Log log;

        protected BittrexAbstractClient()
        {
            log = new Log();

            if (BittrexDefaults.LogWriter != null)
                SetLogOutput(BittrexDefaults.LogWriter);

            if (BittrexDefaults.LogVerbositySet)
                SetLogVerbosity(BittrexDefaults.LogVerbosity);

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

        protected BittrexApiResult<T> ThrowErrorMessage<T>(string message)
        {
            log.Write(LogVerbosity.Warning, $"Call failed: {message}");
            var result = (BittrexApiResult<T>)Activator.CreateInstance(typeof(BittrexApiResult<T>));
            result.Message = message;
            return result;
        }
    }
}
