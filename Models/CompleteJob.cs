using Newtonsoft.Json;

namespace MagnumBi.Dispatch.Client.Models{
    public class CompleteJob{
        [JsonProperty("queueId", Required = Required.Always)]
        public string QueueId{ get; set; }

        [JsonProperty("jobId", Required = Required.Always)]
        public string JobId{ get; set; }
    }
}