using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Moq;
using Moq.EntityFrameworkCore;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "contact")]
    [ExcludeFromCodeCoverage]
    public class ContactRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> ContactFilterData =>
            new List<object[]>
            {
                new object[] { new ContactFilter() { Municipality = "victoria" }, 2 },
                new object[] { new ContactFilter() { Municipality = "fake city" }, 0 },
                new object[] { new ContactFilter() { Summary = "person name" }, 1 },
                new object[] { new ContactFilter() { Summary = "organization" }, 1 },
                new object[] { new ContactFilter() { ActiveContactsOnly = true, SearchBy = "all" }, 2 },
                new object[] { new ContactFilter() { ActiveContactsOnly = false, SearchBy = "all" }, 3 },
                new object[] { new ContactFilter() { SearchBy = "organizations" }, 1 },
                new object[] { new ContactFilter() { SearchBy = "persons" }, 1 },
                new object[] { new ContactFilter() { SearchBy = "all" }, 2 },
            };
        #endregion

        #region Tests

        [Theory]
        [MemberData(nameof(ContactFilterData))]
        public void Get_Contacts(ContactFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);
            var ePersonAddress = EntityHelper.CreateAddress(2, "test person address", "201", "victoria", "V8P 5V6");
            var eOrgAddress = EntityHelper.CreateAddress(3, "test org address", "101", "victoria", "V8P 5V4");
            var eInactiveAddress = EntityHelper.CreateAddress(4);
            var person = EntityHelper.CreatePerson(2, "person", "name");
            var inactivePerson = EntityHelper.CreatePerson(3, "test", "inactive person");
            var organization = EntityHelper.CreateOrganization(4, "organization");
            var personContact = EntityHelper.CreateContact("P1", false, ePersonAddress, null, person);
            var inactiveContact = EntityHelper.CreateContact("P2", true, eInactiveAddress, null, inactivePerson);
            var organizationContact = EntityHelper.CreateContact("O1", false, eOrgAddress, organization, null);

            var dbContextMock = new Mock<PimsContext>();
            dbContextMock.Setup(x => x.PimsContactMgrVws).ReturnsDbSet(new Entity.PimsContactMgrVw[] { organizationContact, personContact, inactiveContact });
            dbContextMock.Setup(x => x.PimsAddresses).ReturnsDbSet(new Entity.PimsAddress[] { ePersonAddress, eOrgAddress, eInactiveAddress });
            dbContextMock.Setup(x => x.PimsPeople).ReturnsDbSet(new Entity.PimsPerson[] { person, inactivePerson });
            dbContextMock.Setup(x => x.PimsOrganizations).ReturnsDbSet(new Entity.PimsOrganization[] { organization });
            dbContextMock.Setup(x => x.PimsPersonOrganizations).ReturnsDbSet(new Entity.PimsPersonOrganization[] { });

            var service = helper.CreateMockRepository<ContactRepository>(user, dbContextMock.Object);

            // Act
            var result = service.Get(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [MemberData(nameof(ContactFilterData))]
        public void Get_Contacts_Paged(ContactFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);
            var ePersonAddress = EntityHelper.CreateAddress(2, "test person address", "201", "victoria", "V8P 5V6");
            var eOrgAddress = EntityHelper.CreateAddress(3, "test org address", "101", "victoria", "V8P 5V4");
            var eInactiveAddress = EntityHelper.CreateAddress(4);
            var person = EntityHelper.CreatePerson(2, "person", "name");
            var inactivePerson = EntityHelper.CreatePerson(3, "test", "inactive person");
            var organization = EntityHelper.CreateOrganization(4, "organization");
            var personContact = EntityHelper.CreateContact("P1", false, ePersonAddress, null, person);
            var inactiveContact = EntityHelper.CreateContact("P2", true, eInactiveAddress, null, inactivePerson);
            var organizationContact = EntityHelper.CreateContact("O1", false, eOrgAddress, organization, null);

            var dbContextMock = new Mock<PimsContext>();
            dbContextMock.Setup(x => x.PimsContactMgrVws).ReturnsDbSet(new Entity.PimsContactMgrVw[] { organizationContact, personContact, inactiveContact });
            dbContextMock.Setup(x => x.PimsAddresses).ReturnsDbSet(new Entity.PimsAddress[] { ePersonAddress, eOrgAddress, eInactiveAddress });
            dbContextMock.Setup(x => x.PimsPeople).ReturnsDbSet(new Entity.PimsPerson[] { person, inactivePerson });
            dbContextMock.Setup(x => x.PimsOrganizations).ReturnsDbSet(new Entity.PimsOrganization[] { organization });
            dbContextMock.Setup(x => x.PimsPersonOrganizations).ReturnsDbSet(new Entity.PimsPersonOrganization[] { });

            var service = helper.CreateMockRepository<ContactRepository>(user, dbContextMock.Object);

            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Items.Count());
        }
        #endregion
    }
}
