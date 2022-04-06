using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// ResearchFileStatusType class, provides an entity for the datamodel to manage research status types.
    /// </summary>
    public partial class PimsResearchFileStatusType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property tenure type.
        /// </summary>
        [NotMapped]
        public string Id { get => ResearchFileStatusTypeCode; set => ResearchFileStatusTypeCode = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyTenureType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsResearchFileStatusType(string id) : this()
        {
            Id = id;
        }
        #endregion
    }
}
