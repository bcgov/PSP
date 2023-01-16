import './TooltipIcon.scss';

import classNames from 'classnames';
import React from 'react';
import Overlay, { OverlayChildren } from 'react-bootstrap/Overlay';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import { FaInfoCircle } from 'react-icons/fa';

interface TooltipIconProps extends Partial<React.ComponentPropsWithRef<typeof Overlay>> {
  toolTip?: React.ReactNode;
  toolTipId: string;
  className?: string;
  innerClassName?: string;
  customOverlay?: OverlayChildren;
  customToolTipIcon?: React.ReactNode;
}

const TooltipIcon: React.FunctionComponent<React.PropsWithChildren<TooltipIconProps>> = props => {
  const overlay =
    props.customOverlay === undefined
      ? ((
          <Tooltip id={props.toolTipId} className={props.className}>
            {props.toolTip}
          </Tooltip>
        ) as OverlayChildren)
      : props.customOverlay;

  const icon =
    props.customToolTipIcon === undefined ? (
      <FaInfoCircle className={classNames('tooltip-icon', props.innerClassName)} />
    ) : (
      props.customToolTipIcon
    );

  return (
    <OverlayTrigger placement={props.placement} overlay={overlay}>
      <span
        data-testid={`tooltip-icon-${props.toolTipId}`}
        className="tooltip-icon"
        id={props.toolTipId}
      >
        {icon}
      </span>
    </OverlayTrigger>
  );
};

export default TooltipIcon;
