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
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Entity = Pims.Dal.Entities;
using SModel = Pims.Api.Areas.Notes;

namespace Pims.Api.Test.Controllers.Note
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "note")]
    [ExcludeFromCodeCoverage]
    public class NoteControllerTest
    {
        #region Variables

        #endregion

        private Mock<INoteService> _service;
        private NoteController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public NoteControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<NoteController>(Permissions.NoteAdd, Permissions.NoteView, Permissions.NoteEdit);
            this._mapper = this._helper.GetService<IMapper>();
            this._service = this._helper.GetService<Mock<INoteService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to add a note to a parent entity.
        /// </summary>
        [Fact]
        public void AddNote_Success()
        {
            // Arrange
            var note = EntityHelper.CreateProjectNote();
            var noteModel = this._mapper.Map<EntityNoteModel>(note);

            this._service.Setup(m => m.Add(It.IsAny<NoteType>(), It.IsAny<EntityNoteModel>())).Returns(noteModel);

            // Act
            var result = this._controller.AddNote(Constants.NoteType.Project, noteModel);

            // Assert
            this._service.Verify(m => m.Add(It.IsAny<NoteType>(), It.IsAny<EntityNoteModel>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get all notes for a given parent entity.
        /// </summary>
        [Fact]
        public void GetNotes_All_Success()
        {
            // Arrange
            var notes = new[] { EntityHelper.CreateNote("Note 1"), EntityHelper.CreateNote("Note 2") };

            this._service.Setup(m => m.GetNotes(It.IsAny<NoteType>(), It.IsAny<long>())).Returns(notes);

            // Act
            var result = this._controller.GetNotes(Constants.NoteType.Project, 1);

            // Assert
            this._service.Verify(m => m.GetNotes(It.IsAny<NoteType>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get a note by id.
        /// </summary>
        [Fact]
        public void GetNoteById_Success()
        {
            // Arrange
            var note = EntityHelper.CreateNote("Note 1");
            var noteModel = this._mapper.Map<NoteModel>(note);

            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(noteModel);

            // Act
            var result = this._controller.GetNoteById(1);

            // Assert
            this._service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update a note.
        /// </summary>
        [Fact]
        public void UpdateNote_Success()
        {
            // Arrange
            var note = EntityHelper.CreateNote("Note 1");
            var noteModel = this._mapper.Map<NoteModel>(note);

            this._service.Setup(m => m.Update(It.IsAny<NoteModel>())).Returns(noteModel);

            // Act
            var result = this._controller.UpdateNote(1, noteModel);

            // Assert
            this._service.Verify(m => m.Update(It.IsAny<NoteModel>()), Times.Once());
        }

        [Fact]
        public void Delete_Note_Success()
        {
            // Arrange
            this._service.Setup(m => m.DeleteNote(It.IsAny<NoteType>(), It.IsAny<long>(), true));

            // Act
            var result = this._controller.DeleteNote(Constants.NoteType.Project, 1);

            // Assert
            this._service.Verify(m => m.DeleteNote(It.IsAny<NoteType>(), It.IsAny<long>(), true), Times.Once());
        }
        #endregion
    }
}
