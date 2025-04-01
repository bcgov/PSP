using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pims.Dal.Entities.Extensions;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDspChklstItemType class, provides an entity for the datamodel to manage a list of Disposition checklist item types.
    /// </summary>
    public partial class PimsDspChklstItemType : IBaseEntity, IExpiringTypeEntity<DateOnly, DateOnly?>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => DspChklstItemTypeCode; set => DspChklstItemTypeCode = value; }

        /// <summary>
        /// get/set - Primary key to the parent section of this checklist item.
        /// </summary>
        [NotMapped]
        public string ParentId { get => DspChklstSectionTypeCode; set => DspChklstSectionTypeCode = value; }

        [NotMapped]
        public bool? IsDisabled
        {
            get => this.IsExpiredType();
            set { }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsDspChklstItemType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDspChklstItemType(string id)
            : this()
        {
            Id = id;
        }

        public PimsDspChklstItemType()
        {
        }
        #endregion
    }
}
