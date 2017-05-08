using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Optimal.MagnumMicroservices.Library.Models{
    [JsonObject]
    public class RequestJob{
        [JsonProperty]
        public string AppId{ get; set; }

        [JsonProperty("JobHandleTimeoutSeconds")]
        public int JobTimeout = 20;

        [JsonProperty("Timeout")]
        public int RequestTimeout = -1;
    }
}