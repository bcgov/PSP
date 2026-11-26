using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [ExcludeFromCodeCoverage]
    public class NotificationRepositoryTest
    {
        [Fact]
        public void Delete_ForUser_RemovesParentAndChildren_WhenLastUserIsRemoved()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<NotificationRepository>(user);
            var notification = CreateNotification(1);
            context.Add(notification);
            context.SaveChanges();

            // Act
            repository.Delete(notification.NotificationId, 1);
            repository.CommitTransaction();

            // Assert
            context.PimsNotifications.Should().BeEmpty();
            context.PimsNotificationUsers.Should().BeEmpty();
            context.PimsNotificationUserOutputs.Should().BeEmpty();
        }

        [Fact]
        public void Delete_ForUser_RemovesSelectedUserAndOutputs_AndPreservesOtherUser()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<NotificationRepository>(user);
            var notification = CreateNotification(1);
            notification.PimsNotificationUsers.Add(CreateNotificationUser(2));
            context.Add(notification);
            context.SaveChanges();
            var expectedRemainingOutputIds = notification.PimsNotificationUsers
                .Single(n => n.UserId == 2).PimsNotificationUserOutputs
                .Select(o => o.NotificationUserOutputId).ToList();

            // Act
            repository.Delete(notification.NotificationId, 1);
            repository.CommitTransaction();

            // Assert
            context.PimsNotifications.Select(n => n.NotificationId).Should().Equal(notification.NotificationId);
            context.PimsNotificationUsers.Select(n => n.UserId).Should().Equal(2);
            context.PimsNotificationUserOutputs.Select(o => o.NotificationUserOutputId)
                .Should().BeEquivalentTo(expectedRemainingOutputIds);
        }

        [Fact]
        public void Delete_ForAllUsers_RemovesParentAndChildren()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<NotificationRepository>(user);
            var notification = CreateNotification(1);
            notification.PimsNotificationUsers.Add(CreateNotificationUser(2));
            context.Add(notification);
            context.SaveChanges();

            // Act
            repository.Delete(notification.NotificationId);
            repository.CommitTransaction();

            // Assert
            context.PimsNotifications.Should().BeEmpty();
            context.PimsNotificationUsers.Should().BeEmpty();
            context.PimsNotificationUserOutputs.Should().BeEmpty();
        }

        [Fact]
        public void Delete_ForUnlinkedUser_PreservesNotificationAndChildren()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);
            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<NotificationRepository>(user);
            var notification = CreateNotification(1);
            context.Add(notification);
            context.SaveChanges();

            // Act
            repository.Delete(notification.NotificationId, 2);
            repository.CommitTransaction();

            // Assert
            context.PimsNotifications.Select(n => n.NotificationId).Should().Equal(notification.NotificationId);
            context.PimsNotificationUsers.Select(n => n.UserId).Should().Equal(1);
            context.PimsNotificationUserOutputs.Count().Should().Be(2);
        }


        private static PimsNotification CreateNotification(long userId)
        {
            var notification = new PimsNotification { NotificationTypeCode = "AGMT_POSNDT" };
            notification.PimsNotificationUsers.Add(CreateNotificationUser(userId));
            return notification;
        }

        private static PimsNotificationUser CreateNotificationUser(long userId)
        {
            var user = new PimsNotificationUser { UserId = userId };
            user.PimsNotificationUserOutputs.Add(new PimsNotificationUserOutput { NotificationOutputTypeCode = "EMAIL" });
            user.PimsNotificationUserOutputs.Add(new PimsNotificationUserOutput { NotificationOutputTypeCode = "PIMS" });
            return user;
        }
    }
}

