using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class ManagementActivityRepositoryTest
    {
        private TestHelper _helper;

        public ManagementActivityRepositoryTest()
        {
            _helper = new TestHelper();
        }

        private ManagementActivityRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<ManagementActivityRepository>(user);
        }

        #region Tests

        #region Create
        [Fact]
        public void Create_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementAdd);
            var activity = EntityHelper.CreateManagementActivity(1);

            // Act
            var result = repository.Create(activity);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsManagementActivity>();
            result.Internal_Id.Should().Be(1);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementEdit, Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            _helper.AddAndSaveChanges(activity);

            // mutate
            activity.MgmtActivityStatusTypeCode = "INPROGRESS";

            // Act
            var result = repository.Update(activity);

            // Assert
            result.Should().NotBeNull();
            result.MgmtActivityStatusTypeCode.Should().Be("INPROGRESS");
        }

        [Fact]
        public void Update_KeyNotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementEdit);
            var activity = EntityHelper.CreateManagementActivity(1);

            // Act
            Action act = () => repository.Update(activity);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetActivity / GetById
        [Fact]
        public void GetActivity_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.GetActivity(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsManagementActivity>();
            result.Internal_Id.Should().Be(1);
            result.MgmtActivityTypeCode.Should().Be("PROPERTYMTC");
        }

        [Fact]
        public void GetManagementActivityById_Throw_KeyNotFoundException()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);

            // Act
            Action act = () => repository.GetActivity(9999);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetActivitiesByProperty / ManagementFile / PropertyIds
        [Fact]
        public void GetActivitiesByProperty_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            activity.PimsManagementActivityProperties = new List<PimsManagementActivityProperty>()
            {
                new PimsManagementActivityProperty()
                {
                    PropertyId = 1,
                    Property = EntityHelper.CreateProperty(1, 2)
                }
            };
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.GetActivitiesByProperty(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsManagementActivity>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetActivitiesByManagementFile_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            // set ManagementFileId so query can match on the FK
            activity.ManagementFileId = 1;
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.GetActivitiesByManagementFile(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsManagementActivity>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetActivitiesByPropertyIds_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            activity.PimsManagementActivityProperties = new List<PimsManagementActivityProperty>()
            {
                new PimsManagementActivityProperty()
                {
                    PropertyId = 1,
                    Property = EntityHelper.CreateProperty(1, 2)
                }
            };
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.GetActivitiesByPropertyIds(new long[] { 1 });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsManagementActivity>>();
            result.Should().HaveCount(1);
        }
        #endregion

        #region Count
        [Fact]
        public void Count_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.Count();

            // Assert
            result.Should().Be(1);
        }
        #endregion

        #region TryDelete
        [Fact]
        public void TryDelete_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementDelete);
            var activity = EntityHelper.CreateManagementActivity(1);
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.TryDelete(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TryDelete_NotFound_ReturnsTrue()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementDelete);

            // Act
            var result = repository.TryDelete(9999);

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region GetPageDeep
        [Fact]
        public void GetPageDeep_NoFilter_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            activity.ManagementFile = EntityHelper.CreateManagementFile(1);

            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.GetPageDeep(new ManagementActivityFilter() { Page = 1, Quantity = 10 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Filter_FileName_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            activity.ManagementFile = EntityHelper.CreateManagementFile(1, name: "TestFile");
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.GetPageDeep(new ManagementActivityFilter()
            {
                FileNameOrNumberOrReference = "TestFile",
                Page = 1,
                Quantity = 10
            });

            // Assert
            result.Should().HaveCount(1);
        }
        #endregion

        #region SearchManagementActivities
        [Fact]
        public void SearchManagementActivities_FilterByAddress()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var property = EntityHelper.CreateProperty(1, 2);
            property.Address.StreetAddress1 = "123 Street";
            var activity = EntityHelper.CreateManagementActivity(1);
            activity.PimsManagementActivityProperties = new List<PimsManagementActivityProperty>()
            {
                new() { Property = property, PropertyId = property.Internal_Id }
            };

            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.SearchManagementActivities(new ManagementActivityFilter() { Address = "123" });

            // Assert
            result.Should().HaveCount(1);
        }
        #endregion

        #region SearchManagementActivityInvoices
        [Fact]
        public void SearchManagementActivityInvoices_FilterByFileName()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var activity = EntityHelper.CreateManagementActivity(1);
            activity.ManagementFile = EntityHelper.CreateManagementFile(1, name: "InvoiceFile");
            var invoice = new PimsManagementActivityInvoice()
            {
                ManagementActivity = activity,
                ManagementActivityId = 1,
                PretaxAmt = 450,
                GstAmt = 50,
                PstAmt = 0,
                TotalAmt = 500
            };
            activity.PimsManagementActivityInvoices = new List<PimsManagementActivityInvoice>() { invoice };
            _helper.AddAndSaveChanges(activity);

            // Act
            var result = repository.SearchManagementActivityInvoices(new ManagementActivityFilter()
            {
                FileNameOrNumberOrReference = "InvoiceFile"
            });

            // Assert
            result.Should().HaveCount(1);
            result.First().TotalAmt.Should().Be(500);
        }
        #endregion

        #region Update Child Collections
        [Fact]
        public void Update_AddChildEntities_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementEdit);
            var activity = EntityHelper.CreateManagementActivity(1);
            _helper.AddAndSaveChanges(activity);

            // add a child
            activity.PimsMgmtActMinContacts = new List<PimsMgmtActMinContact>()
            {
                new() { PersonId = 100 }
            };

            // Act
            var result = repository.Update(activity);

            // Assert
            result.PimsMgmtActMinContacts.Should().HaveCount(1);
        }

        [Fact]
        public void Update_RemoveChildEntities_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementEdit);
            var activity = EntityHelper.CreateManagementActivity(1);
            activity.PimsMgmtActMinContacts = new List<PimsMgmtActMinContact>()
            {
                new() { PersonId = 200 }
            };

            var context = _helper.AddAndSaveChanges(activity);

            // remove child
            activity.PimsMgmtActMinContacts.Clear();

            // Act
            context.ChangeTracker.Clear();
            var result = repository.Update(activity);

            // Assert
            result.PimsMgmtActMinContacts.Should().HaveCount(0);
        }
        #endregion

        #endregion
    }
}
