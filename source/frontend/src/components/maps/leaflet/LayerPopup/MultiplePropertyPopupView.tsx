import React from 'react';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { exists } from '@/utils';

export interface IMultiplePropertyPopupView {
  featureDataset: LocationFeatureDataset | null;
}

export const MultiplePropertyPopupView: React.FC<
  React.PropsWithChildren<IMultiplePropertyPopupView>
> = ({ featureDataset }) => {
  const mapMachine = useMapStateMachine();

  const onCloseButtonPressed = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.closePopup();
  };

  const handlePropertySelect = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    debugger;
  };

  const getFirstBounds = (layers: LayerData[]) => {
    return layers.find(layer => exists(layer?.bounds))?.bounds;
  };

  return (
    <StyledContainer>
      <TooltipWrapper tooltipId="close-sidebar-tooltip" tooltip="Close Form">
        <StyledCloseButton title="close" onClick={onCloseButtonPressed}>
          <CloseIcon />
        </StyledCloseButton>
      </TooltipWrapper>
      MULTIPLE HERE
    </StyledContainer>
  );
};

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 2rem;
  cursor: pointer;
`;

const StyledCloseButton = styled(StyledIconButton)`
  position: fixed;
  top: 0rem;
  right: 1rem;
`;

const StyledContainer = styled.div`
  position: relative;
  min-width: 30rem;
  padding-left: 1.6rem;
  padding-right: 1.6rem;
  margin: 0rem;
`;
