using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        public void Get_Contacts_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<ContactRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get("123"));
        }
        #endregion
    }
}
