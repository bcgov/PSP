using System.Net;
using Pims.Api.Concepts.CodeTypes;

namespace Pims.Api.Concepts.Models.Concepts.Http
{
    /// <summary>
    /// Defines the results coming back from an external resource.
    /// </summary>
    /// <typeparam name="T">The type of the object in the payload wrapped by this result.</typeparam>
    public class ExternalResult<T>
    {
        /// <summary>
        /// get/set - Result status.
        /// </summary>
        public ExternalResultStatus Status { get; set; }

        /// <summary>
        /// get/set - Additional message for the result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// get/set - A description of the type.
        /// </summary>
        public T Payload { get; set; }

        /// <summary>
        /// get/set - The http status code returned.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
