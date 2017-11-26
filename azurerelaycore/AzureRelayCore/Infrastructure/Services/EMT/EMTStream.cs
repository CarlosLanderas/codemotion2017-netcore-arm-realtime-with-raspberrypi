
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AzureRelayCore.Domain.EMT;
using AzureRelayCore.Infrastructure.Services.EMT;
using AzureRelayCore.Infrastructure.Services.RelayConnection;
using Newtonsoft.Json;
using System.Collections.Generic;
using AzureRelayCore.Infrastructure.Extensions;
using Newtonsoft.Json.Serialization;
using System;

namespace AzureRelayCore.Infrastructure.Services.EMTStream
{
    public class EMTStream : IStream
    {
        private readonly IRelayConnection _relayConnection;
        private readonly EMTStreamHandler _emtStreamHandler;
        private StreamWriter _connectionWriter;
        private CancellationTokenSource _cancellationTokenSoure = new CancellationTokenSource();
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public EMTStream(IRelayConnection relayConnection, EMTStreamHandler emtStreamHandler)
        {
            _relayConnection = relayConnection;
            _emtStreamHandler = emtStreamHandler;
        }
        public async Task StartStreamAsync()
        {
            var hybridConnection = await _relayConnection.CreateRelayStreamAsync();
            _connectionWriter = new StreamWriter(hybridConnection) { AutoFlush = true };
            _emtStreamHandler.onArrivalInfoUpdated += ArriveInfoUpdated;
            
            await _emtStreamHandler.Start(_cancellationTokenSoure.Token);
        }

        public async Task StopStreamAsync()
        {
            _cancellationTokenSoure.Cancel();
            _emtStreamHandler.onArrivalInfoUpdated -= ArriveInfoUpdated;
            _connectionWriter.Dispose();
            await _relayConnection.CloseRelayStreamAsync();
        }

        private async Task ArriveInfoUpdated(Arrive[] arriveInfo)
        {
            Console.WriteLine($"Sending {arriveInfo.Length} registers to hybrid connection");
            var arriveList = new List<Arrive>(arriveInfo);
            var chunkedLists = arriveList.ChunkBy(5);
            foreach (var chunk in chunkedLists)
            {
                await _connectionWriter.WriteAsync(JsonConvert.SerializeObject(chunk, jsonSettings));
            }
        }

    }
}
