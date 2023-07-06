using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> LeaseFilterData =>
            new List<object[]>
            {
                new object[] { new LeaseFilter() { TenantName = "tenant" }, 1 },
                new object[] { new LeaseFilter() { TenantName = "fake" }, 0 },
                new object[] { new LeaseFilter() { LFileNo = "123" }, 1 },
                new object[] { new LeaseFilter() { LFileNo = "fake" }, 0 },
                new object[] { new LeaseFilter() { PinOrPid = "456" }, 1 },
                new object[] { new LeaseFilter() { PinOrPid = "789" }, 0 },
                new object[] { new LeaseFilter() { ExpiryAfterDate = new DateTime(2001,1,1) }, 0 },
                new object[] { new LeaseFilter() { ExpiryAfterDate = new DateTime(2000,1,1) }, 1 },
                new object[] { new LeaseFilter() { StartBeforeDate = new DateTime(1999, 1,1) }, 0 },
                new object[] { new LeaseFilter() { StartBeforeDate = new DateTime(2000,1,1) }, 1 },
                new object[] { new LeaseFilter() { NotInStatus = new List<string>() { "testStatusType" } }, 0 },
                new object[] { new LeaseFilter() { NotInStatus = new List<string>() { "someOtherValue" } }, 1 },
                new object[] { new LeaseFilter() { IsReceivable = true }, 0 },
                    new object[] { new LeaseFilter() { IsReceivable = false }, 1 },
                new object[] { new LeaseFilter() { PinOrPid = "789" }, 0 },
                new object[] { new LeaseFilter(), 1 },
            };
        #endregion

        #region Tests
        [Fact]
        public void Lease_Count()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(elease);

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.Count();

            // Assert
            Assert.Equal(1, result);
        }

        #region Get
        [Theory]
        [MemberData(nameof(LeaseFilterData))]
        public void Get_Leases_Paged(LeaseFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", tenantLastName: "tenant", addTenant: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetAllByFilter(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsLease[]>(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void Get_Leases_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetAllByFilter(null, new HashSet<short>()));
        }

        [Theory]
        [MemberData(nameof(LeaseFilterData))]
        public void Get_Leases_Filter(LeaseFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", tenantLastName: "tenant", addTenant: true);
            elease.LeaseId = 1;
            elease.OrigExpiryDate = new DateTime(2000, 1, 1);
            elease.OrigStartDate = new DateTime(2000, 1, 1);

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(elease);

            var service = helper.CreateRepository<LeaseRepository>(user);

            // Act
            var result = service.GetPage(filter, new HashSet<short>());

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsLease>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
        }
        #endregion

        #region Update Tenant
        [Fact]
        public void Update_Lease_Tenants_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseTenantRepository>(user);

            // Act
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            var addTenantPerson = new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1" }, TenantTypeCodeNavigation = new PimsTenantType() { Id = "TEN1" } };
            var addTenantOrganization = new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER2" }, TenantTypeCodeNavigation = new PimsTenantType() { Id = "TEN2" } };
            lease.PimsLeaseTenants.Add(addTenantPerson);
            lease.PimsLeaseTenants.Add(addTenantOrganization);
            repository.Update(1, lease.PimsLeaseTenants);
            repository.SaveChanges();
            var updatedTenants = repository.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedTenants.Should().HaveCount(2);
            updatedTenants.FirstOrDefault().Internal_Id.Should().Be(addTenantOrganization.Internal_Id);
        }

        [Fact]
        public void Update_Lease_Tenants_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var updatePerson = EntityHelper.CreatePerson(2, "tester", "two");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1" }, TenantTypeCodeNavigation = new PimsTenantType() { Id = "TEN1" } });
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER2" }, TenantTypeCodeNavigation = new PimsTenantType() { Id = "TEN2" } });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseTenantRepository>(user);

            // Act
            var updateTenant = lease.PimsLeaseTenants.FirstOrDefault();
            updateTenant.PersonId = updatePerson.PersonId;
            repository.Update(1, lease.PimsLeaseTenants);
            repository.SaveChanges();
            var updatedTenants = repository.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedTenants.Should().HaveCount(2);
            updatedTenants.FirstOrDefault().Internal_Id.Should().Be(updateTenant.Internal_Id);
        }
        [Fact]
        public void Update_Lease_Tenants_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1" }, TenantTypeCodeNavigation = new PimsTenantType() { Id = "TEN1" } });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var service = helper.CreateRepository<LeaseTenantRepository>(user);

            // Act
            var deleteTenant = lease.PimsLeaseTenants.FirstOrDefault();
            lease.PimsLeaseTenants.Remove(deleteTenant);
            context.ChangeTracker.Clear();
            service.Update(1, lease.PimsLeaseTenants);
            service.SaveChanges();
            var updatedLeaseTenants = service.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedLeaseTenants.Should().BeEmpty();
        }
        [Fact]
        public void Update_Lease_Tenants_AddRemove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var addPerson = EntityHelper.CreatePerson(2, "tester", "two");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER1" }, TenantTypeCodeNavigation = new PimsTenantType() { Id = "TEN1" } });
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId, LessorTypeCodeNavigation = new PimsLessorType() { Id = "PER2" }, TenantTypeCodeNavigation = new PimsTenantType() { Id = "TEN2" } });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<LeaseTenantRepository>(user);

            // Act
            var deleteTenant = lease.PimsLeaseTenants.FirstOrDefault();
            lease.PimsLeaseTenants.Remove(deleteTenant);
            var addTenant = new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = addPerson.PersonId };
            lease.PimsLeaseTenants.Add(addTenant);
            context.ChangeTracker.Clear();
            repository.Update(1, lease.PimsLeaseTenants);
            repository.SaveChanges();
            var updatedLeaseTenants = repository.GetByLeaseId(lease.LeaseId);

            // Assert
            updatedLeaseTenants.Should().HaveCount(1);
            updatedLeaseTenants.FirstOrDefault().Internal_Id.Should().NotBe(deleteTenant.Internal_Id);
        }
        #endregion

        #region Update Lease Properties
        [Fact]
        public void Update_Lease_Properties_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddRange(propertyOne, lease);
            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);
            helper.SaveChanges();

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            repository.UpdatePropertyLeases(1, new List<PimsPropertyLease>() { addProperty });
            repository.CommitTransaction();
            var updatedPropertyLeases = repository.GetAllByLeaseId(lease.LeaseId);

            // Assert
            updatedPropertyLeases.Should().HaveCount(1);
            updatedPropertyLeases.FirstOrDefault().Internal_Id.Should().Be(addProperty.Internal_Id);
        }

        [Fact]
        public void Update_Lease_Properties_AddPropertyInLease()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(propertyOne, lease);
            var service = helper.Create<LeaseService>(user);
            helper.SaveChanges();
            var leaseTwo = context.CreateLease(2, addProperty: false);
            propertyOne.PimsPropertyLeases = new List<PimsPropertyLease>() { new Dal.Entities.PimsPropertyLease() { LeaseId = leaseTwo.LeaseId, Lease = leaseTwo, PropertyId = propertyOne.PropertyId } };
            helper.SaveChanges();

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(propertyOne);

            var repository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            repository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(propertyOne.PimsPropertyLeases);

            var leaseRepository = helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, Lease = lease, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);

            // Assert
            Assert.Throws<UserOverrideException>(() =>
                service.Update(lease, Array.Empty<UserOverrideCode>()));
        }

        [Fact]
        public void Update_Lease_Properties_AddPropertyInLeaseOverride()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(propertyOne, lease);
            var service = helper.Create<LeaseService>();
            helper.SaveChanges();
            var leaseTwo = context.CreateLease(2, addProperty: false);
            propertyOne.PimsPropertyLeases = new List<PimsPropertyLease>() { new Dal.Entities.PimsPropertyLease() { LeaseId = leaseTwo.LeaseId, PropertyId = propertyOne.PropertyId, Lease = leaseTwo } };

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(propertyOne);

            var repository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            repository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(propertyOne.PimsPropertyLeases);

            var leaseRepository = helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Update(It.IsAny<PimsLease>(), false));

            var propertyLeaseRepository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeaseRepository.Setup(x => x.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<List<PimsPropertyLease>>()));

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty, UserOverrideCode.AddPropertyToInventory });

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
            propertyLeaseRepository.Verify(x => x.UpdatePropertyLeases(lease.LeaseId, lease.PimsPropertyLeases), Times.Once);
        }

        [Fact]
        public void Update_Lease_Properties_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            lease.PimsPropertyLeases.Add(new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne });
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease, propertyOne);
            var service = helper.Create<LeaseService>(user);
            helper.SaveChanges();

            var propertyRepository = helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(propertyOne);

            var repository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            repository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>());

            var leaseRepository = helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);
            leaseRepository.Setup(x => x.Update(It.IsAny<PimsLease>(), false));

            var propertyLeaseRepository = helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeaseRepository.Setup(x => x.UpdatePropertyLeases(It.IsAny<long>(), It.IsAny<List<PimsPropertyLease>>()));

            var userRepository = helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("test"));

            // Act
            var updateProperty = EntityHelper.CreateProperty(context, 2);
            helper.SaveChanges();
            var propertyToUpdate = lease.PimsPropertyLeases.FirstOrDefault();
            propertyToUpdate.PropertyId = updateProperty.PropertyId;
            propertyToUpdate.Property = updateProperty;
            context.ChangeTracker.Clear();
            var updatedLease = service.Update(lease, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
            propertyLeaseRepository.Verify(x => x.UpdatePropertyLeases(lease.LeaseId, lease.PimsPropertyLeases), Times.Once);
        }

        [Fact]
        public void Update_Lease_Properties_UpdateNonExistantProperty()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            lease.PimsPropertyLeases.Add(new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne });
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease, propertyOne);
            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);
            helper.SaveChanges();

            // Act
            var propertyToUpdate = lease.PimsPropertyLeases.FirstOrDefault();
            propertyToUpdate.Property = EntityHelper.CreateProperty(2);

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
                repository.UpdatePropertyLeases(1, lease.PimsPropertyLeases));
        }

        [Fact]
        public void Update_Lease_Properties_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            lease.PimsPropertyLeases.Add(new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);

            // Act
            var deleteProperty = lease.PimsPropertyLeases.FirstOrDefault();
            lease.PimsPropertyLeases.Remove(deleteProperty);
            context.ChangeTracker.Clear();
            var properties = repository.UpdatePropertyLeases(1, lease.PimsPropertyLeases);

            // Assert
            properties.Should().BeEmpty();
        }
        [Fact]
        public void Update_Lease_Properties_AddRemove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var property = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(property, lease);
            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);
            helper.SaveChanges();

            // Act
            var deleteProperty = lease.PimsPropertyLeases.FirstOrDefault();
            lease.PimsPropertyLeases.Remove(deleteProperty);

            var addProperty = EntityHelper.CreateProperty(context, 2);
            helper.SaveChanges();

            var addPropertyLease = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = addProperty.PropertyId, Property = addProperty };
            lease.PimsPropertyLeases.Add(addPropertyLease);
            repository.UpdatePropertyLeases(1, lease.PimsPropertyLeases);
            repository.CommitTransaction();
            var updatedProperties = repository.GetAllByLeaseId(lease.LeaseId);

            // Assert
            updatedProperties.Should().HaveCount(1);
            updatedProperties.Should().NotContain(deleteProperty);
            updatedProperties.FirstOrDefault().Internal_Id.Should().Be(addPropertyLease.Internal_Id);
        }

        [Fact]
        public void Update_Lease_Properties_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var lease = EntityHelper.CreateLease(1);

            var repository = helper.CreateRepository<PropertyLeaseRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                repository.UpdatePropertyLeases(1, new List<Pims.Dal.Entities.PimsPropertyLease>()));
        }
        #endregion

        #region Update Lease Property Improvements
        [Fact]
        public void Update_Lease_Improvements_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddRange(lease);
            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);
            helper.SaveChanges();

            // Act
            var addImprovement = new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId };
            lease.PimsPropertyImprovements.Add(addImprovement);
            var improvements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            improvements.Should().HaveCount(1);
            improvements.Should().Contain(addImprovement);
        }

        [Fact]
        public void Update_Lease_Improvements_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements.Add(new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId, PropertyImprovementId = 1 });
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease);
            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);
            helper.SaveChanges();

            // Act
            var ImprovementToUpdate = lease.PimsPropertyImprovements.FirstOrDefault();
            ImprovementToUpdate.Address = "test update";
            var updatedImprovements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            updatedImprovements.Should().HaveCount(1);
            updatedImprovements.Should().Contain(ImprovementToUpdate);
        }

        [Fact]
        public void Update_Lease_Improvements_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements.Add(new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId, PropertyImprovementId = 1 });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);

            // Act
            var deleteImprovement = lease.PimsPropertyImprovements.FirstOrDefault();
            lease.PimsPropertyImprovements.Remove(deleteImprovement);
            context.ChangeTracker.Clear();
            var updatedImprovements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            updatedImprovements.Should().BeEmpty();
        }
        [Fact]
        public void Update_Lease_Improvements_AddRemove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease);
            var repository = helper.CreateRepository<PropertyImprovementRepository>(user);
            helper.SaveChanges();

            // Act
            var deleteProperty = lease.PimsPropertyImprovements.FirstOrDefault();
            lease.PimsPropertyImprovements.Remove(deleteProperty);

            var addPropertyImprovement = new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId };
            lease.PimsPropertyImprovements.Add(addPropertyImprovement);
            var updatedImprovements = repository.Update(1, lease.PimsPropertyImprovements);

            // Assert
            updatedImprovements.Should().HaveCount(1);
            updatedImprovements.Should().NotContain(deleteProperty);
            updatedImprovements.Should().Contain(addPropertyImprovement);
        }
        #endregion

        #endregion
    }
}
