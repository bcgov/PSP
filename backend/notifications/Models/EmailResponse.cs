using System;
using System.Collections.Generic;
using System.Linq;

namespace Pims.Notifications.Models
{
    /// <summary>
    /// EmailResponse class, provides a model that represents an email response.
    /// </summary>
    public class EmailResponse
    {
        #region Properties
        /// <summary>
        /// get/set - The email transaction id.
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// get/set - The email messages for the transaction.
        /// </summary>
        /// <typeparam name="MessageResponse"></typeparam>
        public IEnumerable<MessageResponse> Messages { get; set; } = new List<MessageResponse>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an EmailResponse object.
        /// </summary>
        public EmailResponse()
        {

        }

        /// <summary>
        /// Creates a new instance of an EmailResponse object, initializes with specified parameters.
        /// </summary>
        /// <param name="response"></param>
        public EmailResponse(Ches.Models.EmailResponseModel response)
        {
            this.TransactionId = response.TransactionId;

            if (response.Messages?.Any() ?? false)
            {
                ((List<MessageResponse>)this.Messages).AddRange(response.Messages.Select(m => new MessageResponse(m)));
            }
        }
        #endregion
    }
}
