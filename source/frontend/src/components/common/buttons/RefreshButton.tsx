import React from 'react';
import { TbRefresh } from 'react-icons/tb';

import TooltipWrapper from '../TooltipWrapper';
import { Button, ButtonProps } from './Button';

interface IRefreshButtonProps extends ButtonProps {
  /** set the text of the tooltip that appears on hover of the plus button */
  toolText: string;
  /** set the id of the tool tip use for on hover of the plus buttons */
  toolId: string;
  /** set the refresh button id */
  refreshButtonId?: string | null;
  /** set the refresh button id */
  refreshButtonTestId?: string | null;
  /** set the refresh button id */
  refreshButtonTitle?: string | null;
}

/**
 * Button displaying a refresh/recycle icon, used to reset form data.
 * @param param0
 */
const RefreshButton: React.FC<React.PropsWithChildren<IRefreshButtonProps>> = ({
  toolId,
  toolText,
  refreshButtonId,
  refreshButtonTestId,
  refreshButtonTitle,
  ...props
}) => {
  const refreshButtonIdValue = refreshButtonId ?? 'refresh-button';
  const refreshButtonTestIdValue = refreshButtonTestId ?? 'refresh-button';
  const refreshButtonTitleValue = refreshButtonTitle ?? 'refresh-button';

  return (
    <TooltipWrapper tooltipId={toolId} tooltip={toolText}>
      <Button
        id={refreshButtonIdValue}
        data-testid={refreshButtonTestIdValue}
        title={refreshButtonTitleValue}
        type={props.type ?? 'submit'}
        className={props.className ?? 'primary'}
        {...props}
        icon={<TbRefresh size={24} />}
      ></Button>
    </TooltipWrapper>
  );
};

export default RefreshButton;
