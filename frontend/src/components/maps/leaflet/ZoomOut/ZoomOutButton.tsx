import variables from '_variables.module.scss';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { LatLngBounds } from 'leaflet';
import React from 'react';
import Button from 'react-bootstrap/Button';
import { FaExpandArrowsAlt } from 'react-icons/fa';
import { useMap } from 'react-leaflet';
import styled from 'styled-components';

import Control from '../Control/Control';

const ZoomButton = styled(Button)`
  background-color: #ffffff !important;
  color: ${variables.darkVariantColor} !important;
  width: 4rem;
  height: 4rem;
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
