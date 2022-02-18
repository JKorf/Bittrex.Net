namespace Bittrex.Net.Objects.Internal
{
    internal class ConnectionResponse
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
    }
}
