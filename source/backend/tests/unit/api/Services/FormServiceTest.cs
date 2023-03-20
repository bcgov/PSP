using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "form")]
    [ExcludeFromCodeCoverage]
    public class FormServiceTest
    {
        private TestHelper _helper;

        public FormServiceTest()
        {
            _helper = new TestHelper();
        }

        private FormService CreateFormServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            var service = _helper.Create<FormService>(user);
            return service;
        }

        #region Tests
        #region Add

        [Fact]
        public void AddFormFile_Success()
        {
            // Arrange
            var service = CreateFormServiceWithPermissions(Permissions.ActivityAdd, Permissions.ResearchFileEdit);

            var repository = _helper.GetService<Mock<IAcquisitionFileFormRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFileForm>()));

            // Act
            var result = service.AddAcquisitionForm(new Models.Lookup.LookupModel<string>() { Id = "H120" }, 1);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFileForm>()), Times.Once);
        }

        [Fact]
        public void AddFormFile_NoPermission()
        {
            // Arrange
            var service = CreateFormServiceWithPermissions();

            var repository = _helper.GetService<Mock<IAcquisitionFileFormRepository>>();

            // Act
            Action act = () => service.AddAcquisitionForm(new Models.Lookup.LookupModel<string>() { Id = "H120" }, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFileForm>()), Times.Never);
        }

        
        #endregion

        #endregion
    }
}
