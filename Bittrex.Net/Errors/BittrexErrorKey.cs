namespace Bittrex.Net.Errors
{
    public enum BittrexErrorKey
    {
        NoApiCredentialsProvided,

        ParseErrorReader,
        ParseErrorSerialization,
        ErrorWeb,
        CantConnectToServer,

        UnknownError
    }
}
