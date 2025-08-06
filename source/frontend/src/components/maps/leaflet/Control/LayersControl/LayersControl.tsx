import L from 'leaflet';
import React, { useEffect } from 'react';
import { FaLayerGroup } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';

const LayersIcon = styled(FaLayerGroup)`
  font-size: 3rem;
`;

const ControlButton = styled(Button)`
  &.btn {
    width: 5.2rem;
    height: 5.2rem;
    background-color: #fff;
    color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    border-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
    box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);

    &:hover {
      opacity: 1;
    }
  }
`;

export type ILayersControl = {
  /** set the slide out as open or closed */
  onToggle: () => void;
};

/**
 * Component to display the layers control on the map
 * @example ./LayersControl.md
 */
const LayersControl: React.FC<React.PropsWithChildren<ILayersControl>> = ({ onToggle }) => {
  useEffect(() => {
    const elem = L.DomUtil.get('layersContainer');
    if (elem) {
      L.DomEvent.on(elem, 'mousewheel', L.DomEvent.stopPropagation);
    }
  });

  return (
    <TooltipWrapper tooltipId="layer-control-id" tooltip="Layer Controls">
      <ControlButton id="layersControlButton" variant="outline-secondary" onClick={onToggle}>
        <LayersIcon />
      </ControlButton>
    </TooltipWrapper>
  );
};

export default LayersControl;
