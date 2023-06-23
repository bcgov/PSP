using System;
using System.Runtime.Serialization;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// ForeignKeyDependencyException class, provides a way to express foreign key dependency.
    /// </summary>
    [Serializable]
    public class ForeignKeyDependencyException : Exception
    {
        public ForeignKeyDependencyException()
        {
        }

        public ForeignKeyDependencyException(string message)
            : base(message)
        {
        }

        public ForeignKeyDependencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ForeignKeyDependencyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
