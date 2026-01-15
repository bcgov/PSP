import debounce from 'lodash/debounce';
import { useCallback, useRef } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationBoundaryDataset } from '@/components/common/mapFSM/models';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';

/**
 * A hook that automatically syncs any updates to the lat/lngs of the parcel form with the map.
 * @param modifiedProperties array that contains the properties to be drawn.
 */
const useDraftMarkerSynchronizer = (modifiedProperties: LocationBoundaryDataset[]) => {
  const isMounted = useIsMounted();

  const { setFilePropertyLocations } = useMapStateMachine();

  /**
   * Synchronize the markers that have been updated in the parcel form with the map, adding all new markers.
   * @param modifiedProperties the current properties
   */
  const synchronizeMarkers = useCallback(
    (modifiedProperties: LocationBoundaryDataset[]) => {
      if (isMounted()) {
        const filePropertyLocations = modifiedProperties;
        if (filePropertyLocations.length > 0) {
          setFilePropertyLocations(filePropertyLocations);
        } else {
          setFilePropertyLocations([]);
        }
      }
    },
    [setFilePropertyLocations, isMounted],
  );

  const synchronize = useRef(
    debounce((modifiedProperties: LocationBoundaryDataset[]) => {
      synchronizeMarkers(modifiedProperties);
    }, 400),
  ).current;

  useDeepCompareEffect(() => {
    synchronize(modifiedProperties);
  }, [modifiedProperties, synchronize]);
};

export default useDraftMarkerSynchronizer;
