using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionFileChecklistRepositoryTest
    {
        private readonly TestHelper _helper;
        private readonly ClaimsPrincipal _user;
        private readonly PimsContext _context;
        private readonly AcquisitionFileChecklistRepository _repository;

        public AcquisitionFileChecklistRepositoryTest()
        {
            this._helper = new TestHelper();
            this._user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView, Permissions.AcquisitionFileEdit);
            this._context = this._helper.CreatePimsContext(this._user, true);
            this._repository = this._helper.CreateRepository<AcquisitionFileChecklistRepository>(this._user);
        }

        #region Get Acquisition Checklist Items By File Id
        [Fact]
        public void GetByAcquisitionFileId_Success()
        {
            // Arrange
            var checklistItem = EntityHelper.CreateAcquisitionChecklistItem();
            this._context.AddAndSaveChanges(checklistItem);

            // Act
            var result = this._repository.GetAllChecklistItemsByAcquisitionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsAcquisitionChecklistItem>>();
            result.Should().HaveCount(1);
            result.FirstOrDefault().Internal_Id.Should().Be(checklistItem.Internal_Id);
        }

        [Fact]
        public void GetByAcquisitionFileId_NotFound()
        {
            // Act
            var result = this._repository.GetAllChecklistItemsByAcquisitionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsAcquisitionChecklistItem>>();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var checklistItem = EntityHelper.CreateAcquisitionChecklistItem();
            this._context.AddAndSaveChanges(checklistItem);

            // Act
            checklistItem.AcqChklstItemStatusTypeCode = "updated";
            var result = this._repository.Update(checklistItem);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsAcquisitionChecklistItem>();
            result.AcqChklstItemStatusTypeCode.Should().Be("updated");
        }

        [Fact]
        public void Update_ThrowIfNull()
        {
            // Act
            Action act = () => this._repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion
    }
}
