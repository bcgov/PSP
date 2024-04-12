using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Linq;
using System.Threading.Channels;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Converters;
using NExpect.Interfaces;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "propertyoperation")]
    [ExcludeFromCodeCoverage]
    public class PropertyOperationServiceTest
    {
        private readonly TestHelper _helper;

        public PropertyOperationServiceTest()
        {
            this._helper = new TestHelper();
        }

        private PropertyOperationService CreateDispositionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<PropertyOperationService>(user);
        }

        #region Subdivide

        [Fact]
        public void Subdivide_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.SubdivideProperty(new List<PimsPropertyOperation>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Subdivide_Should_Fail_SourceRetired()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var retiredProperty = EntityHelper.CreateProperty(3);
            retiredProperty.IsRetired = true;
            retiredProperty.IsOwned = true;

            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(retiredProperty);

            var operation = EntityHelper.CreatePropertyOperation();
            operation.SourceProperty = retiredProperty;
            operation.SourcePropertyId = retiredProperty.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operation };

            // Act
            Action act = () => service.SubdivideProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Retired properties cannot be subdivided.");
        }

        [Fact]
        public void Subdivide_Should_Fail_MismatchNumbers()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3);
            property.IsOwned = true;
            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.PropertyOperationNo = -1;
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne, EntityHelper.CreatePropertyOperation() };

            // Act
            Action act = () => service.SubdivideProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("All property operations must have matching operation numbers.");
        }

        [Fact]
        public void Subdivide_Should_Fail_MismatchTypes()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3);
            property.IsOwned = true;
            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.PropertyOperationTypeCode = PropertyOperationTypes.CONSOLIDATE.ToString();
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne, EntityHelper.CreatePropertyOperation() };

            // Act
            Action act = () => service.SubdivideProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("All property operations must have matching type codes.");
        }

        [Fact]
        public void Subdivide_Should_Fail_MismatchSource()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3);
            property.IsOwned = true;
            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.SourcePropertyId = 2;
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne, EntityHelper.CreatePropertyOperation() };

            // Act
            Action act = () => service.SubdivideProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("All property operations must have the same PIMS parent property.");
        }

        [Fact]
        public void Subdivide_Should_Fail_DestinationEmpty()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3);
            property.IsOwned = true;
            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne };

            // Act
            Action act = () => service.SubdivideProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Subdivisions must contain at least two child properties.");
        }

        [Fact]
        public void Subdivide_Should_Fail_PidExists()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3);
            property.IsOwned = true;
            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            propertyService.Setup(x => x.GetByPid(It.IsAny<string>())).Returns(EntityHelper.CreateProperty(4));

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;

            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.SourceProperty = property;
            operationTwo.SourcePropertyId = property.PropertyId;

            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            // Act
            Action act = () => service.SubdivideProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Subdivision children may not already be in the PIMS inventory.");
        }

        [Fact]
        public void Subdivide_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var sameProperty = EntityHelper.CreateProperty(3);
            sameProperty.IsOwned = true;
            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(sameProperty);
            propertyService.Setup(x => x.GetByPid(It.IsAny<string>())).Throws(new KeyNotFoundException());
            propertyService.Setup(x => x.RetireProperty(It.IsAny<PimsProperty>(), false)).Returns(sameProperty);
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false)).Returns(sameProperty);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.SourceProperty = sameProperty;
            operationOne.SourcePropertyId = sameProperty.PropertyId;

            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.SourceProperty = sameProperty;
            operationTwo.SourcePropertyId = sameProperty.PropertyId;

            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            var repository = this._helper.GetService<Mock<IPropertyOperationRepository>>();
            repository.Setup(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>())).Returns(operations);

            // Act
            var response = service.SubdivideProperty(operations);

            // Assert
            repository.Verify(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>()), Times.Once);
            propertyService.Verify(x => x.RetireProperty(It.IsAny<PimsProperty>(), false), Times.Once);
            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false), Times.Exactly(2));
        }

        [Fact]
        public void Subdivide_Success_SameSourceDestinationPid()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var sameProperty = EntityHelper.CreateProperty(3, isCoreInventory: true);
            propertyService.Setup(x => x.GetById(It.IsAny<long>())).Returns(sameProperty);
            propertyService.Setup(x => x.GetByPid(It.IsAny<string>())).Returns(sameProperty);
            propertyService.Setup(x => x.RetireProperty(It.IsAny<PimsProperty>(), false)).Returns(sameProperty);
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false)).Returns(sameProperty);

            var operationWithSameDestSource = EntityHelper.CreatePropertyOperation();
            operationWithSameDestSource.DestinationProperty = sameProperty;
            operationWithSameDestSource.DestinationPropertyId = 0;
            operationWithSameDestSource.SourceProperty = sameProperty;
            operationWithSameDestSource.SourcePropertyId = sameProperty.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationWithSameDestSource, operationWithSameDestSource };

            var repository = this._helper.GetService<Mock<IPropertyOperationRepository>>();
            repository.Setup(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>())).Returns(operations);

            // Act
            var response = service.SubdivideProperty(operations);

            // Assert
            repository.Verify(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>()), Times.Once);
            propertyService.Verify(x => x.RetireProperty(It.IsAny<PimsProperty>(), false), Times.Once);
            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false), Times.Exactly(2));
        }

        #endregion

        #region Consolidate

        [Fact]
        public void Consolidate_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.ConsolidateProperty(new List<PimsPropertyOperation>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Consolidate_Should_Fail_SourceCount()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3, isCoreInventory: true);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { property });

            var operation = EntityHelper.CreatePropertyOperation();
            operation.SourceProperty = property;
            operation.SourcePropertyId = property.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operation };

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Consolidations must contain at least two different parent properties.");
        }

        [Fact]
        public void Consolidate_Should_Fail_SourceRetired()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var retiredProperty = EntityHelper.CreateProperty(3, isCoreInventory: true);
            retiredProperty.IsRetired = true;
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { retiredProperty });

            var operation = EntityHelper.CreatePropertyOperation();
            operation.SourceProperty = retiredProperty;
            operation.SourcePropertyId = retiredProperty.PropertyId;

            var operations = new List<PimsPropertyOperation>() { operation };

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Retired properties cannot be consolidated.");
        }

        [Fact]
        public void Consolidate_Should_Fail_MismatchNumbers()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3, isCoreInventory: true);
            var propertyTwo = EntityHelper.CreateProperty(4, isCoreInventory: true);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { property, propertyTwo });

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.PropertyOperationNo = -1;
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.SourceProperty = propertyTwo;
            operationTwo.SourcePropertyId = propertyTwo.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("All property operations must have matching operation numbers.");
        }

        [Fact]
        public void Consolidate_Should_Fail_MismatchTypes()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3, isCoreInventory: true);
            var propertyTwo = EntityHelper.CreateProperty(4, isCoreInventory: true);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { property, propertyTwo });

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.PropertyOperationTypeCode = PropertyOperationTypes.CONSOLIDATE.ToString();
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.SourceProperty = propertyTwo;
            operationTwo.SourcePropertyId = propertyTwo.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("All property operations must have matching type codes.");
        }

        [Fact]
        public void Consolidate_Should_Fail_MismatchDestination()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3, isCoreInventory: true);
            var propertyTwo = EntityHelper.CreateProperty(4, isCoreInventory: true);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { property, propertyTwo });

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.DestinationProperty.Pid = -1;
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.SourceProperty = propertyTwo;
            operationTwo.SourcePropertyId = propertyTwo.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("All property operations must have the same child property with the same PID.");
        }

        [Fact]
        public void Consolidate_Should_Fail_DestinationNull()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3, isCoreInventory: true);
            var propertyTwo = EntityHelper.CreateProperty(4, isCoreInventory: true);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { property, propertyTwo });

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.DestinationProperty = null;
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationOne };

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Consolidation child must have a property with a valid PID.");
        }

        [Fact]
        public void Consolidate_Should_Fail_DestinationInInventory()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var property = EntityHelper.CreateProperty(3, isCoreInventory: true);
            property.Pid = 2;
            var propertyTwo = EntityHelper.CreateProperty(4, isCoreInventory: true);
            propertyTwo.Pid = 3;
            var propertyThree = EntityHelper.CreateProperty(5, isCoreInventory: true);
            propertyThree.Pid = 4;
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { property, propertyTwo });
            propertyService.Setup(x => x.GetByPid(It.IsAny<string>())).Returns(propertyThree);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.SourceProperty = property;
            operationOne.SourcePropertyId = property.PropertyId;
            operationOne.DestinationProperty = propertyThree;
            operationOne.DestinationPropertyId = 0;

            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.SourceProperty = propertyTwo;
            operationTwo.SourcePropertyId = propertyTwo.PropertyId;
            operationTwo.DestinationProperty = propertyThree;
            operationTwo.DestinationPropertyId = 0;

            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Consolidated child property may not be in the PIMS inventory unless also in the parent property list.");
        }

        [Fact]
        public void Consolidate_Should_Fail_NotOwnedSource()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var sameProperty = EntityHelper.CreateProperty(3);
            var otherProperty = EntityHelper.CreateProperty(4);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { sameProperty, otherProperty });
            propertyService.Setup(x => x.GetByPid(It.IsAny<string>())).Throws(new KeyNotFoundException());
            propertyService.Setup(x => x.RetireProperty(It.IsAny<PimsProperty>(), false)).Returns((PimsProperty p, bool b) => p);
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false)).Returns(sameProperty);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.DestinationProperty.PropertyId = 0;
            operationOne.SourceProperty = sameProperty;
            operationOne.SourcePropertyId = sameProperty.PropertyId;
            sameProperty.IsOwned = false;

            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.DestinationProperty.PropertyId = 0;
            operationTwo.SourceProperty = otherProperty;
            operationTwo.SourcePropertyId = otherProperty.PropertyId;
            sameProperty.IsOwned = false;

            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            var repository = this._helper.GetService<Mock<IPropertyOperationRepository>>();
            repository.Setup(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>())).Returns(operations);

            // Act
            Action act = () => service.ConsolidateProperty(operations);

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("All source properties must be owned.");
        }

        [Fact]
        public void Consolidate_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var sameProperty = EntityHelper.CreateProperty(3, isCoreInventory: true);
            var otherProperty = EntityHelper.CreateProperty(4, isCoreInventory: true);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { sameProperty, otherProperty });
            propertyService.Setup(x => x.GetByPid(It.IsAny<string>())).Throws(new KeyNotFoundException());
            propertyService.Setup(x => x.RetireProperty(It.IsAny<PimsProperty>(), false)).Returns((PimsProperty p, bool b) => p);
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false)).Returns(sameProperty);

            var operationOne = EntityHelper.CreatePropertyOperation();
            operationOne.DestinationProperty.PropertyId = 0;
            operationOne.SourceProperty = sameProperty;
            operationOne.SourcePropertyId = sameProperty.PropertyId;

            var operationTwo = EntityHelper.CreatePropertyOperation();
            operationTwo.DestinationProperty.PropertyId = 0;
            operationTwo.SourceProperty = otherProperty;
            operationTwo.SourcePropertyId = otherProperty.PropertyId;

            var operations = new List<PimsPropertyOperation>() { operationOne, operationTwo };

            var repository = this._helper.GetService<Mock<IPropertyOperationRepository>>();
            repository.Setup(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>())).Returns(operations);

            // Act
            var response = service.ConsolidateProperty(operations);

            // Assert
            repository.Verify(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>()), Times.Once);
            propertyService.Verify(x => x.RetireProperty(It.IsAny<PimsProperty>(), false), Times.Exactly(2));
            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false), Times.Once);
        }

        [Fact]
        public void Consolidate_Success_SameSourceDestinationPid()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.PropertyEdit);
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            var sameProperty = EntityHelper.CreateProperty(3, isCoreInventory: true);
            var otherProperty = EntityHelper.CreateProperty(4, isCoreInventory: true);
            propertyService.Setup(x => x.GetMultipleById(It.IsAny<List<long>>())).Returns(new List<PimsProperty> { sameProperty, otherProperty });
            propertyService.Setup(x => x.GetByPid(It.IsAny<string>())).Returns(sameProperty);
            propertyService.Setup(x => x.RetireProperty(It.IsAny<PimsProperty>(), false)).Returns((PimsProperty p, bool b) => p);
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false)).Returns(sameProperty);

            var operationWithSameDestSource = EntityHelper.CreatePropertyOperation();
            operationWithSameDestSource.DestinationProperty = sameProperty;
            operationWithSameDestSource.SourceProperty = sameProperty;
            operationWithSameDestSource.SourcePropertyId = sameProperty.PropertyId;

            var operationWithSameDestSourceTwo = EntityHelper.CreatePropertyOperation();
            operationWithSameDestSourceTwo.DestinationProperty = sameProperty;
            operationWithSameDestSourceTwo.SourceProperty = otherProperty;
            operationWithSameDestSourceTwo.SourcePropertyId = otherProperty.PropertyId;
            var operations = new List<PimsPropertyOperation>() { operationWithSameDestSource, operationWithSameDestSourceTwo };

            var repository = this._helper.GetService<Mock<IPropertyOperationRepository>>();
            repository.Setup(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>())).Returns(operations);

            // Act
            var response = service.ConsolidateProperty(operations);

            // Assert
            repository.Verify(x => x.AddRange(It.IsAny<List<PimsPropertyOperation>>()), Times.Once);
            propertyService.Verify(x => x.RetireProperty(It.IsAny<PimsProperty>(), false), Times.Exactly(2));
            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), true, false), Times.Once);
        }

        #endregion
    }
}
