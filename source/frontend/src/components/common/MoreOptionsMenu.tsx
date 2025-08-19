// MoreOptionsMenu.tsx
import React from 'react';
import { Dropdown } from 'react-bootstrap';
import { FaEllipsisH } from 'react-icons/fa'; // Horizontal three dots
import styled, { css } from 'styled-components';

import TooltipIcon from './TooltipIcon';
import TooltipWrapper from './TooltipWrapper';

export interface MenuOption {
  label: string;
  onClick?: () => void;
  disabled?: boolean;
  icon?: React.ReactNode; // optional leading icon
  tooltip?: string; // optional tooltip for disabled items
  separator?: boolean; // If true, a separator (Dropdown.Divider) will be rendered before this item.
}

export interface IMoreOptionsMenuProps {
  /** ARIA label for accessibility */
  ariaLabel?: string;
  /** If true, aligns the dropdown menu to the right side of the toggle button */
  alignRight?: boolean;
  /** Controls the color variant of the toggle icon for use on light or dark backgrounds. */
  variant?: 'light' | 'dark';
  /** List of menu options */
  options: MenuOption[];
}

/**
 * A reusable "More Options" dropdown menu component.
 *
 * Supports:
 * - Icons per menu option
 * - Disabled menu options with explanatory tooltips
 * - Optional separators before menu items (never on the first item)
 * - Toggle button variant for light or dark backgrounds
 */
const MoreOptionsMenu: React.FC<IMoreOptionsMenuProps> = ({
  options,
  ariaLabel = 'More options',
  alignRight = true,
  variant = 'light',
}) => {
  return (
    <Dropdown alignRight={alignRight}>
      <TooltipWrapper tooltipId="more-options-tooltip" tooltip="More options...">
        <StyledToggleButton
          id="dropdown-ellipsis"
          variant="link"
          $toggleVariant={variant}
          aria-label={ariaLabel}
        >
          <FaEllipsisH size={18} />
        </StyledToggleButton>
      </TooltipWrapper>

      <StyledDropdownMenu data-testid="more-options-menu">
        {options.map((opt, index) => {
          // Disabled option with optional tooltip
          if (opt.disabled) {
            return (
              <React.Fragment key={index}>
                {opt.separator && index > 0 && <Dropdown.Divider key={`divider-${index}`} />}
                <StyledDropdownItemText>
                  {opt.icon && <span>{opt.icon}</span>}
                  {opt.label}
                  {opt.tooltip && (
                    <TooltipIcon toolTipId={`tooltip-${index}`} toolTip={opt.tooltip} />
                  )}
                </StyledDropdownItemText>
              </React.Fragment>
            );
          }

          // Normal clickable item
          return (
            <React.Fragment key={index}>
              {opt.separator && index > 0 && <Dropdown.Divider key={`divider-${index}`} />}
              <StyledDropdownItem
                aria-label={opt.label}
                onClick={e => {
                  e.preventDefault();
                  e.stopPropagation();
                  opt.onClick();
                }}
              >
                {opt.icon && <span>{opt.icon}</span>}
                {opt.label}
              </StyledDropdownItem>
            </React.Fragment>
          );
        })}
      </StyledDropdownMenu>
    </Dropdown>
  );
};

export default MoreOptionsMenu;

/** Styled toggle without the Bootstrap caret/chevron */
const StyledToggleButton = styled(Dropdown.Toggle)<{ $toggleVariant: 'light' | 'dark' }>`
  &::after {
    display: none !important; /* hide caret locally */
  }

  border: none;
  padding: 0;
  display: flex;
  align-items: center;
  background-color: transparent !important;
  box-shadow: none !important;
  outline: none !important;

  svg {
    ${({ $toggleVariant }) =>
      $toggleVariant === 'light'
        ? css`
            color: ${props => props.theme.css.pimsBlue200};
          `
        : css`
            color: ${props => props.theme.bcTokens.surfaceColorFormsDefault};
          `}
  }

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
  gap: 0.5rem; /* adds space between icon and text */

  color: ${props => props.theme.css.pimsBlue200} !important;
  font-weight: 700;
  font-size: 1.4rem;

  padding-top: 0.5rem;
  padding-bottom: 0.5rem;

  /* Adds gap between items */
  & + & {
    margin-top: 0.25rem;
  }

  &:hover,
  &:focus {
    // Adding a 38% opacity to the background color (to match the mockups)
    background-color: ${props => props.theme.css.pimsBlue10 + '38'} !important;
  }
`;

const StyledDropdownItemText = styled(Dropdown.ItemText)`
  display: flex;
  align-items: center;
  gap: 0.5rem; /* adds space between icon and text */
  font-size: 1.4rem;
  color: ${props => props.theme.css.pimsGrey80} !important;
`;
