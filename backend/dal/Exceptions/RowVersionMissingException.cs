using System;
using System.Runtime.Serialization;

namespace Pims.Dal.Exceptions
{
    /// <summary>
    /// ConcurrencyControlNumberMissingException class, provides a way to throw an exception when an attempt to update or remove an item does not include a ConcurrencyControlNumber value.
    /// </summary>
    [Serializable]
    public class ConcurrencyControlNumberMissingException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ConcurrencyControlNumberMissingException class.
        /// </summary>
        /// <returns></returns>
        public ConcurrencyControlNumberMissingException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a ConcurrencyControlNumberMissingException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ConcurrencyControlNumberMissingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a ConcurrencyControlNumberMissingException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public ConcurrencyControlNumberMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a ConcurrencyControlNumberMissingException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ConcurrencyControlNumberMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
