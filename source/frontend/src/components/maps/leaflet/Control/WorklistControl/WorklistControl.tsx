import React from 'react';
import { FaListUl } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

export type IWorklistControl = {
  /** whether the button should be displayed as active - ie. whether the worklist has active items */
  active?: boolean;
  /** set the slide out as open or closed */
  onToggle: () => void;
};

/**
 * Button to display the property worklist on the right-hand sidebar
 */
const WorklistControl: React.FC<IWorklistControl> = ({ active = false, onToggle }) => {
  return (
    <TooltipWrapper tooltipId="worklist-control-id" tooltip="Property Worklist">
      <WorklistButton
        id="worklistControlButton"
        variant="outline-secondary"
        $active={active}
        onClick={onToggle}
      >
        <WorklistIcon />
      </WorklistButton>
    </TooltipWrapper>
  );
};

export default WorklistControl;

const WorklistIcon = styled(FaListUl)`
  font-size: 3rem;
`;

const WorklistButton = styled(Button)<{ $active?: boolean }>`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    background-color: ${({ theme, $active }) =>
      $active ? theme.bcTokens.surfaceColorPrimaryButtonDefault : '#FFFFFF'};
    color: ${({ theme, $active }) =>
      $active ? '#FFFFFF' : theme.bcTokens.surfaceColorPrimaryButtonDefault};
    border-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);

    &:hover {
      opacity: 1;
    }
  }
`;
