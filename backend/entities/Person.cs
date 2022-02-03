using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// Person class, provides an entity for the datamodel to manage persons.
    /// </summary>
    public partial class PimsPerson : IdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify person.
        /// </summary>
        [NotMapped]
        public override long Id { get => PersonId; set => PersonId = value; }

        public long? GetPersonOrganizationId() => PimsPersonOrganizations?.FirstOrDefault(t => t != null)?.PersonOrganizationId;
        public long? GetPersonOrganizationRowVersion() => PimsPersonOrganizations?.FirstOrDefault(t => t != null)?.ConcurrencyControlNumber;
        public ICollection<PimsOrganization> GetOrganizations() => PimsPersonOrganizations?.Select(p => p.Organization).Select(o => { o.PimsPersonOrganizations = null; return o; }).ToArray();
        public ICollection<PimsAddress> GetAddresses() => PimsPersonAddresses?.Select(pa => pa.Address).ToArray();

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Person class, initializes with specified arguments.
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="firstname"></param>
        public PimsPerson(string surname, string firstname, PimsAddress address) : this()
        {
            if (String.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(surname));
            if (String.IsNullOrWhiteSpace(firstname)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(firstname));

            this.Surname = surname;
            this.FirstName = firstname;
            this.PimsPersonAddresses = new List<PimsPersonAddress>() { new PimsPersonAddress() { Person = this, Address = address } };
        }
        #endregion
    }
}
