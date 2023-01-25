using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "financialcode")]
    [ExcludeFromCodeCoverage]
    public class ChartOfAccountsCodeRepositoryTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper = new();

        private ChartOfAccountsCodeRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<ChartOfAccountsCodeRepository>(user);
        }

        [Fact]
        public void Add_Success()
        {
            // Arrange
            var codeToAdd = new PimsChartOfAccountsCode()
            {
                Code = "FOO",
                Description = "Bar",
                EffectiveDate = new DateTime(1995, 07, 20)
            };

            var repository = CreateWithPermissions(Permissions.SystemAdmin);

            // Act
            var result = repository.Add(codeToAdd);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsChartOfAccountsCode>();
            result.Code.Should().Be("FOO");
            result.Description.Should().Be("Bar");
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.SystemAdmin);

            // Act
            Action act = () => repository.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
