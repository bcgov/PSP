using FluentAssertions;
using Pims.Ches.Models;
using Pims.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Pims.Dal.Test.Libraries.Notifications
{
    [Trait("category", "unit")]
    [Trait("category", "notification")]
    [Trait("group", "notification")]
    [ExcludeFromCodeCoverage]
    public class ModelTest
    {
        #region Tests
        #region EmailResponse
        [Fact]
        public void EmailResponse_Default_Constructor()
        {
            // Arrange
            // Act
            var response = new EmailResponse();

            // Assert
            response.TransactionId.Should().Be(Guid.Empty);
            response.Messages.Should().BeEmpty();
        }

        [Fact]
        public void EmailResponse_Constructor()
        {
            // Arrange
            var model = new EmailResponseModel()
            {
                TransactionId = Guid.NewGuid()
            };

            // Act
            var response = new EmailResponse(model);

            // Assert
            response.TransactionId.Should().Be(model.TransactionId);
            response.Messages.Should().BeEmpty();
        }

        [Fact]
        public void EmailResponse_WithNoMessages_Constructor()
        {
            // Arrange
            var model = new EmailResponseModel()
            {
                TransactionId = Guid.NewGuid(),
                Messages = new List<MessageResponseModel>()
            };

            // Act
            var response = new EmailResponse(model);

            // Assert
            response.TransactionId.Should().Be(model.TransactionId);
            response.Messages.Should().BeEmpty();
        }

        [Fact]
        public void EmailResponse_WithNullMessages_Constructor()
        {
            // Arrange
            var model = new EmailResponseModel()
            {
                TransactionId = Guid.NewGuid(),
                Messages = null
            };

            // Act
            var response = new EmailResponse(model);

            // Assert
            response.TransactionId.Should().Be(model.TransactionId);
            response.Messages.Should().BeEmpty();
        }

        [Fact]
        public void EmailResponse_WithMessages_Constructor()
        {
            // Arrange
            var model = new EmailResponseModel()
            {
                TransactionId = Guid.NewGuid(),
                Messages = new List<MessageResponseModel>()
                {
                    new MessageResponseModel()
                    {
                        MessageId = Guid.NewGuid(),
                        Tag = "tag"
                    }
                }
            };

            // Act
            var response = new EmailResponse(model);

            // Assert
            response.TransactionId.Should().Be(model.TransactionId);
            response.Messages.Count().Should().Be(1);
            response.Messages.First().MessageId.Should().Be(model.Messages.First().MessageId);
            response.Messages.First().Tag.Should().Be(model.Messages.First().Tag);
        }
        #endregion

        #region MessageResponse
        [Fact]
        public void MessageResponse_Constructor()
        {
            // Arrange
            var model = new MessageResponseModel()
            {
                MessageId = Guid.NewGuid(),
                Tag = "tag"
            };

            // Act
            var response = new MessageResponse(model);

            // Assert
            response.MessageId.Should().Be(model.MessageId);
            response.Tag.Should().Be(model.Tag);
            response.To.Should().BeEmpty();
        }

        [Fact]
        public void MessageResponse_WithTo_Constructor()
        {
            // Arrange
            var model = new MessageResponseModel()
            {
                MessageId = Guid.NewGuid(),
                Tag = "tag",
                To = new[] { "one", "two" }
            };

            // Act
            var response = new MessageResponse(model);

            // Assert
            response.MessageId.Should().Be(model.MessageId);
            response.Tag.Should().Be(model.Tag);
            response.To.Count().Should().Be(2);
            response.To.First().Should().Be("one");
            response.To.Last().Should().Be("two");
        }

        [Fact]
        public void MessageResponse_ToNull_Constructor()
        {
            // Arrange
            var model = new MessageResponseModel()
            {
                MessageId = Guid.NewGuid(),
                Tag = "tag",
                To = null
            };

            // Act
            var response = new MessageResponse(model);

            // Assert
            response.MessageId.Should().Be(model.MessageId);
            response.Tag.Should().Be(model.Tag);
            response.To.Should().BeEmpty();
        }
        #endregion
        #endregion
    }
}
