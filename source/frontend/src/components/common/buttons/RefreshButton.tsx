import React from 'react';
import { TbRefresh } from 'react-icons/tb';

import TooltipWrapper from '../TooltipWrapper';
import { Button, ButtonProps } from './Button';

/**
 * Button displaying a refresh/recycle icon, used to reset form data.
 * @param param0
 */
const RefreshButton: React.FC<React.PropsWithChildren<ButtonProps>> = ({ ...props }) => {
  return (
    <TooltipWrapper tooltipId="btn-refresh-tooltip" tooltip="Refresh">
      <Button
        data-testid="refresh-button"
        title="refresh-button"
        id="refresh-button"
        type={props.type ?? 'submit'}
        className={props.className ?? 'primary'}
        {...props}
        icon={<TbRefresh size={24} />}
      ></Button>
    </TooltipWrapper>
  );
};

export default RefreshButton;
