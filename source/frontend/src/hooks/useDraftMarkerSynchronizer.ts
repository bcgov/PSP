import debounce from 'lodash/debounce';
import { useCallback, useRef } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import useIsMounted from '@/hooks/util/useIsMounted';

import useDeepCompareEffect from './util/useDeepCompareEffect';

/**
 * A hook that automatically syncs any updates to the lat/lngs of the parcel form with the map.
 * @param modifiedProperties array that contains the properties to be drawn.
 */
const useDraftMarkerSynchronizer = (modifiedProperties: PropertyForm[]) => {
  const isMounted = useIsMounted();

  const { setFilePropertyLocations } = useMapStateMachine();

  /**
   * Synchronize the markers that have been updated in the parcel form with the map, adding all new markers.
   * @param modifiedProperties the current properties
   */
  const synchronizeMarkers = useCallback(
    (modifiedProperties: PropertyForm[]) => {
      if (isMounted()) {
        const filePropertyLocations = modifiedProperties.map(x => x.toFilePropertyApi());
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
    debounce((modifiedProperties: PropertyForm[]) => {
      synchronizeMarkers(modifiedProperties);
    }, 400),
  ).current;

  useDeepCompareEffect(() => {
    synchronize(modifiedProperties);
  }, [modifiedProperties, synchronize]);
};

export default useDraftMarkerSynchronizer;
