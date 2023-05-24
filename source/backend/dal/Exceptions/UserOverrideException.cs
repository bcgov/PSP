using System;
using System.Runtime.Serialization;

namespace Pims.Dal.Exceptions
{
    /// <summary>
    /// UserOverrideException class, provides a way to throw an exception indicating that user action is required to proceed.
    /// </summary>
    [Serializable]
    public class UserOverrideException : Exception
    {
        /// <summary>
        /// get/set - For some errors that could be handled programmatically, a short string indicating the error code reported.
        /// </summary>
        public string ErrorCode { get; set; }

        #region Constructors

        /// <summary>
        /// Creates a new instance of a UserOverrideException class.
        /// </summary>
        /// <returns></returns>
        public UserOverrideException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a UserOverrideException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public UserOverrideException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a UserOverrideException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public UserOverrideException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a new instance of a UserOverrideException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public UserOverrideException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a UserOverrideException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected UserOverrideException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
