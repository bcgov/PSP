using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using Pims.Core.Exceptions;
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
                new object[] { new PropertyFilter() { PinOrPid = "111-111-111" , Ownership = new List<string>()}, 1 },
                new object[] { new PropertyFilter() { PinOrPid = "111"  , Ownership = new List<string>()}, 2 },
                new object[] { new PropertyFilter() { Address = "12342 Test Street"  , Ownership = new List<string>()}, 8 },
                new object[] { new PropertyFilter() { PlanNumber = "SP-89TTXY", Ownership = new List<string>()}, 1 },
                new object[] { new PropertyFilter() { Page = 1, Quantity = 10 , Ownership = new List<string>() }, 8 },
                new object[] { new PropertyFilter(), 8 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isCoreInventory" }}, 4 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isPropertyOfInterest" }}, 2 },

                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isDisposed"}}, 1 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isRetired"}}, 2 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isOtherInterest"}}, 1 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isCoreInventory", "isPropertyOfInterest"}}, 6 },
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

        /*
        // TODO: Figure out how to add DB views to the context
        [Theory]
        [MemberData(nameof(AllPropertyFilters))]
        public void GetPage_Properties(PropertyFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            using var init = helper.InitializeDatabase(user);

            PimsPropertyLocationVw testProperty = null;

            testProperty = init.CreatePropertyView(2);
            testProperty.IsOwned = true;

            testProperty = init.CreatePropertyView(3, pin: 111);
            testProperty.IsOwned = false;

            testProperty = init.CreatePropertyView(4, address: init.PimsAddresses.FirstOrDefault());
            testProperty.IsOwned = false;

            testProperty = init.CreatePropertyView(5, classification: init.PimsPropertyClassificationTypes.FirstOrDefault(c => c.PropertyClassificationTypeCode == "Core Operational"));
            testProperty.IsOwned = false;

            testProperty = init.CreatePropertyView(6, location: new NetTopologySuite.Geometries.Point(-123.720810, 48.529338));
            testProperty.IsOwned = true;

            testProperty = init.CreatePropertyView(111111111);
            testProperty.IsOwned = true;

            testProperty = init.CreatePropertyView(22222);
            testProperty.IsRetired = true;

            testProperty = init.CreatePropertyView(33333);
            testProperty.SurveyPlanNumber = "SP-89TTXY";

            testProperty = init.CreatePropertyView(44444);
            testProperty.IsRetired = true;
            testProperty.IsOwned = true;

            init.SaveChanges();

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = repository.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.PimsProperty>>(result);
            Assert.Equal(expectedCount, result.Total);
        }
        */
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

        #region GetAllByIds
        [Fact]
        public void GetAllByIds_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.Internal_Id = 1;
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetAllByIds(new List<long>() { 1 });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetAllByIds_NotAuthorized()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyDelete);
            var property = EntityHelper.CreateProperty(100);
            property.Internal_Id = 1;
            _helper.AddAndSaveChanges(property);

            // Act
            Action act = () => repository.GetAllByIds(new List<long>() { 1 });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetAllAssociationsById
        [Fact]
        public void GetAllAssociationsById_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100);
            property.Internal_Id = 1;
            property.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { AcquisitionFile = new PimsAcquisitionFile() {
                AcquisitionTypeCode = "TYPE", FileName = "ACQFILE", FileNumber = "1234", AcquisitionFileStatusTypeCodeNavigation = new PimsAcquisitionFileStatusType() { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" } } } };
            property.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>() { new PimsPropertyResearchFile() { ResearchFile = new PimsResearchFile() {
                Name = "Research", RfileNumber = "1234", ResearchFileStatusTypeCodeNavigation = new PimsResearchFileStatusType() { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" } } } };
            property.PimsPropertyLeases = new List<PimsPropertyLease>() { new PimsPropertyLease() { Lease = new PimsLease() {
                LeaseLicenseTypeCode = "TYPE", LeasePayRvblTypeCode = "RCVBL", LeaseProgramTypeCode = "PROGRAM", LeasePurposeTypeCode = "PURPOSE",
                LeaseStatusTypeCodeNavigation = new PimsLeaseStatusType () { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" } } } };
            property.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>() { new PimsDispositionFileProperty() { DispositionFile = new PimsDispositionFile() {
                DispositionStatusTypeCode = "DRAFT", DispositionTypeCode = "TYPE", DispositionFileStatusTypeCodeNavigation = new PimsDispositionFileStatusType() { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" } } } };
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetAllAssociationsById(1);

            // Assert
            result.Should().NotBeNull();
            result.PimsPropertyAcquisitionFiles.Should().HaveCount(1);
            result.PimsPropertyResearchFiles.Should().HaveCount(1);
            result.PimsPropertyLeases.Should().HaveCount(1);
            result.PimsDispositionFileProperties.Should().HaveCount(1);
        }
        #endregion

        #region GetMatchingIds
        [Fact]
        public void GetMatchingIds_LeaseRcbvl_All_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
            var lease = EntityHelper.CreateLease(1, pimsLeaseStatusType: new PimsLeaseStatusType() { Id = "test2", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test" }, addProperty: false);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
            var lease = EntityHelper.CreateLease(1, pimsLeaseLicenseType: new PimsLeaseLicenseType() { Id = "test", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test" }, addProperty: false);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
            var lease = EntityHelper.CreateLease(1, pimsLeasePurposeType: new PimsLeasePurposeType() { Id = "test", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test" }, addProperty: false);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
            var file = EntityHelper.CreateAcquisitionFile(1);
            file.ProjectId = 1;
            property.PimsPropertyAcquisitionFiles.Add(new PimsPropertyAcquisitionFile() { AcquisitionFile = file });
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
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

        [Fact]
        public void GetByPid_Success_Retired()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var pid = 1111;
            var property = EntityHelper.CreateProperty(pid);
            property.IsRetired = true;
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetByPid(pid, true);

            // Assert
            result.Should().NotBeNull();
            result.Pid.Should().Be(pid);
        }

        [Fact]
        public void GetByPid_Filter_Retired()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var pid = 1111;
            var property = EntityHelper.CreateProperty(pid);
            property.IsRetired = true;
            _helper.AddAndSaveChanges(property);

            // Act
            Action result = () => repository.GetByPid(pid, false);
            result.Should().Throw<KeyNotFoundException>();
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

        [Fact]
        public void GetByPin_Success_Retired()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var pin = 1111;
            var property = EntityHelper.CreateProperty(1, pin);
            property.IsRetired = true;
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetByPin(pin, true);

            // Assert
            result.Should().NotBeNull();
            result.Pin.Should().Be(pin);
        }

        [Fact]
        public void GetByPin_Filter_Retired()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var pin = 1111;
            var property = EntityHelper.CreateProperty(1, pin);
            property.IsRetired = true;
            _helper.AddAndSaveChanges(property);

            // Act
            Action action = () => repository.GetByPin(pin, false);

            // Assert
            action.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Update
        [Theory]
        [InlineData("test", 200, null)]
        [InlineData("test", 200, false)]
        public void Update_Property_Success_Not_Retired(string propertyDescription, int pid, bool? isRetired)
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1, isRetired: isRetired);
            _helper.AddAndSaveChanges(property);

            var newValues = new Entity.PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = propertyDescription;
            newValues.Pid = pid;

            // Act
            var updatedProperty = repository.Update(newValues);

            // Assert
            updatedProperty.Description.Should().Be(propertyDescription);
            updatedProperty.Pid.Should().Be(pid);
        }

        [Fact]
        public void Update_Property_Retired_Violation()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1, isRetired: true);
            _helper.AddAndSaveChanges(property);

            var newValues = new Entity.PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;

            // Act
            Action act = () => repository.Update(newValues);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Retired records are referenced for historical purposes only and cannot be edited or deleted.");
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

        #region Delete
        [Fact]
        public void Delete_Property_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            var context = _helper.AddAndSaveChanges(property);

            // Act
            repository.Delete(property);

            // Assert
            var deletedProperty = context.PimsProperties.FirstOrDefault();
            context.ChangeTracker.Entries().Should().NotBeEmpty();
        }
        #endregion

        #region TransferFileProperties
        [Fact]
        public void TransferFileProperties_Success_Owned()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            var context = _helper.AddAndSaveChanges(property);


            // Act
            var transferredProperty = repository.TransferFileProperty(property, true);
            context.CommitTransaction();

            // Assert
            transferredProperty.IsOwned.Should().BeTrue();
            transferredProperty.PropertyClassificationTypeCode.Should().Be("COREOPER");
        }

        [Fact]
        public void TransferFileProperties_Success_NotOwned()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            var context = _helper.AddAndSaveChanges(property);


            // Act
            var transferredProperty = repository.TransferFileProperty(property, false);
            context.CommitTransaction();

            // Assert
            transferredProperty.IsOwned.Should().BeFalse();
            transferredProperty.PropertyClassificationTypeCode.Should().Be("OTHER");
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
