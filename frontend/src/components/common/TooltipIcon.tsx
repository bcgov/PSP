import './TooltipIcon.scss';

import classNames from 'classnames';
import React from 'react';
import Overlay from 'react-bootstrap/Overlay';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import { FaInfoCircle } from 'react-icons/fa';

interface TooltipIconProps extends Partial<React.ComponentPropsWithRef<typeof Overlay>> {
  toolTip?: string;
  toolTipId: string;
  className?: string;
}

const TooltipIcon = (props: TooltipIconProps) => (
  <OverlayTrigger
    placement={props.placement}
    overlay={<Tooltip id={props.toolTipId}>{props.toolTip}</Tooltip>}
  >
    <span data-testid="tooltip-icon" className="tooltip-icon">
      <FaInfoCircle className={classNames('tooltip-icon', props.className)} />
    </span>
  </OverlayTrigger>
);

export default TooltipIcon;
