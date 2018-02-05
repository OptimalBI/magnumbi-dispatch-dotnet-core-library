using Newtonsoft.Json;

namespace MagnumBi.Dispatch.Client.Models{
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