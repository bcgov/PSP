using System;
using System.Runtime.Serialization;

namespace Pims.Core.Exceptions
{
    /// <summary>
    /// ContractorNotInTeamException class, provides a way to validate contractor in Team when creating Acq File.
    /// </summary>
    [Serializable]
    public class ContractorNotInTeamException : Exception
    {
        public ContractorNotInTeamException()
        {
        }

        public ContractorNotInTeamException(string message)
            : base(message)
        {
        }

        public ContractorNotInTeamException(string message, Exception innerException)
    : base(message, innerException)
        {
        }

        protected ContractorNotInTeamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
