import cx from 'classnames';
import React from 'react';
import { FaFilter } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { TooltipWrapper } from '@/components/common/TooltipWrapper';

import Control from '../Control';

export type IAdvanceFilterButtonProps = {
  /** whether the advanced filter slide out is open or closed */
  open?: boolean;
  onOpen?: () => void;
  onClose?: () => void;
};

/**
 * Component to launch the Advanced Map Filter Bar
 */
const AdvancedFilterButton: React.FC<IAdvanceFilterButtonProps> = ({
  open = false,
  onOpen,
  onClose,
}) => {
  const onButtonClick = () => {
    if (!open) {
      onOpen?.();
    } else {
      onClose?.();
    }
  };

  return (
    <Control position="topright">
      <div className="w-100">
        <TooltipWrapper toolTipId="advanced-filter-button-id" toolTip="Advanced Map Filters">
          <ControlButton
            title="advanced-filter-button"
            variant="outline-secondary"
            onClick={onButtonClick}
            className={cx({ open })}
          >
            <FaFilter size="1.6em" />
          </ControlButton>
        </TooltipWrapper>
      </div>
    </Control>
  );
};

export default AdvancedFilterButton;

const ControlButton = styled(Button)`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    background-color: #fff;
    color: ${({ theme }) => theme.css.slideOutBlue};
    border-color: ${({ theme }) => theme.css.slideOutBlue};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
    transition: 1s;
    &.open {
      border-top-right-radius: 0;
      border-bottom-right-radius: 0;
    }
  }
`;
