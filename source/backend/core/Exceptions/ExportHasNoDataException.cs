using System;
using System.Runtime.Serialization;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// ExportHasNoDataException class provides and exception when export has no data.
    /// </summary>
    [Serializable]
    public class ExportHasNoDataException : Exception
    {
        public ExportHasNoDataException()
        {
        }

        public ExportHasNoDataException(string message)
            : base(message)
        {
        }

        public ExportHasNoDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ExportHasNoDataException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
