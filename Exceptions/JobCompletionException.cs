using System;

namespace MagnumBi.Dispatch.Client.Exceptions {
    public class JobCompletionException : Exception {
        public JobCompletionException(string message) : base(message) {

        }
    }
}