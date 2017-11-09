namespace Bittrex.Net.Objects
{
    public class BittrexError
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public BittrexError(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
