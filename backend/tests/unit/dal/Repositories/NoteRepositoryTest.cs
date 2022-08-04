using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "note")]
    [ExcludeFromCodeCoverage]
    public class NoteRepositoryTest
    {
        #region Constructors
        public NoteRepositoryTest() { }
        #endregion

        #region Tests

        #region Add Note
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var activity = EntityHelper.CreateActivity();
            activity.ActivityInstanceId = 1;

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            var activityNote = EntityHelper.CreateActivityNote(activity);

            // Act
            var result = repository.Add(activityNote);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsActivityInstanceNote>();
            result.Note.NoteTxt.Should().Be("Test Note");
            result.ActivityInstance.ActivityInstanceId.Should().Be(1);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            Action act = () => repository.Add<PimsActivityInstance>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #endregion
    }
}
