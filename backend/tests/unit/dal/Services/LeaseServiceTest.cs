using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using System;
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
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServiceTest
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

            var service = helper.CreateService<LeaseService>(user);

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

            helper.CreatePimsContext(user, true).AddAndSaveChanges(elease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var result = service.Get(filter);

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

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get(null));
        }

        [Theory]
        [MemberData(nameof(LeaseFilterData))]
        public void Get_Leases_Filter(LeaseFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", tenantLastName: "tenant", addTenant: true);

            helper.CreatePimsContext(user, true).AddAndSaveChanges(elease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var result = service.GetPage(filter);

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

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var organization = EntityHelper.CreateOrganization(1, "tester org");
            var addTenantPerson = new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId };
            var addTenantOrganization = new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId };
            lease.PimsLeaseTenants.Add(addTenantPerson);
            lease.PimsLeaseTenants.Add(addTenantOrganization);
            var updatedLease = service.UpdateLeaseTenants(1, 2, lease.PimsLeaseTenants);

            // Assert
            Assert.Equal(2, updatedLease.PimsLeaseTenants.Count);
            updatedLease.PimsLeaseTenants.Should().Contain(addTenantPerson);
            updatedLease.PimsLeaseTenants.Should().Contain(addTenantOrganization);
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
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId });
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var updateTenant = lease.PimsLeaseTenants.FirstOrDefault();
            updateTenant.PersonId = updatePerson.PersonId;
            var updatedLease = service.UpdateLeaseTenants(1, 2, lease.PimsLeaseTenants);

            // Assert
            Assert.Equal(2, updatedLease.PimsLeaseTenants.Count);
            updatedLease.PimsLeaseTenants.Should().Contain(updateTenant);
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
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var deleteTenant = lease.PimsLeaseTenants.FirstOrDefault();
            lease.PimsLeaseTenants.Remove(deleteTenant);
            context.ChangeTracker.Clear();
            var updatedLease = service.UpdateLeaseTenants(1, 2, lease.PimsLeaseTenants);

            // Assert
            updatedLease.PimsLeaseTenants.Should().BeEmpty();
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
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = person.PersonId });
            lease.PimsLeaseTenants.Add(new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, OrganizationId = organization.OrganizationId });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var deleteTenant = lease.PimsLeaseTenants.FirstOrDefault();
            lease.PimsLeaseTenants.Remove(deleteTenant);
            var addTenant = new Dal.Entities.PimsLeaseTenant() { LeaseId = lease.LeaseId, PersonId = addPerson.PersonId };
            lease.PimsLeaseTenants.Add(addTenant);
            context.ChangeTracker.Clear();
            var updatedLease = service.UpdateLeaseTenants(1, 2, lease.PimsLeaseTenants);

            // Assert
            Assert.Equal(2, updatedLease.PimsLeaseTenants.Count);
            updatedLease.PimsLeaseTenants.Should().NotContain(deleteTenant);
            updatedLease.PimsLeaseTenants.Should().Contain(addTenant);
        }

        [Fact]
        public void Update_Lease_Tenants_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var lease = EntityHelper.CreateLease(1);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.UpdateLeaseTenants(1, 1, new List<Pims.Dal.Entities.PimsLeaseTenant>()));
        }

        [Fact]
        public void Update_Lease_Tenants_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.UpdateLeaseTenants(1, 1, new List<Pims.Dal.Entities.PimsLeaseTenant>()));
        }

        [Fact]
        public void Update_Lease_Tenants_Concurrency()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            lease.ConcurrencyControlNumber = lease.ConcurrencyControlNumber - 1;

            // Assert
            Assert.Throws<DbUpdateConcurrencyException>(() =>
                service.UpdateLeaseTenants(lease.LeaseId, lease.ConcurrencyControlNumber, lease.PimsLeaseTenants));
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
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);
            var updatedLease = service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases);

            // Assert
            Assert.Equal(1, updatedLease.PimsPropertyLeases.Count);
            updatedLease.PimsPropertyLeases.Should().Contain(addProperty);
        }

        [Fact(Skip = "this is corrected in a separate PR")]
        public void Update_Lease_Properties_AddPropertyInLease()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            var propertyOne = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(propertyOne, lease);
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();
            var leaseTwo = context.CreateLease(2, addProperty: false);
            propertyOne.PimsPropertyLeases = new List<PimsPropertyLease>() { new Dal.Entities.PimsPropertyLease() { LeaseId = leaseTwo.LeaseId, PropertyId = propertyOne.PropertyId, Lease = leaseTwo } };

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);

            // Assert
            Assert.Throws<UserOverrideException>(() =>
                service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases));
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
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();
            var leaseTwo = context.CreateLease(2, addProperty: false);
            propertyOne.PimsPropertyLeases = new List<PimsPropertyLease>() { new Dal.Entities.PimsPropertyLease() { LeaseId = leaseTwo.LeaseId, PropertyId = propertyOne.PropertyId, Lease = leaseTwo } };

            // Act
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);
            var updatedLease = service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases, true);

            // Assert
            Assert.Equal(1, updatedLease.PimsPropertyLeases.Count);
            updatedLease.PimsPropertyLeases.Should().Contain(addProperty);
        }

        [Fact]
        public void Update_Lease_Properties_AddNonExistantProperty()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1, addProperty: false);
            helper.CreatePimsContext(user, true).AddRange(lease);
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var propertyOne = EntityHelper.CreateProperty(1);
            var addProperty = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = propertyOne.PropertyId, Property = propertyOne };
            lease.PimsPropertyLeases.Add(addProperty);

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
                service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases));
        }

        [Fact(Skip = "this is corrected in a separate PR")]
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
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var updateProperty = EntityHelper.CreateProperty(context, 2);
            var propertyToUpdate = lease.PimsPropertyLeases.FirstOrDefault();
            propertyToUpdate.PropertyId = updateProperty.PropertyId;
            var updatedLease = service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases);

            // Assert
            Assert.Equal(1, updatedLease.PimsPropertyLeases.Count);
            updatedLease.PimsPropertyLeases.Should().Contain(propertyToUpdate);
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
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var propertyToUpdate = lease.PimsPropertyLeases.FirstOrDefault();
            propertyToUpdate.Property = EntityHelper.CreateProperty(2);

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
                service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases));
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

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var deleteProperty = lease.PimsPropertyLeases.FirstOrDefault();
            lease.PimsPropertyLeases.Remove(deleteProperty);
            context.ChangeTracker.Clear();
            var updatedLease = service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases);

            // Assert
            updatedLease.PimsPropertyLeases.Should().BeEmpty();
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
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var deleteProperty = lease.PimsPropertyLeases.FirstOrDefault();
            lease.PimsPropertyLeases.Remove(deleteProperty);

            var addProperty = EntityHelper.CreateProperty(context, 2);
            helper.SaveChanges();

            var addPropertyLease = new Dal.Entities.PimsPropertyLease() { LeaseId = lease.LeaseId, PropertyId = addProperty.PropertyId, Property = addProperty };
            lease.PimsPropertyLeases.Add(addPropertyLease);
            var updatedLease = service.UpdatePropertyLeases(1, 2, lease.PimsPropertyLeases);

            // Assert
            Assert.Equal(1, updatedLease.PimsPropertyLeases.Count);
            updatedLease.PimsPropertyLeases.Should().NotContain(deleteProperty);
            updatedLease.PimsPropertyLeases.Should().Contain(addPropertyLease);
        }

        [Fact]
        public void Update_Lease_Properties_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var lease = EntityHelper.CreateLease(1);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.UpdatePropertyLeases(1, 1, new List<Pims.Dal.Entities.PimsPropertyLease>()));
        }

        [Fact]
        public void Update_Lease_Properties_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.UpdatePropertyLeases(1, 1, new List<Pims.Dal.Entities.PimsPropertyLease>()));
        }

        [Fact]
        public void Update_Lease_Properties_Concurrency()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            lease.ConcurrencyControlNumber = lease.ConcurrencyControlNumber - 1;

            // Assert
            Assert.Throws<DbUpdateConcurrencyException>(() =>
                service.UpdatePropertyLeases(lease.LeaseId, lease.ConcurrencyControlNumber, lease.PimsPropertyLeases));
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
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var addImprovement = new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId };
            lease.PimsPropertyImprovements.Add(addImprovement);
            var updatedLease = service.UpdateLeaseImprovements(1, 2, lease.PimsPropertyImprovements);

            // Assert
            Assert.Equal(1, updatedLease.PimsPropertyImprovements.Count);
            updatedLease.PimsPropertyImprovements.Should().Contain(addImprovement);
        }

        [Fact]
        public void Update_Lease_Improvements_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements.Add(new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId, Id = 1 });
            var context = helper.CreatePimsContext(user, true);
            context.AddRange(lease);
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var ImprovementToUpdate = lease.PimsPropertyImprovements.FirstOrDefault();
            ImprovementToUpdate.Address = "test update";
            var updatedLease = service.UpdateLeaseImprovements(1, 2, lease.PimsPropertyImprovements);

            // Assert
            Assert.Equal(1, updatedLease.PimsPropertyImprovements.Count);
            updatedLease.PimsPropertyImprovements.Should().Contain(ImprovementToUpdate);
        }

        [Fact]
        public void Update_Lease_Improvements_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsPropertyImprovements.Add(new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId, Id = 1 });
            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(lease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var deleteImprovement = lease.PimsPropertyImprovements.FirstOrDefault();
            lease.PimsPropertyImprovements.Remove(deleteImprovement);
            context.ChangeTracker.Clear();
            var updatedLease = service.UpdateLeaseImprovements(1, 2, lease.PimsPropertyImprovements);

            // Assert
            updatedLease.PimsPropertyImprovements.Should().BeEmpty();
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
            var service = helper.CreateService<LeaseService>(user);
            helper.SaveChanges();

            // Act
            var deleteProperty = lease.PimsPropertyImprovements.FirstOrDefault();
            lease.PimsPropertyImprovements.Remove(deleteProperty);

            var addPropertyImprovement = new Dal.Entities.PimsPropertyImprovement() { LeaseId = lease.LeaseId };
            lease.PimsPropertyImprovements.Add(addPropertyImprovement);
            var updatedLease = service.UpdateLeaseImprovements(1, 2, lease.PimsPropertyImprovements);

            // Assert
            Assert.Equal(1, updatedLease.PimsPropertyImprovements.Count);
            updatedLease.PimsPropertyImprovements.Should().NotContain(deleteProperty);
            updatedLease.PimsPropertyImprovements.Should().Contain(addPropertyImprovement);
        }

        [Fact]
        public void Update_Lease_Improvements_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var lease = EntityHelper.CreateLease(1);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.UpdateLeaseImprovements(1, 1, new List<Pims.Dal.Entities.PimsPropertyImprovement>()));
        }

        [Fact]
        public void Update_Lease_Improvements_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.UpdateLeaseImprovements(1, 1, new List<Pims.Dal.Entities.PimsPropertyImprovement>()));
        }

        [Fact]
        public void Update_Lease_Improvements_Concurrency()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            lease.ConcurrencyControlNumber = lease.ConcurrencyControlNumber - 1;

            // Assert
            Assert.Throws<DbUpdateConcurrencyException>(() =>
                service.UpdatePropertyLeases(lease.LeaseId, lease.ConcurrencyControlNumber, lease.PimsPropertyLeases));
        }
        #endregion

        #endregion
    }
}
