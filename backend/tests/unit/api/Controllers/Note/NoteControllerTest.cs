using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Entity = Pims.Dal.Entities;
using SModel = Pims.Api.Areas.Notes;
using FluentAssertions;
using Pims.Api.Areas.Notes.Controllers;
using Pims.Api.Areas.Notes.Models;
using Pims.Dal.Entities;

namespace Pims.Api.Test.Controllers.Research
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "research")]
    [ExcludeFromCodeCoverage]
    public class NoteControllerTest
    {
        #region Variables

        #endregion

        #region Tests
        #region GetNotes
        /// <summary>
        /// Make a successful request that includes the research filter.
        /// </summary>
        [Fact]
        public void GetNotes_All_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<NoteController>(Permissions.ContactView);

            var notes = new[] { new PimsNote() {
                NoteId = 1,
                NoteTxt = "Note 1",
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin"
            }, new PimsNote() {
                NoteId = 1,
                NoteTxt = "Note 1",
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin" } };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Note.GetActivityNotes()).Returns(notes);

            // Act
            var result = controller.GetNotes(Areas.Notes.Models.NoteType.Activity);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<List<NoteModel>>(actionResult.Value);
            var expectedResult = mapper.Map<List<NoteModel>>(notes);
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Note.GetActivityNotes(), Times.Once());
        }

        [Fact]
        public void Delete_Note_All_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<NoteController>(Permissions.ContactView);

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Note.DeleteActivityNotes(It.IsAny<int>()));

            // Act
            var result = controller.DeleteNote(Areas.Notes.Models.NoteType.Activity, 1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(true, actionResult.Value);
            service.Verify(m => m.Note.DeleteActivityNotes(It.IsAny<int>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
