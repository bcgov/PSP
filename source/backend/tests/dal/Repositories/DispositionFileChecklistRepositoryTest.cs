using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
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
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionFileChecklistRepositoryTest
    {
        private readonly TestHelper _helper;
        private readonly ClaimsPrincipal _user;
        private readonly PimsContext _context;
        private readonly DispositionFileChecklistRepository _repository;

        public DispositionFileChecklistRepositoryTest()
        {
            this._helper = new TestHelper();
            this._user = PrincipalHelper.CreateForPermission(Permissions.DispositionView, Permissions.DispositionEdit);
            this._context = this._helper.CreatePimsContext(this._user, true);
            this._repository = this._helper.CreateRepository<DispositionFileChecklistRepository>(this._user);
        }

        #region Get Disposition Checklist Items By File Id
        [Fact]
        public void GetByDispositionFileId_Success()
        {
            // Arrange
            var checklistItem = EntityHelper.CreateDispositionChecklistItem();
            this._context.AddAndSaveChanges(checklistItem);

            // Act
            var result = this._repository.GetAllChecklistItemsByDispositionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionChecklistItem>>();
            result.Should().HaveCount(1);
            result.FirstOrDefault().Internal_Id.Should().Be(checklistItem.Internal_Id);
        }

        [Fact]
        public void GetByDispositionFileId_NotFound()
        {
            // Act
            var result = this._repository.GetAllChecklistItemsByDispositionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionChecklistItem>>();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var checklistItem = EntityHelper.CreateDispositionChecklistItem();
            this._context.AddAndSaveChanges(checklistItem);

            // Act
            checklistItem.ChklstItemStatusTypeCode = "updated";
            var result = this._repository.Update(checklistItem);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionChecklistItem>();
            result.ChklstItemStatusTypeCode.Should().Be("updated");
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
