using Pims.Api.Areas.Notes.Controllers;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Security;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Api.Test.Routes
{
    /// <summary>
    /// NoteControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "note")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class NoteControllerTest
    {

        #region Constructors
        public NoteControllerTest()
        {
        }
        #endregion

        #region Tests
        [Fact]
        public void Controller_Route()
        {
            // Arrange
            // Act
            // Assert
            var type = typeof(NoteController);
            type.HasAuthorize();
            type.HasArea("notes");
            type.HasRoute("[area]");
            type.HasRoute("v{version:apiVersion}/[area]");
        }

        [Fact]
        public void AddNote_Route()
        {
            // Arrange
            var endpoint = typeof(NoteController).FindMethod(nameof(NoteController.AddNote), typeof(NoteType), typeof(EntityNoteModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPost("{type}");
            endpoint.HasPermissions(Permissions.NoteAdd);
        }

        [Fact]
        public void GetNotes_Route()
        {
            // Arrange
            var endpoint = typeof(NoteController).FindMethod(nameof(NoteController.GetNotes), typeof(NoteType), typeof(long));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet("{type}/{entityId}");
            endpoint.HasPermissions(Permissions.NoteView);
        }

        [Fact]
        public void DeleteNote_Route()
        {
            // Arrange
            var endpoint = typeof(NoteController).FindMethod(nameof(NoteController.DeleteNote), typeof(NoteType), typeof(long));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasDelete("{type}/{noteId}");
            endpoint.HasPermissions(Permissions.NoteDelete);
        }
        #endregion
    }
}
