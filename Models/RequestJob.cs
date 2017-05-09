using Newtonsoft.Json;

namespace Optimal.MagnumMicroservices.Library.Models{
    [JsonObject]
    public class RequestJob{
        [JsonProperty("JobHandleTimeoutSeconds")]
        public int JobTimeout = 20;

        [JsonProperty("Timeout")]
        public int RequestTimeout = -1;

        [JsonProperty]
        public string AppId{ get; set; }
    }
}