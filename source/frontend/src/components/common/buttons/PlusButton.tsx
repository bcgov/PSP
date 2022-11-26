import React from 'react';
import { FaPlus } from 'react-icons/fa';

import { StyledAddButton } from '../styles';
import TooltipWrapper from '../TooltipWrapper';
import { ButtonProps } from '.';

interface IPlusButtonProps extends ButtonProps {
  /** set the text of the tooltip that appears on hover of the plus button */
  toolText: string;
  /** set the id of the tool tip use for on hover of the plus buttons */
  toolId: string;
}

/**
 * PlusButton displaying a plus button, used to add new items.
 * @param param0
 */
export const PlusButton: React.FC<React.PropsWithChildren<IPlusButtonProps>> = ({
  toolId,
  toolText,
  ...props
}) => {
  return (
    <TooltipWrapper toolTipId={toolId} toolTip={toolText}>
      <StyledAddButton className="primary" {...props} icon={<FaPlus size={20} />} />
    </TooltipWrapper>
  );
};
