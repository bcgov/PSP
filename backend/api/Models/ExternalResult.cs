namespace Pims.Api.Models
{
    /// <summary>
    /// Defines the results comming back from an external resource.
    /// </summary>
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
    }
}