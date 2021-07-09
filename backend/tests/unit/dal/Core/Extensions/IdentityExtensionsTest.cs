using FluentAssertions;
using Pims.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Pims.Api.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class IdentityExtensionsTest
    {
        #region Tests
        #region GetUserKey
        [Fact]
        public void GetUserKey()
        {
            // Arrange
            var id = Guid.NewGuid();
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var key = principal.GetUserKey();

            // Assert
            key.Should().Be(id);
        }

        [Fact]
        public void GetUserKey_NoIdentity()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var key = principal.GetUserKey();

            // Assert
            key.Should().BeEmpty();
        }

        [Fact]
        public void GetUserKey_Empty()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var key = principal.GetUserKey();

            // Assert
            key.Should().BeEmpty();
        }

        [Fact]
        public void GetUserKey_Whitespace()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, " ")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var key = principal.GetUserKey();

            // Assert
            key.Should().BeEmpty();
        }
        #endregion

        #region GetAgencies
        [Fact]
        public void GetAgencies_Empty()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", "")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgencies();

            // Assert
            agencies.Should().BeEmpty();
        }

        [Fact]
        public void GetAgencies_Whitespace()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", " ")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgencies();

            // Assert
            agencies.Should().BeEmpty();
        }

        [Fact]
        public void GetAgencies()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", "1,2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgencies();

            // Assert
            agencies.Count().Should().Be(2);
        }

        [Fact]
        public void GetAgencies_WithSeparator()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", "1;2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgencies(";");

            // Assert
            agencies.Count().Should().Be(2);
        }

        [Fact]
        public void GetAgencies_NoUser()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var agencies = principal.GetAgencies();

            // Assert
            agencies.Should().BeEmpty();
        }
        #endregion

        #region GetAgenciesAsNullable
        [Fact]
        public void GetAgenciesAsNullable_Empty()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", "")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgenciesAsNullable();

            // Assert
            agencies.Count().Should().Be(1);
            agencies.First().Should().BeNull();
        }

        [Fact]
        public void GetAgenciesAsNullable_Whitespace()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", " ")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgenciesAsNullable();

            // Assert
            agencies.Count().Should().Be(1);
            agencies.First().Should().BeNull();
        }

        [Fact]
        public void GetAgenciesAsNullable()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", "1,2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgenciesAsNullable();

            // Assert
            agencies.Count().Should().Be(2);
        }

        [Fact]
        public void GetAgenciesAsNullable_WithSeparator()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("agencies", "1;2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var agencies = principal.GetAgenciesAsNullable(";");

            // Assert
            agencies.Count().Should().Be(2);
        }

        [Fact]
        public void GetAgenciesAsNullable_NoUser()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var agencies = principal.GetAgenciesAsNullable();

            // Assert
            agencies.Should().BeEmpty();
        }
        #endregion

        #region GetUsername
        [Fact]
        public void GetUsername()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("username", "test")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetUsername();

            // Assert
            username.Should().Be("test");
        }

        [Fact]
        public void GetUsername_First()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("username", "test1"),
                new Claim("username", "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetUsername();

            // Assert
            username.Should().Be("test1");
        }

        [Fact]
        public void GetUsername_NoUser()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var username = principal.GetUsername();

            // Assert
            username.Should().BeNull();
        }
        #endregion

        #region GetDisplayName
        [Fact]
        public void GetDisplayName()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("name", "test")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetDisplayName();

            // Assert
            username.Should().Be("test");
        }

        [Fact]
        public void GetDisplayName_First()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim("name", "test1"),
                new Claim("name", "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetDisplayName();

            // Assert
            username.Should().Be("test1");
        }

        [Fact]
        public void GetDisplayName_NoUser()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var username = principal.GetDisplayName();

            // Assert
            username.Should().BeNull();
        }
        #endregion

        #region GetFirstName
        [Fact]
        public void GetFirstName()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, "test")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetFirstName();

            // Assert
            username.Should().Be("test");
        }

        [Fact]
        public void GetFirstName_First()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, "test1"),
                new Claim(ClaimTypes.GivenName, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetFirstName();

            // Assert
            username.Should().Be("test1");
        }

        [Fact]
        public void GetFirstName_NoUser()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var username = principal.GetFirstName();

            // Assert
            username.Should().BeNull();
        }
        #endregion

        #region GetLastName
        [Fact]
        public void GetLastName()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "test")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetLastName();

            // Assert
            username.Should().Be("test");
        }

        [Fact]
        public void GetLastName_First()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "test1"),
                new Claim(ClaimTypes.Surname, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetLastName();

            // Assert
            username.Should().Be("test1");
        }

        [Fact]
        public void GetLastName_NoUser()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var username = principal.GetLastName();

            // Assert
            username.Should().BeNull();
        }
        #endregion

        #region GetEmail
        [Fact]
        public void GetEmail()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, "test")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetEmail();

            // Assert
            username.Should().Be("test");
        }

        [Fact]
        public void GetEmail_First()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, "test1"),
                new Claim(ClaimTypes.Email, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var username = principal.GetEmail();

            // Assert
            username.Should().Be("test1");
        }

        [Fact]
        public void GetEmail_NoUser()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            var username = principal.GetEmail();

            // Assert
            username.Should().BeNull();
        }
        #endregion

        #region HasRole
        [Fact]
        public void HasRole_NullRequested()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => principal.HasRole((string[])null));
        }

        [Fact]
        public void HasRole_NoneRequested()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => principal.HasRole(new string[0]));
        }

        [Fact]
        public void HasRole_None()
        {
            // Arrange
            var claims = new List<Claim>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRole("test");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasRole_NotFound()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "test1"),
                new Claim(ClaimTypes.Role, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRole("test3");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasRole_NotFoundCase()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "test1"),
                new Claim(ClaimTypes.Role, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRole("Test1");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasRole_Found()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "test1"),
                new Claim(ClaimTypes.Role, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRole("test2");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void HasRole_OneFound()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "test1"),
                new Claim(ClaimTypes.Role, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRole("notfound", "test2");

            // Assert
            result.Should().BeTrue();
        }

        #region HasRoles
        [Fact]
        public void HasRoles_NullRequested()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => principal.HasRoles((string[])null));
        }

        [Fact]
        public void HasRoles_NoneRequested()
        {
            // Arrange
            var principal = new ClaimsPrincipal();

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => principal.HasRoles(new string[0]));
        }

        [Fact]
        public void HasRoles_None()
        {
            // Arrange
            var claims = new List<Claim>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRoles("test");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasRoles_NotAllFound()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "test1"),
                new Claim(ClaimTypes.Role, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRoles("test2", "test1", "test3");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasRoles_AllFound()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "test1"),
                new Claim(ClaimTypes.Role, "test2")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Act
            var result = principal.HasRoles("test2", "test1");

            // Assert
            result.Should().BeTrue();
        }
        #endregion
        #endregion
        #endregion
    }
}
