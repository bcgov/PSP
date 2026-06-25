import { ApiGen_CodeTypes_NotificationTypes } from '@/models/api/generated/ApiGen_CodeTypes_NotificationTypes';
import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';
import { ApiGen_Concepts_NotificationInboxItem } from '@/models/api/generated/ApiGen_Concepts_NotificationInboxItem';
import { exists, isValidId } from '@/utils';

import { DeepLinkGenerator } from './DeepLinkGenerator';

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

  const notificationType = notification.notificationTypeCode ?? '';

  // TODO: Add more deep-links as more notification types are implemented.
  switch (notificationType) {
    case ApiGen_CodeTypes_NotificationTypes.L_RENEWAL:
      return isValidId(notification.leaseId)
        ? DeepLinkGenerator.showFile('lease', notification.leaseId)
        : null;
    default:
      return null;
  }
};
