using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureRelayCore.Domain.EMT;
using Microsoft.Extensions.Options;

namespace AzureRelayCore.Infrastructure.Services.EMT
{
    public class EMTStreamHandler
    {
        public delegate Task OnArrivalInfoUpdated(Arrive[] arriveInfo);

        public event OnArrivalInfoUpdated onArrivalInfoUpdated;

        private readonly EMTClient _emtClient;

        private int PollingInterval = 1000;

        private const int LineId = 27;

        private Lazy<Task<Line>> lineInfo;

        public EMTStreamHandler(IOptions<EMTConfig> emtConfig)
        {
            _emtClient = new EMTClient(emtConfig.Value);

             lineInfo  = new Lazy<Task<Line>>(async () =>
                await new EMTClient(emtConfig.Value).GetLine(LineId, new CancellationToken())
            );
        }
        
        public async Task Start(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                var stopIds = (await lineInfo.Value).Stops.Select(s => s.Node);
                var stopArrivalInfoTasks = new List<Task<Arrive[]>>();
                foreach (var stopId in stopIds)
                {
                    stopArrivalInfoTasks.Add(_emtClient.GetArriveStops(LineId, stopId, cancelToken));
                }

                var taskResults = await Task.WhenAll(stopArrivalInfoTasks);
                await onArrivalInfoUpdated(taskResults.Where(t => t != null).SelectMany(arrive => arrive).ToArray());
                await Task.Delay(PollingInterval);
            }

        }
    }
}
