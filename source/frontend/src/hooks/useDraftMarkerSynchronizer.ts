import { MapStateActionTypes, MapStateContext } from 'components/maps/providers/MapStateContext';
import { PointFeature } from 'components/maps/types';
import { IMapProperty } from 'components/propertySelector/models';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useIsMounted from 'hooks/useIsMounted';
import debounce from 'lodash/debounce';
import * as React from 'react';
import { useContext, useEffect } from 'react';

/**
 * Get a list of draft markers from the current form values.
 * As long as a parcel/building has both a lat and a lng it will be returned by this method.
 * @param modifiedProperties the current form values to extract lat/lngs from.
 * @param initialProperties the original form values, used to exclude unchanged lat/lngs
 */
const getDraftMarkers = (modifiedProperties: IMapProperty[]): PointFeature[] => {
  return modifiedProperties
    .filter((property: IMapProperty) => {
      if (!property?.latitude || !property?.longitude) {
        return false;
      }
      return true;
    })
    .map<PointFeature>((property: IMapProperty) => {
      return {
        type: 'Feature',
        geometry: {
          type: 'Point',
          coordinates: [+(property?.longitude ?? 0), +(property?.latitude ?? 0)],
        },
        properties: {
          id: 0,
          name: property.name?.length ? property.name : 'New Parcel',
        },
      };
    });
};

/**
 * A hook that automatically syncs any updates to the lat/lngs of the parcel form with the map.
 * @param param0 the namespace of the array within the active formik instance that contains the properties.
 */
const useDraftMarkerSynchronizer = (modifiedProperties: IMapProperty[], source: string) => {
  const { setState } = useContext(MapStateContext);
  const isMounted = useIsMounted();
  useEffect(() => {
    return () => setState({ type: MapStateActionTypes.DRAFT_PROPERTIES, draftProperties: [] });
  }, [setState]);

  /**
   * Synchronize the markers that have been updated in the parcel form with the map, adding all new markers as drafts.
   * @param modifiedProperties the current form values
   * @param initialProperties the initial form values
   */
  const synchronizeMarkers = React.useCallback(
    (modifiedProperties: IMapProperty[]) => {
      if (isMounted()) {
        const draftMarkers = getDraftMarkers(modifiedProperties);
        if (draftMarkers.length) {
          setState({
            type: MapStateActionTypes.DRAFT_PROPERTIES,
            draftProperties: draftMarkers,
          });
        } else {
          setState({ type: MapStateActionTypes.DRAFT_PROPERTIES, draftProperties: [] });
        }
      }
    },
    [setState, isMounted],
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
