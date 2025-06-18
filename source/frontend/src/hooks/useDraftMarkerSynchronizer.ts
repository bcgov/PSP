import debounce from 'lodash/debounce';
import { useCallback, useRef } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { IMapProperty } from '@/components/propertySelector/models';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';

/**
 * A hook that automatically syncs any updates to the lat/lngs of the parcel form with the map.
 * @param modifiedProperties array that contains the properties to be drawn.
 */
const useDraftMarkerSynchronizer = (modifiedProperties: IMapProperty[]) => {
  const isMounted = useIsMounted();

  const { setFilePropertyLocations } = useMapStateMachine();

  /**
   * Synchronize the markers that have been updated in the parcel form with the map, adding all new markers.
   * @param modifiedProperties the current properties
   */
  const synchronizeMarkers = useCallback(
    (modifiedProperties: IMapProperty[]) => {
      if (isMounted()) {
        if (modifiedProperties.length > 0) {
          setFilePropertyLocations(modifiedProperties);
        } else {
          setFilePropertyLocations([]);
        }
      }
    },
    [setFilePropertyLocations, isMounted],
  );

  const synchronize = useRef(
    debounce((modifiedProperties: IMapProperty[]) => {
      synchronizeMarkers(modifiedProperties);
    }, 400),
  ).current;

  useDeepCompareEffect(() => {
    synchronize(modifiedProperties);
  }, [modifiedProperties, synchronize]);
};

export default useDraftMarkerSynchronizer;
