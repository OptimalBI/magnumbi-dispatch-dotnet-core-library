using System;

namespace MagnumBi.Dispatch.Client.Exceptions {
    /// <summary>
    ///     Exception thrown when the MagnumBI Dispatch client received a message but could not serialize it.
    /// </summary>
    public class JobSerializationException : Exception {
        public JobSerializationException(string message) : base(message) {
        }

        public JobSerializationException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}