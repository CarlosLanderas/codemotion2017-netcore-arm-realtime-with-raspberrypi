using AzureRelayCore.Infrastructure.Services.RelayConnection;
using System;
using System.IO;
using System.Threading.Tasks;
using AzureRelayCore.Infrastructure;
using System.Threading;

namespace AzureRelayCore.Infrastructure.Services.ConsoleStream
{
    public class ConsoleStream : IStream
    {
        private readonly IRelayConnection _relayConnection;
        private const string INTRO_MESSAGE = "Enter lines of text to send to the server with ENTER";
        public ConsoleStream(IRelayConnection relayConnection)
        {
            _relayConnection = relayConnection;
        }

        public async Task StartStreamAsync()
        {
            Stream hyConnection = await _relayConnection.CreateRelayStreamAsync();
            Console.WriteLine(INTRO_MESSAGE);

            var reads = Task.Run(async () =>
            {
                var reader = new StreamReader(hyConnection);
                var writer = Console.Out;
                do
                {
                    string line = await reader.ReadLineAsync();
                    if (String.IsNullOrEmpty(line))
                    {
                        break;
                    }
                    await writer.WriteLineAsync($"Received from hybrid connection: {line}");
                }
                while (true);
                reader.Dispose();
                writer.Dispose();
            });

            var writes = Task.Run(async () =>
            {
                var reader = Console.In;
                var writer = new StreamWriter(hyConnection) { AutoFlush = true };
                do
                {
                    string line = await reader.ReadLineAsync();
                    await writer.WriteLineAsync(line);
                    Console.WriteLine($"Sending to hybrid connection: {line}");
                    if (String.IsNullOrEmpty(line))
                    {
                        break;
                    }
                }
                while (true);
                reader.Dispose();
                writer.Dispose();
            });

            await Task.WhenAll(reads, writes);
            await StopStreamAsync();
        }

        public async Task StopStreamAsync()
        {
            await _relayConnection.CloseRelayStreamAsync();
        }
    }
}
