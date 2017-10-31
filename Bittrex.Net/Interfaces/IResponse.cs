using System.IO;

namespace Bittrex.Net.Interfaces
{
    public interface IResponse
    {
        Stream GetResponseStream();
    }
}
