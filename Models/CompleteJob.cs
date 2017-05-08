using Newtonsoft.Json;

namespace Optimal.MagnumMicroservices.Library.Models{
    public class CompleteJob{
        [JsonProperty("appId", Required = Required.Always)]
        public string AppId{ get; set; }

        [JsonProperty("jobId", Required = Required.Always)]
        public string JobId{ get; set; }
    }
}