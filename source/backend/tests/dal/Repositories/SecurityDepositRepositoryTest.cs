using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "securitydepositrepository")]
    [ExcludeFromCodeCoverage]
    public class SecurityDepositRepositoryTest
    {
        public SecurityDepositRepositoryTest() { }

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseAdd);

            var deposit = new PimsSecurityDeposit() { Description = "Blah", SecurityDepositTypeCode = "PET" };

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<SecurityDepositRepository>(user);

            // Act
            var result = repository.Add(deposit);
            context.CommitTransaction();

            // Assert
            context.PimsSecurityDeposits.Should().HaveCount(1);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsSecurityDeposits = new List<PimsSecurityDeposit>() { new() { SecurityDepositId = 1, Description = "Blah", SecurityDepositTypeCode = "PET" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var repository = helper.CreateRepository<SecurityDepositRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsSecurityDeposit>();
        }

        [Fact]
        public void GetById_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsSecurityDeposits = new List<PimsSecurityDeposit>() { new() { SecurityDepositId = 1, Description = "Blah", SecurityDepositTypeCode = "PET" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var repository = helper.CreateRepository<SecurityDepositRepository>(user);

            // Act
            Action act = () => repository.GetById(2);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetAllByLeaseId
        [Fact]
        public void GetAllByLeaseId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsSecurityDeposits = new List<PimsSecurityDeposit>()
            {
                new() { LeaseId = 1, Description = "Blah", SecurityDepositTypeCode = "PET", SecurityDepositTypeCodeNavigation = new () { SecurityDepositTypeCode = "PET", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" } },
                new() { LeaseId = 1, Description = "Meh", SecurityDepositTypeCode = "PET" }
            };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var repository = helper.CreateRepository<SecurityDepositRepository>(user);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.GetAllByLeaseId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsSecurityDeposits = new List<PimsSecurityDeposit>() { new() { SecurityDepositId = 1, Description = "Blah", SecurityDepositTypeCode = "PET" } };
            var deposit = lease.PimsSecurityDeposits.FirstOrDefault();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var repository = helper.CreateRepository<SecurityDepositRepository>(user);

            // Act
            deposit.Description = "updated";
            var result = repository.Update(deposit);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsSecurityDeposit>();
            result.Description.Should().Be("updated");
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseDelete);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsSecurityDeposits = new List<PimsSecurityDeposit>()
            {
                new() { SecurityDepositId = 1, Description = "Blah", SecurityDepositTypeCode = "PET", PimsSecurityDepositHolder = new() { PersonId = 1 } }
            };
            var deposit = lease.PimsSecurityDeposits.FirstOrDefault();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var repository = helper.CreateRepository<SecurityDepositRepository>(user);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.Delete(deposit.SecurityDepositId);
            context.CommitTransaction();

            // Assert
            context.PimsSecurityDeposits.Should().HaveCount(0);
            context.PimsSecurityDepositHolders.Should().HaveCount(0);
        }

        [Fact]
        public void Delete_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseDelete);

            var lease = EntityHelper.CreateLease(1);
            lease.PimsSecurityDeposits = new List<PimsSecurityDeposit>() { new() { SecurityDepositId = 1, Description = "Blah", SecurityDepositTypeCode = "PET" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var repository = helper.CreateRepository<SecurityDepositRepository>(user);

            // Act
            context.ChangeTracker.Clear();
            Action act = () => repository.Delete(2);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion
    }
}
