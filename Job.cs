using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagnumBi.Dispatch.Client{
    [JsonObject(MemberSerialization.OptIn)]
    public class Job{
        protected Job(){
        }

        public Job(string jobId, dynamic data, string appId = null, MagnumBiDispatchClient parentClient = null){
            this.ParentClient = parentClient;
            this.JobId = jobId;
            this.Data = data;
            this.AppId = appId;
        }

        public MagnumBiDispatchClient ParentClient{ get; internal set; }

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