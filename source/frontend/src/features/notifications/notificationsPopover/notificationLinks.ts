import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';
import { ApiGen_Concepts_NotificationInboxItem } from '@/models/api/generated/ApiGen_Concepts_NotificationInboxItem';
import { exists, isValidId } from '@/utils';

/**
 * Returns the parent notification from a notification_user_output row, or null when the
 * navigation chain (user_output -> notification_user -> notification) was not loaded.
 */
export const getParentNotification = (
  inboxItem: ApiGen_Concepts_NotificationInboxItem,
): ApiGen_Concepts_Notification | null => {
  return inboxItem?.notificationUser?.notification ?? null;
};

/**
 * Returns the in-app deep-link path for a notification, or null when the row has no
 * recognized parent file FK. Sub-entity FKs (take, agreement, consultation, etc.) are
 * routed to the parent file's main screen — sub-tab deep-linking can be layered on later.
 */
export const getNotificationDeepLink = (
  inboxItem: ApiGen_Concepts_NotificationInboxItem,
): string | null => {
  const notification = getParentNotification(inboxItem);
  if (!exists(notification)) {
    return null;
  }

  if (isValidId(notification.acquisitionFileId)) {
    return `/mapview/sidebar/acquisition/${notification.acquisitionFileId}`;
  }
  if (isValidId(notification.dispositionFileId)) {
    return `/mapview/sidebar/disposition/${notification.dispositionFileId}`;
  }
  if (isValidId(notification.leaseId)) {
    return `/mapview/sidebar/lease/${notification.leaseId}`;
  }
  if (isValidId(notification.researchFileId)) {
    return `/mapview/sidebar/research/${notification.researchFileId}`;
  }
  if (isValidId(notification.managementFileId)) {
    return `/mapview/sidebar/management/${notification.managementFileId}`;
  }
  return null;
};
