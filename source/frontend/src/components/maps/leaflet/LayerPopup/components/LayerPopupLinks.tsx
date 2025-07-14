import { LatLngBounds, LatLngLiteral } from 'leaflet';
import noop from 'lodash/noop';
import React, { useCallback } from 'react';
import { FaEllipsisH, FaEye, FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { exists } from '@/utils/utils';

export interface ILayerPopupLinksProps {
  bounds: LatLngBounds | undefined;
  latLng: LatLngLiteral | undefined;
  onEllipsisClick?: () => void;
  onViewPropertyInfo: (event: React.MouseEvent<HTMLElement>) => void;
  showViewPropertyInfo: boolean;
}

export const LayerPopupLinks: React.FC<React.PropsWithChildren<ILayerPopupLinksProps>> = ({
  bounds,
  latLng,
  onViewPropertyInfo,
  onEllipsisClick,
  showViewPropertyInfo,
}) => {
  const { requestFlyToBounds, requestFlyToLocation } = useMapStateMachine();

  const onZoomToBounds = useCallback(() => {
    if (exists(bounds)) {
      requestFlyToBounds(bounds);
    } else if (exists(latLng)) {
      requestFlyToLocation(latLng);
    }
  }, [bounds, latLng, requestFlyToBounds, requestFlyToLocation]);

  return (
    <StyledContainer>
      <StyledLinksWrapper>
        {showViewPropertyInfo && (
          <LinkButton onClick={onViewPropertyInfo}>
            <FaEye size={18} className="mr-2" />
            <span>View Property Info</span>
          </LinkButton>
        )}
        <LinkButton onClick={onZoomToBounds}>
          <FaSearchPlus size={18} className="mr-2" />
          <span>Zoom map</span>
        </LinkButton>
      </StyledLinksWrapper>

      <StyledEllipsis>
        <TooltipWrapper tooltipId="see-more" tooltip="See more...">
          <LinkButton onClick={onEllipsisClick ?? noop} data-testid="fly-out-ellipsis">
            <FaEllipsisH size={18} />
          </LinkButton>
        </TooltipWrapper>
      </StyledEllipsis>
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

const StyledLinksWrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  margin-top: 1rem;
  margin-bottom: 1rem;
`;

const StyledEllipsis = styled.div`
  align-self: flex-end;
  margin-bottom: 1rem;
`;
