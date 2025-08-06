import { geoJSON } from 'leaflet';
import { useCallback } from 'react';

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
  const { requestFlyToBounds, requestFlyToLocation } = useMapStateMachine();

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

  return (
    <View
      parcels={parcels ?? []}
      selectedId={selectedId}
      onSelect={select}
      onRemove={remove}
      onClearAll={clearAll}
      onZoomToParcel={onZoomToBounds}
    />
  );
};
