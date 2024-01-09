using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDspChklstItemStatusType class, provides an entity for the datamodel to manage a list of disposition checklist item status types.
    /// </summary>
    public partial class PimsDspChklstItemStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => DspChklstItemStatusTypeCode; set => DspChklstItemStatusTypeCode = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsDspChklstItemStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDspChklstItemStatusType(string id)
            : this()
        {
            Id = id;
        }

        public PimsDspChklstItemStatusType()
        {
        }
        #endregion
    }
}
