using System;
using System.Runtime.Serialization;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// DuplicateEntityException class, provides a way to express duplicate entities.
    /// </summary>
    [Serializable]
    public class DuplicateEntityException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of an DuplicateEntityException class.
        /// </summary>
        /// <returns></returns>
        public DuplicateEntityException()
        {
        }

        /// <summary>
        /// Creates a new instance of an DuplicateEntityException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public DuplicateEntityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of an DuplicateEntityException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public DuplicateEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of an DuplicateEntityException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected DuplicateEntityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
