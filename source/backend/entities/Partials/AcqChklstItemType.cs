using System.ComponentModel.DataAnnotations.Schema;
using Pims.Dal.Entities.Extensions;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqChklstItemType class, provides an entity for the datamodel to manage a list of acquisition checklist item types.
    /// </summary>
    public partial class PimsAcqChklstItemType : IBaseEntity, IExpiringTypeEntity
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqChklstItemTypeCode; set => AcqChklstItemTypeCode = value; }

        /// <summary>
        /// get/set - Primary key to the parent section of this checklist item.
        /// </summary>
        [NotMapped]
        public string ParentId { get => AcqChklstSectionTypeCode; set => AcqChklstSectionTypeCode = value; }

        [NotMapped]
        public bool? IsDisabled
        {
            get => this.IsExpiredType();
            set { }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcqChklstItemType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqChklstItemType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
