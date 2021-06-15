import { PARCELS_LAYER_URL, useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import MapSideBarLayout from 'features/mapSideBar/components/MapSideBarLayout';
import {
  SidebarContextType,
  useQueryParamSideBar,
} from 'features/mapSideBar/hooks/useQueryParamSideBar';
import { FormikProps, FormikValues, getIn } from 'formik';
import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { useApiLtsa } from 'hooks/pims-api/useApiLtsa';
import { IGeocoderResponse } from 'hooks/useApi';
import { ParcelInfo } from 'interfaces/ltsaModels';
import { LatLngLiteral } from 'leaflet';
import * as React from 'react';
import { toast } from 'react-toastify';

import PropertyForm from '../forms/PropertyForm';
import PropertySearchForm from '../forms/PropertySearchForm';
import { useMarkerSearch } from '../hooks/useMarkerSearch';
import InventoryTabs from '../InventoryTabs';
import SubmitPropertySelector from '../SubmitPropertySelector';

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
const MotiInventoryContainer: React.FunctionComponent = () => {
  const formikRef = React.useRef<FormikProps<FormikValues>>();
  const { showSideBar, setShowSideBar, size, context: sidebarContext } = useQueryParamSideBar(
    formikRef,
  );

  const latLngSearch = async (latLng?: LatLngLiteral) => {
    if (!latLng || !latLng?.lat || !latLng?.lng) {
      toast.error('Unable to perform search, property missing latitude/longitude');
      return;
    }
    try {
      const parcelLayerResponse = await parcelLayerService.findOneWhereContains(latLng);
      const properties = getIn(parcelLayerResponse, 'features.0.properties');
      const ltsaResponse = await getLtsaParcelInfo(properties?.PID);
      formikParcelDataPopulateCallback(parcelLayerResponse, ltsaResponse?.data);
    } catch (error) {
      toast.error('Failed to load parcel info from parcel layer');
    }
  };

  const handlePidChange = async (pid: string) => {
    if (pid) {
      const ltsaResponse = await getLtsaParcelInfo(pid);
      try {
        const parcelLayerResponse = await parcelLayerService.findByPid(pid);
        formikParcelDataPopulateCallback(parcelLayerResponse, ltsaResponse?.data);
      } catch (error) {
        toast.error('Failed to load parcel info from parcel layer');
      }
    }
  };

  const getLtsaParcelInfo = async (pid: string) => {
    try {
      return await getParcelInfo(pid);
    } catch (error) {
      if (error?.response?.status === 404) {
        toast.warning(`PID: ${pid} not found in Land Title Direct Search Service.`);
      } else {
        toast.error('Failed to load parcel info from Land Title Direct Search Service.');
      }
    }
  };

  /**
   * Populate the formik form using the passed property data.
   * @param nameSpace the formik namespace that should be used to write any retrieved data.
   * @param matchingParcel the parcel to use to populate the formik form.
   */
  const formikParcelDataPopulateCallback = (
    parcelLayerResponse: FeatureCollection<Geometry, GeoJsonProperties>,
    ltsaResponse: ParcelInfo | undefined,
    nameSpace?: string,
  ) => {
    //TODO: populate form with response data for PSP-705
  };

  //hooks
  const { getParcelInfo } = useApiLtsa();
  const parcelLayerService = useLayerQuery(PARCELS_LAYER_URL);
  const { setMovingPinNameSpace } = useMarkerSearch(formikRef, showSideBar, latLngSearch);
  return (
    <MapSideBarLayout
      title="Add Titled Property to Inventory"
      show={showSideBar}
      setShowSideBar={setShowSideBar}
      size={size}
      hidePolicy={true}
    >
      {!sidebarContext || sidebarContext === SidebarContextType.ADD_PROPERTY_TYPE_SELECTOR ? (
        <SubmitPropertySelector
          addProperty={() => setShowSideBar(true, SidebarContextType.ADD_BARE_LAND, 'wide', true)}
        />
      ) : (
        <>
          <PropertySearchForm
            handlePidChange={handlePidChange}
            handleGeocoderChanges={async (data: IGeocoderResponse, nameSpace?: string) => {
              latLngSearch({ lat: data.latitude, lng: data.longitude });
            }}
            setMovingPinNameSpace={setMovingPinNameSpace}
          />
          <InventoryTabs PropertyForm={<PropertyForm formikRef={formikRef}></PropertyForm>} />
        </>
      )}
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;
