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
  tooltip?: string | React.ReactElement;
  /**
   * The tooltip element 'id'.
   *
   * @type {string}
   * @memberof ITooltipWrapperProps
   */
  tooltipId: string;
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
            style={{ visibility: !props.tooltip ? 'hidden' : 'visible' }}
            id={props.tooltipId}
            className={props.className}
          >
            {props.tooltip}
          </Tooltip>
        }
      >
        {props.children ?? <></>}
      </OverlayTrigger>
    </>
  );
};

export default TooltipWrapper;
