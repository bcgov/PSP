import React from 'react';
import { FaExpandArrowsAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { defaultBounds } from '@/components/maps/constants';

import Control from '../Control';

const ZoomButton = styled(Button)`
  &&.btn {
    background-color: #ffffff;
    color: ${({ theme }) => theme.css.darkVariantColor};
    width: 4rem;
    height: 4rem;
  }
`;

/**
 * Displays a button that zooms out to show the entire map when clicked
 * @param map The leaflet map
 * @param bounds The latlng bounds to zoom out to
 */
export const ZoomOutButton: React.FC = () => {
  const mapMachine = useMapStateMachine();
  const zoomOut = () => {
    mapMachine.requestFlyToBounds(defaultBounds);
  };
  return (
    <Control position="topleft">
      <TooltipWrapper toolTipId="zoomout-id" toolTip="View entire province">
        <ZoomButton onClick={zoomOut}>
          <FaExpandArrowsAlt />
        </ZoomButton>
      </TooltipWrapper>
    </Control>
  );
};
