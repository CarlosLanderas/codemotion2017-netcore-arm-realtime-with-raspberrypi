using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureRelayCore.Infrastructure
{
    public interface IStream
    {
        Task StartStreamAsync();
        Task StopStreamAsync();
    }
}
