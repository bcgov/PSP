using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyAnomalyType class, provides an entity for the datamodel to manage a list of property anomalies.
    /// </summary>
    public partial class PimsPropertyAnomalyType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify anomaly type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyAnomalyTypeCode; set => PropertyAnomalyTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsPropertyAnomalyType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyAnomalyType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
