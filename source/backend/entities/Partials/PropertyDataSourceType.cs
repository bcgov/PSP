using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// DataSourceType class, provides an entity for the datamodel to manage a list of data source types.
    /// </summary>
    public partial class PimsDataSourceType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to data source types.
        /// </summary>
        [NotMapped]
        public string Id { get => DataSourceTypeCode; set => DataSourceTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a DataSourceType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDataSourceType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
