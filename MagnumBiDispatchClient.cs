using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MagnumBi.Dispatch.Client.Models;
using Newtonsoft.Json;

namespace MagnumBi.Dispatch.Client{
    public class MagnumBiDispatchClient{
        private readonly CancellationTokenSource cts;
        private readonly HttpClient httpClient;
        private readonly string serverAddress;

        /// <summary>
        ///     Creates a new MagnumBI Dispatch Client
        /// </summary>
        /// <param name="serverAddress">The full uri of the server.</param>
        /// <param name="accessToken">The access token</param>
        /// <param name="secretToken">Secret token</param>
        /// <param name="verifySsl">Verify ssl certificate. Reccomended to leave true.</param>
        /// <param name="cts">Optional token source used to cancel in progress requests.</param>
        public MagnumBiDispatchClient(string serverAddress, string accessToken, string secretToken,
            bool verifySsl = true, CancellationTokenSource cts = default(CancellationTokenSource)){
            this.serverAddress = serverAddress;
            this.cts = cts;
            if (this.cts == null) {
                this.cts = new CancellationTokenSource();
            }

            HttpClientHandler handler =
                new HttpClientHandler{
                    SslProtocols = SslProtocols.Tls12
                };
            if (!verifySsl) {
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            }

            byte[] byteArray = Encoding.ASCII.GetBytes($"{accessToken}:{secretToken}");
            this.httpClient = new HttpClient(handler);
            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            this.httpClient.Timeout = TimeSpan.FromSeconds(45);
        }

        /// <summary>
        ///     Checks the server/connection is running correctly.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckStatus(){
            return await this.CheckStatusCode() == HttpStatusCode.OK;
        }

        /// <summary>
        ///     Gets and returns the status code of the status check request.
        /// </summary>
        /// <returns></returns>
        public async Task<HttpStatusCode> CheckStatusCode(){
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.serverAddress}/job/", this.cts.Token);
            return response.StatusCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="appId">The id of the application queue we wish to poll</param>
        /// <param name="pollTimeout">The long polling max time. -1 to disable.</param>
        /// <param name="jobTimeout">The time allowed to complete job in.</param>
        /// <returns>A job if there is one avalible else null.</returns>
        public async Task<Job> GetJob(string appId, int pollTimeout = -1, int jobTimeout = 20){
            HttpContent content = new StringContent(JsonConvert.SerializeObject(new RequestJob{
                AppId = appId,
                JobTimeout = jobTimeout,
                RequestTimeout = pollTimeout
            }));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            // Send query.
            HttpResponseMessage response =
                await this.httpClient.PostAsync($"{this.serverAddress}/job/request", content, this.cts.Token);
            if (!response.IsSuccessStatusCode) {
                throw new HttpRequestException($"Failed to get job code:{response.StatusCode}");
            }

            // Process response data.
            string data = await response.Content.ReadAsStringAsync();
            dynamic job = JsonConvert.DeserializeObject(data);
            Job actualJob = null;
            try {
                string jobId = job.jobId;
                if (string.IsNullOrWhiteSpace(jobId)) {
                    return null;
                }
                actualJob = new Job(jobId, job.data, appId, this);
            } catch (Exception) {
                // No job on queue.
                return null;
            }
            return actualJob;
        }

        /// <summary>
        /// Queues a new job on the <paramref name="queueId"/> job queue.
        /// </summary>
        /// <param name="queueId">The id of the queue to add job to.</param>
        /// <param name="data">The job data.</param>
        /// <returns></returns>
        public async Task QueueJob(string queueId, dynamic data){
            HttpContent content = new StringContent(JsonConvert.SerializeObject(new QueueJob{
                AppId = queueId,
                Data = data
            }));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            // Send query.
            HttpResponseMessage response =
                await this.httpClient.PostAsync($"{this.serverAddress}/job/submit", content, this.cts.Token);
            if (!response.IsSuccessStatusCode) {
                throw new HttpRequestException($"Failed to queue job:{response.StatusCode}");
            }
        }


        /// <summary>
        /// Completes a job
        /// </summary>
        /// <param name="queueId">The queue id of the job to complete</param>
        /// <param name="jobId">The id of the job we are completing.</param>
        /// <returns></returns>
        public async Task CompleteJob(string queueId, string jobId){
            HttpContent content = new StringContent(JsonConvert.SerializeObject(new CompleteJob{
                AppId = queueId,
                JobId = jobId
            }));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            // Send query.
            HttpResponseMessage response =
                await this.httpClient.PostAsync($"{this.serverAddress}/job/complete", content, this.cts.Token);
            if (!response.IsSuccessStatusCode) {
                string data = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Failed to complete job:{response.StatusCode}, {data}");
            }
        }
    }
}