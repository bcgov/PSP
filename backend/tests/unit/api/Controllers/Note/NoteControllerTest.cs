using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Notes.Controllers;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Entity = Pims.Dal.Entities;
using SModel = Pims.Api.Areas.Notes;

namespace Pims.Api.Test.Controllers.Note
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
                AppCreateUserid = "admin",
            }, new PimsNote() {
                NoteId = 1,
                NoteTxt = "Note 1",
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin", }, };

            var service = helper.GetService<Mock<INoteService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.GetNotes(It.IsAny<NoteType>())).Returns(notes);

            // Act
            var result = controller.GetNotes(Constants.NoteType.Activity);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<List<NoteModel>>(actionResult.Value);
            var expectedResult = mapper.Map<List<NoteModel>>(notes);
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.GetNotes(It.IsAny<NoteType>()), Times.Once());
        }

        [Fact]
        public void Delete_Note_All_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<NoteController>(Permissions.ContactView);

            var service = helper.GetService<Mock<INoteService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.DeleteNote(It.IsAny<NoteType>(), It.IsAny<int>()));

            // Act
            var result = controller.DeleteNote(Constants.NoteType.Activity, 1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(true, actionResult.Value);
            service.Verify(m => m.DeleteNote(It.IsAny<NoteType>(), It.IsAny<int>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
