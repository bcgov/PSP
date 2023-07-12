import { LatLngBounds } from 'leaflet';
import noop from 'lodash/noop';
import React, { useCallback } from 'react';
import { FaEllipsisH } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

export interface ILayerPopupLinksProps {
  bounds: LatLngBounds | undefined;
  onEllipsisClick?: () => void;
}

export const LayerPopupLinks: React.FC<React.PropsWithChildren<ILayerPopupLinksProps>> = ({
  bounds,
  onEllipsisClick,
}) => {
  const { requestFlyToBounds } = useMapStateMachine();

  const onZoomToBounds = useCallback(() => {
    if (bounds !== undefined) {
      requestFlyToBounds(bounds);
    }
  }, [requestFlyToBounds, bounds]);

  return (
    <StyledContainer>
      <LinkButton onClick={onZoomToBounds}>Zoom map</LinkButton>
      <LinkButton onClick={onEllipsisClick ?? noop} data-testid="fly-out-ellipsis">
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
