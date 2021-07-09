using FluentAssertions;
using Pims.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class NotificationTemplateTest
    {
        #region Variables
        public static IEnumerable<object[]> Constructor_01 =>
            new List<object[]>
            {
                new object[] { "", "subject", "body", typeof(ArgumentException) },
                new object[] { " ", "subject", "body", typeof(ArgumentException) },
                new object[] { null, "subject", "body", typeof(ArgumentException) }
            };
        #endregion

        #region Tests
        [Fact]
        public void NotificationTemplate_Default_Constructor()
        {
            // Arrange
            // Act
            var template = new NotificationTemplate();

            // Assert
            template.Id.Should().Be(0);
            template.Name.Should().BeNull();
            template.Description.Should().BeNull();
            template.To.Should().BeNull();
            template.Cc.Should().BeNull();
            template.Bcc.Should().BeNull();
            template.Audience.Should().Be(NotificationAudiences.Default);
            template.Encoding.Should().Be(NotificationEncodings.Utf8);
            template.BodyType.Should().Be(NotificationBodyTypes.Html);
            template.Priority.Should().Be(NotificationPriorities.Normal);
            template.Subject.Should().BeNull();
            template.Body.Should().BeNull();
            template.IsDisabled.Should().BeFalse();
            template.Tag.Should().BeNull();
            template.Notifications.Should().BeEmpty();
        }

        [Fact]
        public void NotificationTemplate_Constructor_01()
        {
            // Arrange
            // Act
            var template = new NotificationTemplate("name", "subject", "body");

            // Assert
            template.Name.Should().Be("name");
            template.Subject.Should().Be("subject");
            template.Body.Should().Be("body");
        }

        [Theory]
        [MemberData(nameof(Constructor_01))]
        public void NotificationTemplate_Constructor_01_ArgumentException(string name, string subject, string body, Type throws)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws(throws, () => new NotificationTemplate(name, subject, body));
        }

        [Fact]
        public void NotificationTemplate_Constructor_02()
        {
            // Arrange
            // Act
            var template = new NotificationTemplate("name", NotificationEncodings.Hex, NotificationBodyTypes.Text, "subject", "body");

            // Assert
            template.Name.Should().Be("name");
            template.Subject.Should().Be("subject");
            template.Body.Should().Be("body");
            template.Encoding.Should().Be(NotificationEncodings.Hex);
            template.BodyType.Should().Be(NotificationBodyTypes.Text);
        }

        [Theory]
        [MemberData(nameof(Constructor_01))]
        public void NotificationTemplate_Constructor_02_ArgumentException(string name, string subject, string body, Type throws)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws(throws, () => new NotificationTemplate(name, NotificationEncodings.Hex, NotificationBodyTypes.Text, subject, body));
        }
        #endregion
    }
}
