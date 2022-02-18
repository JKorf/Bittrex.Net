namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Linked order
    /// </summary>
    public class BittrexLinkedOrder
    {
        /// <summary>
        /// Id of the order
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Type of the order
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
