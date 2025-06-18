using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "expropriationPaymentRepository")]
    [ExcludeFromCodeCoverage]
    public class ExpropriationPaymentRepositoryTest
    {
        #region Constructors
        public ExpropriationPaymentRepositoryTest() { }
        #endregion

        #region Tests

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsExpropriationPayments = new List<PimsExpropriationPayment>() { new PimsExpropriationPayment() };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsExpropriationPayment>();
        }

        [Fact]
        public void GetById_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsExpropriationPayments = new List<PimsExpropriationPayment>() { new PimsExpropriationPayment() };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            Action act = ()=> repository.GetById(2);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetAllByAcquisitionFileId
        [Fact]
        public void GetAllByAcquisitionFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsExpropriationPayments = new List<PimsExpropriationPayment>() { new PimsExpropriationPayment() };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            var result = repository.GetAllByAcquisitionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsExpropriationPayments = new List<PimsExpropriationPayment>() { new PimsExpropriationPayment() };
            var payment = acqFile.PimsExpropriationPayments.FirstOrDefault();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            payment.Description = "updated";
            var result = repository.Update(payment);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsExpropriationPayment>();
            result.Description.Should().Be("updated");
        }

        [Fact]
        public void Update_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            Action action = () => repository.Update(new PimsExpropriationPayment());

            // Assert
            action.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region TryDelete
        [Fact]
        public void TryDelete_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsExpropriationPayments = new List<PimsExpropriationPayment>() { new PimsExpropriationPayment() { PimsExpropPmtPmtItems = new List<PimsExpropPmtPmtItem>() { new PimsExpropPmtPmtItem() { PaymentItemTypeCode = "RECEIPT"} } } };
            var payment = acqFile.PimsExpropriationPayments.FirstOrDefault();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.TryDelete(payment.Internal_Id);

            // Assert
            context.PimsExpropriationPayments.Should().HaveCount(1);
        }

        [Fact]
        public void TryDelete_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.TryDelete(2);

            // Assert
            result.Should().BeFalse();
        }

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var expropPayment = new PimsExpropriationPayment();

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ExpropriationPaymentRepository>(user);

            // Act
            var result = repository.Add(expropPayment);
            context.CommitTransaction();

            // Assert
            context.PimsExpropriationPayments.Should().HaveCount(1);
        }
        #endregion
        #endregion

        #endregion
    }
}
