using System;
using System.Threading.Tasks;
using AzureRelayCore.Infrastructure;

namespace AzureRelayCore
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Startup startup = new Startup();
            IServiceProvider serviceProvider = startup.ConfigureServices();
            IStream relayConnection = (IStream)serviceProvider.GetService(typeof(IStream));

            Console.CancelKeyPress += async (sender, eventArgs) =>
            {
                await relayConnection.StopStreamAsync();
                Console.WriteLine("Process aborted");
            };

            await relayConnection.StartStreamAsync();
            Console.ReadKey();

        }
    }
}