import { useCallback, useEffect, useMemo } from 'react';
import { OverlayTriggerProps } from 'react-bootstrap';

import { useNotificationRepository } from '@/hooks/repositories/useNotificationRepository';
import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';
import { Api_NotificationSearchCriteria } from '@/models/api/NotificationSearchCriteria';
import { exists, firstOrNull, isValidId } from '@/utils';

import { INotificationSource, NotificationFormModel } from './models/NotificationFormModel';
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

  /**
   * The relevant foreign-keys when embedding this container next to a supported key date.
   */
  notificationSource: INotificationSource;

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
  notificationSource,
  View,
  onReminderSaved,
  onReminderRemoved,
}) => {
  const {
    addNotification: { execute: addNotification },
    updateNotification: { execute: updateNotification },
    deleteNotification: { execute: deleteNotification },
    searchNotifications: { execute: searchNotifications, response: existingNotifications },
  } = useNotificationRepository();

  const fetchNotifications = useCallback(
    async (notificationType: string, notificationSource: INotificationSource): Promise<void> => {
      const criteria: Api_NotificationSearchCriteria = {
        type: notificationType,
        acquisitionFileId: notificationSource.acquisitionFileId ?? undefined,
        dispositionFileId: notificationSource.dispositionFileId ?? undefined,
        researchFileId: notificationSource.researchFileId ?? undefined,
        managementFileId: notificationSource.managementFileId ?? undefined,
        leaseId: notificationSource.leaseId ?? undefined,
        takeId: notificationSource.takeId ?? undefined,
        insuranceId: notificationSource.insuranceId ?? undefined,
        leaseConsultationId: notificationSource.leaseConsultationId ?? undefined,
        noticeOfClaimId: notificationSource.noticeOfClaimId ?? undefined,
        leaseRenewalId: notificationSource.leaseRenewalId ?? undefined,
        expropOwnerHistoryId: notificationSource.expropOwnerHistoryId ?? undefined,
        agreementId: notificationSource.agreementId ?? undefined,
      };
      await searchNotifications(criteria);
    },
    [searchNotifications],
  );

  const handleSave = useCallback(
    async (values: NotificationFormModel): Promise<void> => {
      const apiNotification = values.toApi();

      // Create new reminder or update an existing one.
      const saved = isValidId(apiNotification.notificationId)
        ? await updateNotification(apiNotification)
        : await addNotification(apiNotification);

      if (exists(saved) && isValidId(saved?.notificationId)) {
        await onReminderSaved?.(saved);
        await fetchNotifications(notificationType, notificationSource);
      }
    },
    [
      updateNotification,
      addNotification,
      onReminderSaved,
      fetchNotifications,
      notificationType,
      notificationSource,
    ],
  );

  const handleRemove = useCallback(
    async (notificationId: number): Promise<void> => {
      if (!isValidId(notificationId)) {
        return;
      }

      await deleteNotification(notificationId);
      await onReminderRemoved?.();
      await fetchNotifications(notificationType, notificationSource);
    },
    [
      deleteNotification,
      fetchNotifications,
      notificationSource,
      notificationType,
      onReminderRemoved,
    ],
  );

  useEffect(() => {
    fetchNotifications(notificationType, notificationSource);
  }, [fetchNotifications, notificationSource, notificationType]);

  const notification = useMemo(() => firstOrNull(existingNotifications), [existingNotifications]);

  return (
    <View
      keyDate={keyDate}
      keyDateLabel={keyDateLabel}
      popoverPlacement={popoverPlacement}
      notification={
        exists(notification)
          ? NotificationFormModel.fromApi(notification)
          : NotificationFormModel.createEmpty(notificationType, notificationSource)
      }
      onReminderSaved={handleSave}
      onReminderRemoved={handleRemove}
    />
  );
};

export default ReminderContainer;
