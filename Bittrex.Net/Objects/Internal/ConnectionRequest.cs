namespace Bittrex.Net.Objects.Internal
{
    internal class ConnectionRequest
    {
        public string RequestName { get; set; }
        public object[] Parameters { get; set; }

        public ConnectionRequest(string name, params string[] channels)
        {
            RequestName = name;
            Parameters = new object[]
            {
                channels
            };
        }
    }
}
