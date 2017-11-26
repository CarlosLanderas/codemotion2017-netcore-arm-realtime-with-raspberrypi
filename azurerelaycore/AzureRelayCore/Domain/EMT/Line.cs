using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AzureRelayCore.Domain.EMT
{
    public class Line
    {
        [JsonProperty("resultValues")]
        public List<Stop> Stops { get; set; }
    }
}
