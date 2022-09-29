using System;
using System.Runtime.Serialization;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// BusinessRuleViolationException class, provides a way to express an error resulting from violating business rules.
    /// </summary>
    [Serializable]
    public class BusinessRuleViolationException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of an BusinessRuleViolationException class.
        /// </summary>
        /// <returns></returns>
        public BusinessRuleViolationException()
        {
        }

        /// <summary>
        /// Creates a new instance of an BusinessRuleViolationException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public BusinessRuleViolationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of an BusinessRuleViolationException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public BusinessRuleViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of an BusinessRuleViolationException class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected BusinessRuleViolationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
