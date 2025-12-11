import React from 'react';
import { FaListUl } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

export type IWorklistControl = {
  /** number of items in the worklist. defaults to zero */
  itemCount?: number;
  /** whether the button should be displayed as active - ie. whether the worklist has active items */
  active?: boolean;
  /** set the slide out as open or closed */
  onToggle: () => void;
};

/**
 * Button to display the property worklist on the right-hand sidebar
 */
const WorklistControl: React.FC<IWorklistControl> = ({
  itemCount = 0,
  active = false,
  onToggle,
}) => {
  // determine display count (cap at 99+)
  let displayCount: string | number = itemCount;
  if (itemCount > 99) {
    displayCount = '99+';
  }

  return (
    <TooltipWrapper tooltipId="worklist-control-id" tooltip="Property Worklist">
      <BadgeWrapper>
        <WorklistButton
          id="worklistControlButton"
          variant="outline-secondary"
          $active={active}
          onClick={onToggle}
        >
          <WorklistIcon />
        </WorklistButton>

        {itemCount > 0 && <Badge>{displayCount}</Badge>}
      </BadgeWrapper>
    </TooltipWrapper>
  );
};

export default WorklistControl;

const WorklistIcon = styled(FaListUl)`
  font-size: 3rem;
`;

const BadgeWrapper = styled.div`
  position: relative;
  display: inline-block;
`;

const Badge = styled.div`
  position: absolute;
  top: -0.8rem;
  right: -0.8rem;
  background-color: ${({ theme }) => theme.bcTokens.iconsColorDanger};
  color: white;
  border-radius: 50%;
  min-width: 2.2rem;
  height: 2.2rem;
  font-size: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  padding: 0.2rem;
  line-height: 1;
  pointer-events: none; /* allow clicks through the badge */
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
