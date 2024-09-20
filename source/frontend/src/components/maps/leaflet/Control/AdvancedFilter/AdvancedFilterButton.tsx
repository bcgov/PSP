import React from 'react';
import { FaFilter } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { TooltipWrapper } from '@/components/common/TooltipWrapper';

import Control from '../Control';

export type IAdvanceFilterButtonProps = {
  /** whether the button should be displayed as active - ie. whether the map filter is active */
  active?: boolean;
  /** whether the advanced filter slide out is open or closed */
  onToggle: () => void;
};

/**
 * Component to launch the Advanced Map Filter Bar
 */
const AdvancedFilterButton: React.FC<IAdvanceFilterButtonProps> = ({
  active = false,
  onToggle,
}) => {
  return (
    <Control position="topright">
      <div className="w-100">
        <TooltipWrapper tooltipId="advanced-filter-button-id" tooltip="Advanced Map Filters">
          <ControlButton
            title="advanced-filter-button"
            variant="outline-secondary"
            onClick={onToggle}
            $active={active}
          >
            <FaFilter size="1.6em" />
          </ControlButton>
        </TooltipWrapper>
      </div>
    </Control>
  );
};

export default AdvancedFilterButton;

const ControlButton = styled(Button)<{ $active?: boolean }>`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    background-color: ${({ theme, $active }) =>
      $active ? theme.bcTokens.surfaceColorPrimaryButtonDefault : '#FFFFFF'};
    color: ${({ theme, $active }) =>
      $active ? '#FFFFFF' : theme.bcTokens.surfaceColorPrimaryButtonDefault};
    border-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
    transition: 1s;
    &.open {
      border-top-right-radius: 0;
      border-bottom-right-radius: 0;
    }
  }
`;
