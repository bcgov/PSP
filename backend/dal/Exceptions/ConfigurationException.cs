using System;
using System.Runtime.Serialization;

namespace Pims.Dal.Exceptions
{
    /// <summary>
    /// ConfigurationException class, provides a way to throw an exception when a configuration is invalid.
    /// </summary>
    [Serializable]
    public class ConfigurationException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ConfigurationException class.
        /// </summary>
        /// <returns></returns>
        public ConfigurationException()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a ConfigurationException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of a ConfigurationException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of a ConfigurationException class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
