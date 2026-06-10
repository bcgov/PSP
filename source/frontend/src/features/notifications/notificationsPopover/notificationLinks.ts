import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';
import { ApiGen_Concepts_NotificationOutput } from '@/models/api/generated/ApiGen_Concepts_NotificationOutput';
import { exists, isValidId } from '@/utils';

/**
 * Returns the parent notification from a notification_user_output row, or null when the
 * navigation chain (user_output → notification_user → notification) was not loaded.
 */
export const getParentNotification = (
  output: ApiGen_Concepts_NotificationOutput,
): ApiGen_Concepts_Notification | null => output?.notificationRecipient?.notification ?? null;

/**
 * Returns the in-app deep-link path for a notification, or null when the row has no
 * recognized parent file FK. Sub-entity FKs (take, agreement, consultation, etc.) are
 * routed to the parent file's main screen — sub-tab deep-linking can be layered on later.
 */
export const getNotificationDeepLink = (
  output: ApiGen_Concepts_NotificationOutput,
): string | null => {
  const notification = getParentNotification(output);
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

/**
 * Returns a short string identifying the file a notification is attached to. We prefer a
 * type-prefixed file number using the same conventions used in the list view screens
 * (e.g. "L-000-079" for leases). If no file FK is set, returns null.
 *
 * Note: the backend currently returns FK ids only. Looking up an actual `fileName`
 * (e.g. "Hammond, Richard and Sharon") requires hitting each file's API per row —
 * intentionally deferred. When a name is wanted, the caller can pass it via `fileName`.
 */
export const getNotificationFileLabel = (
  output: ApiGen_Concepts_NotificationOutput,
  fileName?: string | null,
): string | null => {
  if (fileName !== undefined && fileName !== null && fileName.trim() !== '') {
    return fileName;
  }

  const notification = getParentNotification(output);
  if (!exists(notification)) {
    return null;
  }

  if (isValidId(notification.acquisitionFileId)) {
    return `A-${formatFileNumber(notification.acquisitionFileId)}`;
  }
  if (isValidId(notification.dispositionFileId)) {
    return `D-${formatFileNumber(notification.dispositionFileId)}`;
  }
  if (isValidId(notification.leaseId)) {
    return `L-${formatFileNumber(notification.leaseId)}`;
  }
  if (isValidId(notification.researchFileId)) {
    return `R-${formatFileNumber(notification.researchFileId)}`;
  }
  if (isValidId(notification.managementFileId)) {
    return `M-${formatFileNumber(notification.managementFileId)}`;
  }
  return null;
};

/**
 * Returns the type label for a notification output, derived from its NotificationTypeCode.
 */
export const getNotificationTypeLabel = (output: ApiGen_Concepts_NotificationOutput): string => {
  const notification = getParentNotification(output);
  return notification?.notificationTypeCode ?? '';
};

/**
 * Returns the unread state for a notification output (NotificationReadDt is null).
 */
export const isUnread = (output: ApiGen_Concepts_NotificationOutput): boolean =>
  output.notificationReadDt === null || output.notificationReadDt === undefined;

const formatFileNumber = (id: number): string =>
  id
    .toString()
    .padStart(6, '0')
    .replace(/(\d{3})(\d{3})/, '$1-$2');
