import React, { useCallback, useState } from 'react';
import { FaWindowClose } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import SimplePagination from '@/components/common/SimplePagination';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Scrollable } from '@/features/projects/styles';
import { exists, isValidId, pidParser, pinParser } from '@/utils';

import { LayerPopupContent } from './components/LayerPopupContent';
import { LayerPopupFlyout } from './components/LayerPopupFlyout';
import { LayerPopupLinks } from './components/LayerPopupLinks';
import { LayerData, LayerPopupInformation } from './LayerPopupContainer';
import { LayerPopupTitle } from './styles';

export interface ILayerPopupViewProps {
  layerPopup: LayerPopupInformation;
  featureDataset: LocationFeatureDataset | null;
}

export const LayerPopupView: React.FC<React.PropsWithChildren<ILayerPopupViewProps>> = ({
  layerPopup,
  featureDataset,
}) => {
  const history = useHistory();

  // open/close map popup fly-out menu
  const [showFlyout, setShowFlyout] = useState(false);
  const openFlyout = useCallback(() => setShowFlyout(true), []);
  const closeFlyout = useCallback(() => setShowFlyout(false), []);

  const mapMachine = useMapStateMachine();

  const pimsPropertyId = featureDataset?.pimsFeature?.properties?.PROPERTY_ID;
  const isInPims = isValidId(Number(pimsPropertyId));
  const isRetiredProperty = featureDataset?.pimsFeature?.properties?.IS_RETIRED ?? false;
  const isDisposedProperty = featureDataset?.pimsFeature?.properties?.IS_DISPOSED ?? false;

  const onPropertyViewClicked = () => {
    if (isInPims) {
      closeFlyout();
      history.push(`/mapview/sidebar/property/${pimsPropertyId}`);
    } else if (exists(featureDataset?.parcelFeature?.properties?.PID)) {
      closeFlyout();
      const parcelFeature = featureDataset?.parcelFeature;
      const parsedPid = pidParser(parcelFeature?.properties?.PID);
      history.push(`/mapview/sidebar/non-inventory-property/pid/${parsedPid}`);
    } else if (exists(featureDataset?.parcelFeature?.properties?.PIN)) {
      closeFlyout();
      const parcelFeature = featureDataset?.parcelFeature;
      const parsedPin = pinParser(parcelFeature?.properties?.PIN);
      history.push(`/mapview/sidebar/non-inventory-property/pin/${parsedPin}`);
    } else {
      console.warn('Invalid marker when trying to see property information');
      toast.warn('A map parcel must have a PID or PIN in order to view detailed information');
    }
  };

  const onCloseButtonPressed = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.closePopup();
  };

  const handleViewPropertyInfo = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.prepareForCreation();
    closeFlyout();
    onPropertyViewClicked();
  };

  const handleCreateResearchFile = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.prepareForCreation();
    closeFlyout();
    history.push('/mapview/sidebar/research/new');
  };

  const handleCreateAcquisitionFile = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.prepareForCreation();
    closeFlyout();
    history.push('/mapview/sidebar/acquisition/new');
  };

  const handleCreateLeaseLicence = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.prepareForCreation();
    closeFlyout();
    history.push('/mapview/sidebar/lease/new');
  };

  const handleCreateDispositionFile = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.prepareForCreation();
    closeFlyout();
    history.push('/mapview/sidebar/disposition/new');
  };

  const handleCreateSubdivision = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.prepareForCreation();
    closeFlyout();
    history.push('/mapview/sidebar/subdivision/new');
  };

  const handleCreateConsolidation = (event: React.MouseEvent<HTMLElement, MouseEvent>) => {
    event.stopPropagation();
    mapMachine.prepareForCreation();
    closeFlyout();
    history.push('/mapview/sidebar/consolidation/new');
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
      <SimplePagination<LayerData>
        items={layerPopup.layers}
        onZeroItemsPagination={'Location Information'}
        onZeroItemsContent={'No layer information at this location'}
      >
        {item =>
          exists(item) ? (
            <>
              <StyledScrollable>
                <LayerPopupTitle>{item.title}</LayerPopupTitle>
                <LayerPopupContent data={item.data} config={item.config} />
              </StyledScrollable>
            </>
          ) : (
            <></>
          )
        }
      </SimplePagination>
      <LayerPopupLinks
        bounds={getFirstBounds(layerPopup.layers)}
        onEllipsisClick={showFlyout ? closeFlyout : openFlyout}
      />

      {showFlyout && (
        <StyledFlyoutContainer>
          <LayerPopupFlyout
            isInPims={isInPims}
            isRetiredProperty={isRetiredProperty}
            isDisposedProperty={isDisposedProperty}
            onViewPropertyInfo={handleViewPropertyInfo}
            onCreateResearchFile={handleCreateResearchFile}
            onCreateAcquisitionFile={handleCreateAcquisitionFile}
            onCreateLeaseLicense={handleCreateLeaseLicence}
            onCreateDispositionFile={handleCreateDispositionFile}
            onCreateSubdivision={handleCreateSubdivision}
            onCreateConsolidation={handleCreateConsolidation}
          />
        </StyledFlyoutContainer>
      )}
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

const StyledScrollable = styled(Scrollable)`
  padding: 1rem;
  margin: 0rem;
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  height: 25rem;
  overflow: auto;
  font-size: 1.4rem;
`;

const StyledFlyoutContainer = styled.div`
  position: absolute;
  top: 90%;
  left: 100.9%;
  border: 2px solid #bcbec5;
  box-shadow: 6px 6px 12px rgb(0 0 0 / 40%);
  min-width: 20rem;
  background-color: white;
`;
