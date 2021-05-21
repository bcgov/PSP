using FluentAssertions;
using Pims.Core.Test;
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
    public class NotificationQueueTest
    {
        #region Variables
        public static IEnumerable<object[]> Constructor_01 =>
            new List<object[]>
            {
                new object[] { "", "subject", "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { " ", "subject", "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { null, "subject", "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "to", "", "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "to", " ", "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "to", null, "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "to", "subject", "", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "to", "subject", " ", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "to", "subject", null, EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "to", "subject", "body", null, typeof(ArgumentNullException) },
            };

        public static IEnumerable<object[]> Constructor_02 =>
            new List<object[]>
            {
                new object[] { "", "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { " ", "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { null, "body", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "subject", "", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "subject", " ", EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "subject", null, EntityHelper.CreateNotificationTemplate(1, "template"), typeof(ArgumentException) },
                new object[] { "subject", "body", null, typeof(ArgumentNullException) },
            };
        #endregion

        #region Tests
        [Fact]
        public void NotificationQueue_Default_Constructor()
        {
            // Arrange
            // Act
            var queue = new NotificationQueue();

            // Assert
            queue.Id.Should().Be(0);
            queue.Key.Should().BeEmpty();
            queue.Status.Should().Be(NotificationStatus.Pending);
            queue.Priority.Should().Be(NotificationPriorities.Normal);
            queue.Encoding.Should().Be(NotificationEncodings.Utf8);
            queue.SendOn.Should().Be(new DateTime());
            queue.Subject.Should().BeNull();
            queue.BodyType.Should().Be(NotificationBodyTypes.Html);
            queue.Body.Should().BeNull();
            queue.Bcc.Should().BeNull();
            queue.Cc.Should().BeNull();
            queue.Tag.Should().BeNull();
            queue.ProjectId.Should().BeNull();
            queue.Project.Should().BeNull();
            queue.ToAgencyId.Should().BeNull();
            queue.ToAgency.Should().BeNull();
            queue.TemplateId.Should().BeNull();
            queue.Template.Should().BeNull();
            queue.ChesMessageId.Should().BeNull();
            queue.ChesTransactionId.Should().BeNull();
            queue.Responses.Should().BeEmpty();
        }

        [Fact]
        public void NotificationQueue_Constructor_01()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var template = new NotificationTemplate("template", "subject", "body")
            {
                BodyType = NotificationBodyTypes.Text,
                Priority = NotificationPriorities.Low,
                Encoding = NotificationEncodings.Base64,
                Tag = "tag"
            };
            var project = EntityHelper.CreateProject(1, 1);

            // Act
            var queue = new NotificationQueue(template, project, "to", "subject", "body");

            // Assert
            queue.Id.Should().Be(0);
            queue.Key.Should().NotBeEmpty();
            queue.TemplateId.Should().Be(template.Id);
            queue.Template.Should().Be(template);
            queue.Subject.Should().Be("subject");
            queue.Body.Should().Be("body");
            queue.BodyType.Should().Be(template.BodyType);
            queue.Priority.Should().Be(template.Priority);
            queue.Encoding.Should().Be(template.Encoding);
            queue.Tag.Should().Be(template.Tag);
            queue.ProjectId.Should().Be(project.Id);
            queue.Project.Should().Be(project);
            queue.To.Should().Be("to");
            queue.SendOn.Should().BeOnOrAfter(date);
        }

        [Theory]
        [MemberData(nameof(Constructor_01))]
        public void NotificationQueue_Constructor_01_ArgumentException(string to, string subject, string body, NotificationTemplate template, Type throws)
        {
            // Arrange
            var project = EntityHelper.CreateProject(1, 1);

            // Act
            // Assert
            Assert.Throws(throws, () => new NotificationQueue(template, project, to, subject, body));
        }

        [Fact]
        public void NotificationQueue_Constructor_02()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var template = new NotificationTemplate("template", "subject", "body")
            {
                BodyType = NotificationBodyTypes.Text,
                Priority = NotificationPriorities.Low,
                Encoding = NotificationEncodings.Base64,
                Tag = "tag"
            };
            var agency = EntityHelper.CreateAgency(2);
            agency.Email = "test@test.com";
            var project = EntityHelper.CreateProject(1, agency);

            // Act
            var queue = new NotificationQueue(template, project, agency, "subject", "body");

            // Assert
            queue.Id.Should().Be(0);
            queue.Key.Should().NotBeEmpty();
            queue.TemplateId.Should().Be(template.Id);
            queue.Template.Should().Be(template);
            queue.Subject.Should().Be("subject");
            queue.Body.Should().Be("body");
            queue.BodyType.Should().Be(template.BodyType);
            queue.Priority.Should().Be(template.Priority);
            queue.Encoding.Should().Be(template.Encoding);
            queue.Tag.Should().Be(template.Tag);
            queue.ProjectId.Should().Be(project.Id);
            queue.Project.Should().Be(project);
            queue.ToAgencyId.Should().Be(agency.Id);
            queue.ToAgency.Should().Be(agency);
            queue.To.Should().Be(agency.Email);
            queue.SendOn.Should().BeOnOrAfter(date);
        }

        [Theory]
        [MemberData(nameof(Constructor_02))]
        public void NotificationQueue_Constructor_02_ArgumentException(string subject, string body, NotificationTemplate template, Type throws)
        {
            // Arrange
            var agency = EntityHelper.CreateAgency(2);
            agency.Email = "test@test.com";
            var project = EntityHelper.CreateProject(1, agency);

            // Act
            // Assert
            Assert.Throws(throws, () => new NotificationQueue(template, project, agency, subject, body));
        }


        [Fact]
        public void NotificationQueue_Constructor_04()
        {
            // Arrange
            var date = DateTime.UtcNow;
            var template = new NotificationTemplate("template", "subject", "body")
            {
                BodyType = NotificationBodyTypes.Text,
                Priority = NotificationPriorities.Low,
                Encoding = NotificationEncodings.Base64,
                Tag = "tag"
            };

            // Act
            var queue = new NotificationQueue(template, "to", "cc", "bcc", "subject", "body");

            // Assert
            queue.Id.Should().Be(0);
            queue.Key.Should().NotBeEmpty();
            queue.TemplateId.Should().Be(template.Id);
            queue.Template.Should().Be(template);
            queue.Subject.Should().Be("subject");
            queue.Body.Should().Be("body");
            queue.BodyType.Should().Be(template.BodyType);
            queue.Priority.Should().Be(template.Priority);
            queue.Encoding.Should().Be(template.Encoding);
            queue.Tag.Should().Be(template.Tag);
            queue.To.Should().Be("to");
            queue.Cc.Should().Be("cc");
            queue.Bcc.Should().Be("bcc");
            queue.SendOn.Should().BeOnOrAfter(date);
        }

        [Theory]
        [MemberData(nameof(Constructor_01))]
        public void NotificationQueue_Constructor_04_ArgumentException(string to, string subject, string body, NotificationTemplate template, Type throws)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws(throws, () => new NotificationQueue(template, to, "cc", "bcc", subject, body));
        }
        #endregion
    }
}
