import { MAP_MAX_ZOOM } from 'constants/strings';
import { FormikProps, FormikValues } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { useProperties } from 'hooks/useProperties';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import React, { useCallback, useEffect, useState } from 'react';
import { pidFormatter } from 'utils';

import useMapSideBarQueryParams from './hooks/useMapSideBarQueryParams';
import { usePropertyDetails } from './hooks/usePropertyDetails';
import MapSideBarLayout from './layout/MapSideBarLayout';
import { MapSlideBarHeader } from './MapSlideBarHeader';
import { InventoryTabs } from './tabs/InventoryTabs';
import LtsaTabView from './tabs/ltsa/LtsaTabView';
import { PropertyDetailsTabView } from './tabs/propertyDetails/PropertyDetailsTabView';

interface IMapSideBarContainer {
  map?: L.Map;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MapSideBarContainer: React.FunctionComponent<IMapSideBarContainer> = ({ map }) => {
  const isMounted = useIsMounted();
  const formikRef = React.useRef<FormikProps<FormikValues>>();
  const { showSideBar, setShowSideBar, pid } = useMapSideBarQueryParams(formikRef);
  const [ltsaData, setLtsaData] = useState<LtsaOrders | undefined>(undefined);
  const [apiProperty, setApiProperty] = useState<IPropertyApiModel | undefined>(undefined);

  const onZoom = useCallback(
    () =>
      apiProperty?.longitude &&
      apiProperty?.latitude &&
      map?.flyTo({ lat: apiProperty?.latitude, lng: apiProperty?.longitude }, MAP_MAX_ZOOM, {
        animate: false,
      }),
    [apiProperty?.latitude, apiProperty?.longitude, map],
  );

  // First, fetch property information from PSP API
  const { getPropertyWithPid } = useProperties();
  useEffect(() => {
    const func = async () => {
      if (!!pid) {
        const propInfo = await getPropertyWithPid(pid);
        if (isMounted() && propInfo.pid === pidFormatter(pid)) {
          setApiProperty(propInfo);
        }
      }
    };

    func();
  }, [getPropertyWithPid, isMounted, pid]);

  // After API property object has been received, we query relevant map layers to find
  // additional information which we store in a different model (IPropertyDetailsForm)
  const propertyViewForm = usePropertyDetails(apiProperty);

  const { getLtsaData } = useLtsa();
  useEffect(() => {
    const func = async () => {
      setLtsaData(undefined);
      if (!!pid) {
        const ltsaData = await getLtsaData(pidFormatter(pid));
        if (
          isMounted() &&
          ltsaData?.parcelInfo?.orderedProduct?.fieldedData.parcelIdentifier === pidFormatter(pid)
        ) {
          setLtsaData(ltsaData);
        }
      }
    };
    func();
  }, [getLtsaData, pid, isMounted]);

  return (
    <MapSideBarLayout
      title="Property Information"
      show={showSideBar}
      setShowSideBar={setShowSideBar}
      hidePolicy={true}
      header={
        <MapSlideBarHeader
          ltsaData={ltsaData}
          property={apiProperty}
          onZoom={apiProperty && map ? onZoom : undefined}
        />
      }
    >
      <InventoryTabs
        PropertyView={<PropertyDetailsTabView property={propertyViewForm} />}
        LtsaView={<LtsaTabView ltsaData={ltsaData} />}
      />
    </MapSideBarLayout>
  );
};

export default MapSideBarContainer;
