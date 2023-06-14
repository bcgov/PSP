import './LegendControl.scss';

import React from 'react';
import Overlay from 'react-bootstrap/Overlay';
import Tooltip from 'react-bootstrap/Tooltip';
import ClickAwayListener from 'react-click-away-listener';
import { FiMapPin } from 'react-icons/fi';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

import Control from '../Control';
import { Legend } from './Legend';

const LegendButton = styled(Button as any)`
  &&.btn {
    background-color: #ffffff;
    color: ${({ theme }) => theme.css.darkVariantColor};
    width: 4rem;
    height: 4rem;
    display: flex;
    align-items: center;
  }
`;

export const LegendControl: React.FC<React.PropsWithChildren<unknown>> = () => {
  const [visible, setVisible] = React.useState<boolean>(false);
  const target = React.useRef(null);

  return (
    <Control position="topleft">
      <ClickAwayListener onClickAway={() => setVisible(false)}>
        <div>
          <TooltipWrapper
            toolTipId="marker-legendId"
            toolTip={visible ? undefined : 'Marker legend'}
          >
            <LegendButton ref={target} onClick={() => setVisible(!visible)}>
              <FiMapPin />
            </LegendButton>
          </TooltipWrapper>
          <Overlay target={target.current} show={visible} placement="right">
            {(props: any) => {
              return (
                <Tooltip
                  id="overlay-legend"
                  {...props}
                  show={`${visible}`}
                  className="legendTooltip"
                >
                  <Legend />
                </Tooltip>
              );
            }}
          </Overlay>
        </div>
      </ClickAwayListener>
    </Control>
  );
};
