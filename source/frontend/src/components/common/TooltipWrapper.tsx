import * as React from 'react';
import OverlayTrigger, { OverlayTriggerProps } from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';

/**
 * TooltipWrapper properties.
 * @interface ITooltipWrapperProps
 * @extends {Partial<OverlayTriggerProps>}
 */
interface ITooltipWrapperProps extends Partial<OverlayTriggerProps> {
  /**
   * The tooltip text to display when hovering over the component.
   *
   * @type {string}
   * @memberof ITooltipWrapperProps
   */
  toolTip?: string | React.ReactElement; // TODO: PSP-4389 Rename 'toolTip' with 'tooltip'
  /**
   * The tooltip element 'id'.
   *
   * @type {string}
   * @memberof ITooltipWrapperProps
   */
  toolTipId: string; // TODO: PSP-4389 Rename 'toolTipId' with 'tooltipId'
  className?: string;
}

/**
 * Wrap whatever you want in a tooltip.
 * @param props ITooltipWrapperProps
 */
export const TooltipWrapper: React.FunctionComponent<
  React.PropsWithChildren<ITooltipWrapperProps>
> = props => {
  return (
    <>
      <OverlayTrigger
        {...props}
        overlay={
          <Tooltip
            style={{ visibility: !props.toolTip ? 'hidden' : 'visible' }}
            id={props.toolTipId}
            className={props.className}
          >
            {props.toolTip}
          </Tooltip>
        }
      >
        {props.children ?? <></>}
      </OverlayTrigger>
    </>
  );
};

export default TooltipWrapper;
