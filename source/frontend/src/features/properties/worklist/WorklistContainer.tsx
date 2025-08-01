import { geoJSON } from 'leaflet';
import { useCallback } from 'react';
import { useHistory } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { exists } from '@/utils';

import { useWorklistContext } from './context/WorklistContext';
import { ParcelFeature } from './models';
import { IWorklistViewProps } from './WorklistView';

export interface IWorklistContainerProps {
  View: React.FC<IWorklistViewProps>;
}

export const WorklistContainer: React.FC<IWorklistContainerProps> = ({ View }) => {
  const { parcels, selectedId, select, remove, clearAll } = useWorklistContext();
  const { requestFlyToBounds, requestFlyToLocation, prepareForCreation } = useMapStateMachine();
  const history = useHistory();

  // A worklist parcel points to either a parcel-map boundary/shape or a lat/long (if no parcels were found at that location)
  const onZoomToBounds = useCallback(
    (parcel: ParcelFeature) => {
      if (exists(parcel.feature)) {
        const bounds = geoJSON(parcel.feature).getBounds();
        if (exists(bounds) && bounds.isValid()) {
          requestFlyToBounds(bounds);
        } else if (exists(parcel.location)) {
          requestFlyToLocation(parcel.location);
        }
      } else if (exists(parcel.location)) {
        requestFlyToLocation(parcel.location);
      }
    },
    [requestFlyToBounds, requestFlyToLocation],
  );

  // Handle creating a research file from the worklist
  const handleCreateResearchFile = useCallback(
    (event: React.MouseEvent<HTMLElement>) => {
      event.stopPropagation();
      event.preventDefault();
      if (parcels.length === 0) {
        return;
      }
      // Set the selected features in the map state machine
      // This will allow the AddResearchContainer to access the selected features
      const featuresSets = parcels.map(p => p.toSelectedFeatureDataset());
      prepareForCreation(featuresSets);
      history.push('/mapview/sidebar/research/new');
    },
    [history, parcels, prepareForCreation],
  );

  return (
    <View
      parcels={parcels ?? []}
      selectedId={selectedId}
      onSelect={select}
      onRemove={remove}
      onZoomToParcel={onZoomToBounds}
      onClearAll={clearAll}
      onCreateResearchFile={handleCreateResearchFile}
    />
  );
};
