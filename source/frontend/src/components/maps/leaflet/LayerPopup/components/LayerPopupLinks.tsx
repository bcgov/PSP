import { LatLngBounds } from 'leaflet';
import noop from 'lodash/noop';
import React, { useCallback } from 'react';
import { FaEllipsisH, FaEye, FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import TooltipWrapper from '@/components/common/TooltipWrapper';

export interface ILayerPopupLinksProps {
  bounds: LatLngBounds | undefined;
  onEllipsisClick?: () => void;
  onViewPropertyInfo: (event: React.MouseEvent<HTMLElement>) => void;
}

export const LayerPopupLinks: React.FC<React.PropsWithChildren<ILayerPopupLinksProps>> = ({
  bounds,
  onViewPropertyInfo,
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
      <StyledLinksWrapper>
        <LinkButton onClick={onViewPropertyInfo}>
          <FaEye size={18} className="mr-2" />
          <span>View Property Info</span>
        </LinkButton>
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
