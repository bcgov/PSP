import moment from 'moment';
import { FC, useState } from 'react';
import Button from 'react-bootstrap/Button';
import DatePicker from 'react-datepicker';
import { IoAlarm } from 'react-icons/io5';
import styled from 'styled-components';

import { differenceInDays, exists, prettyFormatDate } from '@/utils';

export interface IReminderPopoverContentProps {
  /**
   * ISO date string for the key date this reminder is anchored to.
   * e.g. "2027-03-19"
   */
  keyDate: string;

  /**
   * The currently saved reminder ISO date, or undefined if none has been set yet.
   */
  savedReminder?: string | undefined;

  /**
   * Called with the chosen ISO date string when the user clicks "Set Reminder".
   */
  onSave: (isoDate: string) => void;

  /**
   * Called when the user clicks "Remove reminder".
   */
  onRemove: () => void;
}

/**
 * Builds the contextual label that tells the user how many days before/after
 * the key date the reminder will fire.  Returns null until a date is chosen.
 */
export function buildReminderLabel(reminderDate: string, keyDate: string): string | null {
  if (!exists(reminderDate) || !exists(keyDate)) {
    return null;
  }

  const diff = differenceInDays(keyDate, reminderDate);
  if (diff === 0) {
    return `Reminder is set on the same day as the key date (${prettyFormatDate(keyDate)}).`;
  }
  const direction = diff > 0 ? 'before' : 'after';
  const absDiff = Math.abs(diff);
  return `Reminder is set to ${absDiff} day${
    absDiff !== 1 ? 's' : ''
  } ${direction} ${prettyFormatDate(keyDate)}.`;
}

/**
 * Body of the reminder popover.
 *
 * Owns the local picked-date state and renders:
 * - The key date for reference
 * - A date picker for the reminder trigger date
 * - A contextual label describing the offset between the reminder and the key date
 *   (only visible once a date has been selected)
 * - A "Set Reminder" button to save
 * - A "Remove reminder" link when a reminder is already saved
 *
 * Delegates save / remove actions to the parent via callbacks.
 */
export const ReminderPopoverContent: FC<IReminderPopoverContentProps> = ({
  keyDate,
  savedReminder,
  onSave,
  onRemove,
}) => {
  const [pickedDate, setPickedDate] = useState<string | null>(savedReminder ?? null);

  const label: string | null = buildReminderLabel(pickedDate, keyDate);

  const handleSave = (): void => {
    if (exists(pickedDate)) {
      onSave(pickedDate);
    }
  };

  const handleChange = (date: Date | null): void => {
    const isoDate = exists(date) ? moment(date).format('YYYY-MM-DD') : null;
    setPickedDate(isoDate);
  };

  return (
    <div>
      <StyledDatePicker
        showIcon
        selected={exists(pickedDate) ? moment(pickedDate, 'YYYY-MM-DD').toDate() : null}
        dateFormat="MMM dd, yyyy"
        placeholderText="MMM DD, YYYY"
        autoComplete="off"
        showYearDropdown
        scrollableYearDropdown
        yearDropdownItemNumber={10}
        wrapperClassName="d-block"
        onChange={handleChange}
      />
      {exists(label) && <StyledReminderLabelText>{label}</StyledReminderLabelText>}
      <StyledSetReminderButton size="sm" disabled={!pickedDate} onClick={handleSave}>
        <IoAlarm size={16} aria-hidden="true" /> Set Reminder
      </StyledSetReminderButton>
      {exists(savedReminder) && (
        <StyledRemoveButton type="button" onClick={onRemove}>
          Remove reminder
        </StyledRemoveButton>
      )}
    </div>
  );
};

export default ReminderPopoverContent;

// Move datepicker icon to the right and adjust padding on the input to prevent overlap.
const StyledDatePicker = styled(DatePicker)`
  .react-datepicker__view-calendar-icon {
    input {
      padding: 0.8rem 1.2rem 0.8rem 1.2rem;
    }

    .react-datepicker__calendar-icon {
      width: 2.4rem;
      height: 3rem;
      margin-top: 0.5rem;
      margin-right: 1.2rem;
      right: 0;
      fill: ${props => props.theme.css.linkColor};
      pointer-events: none;
    }
  }
`;

const StyledReminderLabelText = styled.div`
  font-size: 0.8rem;
  color: #2471a3;
  background: #eaf4fb;
  border: 1px solid #aed6f1;
  border-radius: 4px;
  padding: 6px 10px;
  margin-top: 8px;
  line-height: 1.4;
`;

const StyledSetReminderButton = styled(Button)`
  && {
    background: #1a5276;
    border-color: #1a5276;
    font-size: 0.85rem;
    padding: 5px 14px;
    margin-top: 10px;
    width: 100%;

    &:hover,
    &:focus {
      background: #154360;
      border-color: #154360;
    }

    &:disabled {
      background: #1a5276;
      border-color: #1a5276;
      opacity: 0.5;
    }
  }
`;

const StyledRemoveButton = styled.button`
  font-size: 0.78rem;
  color: #922b21;
  background: none;
  border: none;
  padding: 0;
  margin-top: 6px;
  text-decoration: underline;
  cursor: pointer;
  display: block;
  width: 100%;
  text-align: center;

  &:hover {
    color: #641e16;
  }
`;
