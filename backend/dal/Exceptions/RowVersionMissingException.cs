using System;
using System.Runtime.Serialization;

namespace Pims.Dal.Exceptions
{
    /// <summary>
    /// RowVersionMissingException class, provides a way to throw an exception when an attempt to update or remove an item does not include a RowVersion value.
    /// </summary>
    [Serializable]
    public class RowVersionMissingException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a RowVersionMissingException class.
        /// </summary>
        /// <returns></returns>
        public RowVersionMissingException() : base() { }

        /// <summary>
        /// Creates a new instance of a RowVersionMissingException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public RowVersionMissingException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance of a RowVersionMissingException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public RowVersionMissingException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Creates a new instance of a RowVersionMissingException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected RowVersionMissingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
