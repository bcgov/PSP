using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeaseAssociationModel
    {
        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The File number.
        /// </summary>
        public string FileNumber { get; set; }

        /// <summary>
        /// get/set - A collection of the stakeholders for this lease.
        /// </summary>
        public IEnumerable<LeaseStakeholderModel> Stakeholders { get; set; }

        /// <summary>
        /// get/set - The date the lease expires.
        /// </summary>
        public DateOnly? LeaseExpiryDate { get; set; }

        /// <summary>
        /// get/set - The file status type.
        /// </summary>
        public CodeTypeModel<string> FileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The user GUID who created the lease.
        /// </summary>
        public Guid? AppCreateUserGuid { get; set; }

        /// <summary>
        /// get/set - The user IDIR who created the lease.
        /// </summary>
        public string AppCreateUserid { get; set; }
    }
}
