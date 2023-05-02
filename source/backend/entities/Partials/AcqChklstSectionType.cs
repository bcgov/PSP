using System.ComponentModel.DataAnnotations.Schema;
using Pims.Dal.Entities.Extensions;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqChklstSectionType class, provides an entity for the datamodel to manage a list of acquisition checklist section types.
    /// </summary>
    public partial class PimsAcqChklstSectionType : ITypeEntity<string>, IExpiringTypeEntity
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqChklstSectionTypeCode; set => AcqChklstSectionTypeCode = value; }

        [NotMapped]
        public bool? IsDisabled
        {
            get => this.IsExpiredType();
            set { }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcqChklstSectionType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqChklstSectionType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
