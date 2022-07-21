using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "note")]
    [ExcludeFromCodeCoverage]
    public class NoteServiceTest
    {
        #region Tests
        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();
            var activityNote = EntityHelper.CreateActivityNote();
            var noteModel = mapper.Map<EntityNoteModel>(activityNote);

            var repository = helper.GetService<Mock<IEntityNoteRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstanceNote>())).Returns(activityNote);

            // Act
            var result = service.Add(NoteType.Activity, noteModel);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstanceNote>()), Times.Once);
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();
            var activityNote = EntityHelper.CreateActivityNote();
            var noteModel = mapper.Map<EntityNoteModel>(activityNote);

            var repository = helper.GetService<Mock<IEntityNoteRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstanceNote>())).Returns(activityNote);

            // Act
            Action act = () => service.Add(NoteType.Activity, noteModel);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstanceNote>()), Times.Never);
        }

        #endregion
        #endregion
    }
}
