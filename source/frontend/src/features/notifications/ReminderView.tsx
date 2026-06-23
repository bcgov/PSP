import { OverlayTriggerProps } from 'react-bootstrap';

import ReminderButton from '@/components/common/form/ReminderButton/ReminderButton';
import { isValidId, isValidIsoDateTime } from '@/utils/utils';

import { NotificationFormModel } from './models/NotificationFormModel';

export interface IReminderViewProps {
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
   * The current reminder state, seeded by the container from the persisted notification.
   */
  notification: NotificationFormModel;

  /**
   */
  onReminderSaved: (values: NotificationFormModel) => Promise<void>;

  /**
   * Called when the user removes a previously saved reminder.
   */
  onReminderRemoved: (notificationId: number) => Promise<void>;

  /**
   * Placement of the popover relative to the trigger button.
   * Defaults to `'right'`.
   */
  popoverPlacement?: OverlayTriggerProps['placement'];
}

export const ReminderView: React.FC<IReminderViewProps> = ({
  keyDate,
  keyDateLabel,
  notification,
  onReminderSaved,
  onReminderRemoved,
  popoverPlacement = 'right',
}) => {
  const handleSave = async (isoDate: string): Promise<void> => {
    notification.triggerDate = isoDate;
    await onReminderSaved(notification);
  };

  const handleRemove = async (): Promise<void> => {
    if (isValidId(notification.id)) {
      await onReminderRemoved(notification.id);
    }
  };

  return (
    <ReminderButton
      keyDate={keyDate}
      keyDateLabel={keyDateLabel}
      existingReminderDate={
        isValidIsoDateTime(notification.triggerDate) ? notification.triggerDate : undefined
      }
      onReminderSaved={handleSave}
      onReminderRemoved={handleRemove}
      popoverPlacement={popoverPlacement}
    />
  );
};

export default ReminderView;
