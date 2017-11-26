using System.IO;
using System.Threading.Tasks;

namespace AzureRelayCore.Infrastructure.Services.RelayConnection
{
    public interface IRelayConnection
    {
        Task<Stream> CreateRelayStreamAsync();
        Task CloseRelayStreamAsync();
    }
}
