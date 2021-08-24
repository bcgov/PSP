using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequestStatusType class, provides an entity for the datamodel to manage a list of access request status types.
    /// </summary>
    [MotiTable("PIMS_ACCESS_REQUEST_STATUS_TYPE", "ARQSTT")]
    public class AccessRequestStatusType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify access request status type.
        /// </summary>
        [Column("ACCESS_REQUEST_STATUS_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - A collection of access requests.
        /// </summary>
        public ICollection<AccessRequest> AccessRequests { get; } = new List<AccessRequest>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a AccessRequestStatusType class.
        /// </summary>
        public AccessRequestStatusType() { }

        /// <summary>
        /// Create a new instance of a AccessRequestStatusType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public AccessRequestStatusType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }

    public static class AccessRequestStatusTypes
    {
        public const string APPROVED = "APPROVED";
        public const string DENIED = "DENIED";
        public const string INITIATED = "INITIATED";
        public const string MOREINFO = "MOREINFO";
        public const string RECEIVED = "RECEIVED";
        public const string REVIEWCOMPLETE = "REVIEWCOMPLETE";
        public const string UNDERREVIEW = "UNDERREVIEW";
    }
}
