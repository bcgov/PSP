using System;
using System.Linq;
using Pims.Dal;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of an ContactMethod.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Entity.PimsContactMethod CreateContactMethod(long id, string value, Entity.PimsPerson person, Entity.PimsOrganization organization = null, Entity.PimsContactMethodType type = null)
        {
            organization ??= person?.GetOrganizations().FirstOrDefault() ?? EntityHelper.CreateOrganization(id, "Test 1");
            type ??= EntityHelper.CreateContactMethodType("Email");
            return new Entity.PimsContactMethod(person, organization, type, value)
            {
                ContactMethodId = id,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of an ContactMethod.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Entity.PimsContactMethod CreateContactMethod(this PimsContext context, long id, string value, Entity.PimsPerson person, Entity.PimsOrganization organization = null, Entity.PimsContactMethodType type = null)
        {
            organization ??= context.PimsOrganizations.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find an organization.");
            type ??= context.PimsContactMethodTypes.FirstOrDefault(t => t.Id == "Email") ?? throw new InvalidOperationException("Unable to find 'Email' contact method type.");
            return new Entity.PimsContactMethod(person, organization, type, value)
            {
                ContactMethodId = id,
                ConcurrencyControlNumber = 1,
            };
        }
    }
}
