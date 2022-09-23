import './TooltipIcon.scss';

import classNames from 'classnames';
import React from 'react';
import Overlay, { OverlayChildren } from 'react-bootstrap/Overlay';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import { FaInfoCircle } from 'react-icons/fa';

interface TooltipIconProps extends Partial<React.ComponentPropsWithRef<typeof Overlay>> {
  toolTip?: string;
  toolTipId: string;
  className?: string;
  customOverlay?: OverlayChildren;
  customToolTipIcon?: React.ComponentType<any> | JSX.Element;
}

const TooltipIcon: React.FunctionComponent<TooltipIconProps> = props => {
  const overlay =
    props.customOverlay === undefined
      ? ((<Tooltip id={props.toolTipId}>{props.toolTip}</Tooltip>) as OverlayChildren)
      : props.customOverlay;

  const icon =
    props.customToolTipIcon === undefined ? (
      <FaInfoCircle className={classNames('tooltip-icon', props.className)} />
    ) : (
      props.customToolTipIcon
    );

  return (
    <OverlayTrigger placement={props.placement} overlay={overlay}>
      <span data-testid="tooltip-icon" className="tooltip-icon" id={props.toolTipId}>
        {icon}
      </span>
    </OverlayTrigger>
  );
};

export default TooltipIcon;
