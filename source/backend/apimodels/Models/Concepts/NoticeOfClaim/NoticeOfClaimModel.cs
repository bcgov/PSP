using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.NoticeOfClaim
{
    public class NoticeOfClaimModel : BaseConcurrentModel
    {
        #region Properties
        public long? Id { get; set; }

        public long? AcquisitionFileId { get; set; }

        public long? ManagementFileId { get; set; }

        public string Comment { get; set; }

        public DateOnly? ReceivedDate { get; set; }

        #endregion
    }
}
