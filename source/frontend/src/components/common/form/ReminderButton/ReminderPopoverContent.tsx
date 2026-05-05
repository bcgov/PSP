import moment from 'moment';
import { FC, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import Button from 'react-bootstrap/Button';
import DatePicker from 'react-datepicker';
import { FaTrash } from 'react-icons/fa';
import { IoCheckmarkCircleOutline } from 'react-icons/io5';
import styled from 'styled-components';

import AlarmIcon from '@/assets/images/alarm-clock-solid-full.svg?react';
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
  existingReminderDate?: string | undefined;

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
export function buildReminderLabel(reminderDate: string, keyDate: string): JSX.Element | null {
  if (!exists(reminderDate) || !exists(keyDate)) {
    return null;
  }

  const diff = differenceInDays(keyDate, reminderDate);
  if (diff === 0) {
    return (
      <span>
        Reminder is set on the <b>same day</b> as the key date ({prettyFormatDate(keyDate)}).
      </span>
    );
  }
  const direction = diff > 0 ? 'before' : 'after';
  const absDiff = Math.abs(diff);
  return (
    <span>
      Reminder is set to{' '}
      <b>
        {absDiff} day{absDiff !== 1 ? 's' : ''}
      </b>{' '}
      {direction} {prettyFormatDate(keyDate)}.
    </span>
  );
}

/**
 * Body of the reminder popover.
 * It delegates save / remove actions to the parent via callbacks.
 */
export const ReminderPopoverContent: FC<IReminderPopoverContentProps> = ({
  keyDate,
  existingReminderDate,
  onSave,
  onRemove,
}) => {
  const [pickedDate, setPickedDate] = useState<string | null>(existingReminderDate ?? null);

  const label = buildReminderLabel(pickedDate, keyDate);

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
    <StyledGroup>
      <DatePicker
        showIcon
        selected={exists(pickedDate) ? moment(pickedDate, 'YYYY-MM-DD').toDate() : null}
        dateFormat="MMM dd, yyyy"
        placeholderText="MMM DD, YYYY"
        autoComplete="off"
        showYearDropdown
        scrollableYearDropdown
        yearDropdownItemNumber={10}
        wrapperClassName="d-block"
        className="form-control date-picker"
        onChange={handleChange}
      />
      {exists(label) && (
        <StyledReminderLabelText>
          <IoCheckmarkCircleOutline size="1.6rem" aria-hidden="true" />
          <span>{label}</span>
        </StyledReminderLabelText>
      )}
      <Row className="no-gutters">
        <Col xs="6">
          <StyledReminderButton
            variant="link"
            disabled={!pickedDate}
            onClick={handleSave}
            aria-label="Set reminder"
          >
            <AlarmIcon width="1.6rem" height="1.6rem" aria-hidden="true" fill="currentColor" /> Set
            Reminder
          </StyledReminderButton>
        </Col>
        <Col xs="6" className="d-flex justify-content-end">
          {exists(existingReminderDate) && (
            <StyledRemoveButton type="button" onClick={onRemove} aria-label="Remove reminder">
              <FaTrash size="1.6rem" />
            </StyledRemoveButton>
          )}
        </Col>
      </Row>
    </StyledGroup>
  );
};

export default ReminderPopoverContent;

// Move datepicker icon to the right and adjust padding on the input to prevent overlap.
const StyledGroup = styled.div`
  .react-datepicker-wrapper {
    .react-datepicker__view-calendar-icon input {
      padding: 0.8rem 1.2rem 0.8rem 1.2rem;
      border: 0.1rem solid #606060;
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
  display: flex;
  align-items: center;
  gap: 0.9rem;
  color: ${props => props.theme.css.calloutSuccessColor};
  background: ${props => props.theme.css.calloutSuccessBackground};
  border-radius: 0.5rem;
  padding: 0.8rem;
  margin-top: 0.8rem;
  line-height: 1.4;

  span {
    font-size: 0.875rem;
  }
`;

const StyledReminderButton = styled(Button)`
  && {
    display: flex;
    align-items: center;
    gap: 0.4rem;
    color: ${props => props.theme.css.linkColor};
    border: none;
    width: 100%;
    text-align: left;
    font-size: 1.25rem;
    font-weight: bold;
    margin-top: 0.5rem;

    &:hover,
    &:focus,
    &:active {
      color: ${props => props.theme.css.linkHoverColor};
      border: none;
      box-shadow: none;
    }

    &:disabled {
      opacity: 0.5;
    }
  }
`;

const StyledRemoveButton = styled.button`
  font-size: 0.78rem;
  color: ${props => props.theme.css.pimsRed100};
  background: none;
  border: none;
  padding: 0;
  margin-top: 6px;
  cursor: pointer;
  display: flex;
  text-align: center;

  &:hover {
    color: ${props => props.theme.css.pimsRed200};
  }
`;
