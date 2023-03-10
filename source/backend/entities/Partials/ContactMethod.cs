using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ContactMethod class, provides an entity for the datamodel to manage personal contact method.
    /// </summary>
    public partial class PimsContactMethod : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ContactMethodId; set => this.ContactMethodId = value; }
        #endregion

        #region Constructors
        public PimsContactMethod()
        {
        }

        /// <summary>
        /// Create a new instance of a ContactMethod class, initializes with specified arguments.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        /// <param name="methodTypeId"></param>
        /// <param name="value"></param>
        public PimsContactMethod(PimsPerson person, PimsOrganization organization, string methodTypeId, string value)
            : this()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(value));
            }

            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.PersonId;
            this.Organization = organization;
            this.OrganizationId = organization?.Internal_Id;
            this.ContactMethodTypeCode = methodTypeId;
            this.ContactMethodValue = value;
        }

        /// <summary>
        /// Create a new instance of a ContactMethod class, initializes with specified arguments.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        /// <param name="methodType"></param>
        /// <param name="value"></param>
        public PimsContactMethod(PimsPerson person, PimsOrganization organization, PimsContactMethodType methodType, string value)
            : this()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(value));
            }

            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.PersonId;
            this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            this.OrganizationId = organization.Internal_Id;
            this.ContactMethodTypeCodeNavigation = methodType ?? throw new ArgumentNullException(nameof(methodType));
            this.ContactMethodTypeCode = methodType.ContactMethodTypeCode;
            this.ContactMethodValue = value;
        }
        #endregion
    }
}
