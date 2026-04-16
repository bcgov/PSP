import { useCallback } from 'react';
import { OverlayTriggerProps } from 'react-bootstrap';

import { useNotificationRepository } from '@/hooks/repositories/useNotificationRepository';
import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';
import { exists, isValidId } from '@/utils';

import { INotificationSourceOptions, NotificationFormModel } from './models/NotificationFormModel';
import { IReminderViewProps } from './ReminderView';

export interface IReminderContainerProps {
  /**
   * ISO date string for the key date this reminder is anchored to.
   * e.g. "2027-03-19"
   */
  keyDate: string;

  /**
   * Human-readable label shown in the popover header.
   * e.g. "Expiry Date"
   */
  keyDateLabel: string;

  /**
   * The type of notification this reminder is associated with.
   * Used when creating a new notification record.
   */
  notificationType: string;

  // Add the relevant foreign-keys when embedding this container next to a supported key date.
  notificationSourceOptions: INotificationSourceOptions;

  /**
   * The existing reminder for this notification type, if any. Used to seed the
   * initial state and to supply the row version for update/delete calls.
   */
  notification?: ApiGen_Concepts_Notification | null;

  /**
   * Placement of the popover relative to the trigger button.
   * Accepts any value supported by react-bootstrap's OverlayTrigger.
   * Defaults to `'right'`.
   */
  popoverPlacement?: OverlayTriggerProps['placement'];

  /**
   * The view component responsible for rendering the reminder UI.
   */
  View: React.FunctionComponent<IReminderViewProps>;

  /**
   * Optional callback invoked after a reminder is successfully saved (create or update).
   * Useful for parent components that need to react (e.g. refresh a list).
   */
  onReminderSaved?: (notification: ApiGen_Concepts_Notification) => Promise<void>;

  /**
   * Optional callback invoked after a previously saved reminder is deleted.
   */
  onReminderRemoved?: () => Promise<void>;
}

/**
 * Container for the Save Reminder feature.
 */
const ReminderContainer: React.FC<IReminderContainerProps> = ({
  keyDate,
  keyDateLabel,
  popoverPlacement = 'right',
  notificationType,
  notificationSourceOptions,
  notification,
  View,
  onReminderSaved,
  onReminderRemoved,
}) => {
  const {
    addNotification: { execute: addNotification },
    updateNotification: { execute: updateNotification },
    deleteNotification: { execute: deleteNotification },
  } = useNotificationRepository();

  const handleSave = useCallback(
    async (values: NotificationFormModel): Promise<void> => {
      const apiNotification = values.toApi();

      // Create new reminder or update an existing one.
      const saved = isValidId(apiNotification.notificationId)
        ? await updateNotification(apiNotification)
        : await addNotification(apiNotification);

      if (exists(saved) && isValidId(saved?.notificationId)) {
        await onReminderSaved?.(saved);
      }
    },
    [addNotification, updateNotification, onReminderSaved],
  );

  const handleRemove = useCallback(
    async (notificationId: number): Promise<void> => {
      if (!isValidId(notificationId)) {
        return;
      }

      await deleteNotification(notificationId);
      await onReminderRemoved?.();
    },
    [deleteNotification, onReminderRemoved],
  );

  return (
    <View
      keyDate={keyDate}
      keyDateLabel={keyDateLabel}
      popoverPlacement={popoverPlacement}
      notification={
        exists(notification)
          ? NotificationFormModel.fromApi(notification)
          : NotificationFormModel.createEmpty(notificationType, notificationSourceOptions)
      }
      onReminderSaved={handleSave}
      onReminderRemoved={handleRemove}
    />
  );
};

export default ReminderContainer;
