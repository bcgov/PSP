import { LatLngBounds } from 'leaflet';
import noop from 'lodash/noop';
import React, { useCallback } from 'react';
import { FaEllipsisH } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import TooltipWrapper from '@/components/common/TooltipWrapper';

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
      <TooltipWrapper tooltipId="see-more" tooltip="See more...">
        <LinkButton onClick={onEllipsisClick ?? noop} data-testid="fly-out-ellipsis">
          <FaEllipsisH />
        </LinkButton>
      </TooltipWrapper>
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
