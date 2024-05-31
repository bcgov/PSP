using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseChklstItemStatusType class, provides an entity for the datamodel to manage a list of lease checklist item status types.
    /// </summary>
    public partial class PimsLeaseChklstItemStatusType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseChklstItemStatusTypeCode; set => LeaseChklstItemStatusTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsLeaseChklstItemStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseChklstItemStatusType(string id)
            : this()
        {
            Id = id;
        }

        public PimsLeaseChklstItemStatusType()
        {
        }
    }
}
