import clsx from 'classnames';
import L from 'leaflet';
import React, { useEffect } from 'react';
import { FaLayerGroup } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

import Control from '../Control';
import LayersTree from './LayersMenu';

const LayersContainer = styled.div`
  margin-right: -1rem;
  width: 34.1rem;
  min-height: 5.2rem;
  height: 50rem;
  max-height: 50rem;
  background-color: #fff;
  position: relative;
  border-radius: 0.4rem;
  box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
  z-index: 1000;

  &.closed {
    width: 0rem;
    height: 0rem;
  }
`;

const LayersHeader = styled.div`
  width: 100%;
  height: 8rem;
  background-color: ${({ theme }) => theme.css.primaryColor};
  color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-evenly;
  padding-top: 1.6rem;
  border-top-right-radius: 0.4rem;
  border-top-left-radius: 0.4rem;
`;

const LayersContent = styled.div`
  width: 100%;
  height: calc(100% - 8rem);
  padding: 1.6rem;

  &.open {
    overflow-y: scroll;
  }
`;

const LayersIcon = styled(FaLayerGroup)`
  font-size: 3rem;
`;

const Title = styled.p`
  font-size: 1.8rem;
  color: #ffffff;
  text-decoration: none solid rgb(255, 255, 255);
  line-height: 1.8rem;
  font-weight: bold;
`;

const ControlButton = styled(Button)`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    margin-left: -5.1rem;
    background-color: #fff;
    color: ${({ theme }) => theme.css.slideOutBlue};
    border-color: ${({ theme }) => theme.css.slideOutBlue};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
    &.open {
      border-top-right-radius: 0;
      border-bottom-right-radius: 0;
      margin-top: -5.2rem;
    }
  }
`;

export type ILayersControl = {
  /** whether the slide out is open or closed */
  open: boolean;
  /** set the slide out as open or closed */
  onToggle: () => void;
};

/**
 * Component to display the layers control on the map
 * @example ./LayersControl.md
 */
const LayersControl: React.FC<React.PropsWithChildren<ILayersControl>> = ({ open, onToggle }) => {
  useEffect(() => {
    const elem = L.DomUtil.get('layersContainer');
    if (elem) {
      L.DomEvent.on(elem, 'mousewheel', L.DomEvent.stopPropagation);
    }
  });

  return (
    <Control position="topright">
      <LayersContainer id="layersContainer" className={clsx({ closed: !open })}>
        {open && (
          <LayersHeader>
            <LayersIcon />
            <Title>Layers</Title>
          </LayersHeader>
        )}
        <TooltipWrapper toolTipId="layer-control-id" toolTip="Layer Controls">
          <ControlButton
            id="layersControlButton"
            variant="outline-secondary"
            onClick={onToggle}
            className={clsx({ open })}
          >
            <LayersIcon />
          </ControlButton>
        </TooltipWrapper>
        <LayersContent className={clsx({ open })}>
          <LayersTree />
        </LayersContent>
      </LayersContainer>
    </Control>
  );
};

export default LayersControl;
