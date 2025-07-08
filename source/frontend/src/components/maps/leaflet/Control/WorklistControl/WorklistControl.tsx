import React from 'react';
import { FaListUl } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

import Control from '../Control';

export type IWorklistControl = {
  /** set the slide out as open or closed */
  onToggle: () => void;
};

/**
 * Button to display the property worklist on the right-hand sidebar
 */
const WorklistControl: React.FC<IWorklistControl> = ({ onToggle }) => {
  return (
    <Control position="topright">
      <TooltipWrapper tooltipId="worklist-control-id" tooltip="Property Worklist">
        <WorklistButton id="worklistControlButton" variant="outline-secondary" onClick={onToggle}>
          <WorklistIcon />
        </WorklistButton>
      </TooltipWrapper>
    </Control>
  );
};

export default WorklistControl;

const WorklistIcon = styled(FaListUl)`
  font-size: 3rem;
`;

const WorklistButton = styled(Button)`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    margin-left: -5.1rem;
    background-color: #fff;
    color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    border-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
  }
`;
