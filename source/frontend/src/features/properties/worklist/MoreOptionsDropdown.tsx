import React from 'react';
import { Dropdown } from 'react-bootstrap';
import { FaEllipsisH } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import TooltipWrapper from '@/components/common/TooltipWrapper';

export interface IMoreOptionsDropdownProps {
  /** Whether the "Clear All" action is enabled. When false, the option is grayed out and unclickable. */
  canClearAll?: boolean;

  /** Callback invoked when the "Clear All" option is clicked. */
  onClearAll: () => void;

  /** ARIA label for accessibility */
  ariaLabel?: string;
}

/**
 * A dropdown menu component with options to operate on the property worklist
 */
const MoreOptionsDropdown: React.FC<IMoreOptionsDropdownProps> = ({
  onClearAll,
  canClearAll = true,
  ariaLabel = 'More options',
}) => {
  return (
    <Dropdown alignRight>
      <TooltipWrapper tooltipId="more-options-tooltip" tooltip="More options...">
        <StyledToggleButton
          id="dropdown-ellipsis"
          variant="light"
          bsPrefix="btn"
          aria-label={ariaLabel}
        >
          <FaEllipsisH size={18} />
        </StyledToggleButton>
      </TooltipWrapper>

      <StyledDropdownMenu>
        <StyledDropdownItem
          aria-label="Clear list"
          disabled={!canClearAll}
          onClick={canClearAll ? onClearAll : undefined}
        >
          <MdClose size={20} className="mr-1" />
          Clear list
        </StyledDropdownItem>
      </StyledDropdownMenu>
    </Dropdown>
  );
};

export default MoreOptionsDropdown;

const StyledToggleButton = styled(Dropdown.Toggle)`
  border: none;
  padding: 0;
  display: flex;
  align-items: center;
  background-color: transparent !important;
  box-shadow: none !important;
  outline: none !important;

  padding: 0.5rem; /* adds 5px padding on all sides to improve hit-area */
  min-width: 3rem;
  min-height: 3rem;

  &:focus,
  &:active,
  &:focus:active {
    background-color: transparent !important;
    box-shadow: none !important;
    outline: none !important;
  }
`;

const StyledDropdownMenu = styled(Dropdown.Menu)`
  padding-top: 1rem;
  padding-bottom: 1rem;
`;

const StyledDropdownItem = styled(Dropdown.Item)`
  display: flex;
  align-items: center;

  color: ${props => props.theme.css.pimsBlue200} !important;
  font-weight: 700;
  font-size: 1.4rem;

  padding-top: 0.5rem;
  padding-bottom: 0.5rem;

  /* Adds gap between items */
  & + & {
    margin-top: 0.25rem; /* adjust this value as needed */
  }

  &:hover,
  &:focus {
    // Adding a 38% opacity to the background color (to match the mockups)
    background-color: ${props => props.theme.css.pimsBlue10 + '38'} !important;
  }

  &:disabled {
    color: #6c757d; /* Bootstrap's default muted text */
    pointer-events: none;
  }
`;
