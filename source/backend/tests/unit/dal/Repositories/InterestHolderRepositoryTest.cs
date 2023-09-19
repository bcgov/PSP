using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class InterestHolderRepositoryTest
    {
        #region Constructors
        public InterestHolderRepositoryTest() { }
        #endregion

        #region Tests

        #region Get

        [Fact]
        public void Get_Empty()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);

            // Act
            var response = repository.GetInterestHoldersByAcquisitionFile(0);

            // Assert
            response.Should().HaveCount(0);
        }

        [Fact]
        public void Get_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);
            var interestHolder = new PimsInterestHolder()
            {
                AcquisitionFileId = 1,
                InterestHolderTypeCodeNavigation = new PimsInterestHolderType()
                {
                    InterestHolderTypeCode = "test",
                },
            };
            helper.AddAndSaveChanges(interestHolder);

            // Act
            var response = repository.GetInterestHoldersByAcquisitionFile(1);

            // Assert
            response.Should().HaveCount(1);
        }

        #endregion
        #region update

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);
            var interestHolder = new PimsInterestHolder() { AcquisitionFileId = 1, InterestHolderId = 1, PersonId = 1, PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() };
            interestHolder.AcquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            helper.AddAndSaveChanges(interestHolder);

            // Act
            var response = repository.UpdateAllForAcquisition(1, new List<PimsInterestHolder>() { new PimsInterestHolder() { AcquisitionFileId = 1, InterestHolderId = 1, PersonId = 2, PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() } });

            // Assert
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.Should().HaveCount(1);
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.FirstOrDefault().PersonId.Should().Be(2);
        }

        [Fact]
        public void Update_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);
            helper.AddAndSaveChanges(EntityHelper.CreateAcquisitionFile(1));

            // Act
            repository.UpdateAllForAcquisition(1, new List<PimsInterestHolder>() { new PimsInterestHolder() { AcquisitionFileId = 1, PersonId = 1, PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() } });

            // Assert
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.Should().HaveCount(1);
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.FirstOrDefault().PersonId.Should().Be(1);
        }

        [Fact]
        public void Update_AddMultiple()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);
            helper.AddAndSaveChanges(EntityHelper.CreateAcquisitionFile(1));

            // Act
            var response = repository.UpdateAllForAcquisition(1, new List<PimsInterestHolder>() { new PimsInterestHolder() { AcquisitionFileId = 1, PersonId = 1, PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() },
            new PimsInterestHolder() { AcquisitionFileId = 1, PersonId = 2, PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() },});

            // Assert
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.Should().HaveCount(2);
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.FirstOrDefault().PersonId.Should().Be(1);
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.Last().PersonId.Should().Be(2);
        }

        [Fact]
        public void Update_AddGrandchild()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);
            helper.AddAndSaveChanges(EntityHelper.CreateAcquisitionFile(1));

            // Act
            var response = repository.UpdateAllForAcquisition(1, new List<PimsInterestHolder>() { new PimsInterestHolder() { AcquisitionFileId = 1, PersonId = 1,
                PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() { new PimsInthldrPropInterest() { PropertyAcquisitionFileId = 1 } }, }, });

            // Assert
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.Should().HaveCount(1);
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.FirstOrDefault().PimsInthldrPropInterests.Should().HaveCount(1);
            context.PimsAcquisitionFiles.FirstOrDefault().PimsInterestHolders.FirstOrDefault().PimsInthldrPropInterests.FirstOrDefault().PropertyAcquisitionFileId.Should().Be(1);
        }

        [Fact]
        public void Update_UpdateGrandchild()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);
            var interestHolder = new PimsInterestHolder()
            {
                AcquisitionFileId = 1,
                PersonId = 1,
                InterestHolderId = 1,
                PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() {
                new PimsInthldrPropInterest() { PimsInthldrPropInterestId = 1, PropertyAcquisitionFileId = 1, PimsPropInthldrInterestTypes = new List<PimsPropInthldrInterestType>() {new PimsPropInthldrInterestType() {InterestHolderInterestTypeCode="TEST_PimsPropInthldrInterestType"}} }, },
            };
            interestHolder.AcquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            helper.AddAndSaveChanges(interestHolder);

            // Act
            var response = repository.UpdateAllForAcquisition(1, new List<PimsInterestHolder>() { new PimsInterestHolder() { AcquisitionFileId = 1, PersonId = 1, InterestHolderId = 1,
                PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() { new PimsInthldrPropInterest() {
                    PimsInthldrPropInterestId = 1, PropertyAcquisitionFileId = 2,
                    PimsPropInthldrInterestTypes = new List<PimsPropInthldrInterestType>() {
                        new PimsPropInthldrInterestType() { InterestHolderInterestTypeCode="TEST_PimsPropInthldrInterestType"}
                    }
                     } }, }, });

            // Assert
            context.PimsInterestHolders.Should().HaveCount(1);
            context.PimsInthldrPropInterests.Should().HaveCount(1);
            context.PimsInthldrPropInterests.FirstOrDefault().PropertyAcquisitionFileId.Should().Be(2);
        }

        // TODO: Fix this
        /*[Fact]
        public void Update_DeleteGrandchild()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<InterestHolderRepository>(user);
            var interestHolder = new PimsInterestHolder()
            {
                AcquisitionFileId = 1,
                PersonId = 1,
                InterestHolderId = 1,
                InterestHolderTypeCodeNavigation = new PimsInterestHolderType() { InterestHolderTypeCode = "TEST" },
                PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() {
                        new PimsInthldrPropInterest() {
                            PimsPropInthldrInterestTypes= new List<PimsPropInthldrInterestType>() {
                                new PimsPropInthldrInterestType() {
                                    InterestHolderInterestTypeCode="TEST"
                                    }
                                }
                            }
                    }
            };
            interestHolder.AcquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            helper.AddAndSaveChanges(interestHolder);

            // Act
            var response = repository.UpdateAllForAcquisition(1, new List<PimsInterestHolder>() {
                new PimsInterestHolder() {
                AcquisitionFileId = 1, PersonId = 1, InterestHolderId = 1, InterestHolderTypeCodeNavigation = new PimsInterestHolderType() { InterestHolderTypeCode = "TEST" },
                PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() { new PimsInthldrPropInterest() } } });

            // Assert
            response.Should().HaveCount(1);
            response.FirstOrDefault().PimsInthldrPropInterests.Should().HaveCount(0);
        }*/

        [Fact]
        public void Update_DeleteChild()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var interestHolder = new PimsInterestHolder()
            {
                AcquisitionFileId = 1,
                PersonId = 1,
                InterestHolderId = 1,
                PimsInthldrPropInterests = new List<PimsInthldrPropInterest>(),
            };
            var interestHolderPropertyInterest = new PimsInthldrPropInterest()
            {
                PimsInthldrPropInterestId = 1,
                PropertyAcquisitionFileId = 1,
                InterestHolderId = 1,
                InterestHolder = interestHolder,
                PimsPropInthldrInterestTypes = new List<PimsPropInthldrInterestType>() {
                    new PimsPropInthldrInterestType() { InterestHolderInterestTypeCode = "test" },
                    },
            };
            interestHolder.PimsInthldrPropInterests.Add(interestHolderPropertyInterest);
            interestHolder.AcquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            var context = helper.CreatePimsContext(user, true);
            context.Add(interestHolder);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            var repository = helper.CreateRepository<InterestHolderRepository>(user);

            // Act
            var response = repository.UpdateAllForAcquisition(1, new List<PimsInterestHolder>());
            context.SaveChanges();

            // Assert
            context.PimsInterestHolders.Should().HaveCount(0);
        }
        #endregion

        #endregion
    }
}
