using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AdministrativeArea class, provides an entity for the datamodel to manage a list of administrative areas (city, municipality, district, etc.).
    /// </summary>
    [MotiTable("PIMS_ADMINISTRATIVE_AREA", "ADMINA")]
    public class AdministrativeArea : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify administrative area.
        /// </summary>
        /// <value></value>
        [Column("ADMINISTRATIVE_AREA_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - An appreviated name.
        /// </summary>
        [Column("ABBREVIATION")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// get/set - A description of the boundary type for this area (o.e. Legal).
        /// </summary>
        [Column("BOUNDARY_TYPE")]
        public string BoundaryType { get; set; }

        /// <summary>
        /// get/set - The parent group name for this area.
        /// </summary>
        [Column("GROUP_NAME")]
        public string GroupName { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a AdministrativeArea class.
        /// </summary>
        public AdministrativeArea() { }

        /// <summary>
        /// Create a new instance of a AdministrativeArea class.
        /// </summary>
        /// <param name="name"></param>
        public AdministrativeArea(string name) : base(name)
        {
        }
        #endregion
    }
}
