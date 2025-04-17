using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pims.Dal.Entities.Extensions;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDspChklstSectionType class, provides an entity for the datamodel to manage a list of disposition checklist section types.
    /// </summary>
    public partial class PimsDspChklstSectionType : ITypeEntity<string, bool?>, IExpiringTypeEntity<DateOnly, DateOnly?>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => DspChklstSectionTypeCode; set => DspChklstSectionTypeCode = value; }

        [NotMapped]
        public bool? IsDisabled
        {
            get => this.IsExpiredType();
            set { }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsDspChklstSectionType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDspChklstSectionType(string id)
            : this()
        {
            Id = id;
        }

        public PimsDspChklstSectionType()
        {
        }
        #endregion
    }
}
