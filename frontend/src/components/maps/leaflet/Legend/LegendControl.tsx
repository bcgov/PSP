import './LegendControl.scss';

import variables from '_variables.module.scss';
import TooltipWrapper from 'components/common/TooltipWrapper';
import * as React from 'react';
import Button from 'react-bootstrap/Button';
import Overlay from 'react-bootstrap/Overlay';
import Tooltip from 'react-bootstrap/Tooltip';
import ClickAwayListener from 'react-click-away-listener';
import { FiMapPin } from 'react-icons/fi';
import styled from 'styled-components';

import Control from '../Control/Control';
import { Legend } from './Legend';

const LegendButton = styled(Button)`
  background-color: #ffffff !important;
  color: ${variables.darkVariantColor} !important;
  width: 4rem;
  height: 4rem;
  font-size: 2.5rem;
  display: flex;
  align-items: center;
`;

export const LegendControl: React.FC = () => {
  const [visible, setVisible] = React.useState<boolean>(false);
  const target = React.useRef(null);

  return (
    <Control position="topleft">
      <ClickAwayListener onClickAway={() => setVisible(false)}>
        <TooltipWrapper toolTipId="marker-legendId" toolTip={visible ? undefined : 'Marker legend'}>
          <LegendButton ref={target} onClick={() => setVisible(!visible)}>
            <FiMapPin />
          </LegendButton>
        </TooltipWrapper>
        <Overlay target={target.current} show={visible} placement="right">
          {(props: any) => {
            return (
              <Tooltip id="overlay-legend" {...props} show={`${visible}`} className="legendTooltip">
                <Legend />
              </Tooltip>
            );
          }}
        </Overlay>
      </ClickAwayListener>
    </Control>
  );
};
