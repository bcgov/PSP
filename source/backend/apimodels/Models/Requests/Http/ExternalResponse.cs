using System.Net;
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Models.Requests.Http
{
    /// <summary>
    /// Defines the results coming back from an external resource.
    /// </summary>
    /// <typeparam name="T">The type of the object in the payload wrapped by this result.</typeparam>
    public class ExternalResponse<T>
    {
        /// <summary>
        /// get/set - Result status.
        /// </summary>
        public ExternalResponseStatus Status { get; set; }

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
