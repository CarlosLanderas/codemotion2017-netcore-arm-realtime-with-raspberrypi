using System;
using System.Collections.Generic;
using System.Text;

namespace AzureRelayCore.Domain.EMT
{
    public class Arrive
    {
        public int StopId { get; set; }
        public string BusId { get; set; }
        public string LineId { get; set; }
        public int BusTimeLeft { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class ArrivalInfo
    {
        public List<Arrive> Arrives { get; set; }
    }
}
