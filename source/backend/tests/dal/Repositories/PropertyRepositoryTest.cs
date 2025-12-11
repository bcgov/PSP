using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

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
                new object[] { new PropertyFilter() { Pid = "111-111-111", Ownership = new List<string>()}, 1 },
                new object[] { new PropertyFilter() { Pin = "99999", Ownership = new List<string>()}, 1 },
                new object[] { new PropertyFilter() { Address = "12342 Test Street"  , Ownership = new List<string>()}, 6 },
                new object[] { new PropertyFilter() { PlanNumber = "SP-89TTXY", Ownership = new List<string>()}, 1 },
                new object[] { new PropertyFilter() { Page = 1, Quantity = 10 , Ownership = new List<string>() }, 6 },
                new object[] { new PropertyFilter(), 6 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isCoreInventory" }}, 3 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isPropertyOfInterest" }}, 2 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isDisposed"}}, 1 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isRetired"}}, 2 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isOtherInterest"}}, 1 },
                new object[] { new PropertyFilter(){ Ownership = new List<string>(){"isCoreInventory", "isPropertyOfInterest"}}, 5 },
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

            PimsPropertyVw testProperty = null;

            testProperty = init.CreatePropertyView(2);
            testProperty.IsOwned = true;

            testProperty = init.CreatePropertyView(3, pin: 99999);
            testProperty.IsOwned = false;
            testProperty.HasActiveAcquisitionFile = true;

            testProperty = init.CreatePropertyView(4, address: init.PimsAddresses.FirstOrDefault());
            testProperty.IsOwned = false;
            testProperty.HasActiveAcquisitionFile = true;
            testProperty.IsOtherInterest = true;

            testProperty = init.CreatePropertyView(6, location: new NetTopologySuite.Geometries.Point(-123.720810, 48.529338));
            testProperty.IsOwned = true;

            testProperty = init.CreatePropertyView(111111111);
            testProperty.IsOwned = true;

            testProperty = init.CreatePropertyView(22222);
            testProperty.IsRetired = true;

            testProperty = init.CreatePropertyView(33333);
            testProperty.SurveyPlanNumber = "SP-89TTXY";
            testProperty.IsDisposed = true;

            testProperty = init.CreatePropertyView(44444);
            testProperty.IsRetired = true;
            testProperty.IsOwned = true;

            init.SaveChanges();

            var repository = helper.CreateRepository<PropertyRepository>(user);

            // Act
            var result = repository.GetPage(filter);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsPropertyVw>>();
            result.Total.Should().Be(expectedCount);
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
            property.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>()
            {
                new PimsPropertyAcquisitionFile()
                {
                    AcquisitionFile = new PimsAcquisitionFile()
                    {
                        AcquisitionTypeCode = "TYPE",
                        FileName = "ACQFILE",
                        RegionCode = 1,
                        FileNo = 1234,
                        FileNoSuffix = 1,
                        AcquisitionFileStatusTypeCodeNavigation = new PimsAcquisitionFileStatusType() { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" }
                    }
                }
            };
            property.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>()
            {
                new PimsPropertyResearchFile()
                {
                    ResearchFile = new PimsResearchFile()
                    {
                        Name = "Research",
                        RfileNumber = "1234",
                        ResearchFileStatusTypeCodeNavigation = new PimsResearchFileStatusType() { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" }
                    }
                }
            };
            property.PimsPropertyLeases = new List<PimsPropertyLease>()
            {
                new PimsPropertyLease()
                {
                    Lease = new PimsLease()
                    {
                        LeaseLicenseTypeCode = "TYPE",
                        LeasePayRvblTypeCode = "RCVBL",
                        LeaseProgramTypeCode = "PROGRAM",
                        LeaseStatusTypeCodeNavigation = new PimsLeaseStatusType () { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" }
                    }
                }
            };
            property.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>()
            {
                new PimsDispositionFileProperty()
                {
                    DispositionFile = new PimsDispositionFile()
                    {
                        DispositionStatusTypeCode = "DRAFT",
                        DispositionTypeCode = "TYPE",
                        DispositionFileStatusTypeCodeNavigation = new PimsDispositionFileStatusType() { Id = "DRAFT", Description = "Draft", DbCreateUserid = "test", DbLastUpdateUserid = "test" }
                    }
                }
            };

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
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>()
            {
                new PimsLeasePeriod()
                {
                    PeriodExpiryDate= null,
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
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>()
            {
                new PimsLeasePeriod()
                {
                    PeriodExpiryDate= DateTime.Now.AddDays(10),
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
        public void GetMatchingIds_LeasePurpose_ZeroResults()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
            var lease = EntityHelper.CreateLease(1, addProperty: false);

            lease.PimsLeaseLeasePurposes.Add(new PimsLeaseLeasePurpose()
            {
                LeaseLeasePurposeId = 100,
                LeaseId = lease.LeaseId,
                LeasePurposeTypeCode = "test",
            });

            property.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = property.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(property);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePurposes = new List<string>() { "something" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetMatchingIds_LeasePurpose_OneResult()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var propertyOne = EntityHelper.CreateProperty(100, isCoreInventory: true);
            var lease = EntityHelper.CreateLease(1, addProperty: false);

            lease.PimsLeaseLeasePurposes.Add(new PimsLeaseLeasePurpose()
            {
                LeaseLeasePurposeId = 100,
                LeaseId = lease.LeaseId,
                LeasePurposeTypeCode = "something",
            });
            propertyOne.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = propertyOne.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(propertyOne);

            var propertyTwo = EntityHelper.CreateProperty(101, isCoreInventory: true);
            var anotherLease = EntityHelper.CreateLease(2, addProperty: false, generateTypeIds: true);
            anotherLease.PimsLeaseLeasePurposes.Add(new PimsLeaseLeasePurpose()
            {
                LeaseLeasePurposeId = 2,
                LeaseId = anotherLease.LeaseId,
                LeasePurposeTypeCode = "another",
            });

            propertyTwo.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = propertyTwo.Internal_Id, LeaseId = anotherLease.Internal_Id, Lease = anotherLease });
            _helper.AddAndSaveChanges(propertyTwo);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePurposes = new List<string>() { "something" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetMatchingIds_LeasePurpose_TwoResults()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var propertyOne = EntityHelper.CreateProperty(100, isCoreInventory: true);
            var lease = EntityHelper.CreateLease(1, addProperty: false);

            lease.PimsLeaseLeasePurposes.Add(new PimsLeaseLeasePurpose()
            {
                LeaseLeasePurposeId = 100,
                LeaseId = lease.LeaseId,
                LeasePurposeTypeCode = "something",
            });
            propertyOne.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = propertyOne.Internal_Id, LeaseId = lease.Internal_Id, Lease = lease });
            _helper.AddAndSaveChanges(propertyOne);

            var propertyTwo = EntityHelper.CreateProperty(101, isCoreInventory: true);
            var anotherLease = EntityHelper.CreateLease(2, addProperty: false, generateTypeIds: true);
            anotherLease.PimsLeaseLeasePurposes.Add(new PimsLeaseLeasePurpose()
            {
                LeaseLeasePurposeId = 2,
                LeaseId = anotherLease.LeaseId,
                LeasePurposeTypeCode = "another",
            });

            propertyTwo.PimsPropertyLeases.Add(new PimsPropertyLease() { PropertyId = propertyTwo.Internal_Id, LeaseId = anotherLease.Internal_Id, Lease = anotherLease });
            _helper.AddAndSaveChanges(propertyTwo);

            // Act
            var result = repository.GetMatchingIds(new PropertyFilterCriteria() { LeasePurposes = new List<string>() { "something", "another" } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetMatchingIds_Anomaly_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(100, isCoreInventory: true);
            property.PimsPropPropAnomalyTyps.Add(new PimsPropPropAnomalyTyp() { PropertyAnomalyTypeCode = "test" });
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
            property.PimsPropPropTenureTyps.Add(new PimsPropPropTenureTyp() { PropertyTenureTypeCode = "test" });
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
            property.PimsPropPropRoadTyps.Add(new PimsPropPropRoadTyp() { PropertyRoadTypeCode = "test" });
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

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Pid = pid;

            // Act
            var updatedProperty = repository.Update(newValues);

            // Assert
            updatedProperty.Pid.Should().Be(pid);
        }

        [Fact]
        public void Update_Property_Retired_Violation()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1, isRetired: true);
            _helper.AddAndSaveChanges(property);

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
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

        [Fact]
        public void Update_Property_DoesNOT_UPDATE_PPH_Audit()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(1);
            property.PphStatusTypeCode = null;
            property.PphStatusUpdateTimestamp = null;
            property.PphStatusUpdateUserid = null;
            property.PphStatusUpdateUserGuid = null;

            _helper.AddAndSaveChanges(property);

            var updateProperty = new PimsProperty();
            updateProperty.PropertyId = property.PropertyId;
            updateProperty.PphStatusTypeCode = PropertyPPHStatusTypes.UNKNOWN.ToString();

            // Act
            var result = repository.Update(updateProperty);

            // Assert
            Assert.Null(result.PphStatusUpdateTimestamp);
            Assert.Null(result.PphStatusUpdateUserid);
            Assert.Null(result.PphStatusUpdateUserGuid);
        }

        [Fact]
        public void Update_Property_Update_PPH_Audit()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);

            var property = EntityHelper.CreateProperty(1);
            property.PphStatusTypeCode = PropertyPPHStatusTypes.UNKNOWN.ToString();
            property.PphStatusUpdateTimestamp = null;
            property.PphStatusUpdateUserid = null;
            property.PphStatusUpdateUserGuid = null;

            _helper.AddAndSaveChanges(property);

            var updateProperty = new PimsProperty();
            updateProperty.PropertyId = property.PropertyId;
            updateProperty.PphStatusTypeCode = PropertyPPHStatusTypes.COMBO.ToString();

            // Act
            var result = repository.Update(updateProperty);

            // Assert
            Assert.Equal(PropertyPPHStatusTypes.COMBO.ToString(), result.PphStatusTypeCode);
            Assert.NotNull(result.PphStatusUpdateTimestamp);
            Assert.NotNull(result.PphStatusUpdateUserid);
            Assert.NotNull(result.PphStatusUpdateUserGuid);
        }

        #endregion

        #region Delete
        /*[Fact]
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
        */
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

            var newValues = new PimsProperty();
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

            var newValues = new PimsProperty();
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
