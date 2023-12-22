using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Extensions;
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
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyRepositoryTest
    {
        private TestHelper _helper;

        #region Data
        public static IEnumerable<object[]> AllPropertyFilters =>
            new List<object[]>
            {
                new object[] { new PropertyFilter() { PinOrPid = "111-111-111" }, 1 },
                new object[] { new PropertyFilter() { PinOrPid = "111" }, 2 },
                new object[] { new PropertyFilter() { Address = "12342 Test Street" }, 5 },
                new object[] { new PropertyFilter() { Page = 1, Quantity = 10 }, 6 },
                new object[] { new PropertyFilter(), 6 },
            };
        #endregion

        public PropertyRepositoryTest()
        {
            _helper = new TestHelper();
        }

        private PropertyRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<PropertyRepository>();
        }

        #region Tests
        #region Get Paged Properties
        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Properties_ArgumentNullException()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);

            // Act
            Action act = () => repository.GetPage((PropertyFilter)null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// User does not have 'property-view' claim.
        /// </summary>
        [Fact]
        public void GetPage_Properties_NotAuthorized()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions();
            var filter = new PropertyFilter();

            // Act
            Action act = () => repository.GetPage(filter);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Theory]
        [MemberData(nameof(AllPropertyFilters))]
        public void GetPage_Properties(PropertyFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            using var init = helper.InitializeDatabase(user);

            init.CreateProperty(2);
            init.CreateProperty(3, pin: 111);
            init.CreateProperty(4, address: init.PimsAddresses.FirstOrDefault());
            init.Add(new Entity.PimsProperty() { Location = new NetTopologySuite.Geometries.Point(-123.720810, 48.529338) });
            init.CreateProperty(5, classification: init.PimsPropertyClassificationTypes.FirstOrDefault(c => c.PropertyClassificationTypeCode == "Core Operational"));
            init.CreateProperty(111111111);

            init.SaveChanges();

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = repository.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.PimsProperty>>(result);
            Assert.Equal(expectedCount, result.Total);
        }
        #endregion

        #region Get
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.Internal_Id = 1;
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Pid.Should().Be(100);
        }
        #endregion

        #region GetMatchingIds
        [Fact]
        public void GetMatchingIds_LeaseRcbvl_All_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.OrigExpiryDate = DateTime.Now.AddDays(1);
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePayRcvblType = "all" });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_LeaseRcbvl_NoTerms_ExpiryDate_Null()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.OrigExpiryDate = null;
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePayRcvblType = "all" });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_LeaseRcbvl_NoTerms_ExpiryDate_Past()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.OrigExpiryDate = DateTime.Now.AddDays(-10);
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePayRcvblType = "all" });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetMatchingIds_LeaseRcbvl_All_Success_HasTerms_Term_ExpiryDate_Null()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.OrigExpiryDate = DateTime.Now.AddDays(10);
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>()
            {
                new PimsLeaseTerm()
                {
                    TermExpiryDate= null,
                }
            };
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePayRcvblType = "all" });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_LeaseRcbvl_All_Success_HasTerms_Term_ExpiryDate_Future()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, addProperty: false);
            lease.OrigExpiryDate = DateTime.Now.AddDays(10);
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>()
            {
                new PimsLeaseTerm()
                {
                    TermExpiryDate= DateTime.Now.AddDays(10),
                }
            };
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePayRcvblType = "all" });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }


        [Fact]
        public void GetMatchingIds_LeaseStatus_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, pimsLeaseStatusType: new PimsLeaseStatusType() { Id = "test2" }, addProperty: false);
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeaseStatus = "test2" });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_LeaseType_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, pimsLeaseLicenseType: new PimsLeaseLicenseType() { Id = "test" }, addProperty: false);
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeaseTypes = new List<string>() { "test" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_LeasePurpose_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            var lease = EntityHelper.CreateLease(1, pimsLeasePurposeType: new PimsLeasePurposeType() { Id = "test" }, addProperty: false);
            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePurposes = new List<string>() { "test" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_Anomaly_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.PimsPropPropAnomalyTypes.Add(new PimsPropPropAnomalyType() { PropertyAnomalyTypeCode = "test" });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { AnomalyIds = new List<string>() { "test" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_Project_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.PimsPropertyAcquisitionFiles.Add(new PimsPropertyAcquisitionFile() { AcquisitionFile = new PimsAcquisitionFile() { ProjectId = 1 } });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { ProjectId = 1 });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_Tenure_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.PimsPropPropTenureTypes.Add(new PimsPropPropTenureType() { PropertyTenureTypeCode = "test" });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { TenureStatuses = new List<string>() { "test" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_TenureRoad_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.PimsPropPropRoadTypes.Add(new PimsPropPropRoadType() { PropertyRoadTypeCode = "test" });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { TenureRoadTypes = new List<string>() { "test" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_TenurePph_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.PphStatusTypeCode = "test";
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { TenurePPH = "test" });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }
        #endregion

        #region GetByPid
        [Fact]
        public void GetByPid_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var pid = 1111;
            var property = EntityHelper.CreateProperty(pid);
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetByPid(pid.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Pid.Should().Be(pid);
        }
        #endregion

        #region GetByPin
        [Fact]
        public void GetByPin_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var pin = 1111;
            var property = EntityHelper.CreateProperty(1, pin);
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetByPin(pin);

            // Assert
            result.Should().NotBeNull();
            result.Pin.Should().Be(pin);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Property_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            _helper.AddAndSaveChanges(property);

            var newValues = new Entity.PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;

            // Act
            var updatedProperty = repository.Update(newValues);

            // Assert
            updatedProperty.Description.Should().Be("test");
            updatedProperty.Pid.Should().Be(200);
        }

        [Fact]
        public void Update_Property_KeyNotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            // Act
            Action act = () => repository.Update(property);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Update_Property_ThrowIfNull()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            _helper.AddAndSaveChanges(property);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Property Management
        [Fact]
        public void Update_PropertyManagement_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            _helper.AddAndSaveChanges(property);

            var newValues = new Entity.PimsProperty();
            property.CopyValues(newValues);
            newValues.AdditionalDetails = "test";
            newValues.IsTaxesPayable = true;
            newValues.IsUtilitiesPayable = false;

            // Act
            var updatedProperty = repository.UpdatePropertyManagement(newValues);

            // Assert
            updatedProperty.AdditionalDetails.Should().Be("test");
            updatedProperty.IsTaxesPayable.Should().Be(true);
            updatedProperty.IsUtilitiesPayable.Should().Be(false);
        }

        [Fact]
        public void Update_PropertyManagement_ShouldUpdateManagementFieldsOnly()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            _helper.AddAndSaveChanges(property);

            var newValues = new Entity.PimsProperty();
            property.CopyValues(newValues);
            newValues.AdditionalDetails = "test";
            newValues.IsTaxesPayable = true;
            newValues.IsUtilitiesPayable = false;
            // non-management field
            newValues.Pid = 200;

            // Act
            var updatedProperty = repository.UpdatePropertyManagement(newValues);

            // Assert
            updatedProperty.AdditionalDetails.Should().Be("test");
            updatedProperty.IsTaxesPayable.Should().Be(true);
            updatedProperty.IsUtilitiesPayable.Should().Be(false);
            updatedProperty.Pid.Should().Be(1); // change ignored for non-management fields
        }

        [Fact]
        public void Update_PropertyManagement_KeyNotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            // Act
            Action act = () => repository.UpdatePropertyManagement(property);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Update_PropertyManagement_ThrowIfNull()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);

            // Act
            Action act = () => repository.UpdatePropertyManagement(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #endregion
    }
}
