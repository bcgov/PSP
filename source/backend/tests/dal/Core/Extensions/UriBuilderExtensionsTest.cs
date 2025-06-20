using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Extensions;
using Xunit;

namespace Pims.Api.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class UriBuilderExtensionsTest
    {
        #region Tests
        #region AppendQuery
        [Fact]
        public void AppendQuery()
        {
            // Arrange
            var builder = new UriBuilder();

            // Act
            builder.AppendQuery("key1", "value1");
            builder.AppendQuery("key2", null);
            builder.AppendQuery("key3", string.Empty);
            builder.AppendQuery("key4", " ");

            // Assert
            builder.Query.Should().Be("?key1=value1");
        }

        [Fact]
        public void AppendQuery_AddEmpty()
        {
            // Arrange
            var builder = new UriBuilder();

            // Act
            builder.AppendQuery("key1", "value1");
            builder.AppendQuery("key2", null, true);
            builder.AppendQuery("key3", string.Empty, true);
            builder.AppendQuery("key4", " ", true);

            // Assert
            builder.Query.Should().Be("?key1=value1&key2=&key3=&key4=+");
        }

        [Fact]
        public void AppendQuery_ArgumentException()
        {
            // Arrange
            var builder = new UriBuilder();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => builder.AppendQuery(null, "value1"));
            Assert.Throws<ArgumentException>(() => builder.AppendQuery(string.Empty, "value1"));
            Assert.Throws<ArgumentException>(() => builder.AppendQuery(" ", "value1"));
        }
        #endregion
        #endregion
    }
}
