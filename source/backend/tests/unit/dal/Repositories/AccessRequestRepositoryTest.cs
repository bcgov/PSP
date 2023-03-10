using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    public class AccessRequestRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> AccessRequestData =>
            new List<object[]>
            {
                new object[] { new AccessRequestFilter() { SearchText = "test" }, 1 },
                new object[] { new AccessRequestFilter() { SearchText = "value" }, 1 },
                new object[] { new AccessRequestFilter() { StatusType = new Dal.Entities.PimsAccessRequestStatusType { Id = "Received" } }, 1 },
            };
        #endregion

        #region Get

        [Theory]
        [MemberData(nameof(AccessRequestData))]
        public void Get_AccessRequests_Filter(AccessRequestFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);

            var organization = EntityHelper.CreateOrganization(1, "test org");
            var role = EntityHelper.CreateRole("test role");
            var eUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "username", organization: organization, role: role);
            var eSecondUser = EntityHelper.CreateUser(2, Guid.NewGuid(), "username", organization: organization, role: role);
            var region = new PimsRegion() { Code = 2 };

            var eAccessRequest = EntityHelper.CreateAccessRequest(1, organization: organization, role: role, user: eUser, region: region);
            eAccessRequest.User.Person.Surname = "test";
            eAccessRequest.User.BusinessIdentifierValue = "value";

            var secondAccessRequest = EntityHelper.CreateAccessRequest(2, organization: organization, role: role, user: eSecondUser, region: region);
            secondAccessRequest.User.UserId = 2;
            secondAccessRequest.User.Person.Surname = "Other";
            secondAccessRequest.User.BusinessIdentifierValue = "oDisabled";
            secondAccessRequest.AccessRequestStatusTypeCodeNavigation = new PimsAccessRequestStatusType() { Id = "fake" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(eAccessRequest, secondAccessRequest);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            var result = service.GetAll(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<PimsAccessRequest>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
            result.First().AccessRequestId.Should().Be(eAccessRequest.AccessRequestId);
        }

        [Fact]
        public void Get_AccessRequests_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.CreatePimsContext(user, true);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            // Assert
            var result = Assert.Throws<NotAuthorizedException>(() => service.GetAll(new AccessRequestFilter() { SearchText = "test" }));
        }

        [Fact]
        public void Get_AccessRequest_Current()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var eUser = EntityHelper.CreateUser(1, new Guid(user.FindFirstValue("idir_user_guid")), "test user");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(accessRequest);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            var result = service.TryGet();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PimsAccessRequest>(result);
            result.AccessRequestId.Should().Be(accessRequest.AccessRequestId);
        }

        [Fact]
        public void Get_AccessRequest_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var accessRequest = EntityHelper.CreateAccessRequest(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(accessRequest);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            var result = service.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PimsAccessRequest>(result);
            result.AccessRequestId.Should().Be(accessRequest.AccessRequestId);
        }

        [Fact]
        public void Get_AccessRequest_ById_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var accessRequest = EntityHelper.CreateAccessRequest(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(accessRequest);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.GetById(1);

            // Assert
            actionFn.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Get_AccessRequest_ById_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.CreatePimsContext(user, true);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.GetById(1);

            // Assert
            actionFn.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Add
        [Fact]
        public void Add_AccessRequest_Null()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.CreatePimsContext(user, true);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.Add(null);

            // Assert
            actionFn.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_AccessRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var eUser = EntityHelper.CreateUser(1, new Guid(user.FindFirstValue("idir_user_guid")), "test user");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser, region: new PimsRegion() { Id = 2 });
            helper.CreatePimsContext(user, true).AddAndSaveChanges(eUser);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            var result = service.Add(accessRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PimsAccessRequest>(result);
            result.User.Position.Should().Be(eUser.Position);
            result.User.UserId.Should().Be(eUser.UserId);
            result.AccessRequestStatusTypeCode.Should().Be("Received");
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_AccessRequest_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var eUser = EntityHelper.CreateUser("test user 2");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser, region: new PimsRegion() { Id = 2 });
            helper.CreatePimsContext(user, true).AddAndSaveChanges(accessRequest);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.Delete(accessRequest);

            // Assert
            actionFn.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Delete_AccessRequest_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var eUser = EntityHelper.CreateUser("test user 2");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser, region: new PimsRegion() { Id = 2 });
            helper.CreatePimsContext(user, true).AddAndSaveChanges(eUser);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.Delete(accessRequest);

            // Assert
            actionFn.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Delete_AccessRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var eUser = EntityHelper.CreateUser(1, new Guid(user.FindFirstValue("idir_user_guid")), "test user");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser, region: new PimsRegion() { Id = 2 });
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(accessRequest);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            var result = service.Delete(accessRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PimsAccessRequest>(result);
            context.PimsAccessRequests.Should().BeEmpty();
        }

        #endregion

        #region Update
        [Fact]
        public void Update_AccessRequest_Null()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.CreatePimsContext(user, true);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.Update(null);

            // Assert
            actionFn.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_AccessRequest_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var eUser = EntityHelper.CreateUser("test user 2");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser, region: new PimsRegion() { Id = 2 });
            helper.CreatePimsContext(user, true).AddAndSaveChanges(eUser);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.Update(accessRequest);

            // Assert
            actionFn.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_AccessRequest_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var eUser = EntityHelper.CreateUser("test user 2");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser, region: new PimsRegion() { Id = 2 });
            helper.CreatePimsContext(user, true).AddAndSaveChanges(eUser);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            Action actionFn = () => service.Update(accessRequest);

            // Assert
            actionFn.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Update_AccessRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var eUser = EntityHelper.CreateUser(1, new Guid(user.FindFirstValue("idir_user_guid")), "test user");
            var accessRequest = EntityHelper.CreateAccessRequest(1, user: eUser, region: new PimsRegion() { Id = 2 });
            var region = new PimsRegion() { Id = 3 };
            var status = new PimsAccessRequestStatusType() { Id = "updated" };
            var role = EntityHelper.CreateRole(2, "updated role");
            var context = helper.CreatePimsContext(user, true);
            context.Add(region);
            context.Add(status);
            context.Add(role);
            context.AddAndSaveChanges(accessRequest);

            var service = helper.CreateRepository<AccessRequestRepository>(user);

            // Act
            var accessRequestUpdated = EntityHelper.CreateAccessRequest(1, user: eUser, region: region, role: role);
            accessRequestUpdated.User.Position = "updated";
            accessRequestUpdated.Note = "updated";
            accessRequestUpdated.AccessRequestStatusTypeCode = status.AccessRequestStatusTypeCode;
            var result = service.Update(accessRequestUpdated);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<PimsAccessRequest>(result);
            result.User.Position.Should().Be(accessRequestUpdated.User.Position);
            result.AccessRequestId.Should().Be(accessRequestUpdated.AccessRequestId);
            result.AccessRequestStatusTypeCode.Should().Be(status.AccessRequestStatusTypeCode);
            result.Note.Should().Be(accessRequestUpdated.Note);
            result.RegionCode.Should().Be(accessRequestUpdated.RegionCode);
            result.RoleId.Should().Be(accessRequestUpdated.RoleId);
        }
        #endregion
    }
}
