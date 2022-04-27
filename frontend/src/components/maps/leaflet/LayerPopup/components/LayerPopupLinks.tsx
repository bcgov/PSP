import { LinkButton } from 'components/common/buttons';
import noop from 'lodash/noop';
import React, { useCallback } from 'react';
import { FaEllipsisH } from 'react-icons/fa';
import { useMap } from 'react-leaflet';
import styled from 'styled-components';

import { LayerPopupInformation } from '../LayerPopup';

export interface ILayerPopupLinksProps {
  layerPopup: LayerPopupInformation;
  onEllipsisClick?: () => void;
}

export const LayerPopupLinks: React.FC<ILayerPopupLinksProps> = ({
  layerPopup,
  onEllipsisClick,
}) => {
  const mapInstance = useMap();
  const { bounds } = { ...layerPopup };

  const onZoomToBounds = useCallback(
    () => bounds && mapInstance.flyToBounds(bounds, { animate: false }),
    [mapInstance, bounds],
  );

  return (
    <StyledContainer>
      <LinkButton onClick={onZoomToBounds}>Zoom map</LinkButton>
      <LinkButton onClick={onEllipsisClick ?? noop}>
        <FaEllipsisH />
      </LinkButton>
    </StyledContainer>
  );
};

const StyledContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-top: 1px solid #bcbec5;
  margin-top: 0.8rem;
`;
