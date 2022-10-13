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

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
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

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();
            var note = EntityHelper.CreateNote("Test Note");

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(note);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();
            var note = EntityHelper.CreateNote("Test Note");

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(note);

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #region GetNotes
        [Fact]
        public void GetNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();
            var notes = new[] { EntityHelper.CreateNote("Test Note 1"), EntityHelper.CreateNote("Test Note 2") };

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.GetActivityNotes(It.IsAny<long>())).Returns(notes);

            // Act
            var result = service.GetNotes(NoteType.Activity, 1);

            // Assert
            repository.Verify(x => x.GetActivityNotes(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetNotes_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.GetActivityNotes(It.IsAny<long>()));

            // Act
            Action act = () => service.GetNotes(NoteType.Activity, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetActivityNotes(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteEdit, Permissions.NoteView);
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();
            var note = EntityHelper.CreateNote("Test Note");
            var model = mapper.Map<NoteModel>(note);

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsNote>())).Returns(note);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(note);

            // Act
            var result = service.Update(model);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsNote>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<NoteService>(user);

            var mapper = helper.GetService<IMapper>();
            var note = EntityHelper.CreateNote("Test Note");
            var model = mapper.Map<NoteModel>(note);

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsNote>())).Returns(note);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(note);

            // Act
            Action act = () => service.Update(model);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsNote>()), Times.Never);
        }
        #endregion

        #region DeleteNote
        [Fact]
        public void DeleteNote_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteDelete);
            var service = helper.Create<NoteService>(user);
            var mapper = helper.GetService<IMapper>();

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.DeleteActivityNotes(It.IsAny<long>()));

            // Act
            service.DeleteNote(NoteType.Activity, 1);

            // Assert
            repository.Verify(x => x.DeleteActivityNotes(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeleteNote_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<NoteService>(user);
            var mapper = helper.GetService<IMapper>();

            var repository = helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.DeleteActivityNotes(It.IsAny<long>()));

            // Act
            Action act = () => service.DeleteNote(NoteType.Activity, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.DeleteActivityNotes(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #endregion
    }
}
