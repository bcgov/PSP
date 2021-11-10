using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "contact")]
    [ExcludeFromCodeCoverage]
    public class ContactServiceTest
    {
        #region Data
        public static IEnumerable<object[]> ContactFilterData =>
            new List<object[]>
            {
                new object[] { new ContactFilter() { Municipality = "victoria" }, 1 },
                new object[] { new ContactFilter() { Municipality = "fake city" }, 0 },
                new object[] { new ContactFilter() { Summary = "person name" }, 1 },
                new object[] { new ContactFilter() { Summary = "organization name" }, 1 },
                new object[] { new ContactFilter() { ActiveContactsOnly = true }, 2 },
                new object[] { new ContactFilter() { ActiveContactsOnly = false }, 3 },
                new object[] { new ContactFilter() { SearchBy = "organizations" }, 1 },
                new object[] { new ContactFilter() { SearchBy = "persons" }, 1 },
                new object[] { new ContactFilter() { SearchBy = "all" }, 2 },
                new object[] { new ContactFilter(), 2 },
            };
        #endregion

        #region Tests
        [Fact]
        public void Contact_Count()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);
            var econtact = EntityHelper.CreateContact("1", false, firstName: "person");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(econtact);

            var service = helper.CreateService<ContactService>(user);

            // Act
            var result = service.Count();

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Contact_Sort()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);
            var contact1 = EntityHelper.CreateContact("1", false, firstName: "alex aardvark");
            var contact2 = EntityHelper.CreateContact("2", false, firstName: "zeta zellers");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(new Entity.Contact[] { contact1, contact2 });

            var service = helper.CreateService<ContactService>(user);

            var filter = new ContactFilter() { Sort = new string[] { "Summary desc" } };
            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.Equal(contact2.FirstName, result.FirstOrDefault().FirstName);
        }

        #region Get
        [Theory]
        [MemberData(nameof(ContactFilterData))]
        public void Get_Contacts_Paged(ContactFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);
            var personContact = EntityHelper.CreateContact("1", false, firstName: "person", surname: "name", municipality: "victoria");
            var organizationContact = EntityHelper.CreateContact("2", false, organizationName: "organization name");
            var disabledContact = EntityHelper.CreateContact("3", true, firstName: "disabled");

            helper.CreatePimsContext(user, true).AddAndSaveChanges(new Entity.Contact[] { personContact, organizationContact, disabledContact });

            var service = helper.CreateService<ContactService>(user);

            // Act
            var result = service.Get(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.Contact[]>(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void Get_Contacts_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateService<ContactService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get(null));
        }

        [Theory]
        [MemberData(nameof(ContactFilterData))]
        public void Get_Contacts_Filter(ContactFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ContactView);
            var personContact = EntityHelper.CreateContact("1", false, firstName: "person", surname: "name", municipality: "victoria");
            var organizationContact = EntityHelper.CreateContact("2", false, organizationName: "organization name");
            var disabledContact = EntityHelper.CreateContact("3", true, firstName: "disabled");

            helper.CreatePimsContext(user, true).AddAndSaveChanges(new Entity.Contact[] { personContact, organizationContact, disabledContact });

            var service = helper.CreateService<ContactService>(user);

            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.Contact>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
        }
        #endregion
        #endregion
    }
}
