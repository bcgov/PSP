import React, { useCallback, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FiX } from 'react-icons/fi';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { isValidId, pidParser } from '@/utils';

import { LayerPopupContent } from './components/LayerPopupContent';
import { LayerPopupFlyout } from './components/LayerPopupFlyout';
import { LayerPopupLinks } from './components/LayerPopupLinks';
import { LayerPopupInformation } from './LayerPopupContainer';
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

  const onPropertyViewClicked = () => {
    if (isInPims) {
      closeFlyout();
      history.push(`/mapview/sidebar/property/${pimsPropertyId}`);
    } else if (featureDataset?.parcelFeature?.properties?.PID) {
      closeFlyout();
      const parcelFeature = featureDataset?.parcelFeature;
      const parsedPid = pidParser(parcelFeature?.properties?.PID);
      history.push(`/mapview/sidebar/non-inventory-property/${parsedPid}`);
    } else {
      console.warn('Invalid marker when trying to see property information');
      toast.warn('A map parcel must have a PID in order to view detailed information');
    }
  };

  const onCloseButtonPressed = (event: React.MouseEvent<HTMLElement>) => {
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

  return (
    <StyledContainer>
      <StyledRow>
        <Col className="p-0 m-0">
          <LayerPopupTitle>{layerPopup.title}</LayerPopupTitle>
        </Col>
        <Col xs="1" className="p-0 m-0">
          <StyledIconButton className="p-0 m-0" onClick={onCloseButtonPressed}>
            <FiX size="1.5rem" />
          </StyledIconButton>
        </Col>
      </StyledRow>
      <LayerPopupContent layerPopup={layerPopup} />
      <LayerPopupLinks
        bounds={layerPopup.bounds}
        onEllipsisClick={showFlyout ? closeFlyout : openFlyout}
      />

      {showFlyout && (
        <StyledFlyoutContainer>
          <LayerPopupFlyout
            isInPims={isInPims}
            isRetiredProperty={isRetiredProperty}
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

const StyledContainer = styled.div`
  padding: 0.5rem 1.2rem;
  background-color: ${props => props.theme.css.highlightBackgroundColor};

  .btn-link {
    font-size: 1.4rem;
    line-height: 2.2rem;
    padding: 0;
  }
  .list-group {
    .list-group-item {
      background-color: ${props => props.theme.css.highlightBackgroundColor};
      font-size: 1.4rem;
      border: none;
      padding: 0;
      padding-top: 0.8rem;
    }
  }
`;

const StyledRow = styled(Row)`
  margin: 0rem;
  border-bottom: 0.2rem ${props => props.theme.css.borderOutlineColor} solid;
`;

const StyledFlyoutContainer = styled.div`
  position: absolute;
  bottom: -3.5rem;
  left: 100%;
  background: #fffefa;
  border: 2px solid #bcbec5;
  box-shadow: 6px 6px 12px rgb(0 0 0 / 40%);
  min-width: 25rem;
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;
