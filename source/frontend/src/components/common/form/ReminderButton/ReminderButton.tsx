import { FC, useCallback, useState } from 'react';
import OverlayTrigger, { OverlayTriggerProps } from 'react-bootstrap/OverlayTrigger';
import Popover from 'react-bootstrap/Popover';
import styled from 'styled-components';

import AlarmIcon from '@/assets/images/alarm-clock-solid-full.svg?react';
import { isValidIsoDateTime, prettyFormatDate } from '@/utils';

import { ReminderPopoverContent } from './ReminderPopoverContent';

export interface IReminderButtonProps {
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
   * The ISO date string for the saved reminder, or null if none has been set.
   */
  existingReminderDate?: string | undefined;

  /**
   * Called with the chosen ISO date string when the user saves a reminder.
   */
  onReminderSaved?: (isoDate: string) => Promise<void>;

  /**
   * Called when the user removes a previously saved reminder.
   */
  onReminderRemoved?: () => Promise<void>;

  /**
   * Placement of the popover relative to the trigger button.
   * Accepts any value supported by react-bootstrap's OverlayTrigger.
   * Defaults to `'bottom'`.
   */
  popoverPlacement?: OverlayTriggerProps['placement'];
}

/**
 * A button that opens a popover allowing the user to set or remove a reminder for a key date on PIMS.
 *
 * - Clicking outside the popover closes it and discards unsaved changes.
 * - When `existingReminderDate` is populated the button visually inverts (white icon, navy background)
 *   to indicate a prior notification exists for this user / file / entity.
 * - A small red dot badge appears on the button while a reminder is active.
 */
export const ReminderButton: FC<IReminderButtonProps> = ({
  keyDate,
  keyDateLabel,
  existingReminderDate = undefined,
  onReminderSaved,
  onReminderRemoved,
  popoverPlacement = 'right',
}) => {
  const [show, setShow] = useState<boolean>(false);
  const isReminderSet = isValidIsoDateTime(existingReminderDate);

  const handleSave = useCallback(
    async (isoDate: string): Promise<void> => {
      setShow(false);
      await onReminderSaved?.(isoDate);
    },
    [onReminderSaved],
  );

  const handleRemove = useCallback(async (): Promise<void> => {
    setShow(false);
    await onReminderRemoved?.();
  }, [onReminderRemoved]);

  const popover = (
    <StyledPopover id={`reminder-popover-${keyDateLabel}`}>
      <Popover.Title as="h3">{`Set Reminder — ${keyDateLabel}`}</Popover.Title>
      <Popover.Content>
        <ReminderPopoverContent
          keyDate={keyDate}
          existingReminderDate={existingReminderDate}
          onSave={handleSave}
          onRemove={handleRemove}
        />
      </Popover.Content>
    </StyledPopover>
  );

  return (
    <OverlayTrigger
      trigger="click"
      placement={popoverPlacement}
      overlay={popover}
      show={show}
      onToggle={(nextShow: boolean) => setShow(nextShow)}
      rootClose
      rootCloseEvent="mousedown"
    >
      <StyledAlarmButton
        $isSet={isReminderSet}
        type="button"
        title={
          isReminderSet
            ? `Reminder set for ${prettyFormatDate(existingReminderDate)}`
            : `Set reminder for ${keyDateLabel}`
        }
        aria-label={`Reminder for ${keyDateLabel}`}
      >
        <AlarmIcon
          width="1.6rem"
          height="1.6rem"
          aria-hidden="true"
          style={{ opacity: isReminderSet ? 1 : 0.85 }}
        />

        {isReminderSet && <StyledBadgeDot aria-hidden="true" />}
      </StyledAlarmButton>
    </OverlayTrigger>
  );
};

export default ReminderButton;

interface IStyledAlarmButtonProps {
  /** Transient prop — prefixed with $ so styled-components does not forward it to the DOM. */
  $isSet: boolean;
}

const StyledAlarmButton = styled.button<IStyledAlarmButtonProps>`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 3.2rem;
  height: 3.2rem;
  border-radius: 0.4rem;
  border: 0.15rem solid #1a5276;
  background: ${({ $isSet, theme }) =>
    $isSet ? theme.bcTokens.surfaceColorPrimaryButtonDefault : theme.css.pimsWhite};
  color: ${({ $isSet, theme }) =>
    $isSet
      ? theme.bcTokens.typographyColorPrimaryInvert
      : theme.bcTokens.surfaceColorPrimaryButtonDefault};
  cursor: pointer;
  transition: background 0.18s ease, border-color 0.18s ease, color 0.18s ease;
  padding: 0;
  position: relative;
  flex-shrink: 0;

  &:hover,
  &:active,
  &:focus {
    background: ${({ $isSet, theme }) =>
      $isSet ? theme.bcTokens.surfaceColorPrimaryButtonHover : '#eaf0f6'};
    border-color: ${({ $isSet, theme }) =>
      $isSet ? 'transparent' : theme.bcTokens.surfaceColorPrimaryButtonDefault};
  }
`;

const StyledBadgeDot = styled.span`
  position: absolute;
  top: -0.4rem;
  right: -0.4rem;
  width: 1rem;
  height: 1rem;
  border-radius: 50%;
  background-color: ${({ theme }) => theme.bcTokens.iconsColorDanger};
  border: 2px solid #fff;
  pointer-events: none;
`;

const StyledPopover = styled(Popover)`
  min-width: 300px;
  box-shadow: 0 4px 18px rgba(0, 0, 0, 0.14);
  border: 1px solid #c8d6e0;
  border-radius: 6px;
  font-family: 'Segoe UI', Arial, sans-serif;
`;
