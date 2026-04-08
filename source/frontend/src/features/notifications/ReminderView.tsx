import { OverlayTriggerProps } from 'react-bootstrap';

import ReminderButton from '@/components/common/form/ReminderButton/ReminderButton';
import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';

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
   * The existing reminder for this notification type, if any. This is used to determine the initial state of the popover (e.g. whether to show "Save" vs "Update" and whether to pre-populate the date picker with an existing reminder date).
   */
  initialValues: NotificationFormModel;

  /**
   * Called with the chosen ISO date string when the user saves a reminder.
   */
  onReminderSaved?: (reminder: ApiGen_Concepts_Notification) => Promise<void>;

  /**
   * Called when the user removes a previously saved reminder.
   */
  onReminderRemoved?: (reminder: ApiGen_Concepts_Notification) => Promise<void>;

  /**
   * Placement of the popover relative to the trigger button.
   * Accepts any value supported by react-bootstrap's OverlayTrigger.
   * Defaults to `'bottom'`.
   */
  popoverPlacement?: OverlayTriggerProps['placement'];
}

export const ReminderView: React.FC<IReminderViewProps> = ({
  keyDate,
  keyDateLabel,
  initialValues,
  onReminderSaved,
  onReminderRemoved,
  popoverPlacement = 'right',
}) => {
  return (
    <ReminderButton
      keyDate={keyDate}
      keyDateLabel={keyDateLabel}
      savedReminderDate={initialValues.triggerDate}
      onReminderSaved={onReminderSaved}
      onReminderRemoved={onReminderRemoved}
      popoverPlacement={popoverPlacement}
    />
  );
};

export default ReminderView;
