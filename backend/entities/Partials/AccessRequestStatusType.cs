using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequestStatusType class, provides an entity for the datamodel to manage a list of access request status types.
    /// </summary>
    public partial class PimsAccessRequestStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify access request status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AccessRequestStatusTypeCode; set => AccessRequestStatusTypeCode = value; }
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
