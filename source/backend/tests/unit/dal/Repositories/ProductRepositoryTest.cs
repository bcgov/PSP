using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    [Trait("group", "activity")]
    [ExcludeFromCodeCoverage]
    public class ProductRepositoryTest
    {
        #region Constructors
        public ProductRepositoryTest() { }
        #endregion

        #region Tests
        [Fact]
        public void GetByProductBatch_ChangedCode()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var initialProduct = new PimsProduct() { Code = "code1", Description = "desc1" };
            var project = new PimsProject() { PimsProducts = new List<PimsProduct>() { initialProduct } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProductRepository>(user);

            // Act
            var result = repository.GetByProductBatch(new List<PimsProduct>() { new PimsProduct() { Code = "code2", Description = "desc1" } }, project.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetByProductBatch_ChangedDescription()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var initialProduct = new PimsProduct() { Code = "code1", Description = "desc1" };
            var project = new PimsProject() { PimsProducts = new List<PimsProduct>() { initialProduct } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProductRepository>(user);

            // Act
            var result = repository.GetByProductBatch(new List<PimsProduct>() { new PimsProduct() { Code = "code1", Description = "desc2" } }, project.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        [Fact]
        public void GetByProductBatch_MultipleChanges()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var initialProduct = new PimsProduct() { Code = "code1", Description = "desc1" };
            var project = new PimsProject() { PimsProducts = new List<PimsProduct>() { initialProduct } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProductRepository>(user);

            // Act
            var result = repository.GetByProductBatch(new List<PimsProduct>()
            {
                new PimsProduct() { Code = "code1", Description = "desc2" },
                new PimsProduct() { Code = "code2", Description = "desc1" },
            }, project.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetByProductBatch_Duplicate()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var initialProduct = new PimsProduct() { Code = "code1", Description = "desc1" };
            var project = new PimsProject() { PimsProducts = new List<PimsProduct>() { initialProduct } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProductRepository>(user);

            // Act
            var result = repository.GetByProductBatch(new List<PimsProduct>() { new PimsProduct() { Code = "code1", Description = "desc1" } }, 2);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetByProductBatch_ReplacedCodeInProject()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var initialProduct = new PimsProduct() { Code = "code1", Description = "desc1" };
            var project = new PimsProject() { PimsProducts = new List<PimsProduct>() { initialProduct } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProductRepository>(user);

            // Act
            var result = repository.GetByProductBatch(new List<PimsProduct>() { new PimsProduct() { Code = "code1", Description = "desc1" } }, 1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty(); // in this instance even though the code/description is the same the new product is replacing the old product within the given project.
        }

        #endregion
    }
}
