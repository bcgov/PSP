using System;
using System.Runtime.Serialization;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// BusinessRuleViolationException class, provides a way to express an error resulting from violating business rules.
    /// </summary>
    [Serializable]
    public class MayanRepositoryException : Exception
    {
        public MayanRepositoryException()
        {
        }

        public MayanRepositoryException(string message)
            : base(message)
        {
        }

        public MayanRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MayanRepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
