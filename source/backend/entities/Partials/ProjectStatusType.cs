using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectStatusType class, provides an entity for the datamodel to manage a list of project status types.
    /// </summary>
    public partial class PimsProjectStatusType : ITypeEntity<string>
    {
        /// <summary>
        /// Create a new instance of class.
        /// </summary>
        /// <param name="id"></param>
        public PimsProjectStatusType(string id)
            : this()
        {
            Id = id;
        }

        [NotMapped]
        public string Id { get => ProjectStatusTypeCode; set => ProjectStatusTypeCode = value; }

        public static class PimsProjectStatusTypes
        {
            public const string ACTIVE = "AC";
            public const string CANCELLED = "CA";
            public const string CONSOLIDATED = "CNCN";
            public const string COMPLETED = "CO";
            public const string ONHOLD = "HO";
            public const string PLANNING = "PL";
        }
    }
}
