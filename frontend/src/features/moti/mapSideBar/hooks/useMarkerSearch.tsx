import { FormikProps, FormikValues } from 'formik';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { LatLngLiteral } from 'leaflet';
import { useEffect, useState } from 'react';
import { useAppSelector } from 'store/store';
import { isMouseEventRecent } from 'utils';

/**
 * hook co-locating all code related to map marker functionality.
 * @param formikRef a reference to the currently displayed form.
 * @param showSideBar whether or not the parent of this hook is currently visible.
 * @param latLngSeach a callback function to be executed with the lat/lng coords of the most recently clicked map marker.
 */
export const useMarkerSearch = (
  formikRef: React.MutableRefObject<FormikProps<FormikValues> | undefined>,
  showSideBar: boolean,
  latLngSeach: (latLng?: LatLngLiteral) => Promise<void>,
) => {
  const [movingPinNameSpace, setMovingPinNameSpace] = useState<string | undefined>();
  const leafletMouseEvent = useAppSelector(state => state.leafletMouseEvent?.mapClickEvent);

  useEffect(() => {
    if (!showSideBar) {
      document.body.className = '';
    }
    if (movingPinNameSpace !== undefined) {
      document.body.className = 'parcel-cursor';
    } else {
      document.body.className = '';
    }
    return () => {
      //make sure to reset the cursor when this component is disposed.
      document.body.className = '';
    };
  }, [movingPinNameSpace, showSideBar]);

  //Add a pin to the map where the user has clicked.
  useDeepCompareEffect(() => {
    //If we click on the map, create a new pin at the click location.
    if (!!formikRef?.current && isMouseEventRecent(leafletMouseEvent?.originalEvent?.timeStamp)) {
      formikRef.current.setFieldValue(`latitude`, leafletMouseEvent?.latlng?.lat || 0);
      formikRef.current.setFieldValue(`longitude`, leafletMouseEvent?.latlng?.lng || 0);
      latLngSeach(leafletMouseEvent?.latlng);
    }
  }, [leafletMouseEvent, showSideBar]);

  return { movingPinNameSpace, setMovingPinNameSpace };
};
