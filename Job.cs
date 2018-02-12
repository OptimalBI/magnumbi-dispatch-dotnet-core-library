using System.Threading.Tasks;
using MagnumBi.Dispatch.Client.Exceptions;
using Newtonsoft.Json;

namespace MagnumBi.Dispatch.Client {
    public class Job<T> {
        protected Job() {
        }

        public Job(string jobId, T data, string queueId = null, MagnumBiDispatchClient parentClient = null) {
            this.ParentClient = parentClient;
            this.JobId = jobId;
            this.Data = data;
            this.QueueId = queueId;
        }

        public MagnumBiDispatchClient ParentClient { get; internal set; }

        [JsonProperty("data", Required = Required.Default)]
        public T Data { get; }

        [JsonProperty("jobId", Required = Required.DisallowNull)]
        public string JobId { get; }

        public string QueueId { get; internal set; }

        public async Task Complete() {
            if (this.ParentClient == null) {
                throw new JobCompletionException("Job has no client class to complete job with.");
            }

            await this.ParentClient.CompleteJob(
                this.QueueId,
                this.JobId
            );
        }
    }
}