import React from 'react';
import { FaUndo } from 'react-icons/fa';

import TooltipWrapper from '../TooltipWrapper';
import { Button, ButtonProps } from '.';

/**
 * Button displaying a reset/recycle icon, used to reset form data.
 * @param param0
 */
export const ResetButton: React.FC<React.PropsWithChildren<ButtonProps>> = ({ ...props }) => {
  return (
    <TooltipWrapper toolTipId="map-filter-reset-tooltip" toolTip="Reset Filter">
      <Button
        data-testid="reset-button"
        title="reset-button"
        id="reset-button"
        type={props.type ?? 'reset'}
        variant="info"
        {...props}
        icon={<FaUndo size={20} />}
      ></Button>
    </TooltipWrapper>
  );
};
