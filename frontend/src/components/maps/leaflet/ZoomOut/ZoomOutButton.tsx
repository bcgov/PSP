import { Button } from 'components/common/buttons/Button';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { LatLngBounds } from 'leaflet';
import React from 'react';
import { FaExpandArrowsAlt } from 'react-icons/fa';
import { useMap } from 'react-leaflet';
import styled from 'styled-components';

import Control from '../Control/Control';

const ZoomButton = styled(Button)`
  &&.btn {
    background-color: #ffffff;
    color: ${({ theme }) => theme.css.darkVariantColor};
    width: 4rem;
    height: 4rem;
  }
`;

export type ZoomOutProps = {
  /** the default bounds of the map to zoom out to */
  bounds: LatLngBounds;
};

/**
 * Displays a button that zooms out to show the entire map when clicked
 * @param map The leaflet map
 * @param bounds The latlng bounds to zoom out to
 */
export const ZoomOutButton: React.FC<ZoomOutProps> = ({ bounds }) => {
  const mapInstance = useMap();
  const zoomOut = () => {
    mapInstance.fitBounds(bounds);
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
