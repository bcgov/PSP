import { useState } from 'react';
import { OverlayTriggerProps } from 'react-bootstrap';

import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';

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
   * Used to query for existing reminders of the same type when determining whether to show the "active" badge on the button.
   */
  notificationType: string;

  /**
   * The existing reminder for this notification type, if any. This is used to determine the initial state of the popover (e.g. whether to show "Save" vs "Update" and whether to pre-populate the date picker with an existing reminder date).
   */
  notification?: ApiGen_Concepts_Notification | null;

  /**
   * Placement of the popover relative to the trigger button.
   * Accepts any value supported by react-bootstrap's OverlayTrigger.
   * Defaults to `'bottom'`.
   */
  popoverPlacement?: OverlayTriggerProps['placement'];

  /**
   * The view component responsible for rendering the reminder UI.
   */
  View: React.FunctionComponent<IReminderViewProps>;

  /**
   * Called with the chosen ISO date string when the user saves a reminder.
   */
  onReminderSaved?: (reminder: ApiGen_Concepts_Notification) => Promise<void>;

  /**
   * Called when the user removes a previously saved reminder.
   */
  onReminderRemoved?: (reminder: ApiGen_Concepts_Notification) => Promise<void>;
}

const ReminderContainer: React.FC<IReminderContainerProps> = ({
  keyDate,
  keyDateLabel,
  popoverPlacement = 'right',
  notificationType,
  notification,
  View,
  onReminderSaved,
  onReminderRemoved,
}) => {
  const [savedReminder, setSavedReminder] = useState<ApiGen_Concepts_Notification | null>(null);

  // const fetchExistingReminder = useCallback(
  //   async (notificationType: string) => {
  //     try {
  //       const reminder = await searchReminder(notificationType);
  //       setSavedReminder(reminder);
  //     } catch (error) {
  //       setSavedReminder(null);
  //     }
  //   },
  //   [searchReminder],
  // );

  // Load existing reminder for the given notification type and pass it to the View component as `savedReminderDate`.
  // useEffect(() => {
  //   fetchExistingReminder(notificationType);
  // }, [notificationType, fetchExistingReminder]);

  return (
    <View
      keyDate={keyDate}
      keyDateLabel={keyDateLabel}
      popoverPlacement={popoverPlacement}
      savedReminderDate={savedReminder?.notificationTriggerDate ?? null}
      onReminderSaved={onReminderSaved}
      onReminderRemoved={onReminderRemoved}
    />
  );
};

export default ReminderContainer;
