using System;
using System.Runtime.Serialization;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// ClamAvException class, provides a way to express HTTP request exceptions that occur.
    /// </summary>
    [Serializable]
    public class AvException : Exception
    {

        #region Constructors
        public AvException()
        {
        }

        /// <summary>
        /// Creates a new instance of an ClamAvException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public AvException(string message)
            : base(message)
        {
        }

        public AvException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AvException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
        #endregion
    }
}
