import noop from 'lodash/noop';
import React from 'react';
import { FaEllipsisH } from 'react-icons/fa'; // Horizontal three dots

import TooltipWrapper from '../TooltipWrapper';
import { LinkButton } from './LinkButton';

export interface IMoreOptionsButtonProps {
  /** Called when the button is clicked */
  onClick?: () => void;

  /** ARIA label for accessibility */
  ariaLabel?: string;

  /** Optional className for styling */
  className?: string;

  /** Disabled state */
  disabled?: boolean;
}

/**
 * A button that displays a horizontal ellipsis (...) icon, typically used for more options or overflow menus.
 */
export const MoreOptionsButton: React.FC<IMoreOptionsButtonProps> = ({
  onClick,
  ariaLabel = 'More options',
  className,
  disabled = false,
}) => {
  return (
    <TooltipWrapper tooltipId="see-more" tooltip="See more...">
      <LinkButton
        onClick={onClick ?? noop}
        data-testid="more-button"
        aria-label={ariaLabel}
        className={className}
        disabled={disabled}
      >
        <FaEllipsisH size={18} />
      </LinkButton>
    </TooltipWrapper>
  );
};
