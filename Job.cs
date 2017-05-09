using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Optimal.MagnumMicroservices.Library{
    [JsonObject(MemberSerialization.OptIn)]
    public class Job{
        protected Job(){
        }

        public Job(string jobId, dynamic data, string appId = null, MagnumMicroservicesClient parentClient = null){
            this.ParentClient = parentClient;
            this.JobId = jobId;
            this.Data = data;
            this.AppId = appId;
        }

        public MagnumMicroservicesClient ParentClient{ get; internal set; }

        [JsonProperty("data", Required = Required.Default)]
        public dynamic Data{ get; }

        [JsonProperty("jobId", Required = Required.DisallowNull)]
        public string JobId{ get; }

        public string AppId{ get; }

        public async Task Complete(){
            await this.ParentClient.CompleteJob(
                this.AppId,
                this.JobId
            );
        }
    }
}