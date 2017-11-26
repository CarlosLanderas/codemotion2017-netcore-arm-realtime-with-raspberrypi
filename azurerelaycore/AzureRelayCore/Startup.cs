using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using AzureRelayCore.Infrastructure.Services.ConsoleStream;
using AzureRelayCore.Infrastructure.Services.RelayConnection;
using AzureRelayCore.Infrastructure;
using AzureRelayCore.Infrastructure.Services.EMT;
using AzureRelayCore.Infrastructure.Services.EMTStream;

namespace AzureRelayCore
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }
        public IServiceCollection Services { get; private set; }
        public Startup()
        {
            Services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices()
        {
            Services.AddOptions();
            Services.Configure<RelayConfig>(Configuration.GetSection("HybridConnection"));
            Services.Configure<EMTConfig>(Configuration.GetSection("TransportService"));

            Services.AddSingleton<EMTStreamHandler>();
            Services.AddSingleton<IRelayConnection, RelayConnection>();

            //Services.AddSingleton<IStream, ConsoleStream>();
            Services.AddSingleton<IStream, EMTStream>();

            return Services.BuildServiceProvider();
        }
    }
}
