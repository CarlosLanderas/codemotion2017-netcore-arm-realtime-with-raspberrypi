using Microsoft.Azure.Relay;
using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using System.Threading;

namespace AzureRelayCore.Infrastructure.Services.RelayConnection
{
    public class RelayConnection : IRelayConnection
    {
        public TokenProvider TokenProvider { get; private set; }
        public HybridConnectionClient Client { get; private set; }

        private HybridConnectionStream _relayStream;

        public RelayConnection(IOptions<RelayConfig> relayConfig)
        {
            InitTokenProvider(relayConfig.Value);
            InitConnection(relayConfig.Value);
        }

        private void InitTokenProvider(RelayConfig relayConfig)
        {
            TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(
               relayConfig.KeyName,
               relayConfig.Key);
        }

        private void InitConnection(RelayConfig relayConfig)
        {

            Client = new HybridConnectionClient(new Uri(String.Format("sb://{0}/{1}",
                relayConfig.RelayNamespace,
                relayConfig.ConnectionName)),
                TokenProvider);
        }

        public async Task<Stream> CreateRelayStreamAsync()
        {
            _relayStream = await Client.CreateConnectionAsync();
            return _relayStream;
        }

        public async Task CloseRelayStreamAsync()
        {
            await _relayStream.CloseAsync(CancellationToken.None);
        }
    }
}
