using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyDataSourceType class, provides an entity for the datamodel to manage a list of property data source types.
    /// </summary>
    public partial class PimsPropertyDataSourceType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to property data source types.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyDataSourceTypeCode; set => PropertyDataSourceTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyDataSourceType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyDataSourceType(string id):this()
        {
            Id = id;
        }
        #endregion
    }
}
