import { PointFeature } from 'components/maps/types';
import { PropertyTypes } from 'constants/propertyTypes';
import { getIn, useFormikContext } from 'formik';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import _ from 'lodash';
import debounce from 'lodash/debounce';
import * as React from 'react';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useAppSelector } from 'store/hooks';
import { storeDraftProperties } from 'store/slices/properties';

/**
 * Get a list of draft markers from the current form values.
 * As long as a parcel/building has both a lat and a lng it will be returned by this method.
 * @param values the current form values to extract lat/lngs from.
 * @param initialValues the original form values, used to exclude unchanged lat/lngs
 * @param nameSpace path within above objects to extract lat/lngs
 */
const getDraftMarkers = (values: any, initialValues: any, nameSpace: string) => {
  values = getIn(values, nameSpace);
  initialValues = getIn(initialValues, nameSpace);
  if (
    values?.latitude === '' ||
    values?.longitude === '' ||
    (values?.latitude === initialValues?.latitude && values?.longitude === initialValues?.longitude)
  ) {
    return [];
  }
  return [
    {
      type: 'Feature',
      geometry: {
        type: 'Point',
        coordinates: [+values.longitude, +values.latitude],
      },
      properties: {
        id: 0,
        name: values.name?.length ? values.name : 'New Parcel',
        propertyTypeId:
          values.parcelId !== undefined ? PropertyTypes.DraftBuilding : PropertyTypes.DraftLand,
      },
    },
  ];
};

/**
 * A hook that automatically syncs any updates to the lat/lngs of the parcel form with the map.
 * @param param0 The currently displayed list of properties on the map.
 */
const useDraftMarkerSynchronizer = (nameSpace: string) => {
  const { values, initialValues } = useFormikContext();
  const properties = useAppSelector(state => [...state.properties.draftProperties]);
  const dispatch = useDispatch();
  const nonDraftProperties = React.useMemo(
    () =>
      properties.filter(
        (property: PointFeature) =>
          property.properties.propertyTypeId !== undefined &&
          [PropertyTypes.Building, PropertyTypes.Land].includes(property.properties.propertyTypeId),
      ),
    [properties],
  );

  React.useEffect(() => {
    return () => {
      dispatch(storeDraftProperties([]));
    };
  }, [dispatch]);

  /**
   * Synchronize the markers that have been updated in the parcel form with the map, adding all new markers as drafts.
   * @param formValues the current form values
   * @param initialFormValues the initial form values
   * @param dbProperties the currently displayed list of (DB) map properties.
   * @param formNameSpace the path to extract lat/lng values from
   */
  const synchronizeMarkers = React.useMemo(
    () => (
      formValues: any,
      initialFormValues: any,
      dbProperties: PointFeature[],
      formNameSpace: string,
    ) => {
      const draftMarkers = getDraftMarkers(formValues, initialFormValues, formNameSpace);
      if (draftMarkers.length) {
        const newDraftMarkers = _.filter(
          draftMarkers,
          (draftMarker: PointFeature) =>
            _.find(
              dbProperties,
              dbProperty =>
                dbProperty.geometry.coordinates[0] === draftMarker.geometry.coordinates[0] &&
                dbProperty.geometry.coordinates[1] === draftMarker.geometry.coordinates[1],
            ) === undefined,
        );
        dispatch(storeDraftProperties(newDraftMarkers as PointFeature[]));
      } else {
        dispatch(storeDraftProperties([]));
      }
    },
    [dispatch],
  );

  const synchronize = useCallback(
    (
      formValues: any,
      initialFormValues: any,
      dbProperties: PointFeature[],
      formNameSpace: string,
    ) => {
      return debounce(
        (
          debouncedFormValues: any,
          debouncedinitialFormValues: any,
          dbProperties: PointFeature[],
          debouncedformNameSpace: string,
        ) => {
          synchronizeMarkers(
            debouncedFormValues,
            debouncedinitialFormValues,
            dbProperties,
            debouncedformNameSpace,
          );
        },
        400,
      )(formValues, initialFormValues, dbProperties, formNameSpace);
    },
    [synchronizeMarkers],
  );

  useDeepCompareEffect(() => {
    synchronize(values, initialValues, nonDraftProperties, nameSpace);
  }, [values, initialValues, nonDraftProperties, synchronize, nameSpace]);

  return;
};

export default useDraftMarkerSynchronizer;
