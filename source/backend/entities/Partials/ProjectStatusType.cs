using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProjectStatusType class, provides an entity for the datamodel to manage project status types.
    /// </summary>
    public partial class PimsProjectStatusType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify property service file type.
        /// </summary>
        [NotMapped]
        public string Id { get => ProjectStatusTypeCode; set => ProjectStatusTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsProjectStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsProjectStatusType(string id)
            : this()
        {
            Id = id;
        }
    }
}
