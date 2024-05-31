using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseChklstSectionType class, provides an entity for the datamodel to manage a list of lease checklist section types.
    /// </summary>
    public partial class PimsLeaseChklstSectionType : ITypeEntity<string>, IExpiringTypeEntity<DateOnly, DateOnly?>
    {

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseChklstSectionTypeCode; set => LeaseChklstSectionTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsAcqChklstSectionType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseChklstSectionType(string id)
            : this()
        {
            Id = id;
        }

        public PimsLeaseChklstSectionType()
        {
        }
    }
}
