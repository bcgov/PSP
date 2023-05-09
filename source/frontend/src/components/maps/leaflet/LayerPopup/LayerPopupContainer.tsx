import { MapStateActionTypes, MapStateContext } from 'components/maps/providers/MapStateContext';
import React, { useCallback, useContext, useState } from 'react';
import { useHistory } from 'react-router';
import styled from 'styled-components';

import { LayerPopupContent, LayerPopupInformation } from '.';
import { LayerPopupFlyout } from './components/LayerPopupFlyout';
import { LayerPopupLinks } from './components/LayerPopupLinks';
import { LayerPopupTitle } from './styles';

export interface ILayerPopupContainerProps {
  layerPopup: LayerPopupInformation;
  onViewPropertyInfo: (pid?: string | null, id?: number) => void;
}

export const LayerPopupContainer: React.FC<React.PropsWithChildren<ILayerPopupContainerProps>> = ({
  layerPopup,
  onViewPropertyInfo,
}) => {
  const history = useHistory();
  const { setState, selectedFeature } = useContext(MapStateContext);

  // We are interested in the PID field that comes back from parcel map layer attributes
  const pid = layerPopup?.data?.PID;
  // Attempt to get the id field, the id will be populated if the user clicked on a lat/lng that belongs to an existing PIMS Property Boundary.
  const id = layerPopup?.pimsProperty?.properties?.PROPERTY_ID;
  // open/close map popup fly-out menu
  const [showFlyout, setShowFlyout] = useState(false);
  const openFlyout = useCallback(() => setShowFlyout(true), []);
  const closeFlyout = useCallback(() => setShowFlyout(false), []);

  // bubble up the non-inventory property - based on their PID
  const handleViewPropertyInfo = () => {
    onViewPropertyInfo(pid, id);
  };

  const handleCreateResearchFile = () => {
    selectedFeature &&
      setState({
        type: MapStateActionTypes.SELECTED_FILE_FEATURE,
        selectedFileFeature: selectedFeature,
      });
    history.push('/mapview/sidebar/research/new');
  };

  const handleCreateAcquisitionFile = () => {
    selectedFeature &&
      setState({
        type: MapStateActionTypes.SELECTED_FILE_FEATURE,
        selectedFileFeature: selectedFeature,
      });
    history.push('/mapview/sidebar/acquisition/new');
  };

  const handleCreateLeaseLicence = () => {
    selectedFeature &&
      setState({
        type: MapStateActionTypes.SELECTED_FILE_FEATURE,
        selectedFileFeature: selectedFeature,
      });
    history.push('/mapview/sidebar/lease/new');
  };

  return (
    <StyledContainer>
      <LayerPopupTitle>{layerPopup.title}</LayerPopupTitle>
      <LayerPopupContent layerPopup={layerPopup} />
      <LayerPopupLinks
        layerPopup={layerPopup}
        onEllipsisClick={showFlyout ? closeFlyout : openFlyout}
      />

      {showFlyout && (
        <StyledFlyoutContainer>
          <LayerPopupFlyout
            onViewPropertyInfo={handleViewPropertyInfo}
            onCreateResearchFile={handleCreateResearchFile}
            onCreateAcquisitionFile={handleCreateAcquisitionFile}
            onCreateLeaseLicense={handleCreateLeaseLicence}
          />
        </StyledFlyoutContainer>
      )}
    </StyledContainer>
  );
};

const StyledContainer = styled.div`
  padding: 0.5rem 1.2rem;
  background-color: ${props => props.theme.css.mapPopupBackgroundColor};

  .btn-link {
    font-size: 1.4rem;
    line-height: 2.2rem;
    padding: 0;
  }
  .list-group {
    .list-group-item {
      background-color: ${props => props.theme.css.mapPopupBackgroundColor};
      font-size: 1.4rem;
      border: none;
      padding: 0;
      padding-top: 0.8rem;
    }
  }
`;

const StyledFlyoutContainer = styled.div`
  position: absolute;
  bottom: -3.5rem;
  left: 100%;
  background: #fffefa;
  border: 2px solid #bcbec5;
  box-shadow: 6px 6px 12px rgb(0 0 0 / 40%);
  min-width: 25rem;

  .list-group {
    .list-group-item {
      padding: 0.5rem 1rem 0 1rem !important;
      .btn {
        width: 100%;
        border-bottom: 1px solid #bcbec5 !important;
      }
      &:last-of-type {
        padding-bottom: 0.5rem !important;
        .btn {
          border-bottom: none !important;
        }
      }
    }
  }
`;
