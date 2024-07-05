using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseChklstItemType class, provides an entity for the datamodel to manage a list of lease checklist item types.
    /// </summary>
    public partial class PimsLeaseChklstItemType : IBaseEntity, IExpiringTypeEntity<DateOnly, DateOnly?>
    {
        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseChklstItemTypeCode; set => LeaseChklstItemTypeCode = value; }

        /// <summary>
        /// get/set - Primary key to the parent section of this checklist item.
        /// </summary>
        [NotMapped]
        public string ParentId { get => LeaseChklstSectionTypeCode; set => LeaseChklstSectionTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsLeaseChklstItemType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseChklstItemType(string id)
            : this()
        {
            Id = id;
        }

        public PimsLeaseChklstItemType()
        {
        }
    }
}
