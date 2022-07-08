using System;
using System.Runtime.Serialization;

namespace Pims.Api.Helpers.Exceptions
{
    /// <summary>
    /// AuthenticationException class, provides a way to handle exceptions that occur when validating authentication.
    /// </summary>
    [Serializable]
    public class AuthenticationException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AuthenticationException object.
        /// </summary>
        /// <returns></returns>
        public AuthenticationException()
        {
        }

        /// <summary>
        /// Creates a new instance of a AuthenticationException object, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public AuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a AuthenticationException object, initializes it with the specified arguments.
        /// ///. </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a AuthenticationException object, initializes it with the specified arguments.
        /// ///. </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected AuthenticationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
