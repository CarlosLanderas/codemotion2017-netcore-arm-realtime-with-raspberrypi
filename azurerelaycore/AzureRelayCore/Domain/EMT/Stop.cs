using System;
using System.Collections.Generic;
using System.Text;

namespace AzureRelayCore.Domain.EMT
{
    public class Stop
    {
        public int Line { get; set; }
        public int Node { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
