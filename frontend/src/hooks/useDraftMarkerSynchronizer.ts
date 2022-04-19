import { PointFeature } from 'components/maps/types';
import { getIn, useFormikContext } from 'formik';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { IProperty } from 'interfaces';
import debounce from 'lodash/debounce';
import * as React from 'react';
import { useAppDispatch } from 'store/hooks';
import { storeDraftProperties } from 'store/slices/properties/propertiesSlice';
/**
 * Get a list of draft markers from the current form values.
 * As long as a parcel/building has both a lat and a lng it will be returned by this method.
 * @param values the current form values to extract lat/lngs from.
 * @param initialValues the original form values, used to exclude unchanged lat/lngs
 * @param nameSpace path within above objects to extract lat/lngs
 */
const getDraftMarkers = (values: any, initialValues: any, nameSpace: string) => {
  const properties = getIn(values, nameSpace);
  const initialProperties = getIn(initialValues, nameSpace);
  return properties
    .filter((property: IProperty) => {
      if (
        !property?.latitude ||
        !property?.longitude ||
        (property?.latitude === initialProperties?.latitude &&
          property?.longitude === initialProperties?.longitude)
      ) {
        return false;
      }
      return true;
    })
    .map((property: IProperty) => {
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
const useDraftMarkerSynchronizer = (nameSpace: string) => {
  const { values, initialValues } = useFormikContext();
  const dispatch = useAppDispatch();

  React.useEffect(() => {
    return () => {
      dispatch(storeDraftProperties([]));
    };
  }, [dispatch]);

  /**
   * Synchronize the markers that have been updated in the parcel form with the map, adding all new markers as drafts.
   * @param values the current form values
   * @param initialValues the initial form values
   * @param dbProperties the currently displayed list of (DB) map properties.
   * @param nameSpace the path to extract lat/lng values from
   */
  const synchronizeMarkers = React.useCallback(
    (values: any, initialValues: any, nameSpace: string) => {
      const draftMarkers = getDraftMarkers(values, initialValues, nameSpace);
      if (draftMarkers.length) {
        dispatch(storeDraftProperties(draftMarkers as PointFeature[]));
      } else {
        dispatch(storeDraftProperties([]));
      }
    },
    [dispatch],
  );

  const synchronize = React.useRef(
    debounce((values: any, initialValues: any, nameSpace: string) => {
      synchronizeMarkers(values, initialValues, nameSpace);
    }, 400),
  ).current;

  useDeepCompareEffect(() => {
    synchronize(values, initialValues, nameSpace);
  }, [values, initialValues, synchronize, nameSpace]);

  return;
};

export default useDraftMarkerSynchronizer;
