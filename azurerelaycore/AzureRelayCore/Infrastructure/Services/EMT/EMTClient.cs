using AzureRelayCore.Domain.EMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace AzureRelayCore.Infrastructure.Services.EMT
{
    public class EMTClient
    {
        private readonly EMTConfig _emtConfig;

        private const string LineEndpoint =
            "https://openbus.emtmadrid.es:9443/emt-proxy-server/last/bus/GetRouteLines.php";

        private const string ArriveStopEndpoint =
            "https://openbus.emtmadrid.es:9443/emt-proxy-server/last/geo/GetArriveStop.php";

        public EMTClient(EMTConfig emtConfig)
        {
            _emtConfig = emtConfig;
        }
        
        public async Task<Line> GetLine(int lineId, CancellationToken cancellationToken)
        {
            try
            {
                var formData = BuildGetLineRequest(lineId);

                using (var client = new HttpClient())
                {
                    var result =
                        await client.PostAsync(LineEndpoint, new FormUrlEncodedContent(formData), cancellationToken);
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Line>(content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Arrive[]> GetArriveStops(int lineId, int stopId, CancellationToken cancellationToken)
        {
            var formData = BuildArriveStopRequest(stopId);
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync(ArriveStopEndpoint, new FormUrlEncodedContent(formData), cancellationToken);
                var content = await result.Content.ReadAsStringAsync();
                try
                {
                    return JsonConvert.DeserializeObject<ArrivalInfo>
                        (content).Arrives.ToArray();
                }
                catch (Exception)
                {
                    //API returning success code with invalid data
                }
                return null;
            }
        }

        private IEnumerable<KeyValuePair<string, string>> BuildGetLineRequest(int lineId)
        {
            return new Dictionary<string, string>()
            {
                {"idClient", _emtConfig.IdClient },
                { "passKey", _emtConfig.PassKey},
                {"Lines", lineId.ToString()},
                { "SelectDate", DateTime.Today.ToString("dd/MM/yyyy")}

            }.ToList();
        }

        private IEnumerable<KeyValuePair<string, string>> BuildArriveStopRequest(int stopId)
        {
            return new Dictionary<string, string>()
            {
                {"idClient",  _emtConfig.IdClient },
                { "passKey", _emtConfig.PassKey},
                {"idStop", stopId.ToString()}

            }.ToList();
        }

    }
}
