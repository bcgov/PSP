import { LatLngLiteral } from 'leaflet';
import debounce from 'lodash/debounce';
import * as React from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { IMapProperty } from '@/components/propertySelector/models';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';

/**
 * Get a list of draft markers from the current form values.
 * As long as a parcel/building has both a lat and a lng it will be returned by this method.
 * @param modifiedProperties the current form values to extract lat/lngs from.
 */
const getDraftLocations = (modifiedProperties: IMapProperty[]): LatLngLiteral[] => {
  return modifiedProperties.map<LatLngLiteral>((property: IMapProperty) => {
    return { lat: Number(property?.latitude ?? 0), lng: Number(property?.longitude ?? 0) };
  });
};

/**
 * A hook that automatically syncs any updates to the lat/lngs of the parcel form with the map.
 * @param modifiedProperties array that contains the properties to be drawn.
 */
const useDraftMarkerSynchronizer = (modifiedProperties: IMapProperty[]) => {
  const isMounted = useIsMounted();

  const { setDraftLocations } = useMapStateMachine();

  /**
   * Synchronize the markers that have been updated in the parcel form with the map, adding all new markers as drafts.
   * @param modifiedProperties the current properties
   */
  const synchronizeMarkers = React.useCallback(
    (modifiedProperties: IMapProperty[]) => {
      if (isMounted()) {
        const draftLocations = getDraftLocations(modifiedProperties);
        if (draftLocations.length) {
          setDraftLocations(draftLocations);
        } else {
          setDraftLocations([]);
        }
      }
    },
    [setDraftLocations, isMounted],
  );

  const synchronize = React.useRef(
    debounce((modifiedProperties: IMapProperty[]) => {
      synchronizeMarkers(modifiedProperties);
    }, 400),
  ).current;

  useDeepCompareEffect(() => {
    synchronize(modifiedProperties);
  }, [modifiedProperties, synchronize]);

  return;
};

export default useDraftMarkerSynchronizer;
