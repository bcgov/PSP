import { FormikProps, FormikValues } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import { useState } from 'react';
import { useEffect } from 'react';
import { pidFormatter } from 'utils';

import useMapSideBarQueryParams from './hooks/useMapSideBarQueryParams';
import { usePropertyDetails } from './hooks/usePropertyDetails';
import MapSideBarLayout from './layout/MapSideBarLayout';
import { MapSlideBarHeader } from './MapSlideBarHeader';
import { InventoryTabs } from './tabs/InventoryTabs';
import LtsaTabView from './tabs/ltsa/LtsaTabView';

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MotiInventoryContainer: React.FunctionComponent = () => {
  const formikRef = React.useRef<FormikProps<FormikValues>>();
  const { showSideBar, setShowSideBar, pid } = useMapSideBarQueryParams(formikRef);
  const [ltsaData, setLtsaData] = useState<LtsaOrders | undefined>(undefined);

  const { propertyDetails } = usePropertyDetails(pid);

  const { getLtsaData } = useLtsa();
  const isMounted = useIsMounted();
  useEffect(() => {
    const func = async () => {
      setLtsaData(undefined);
      if (!!pid) {
        const ltsaData = await getLtsaData(pidFormatter(pid));
        if (
          isMounted() &&
          ltsaData?.parcelInfo?.orderedProduct?.fieldedData.parcelIdentifier === pidFormatter(pid)
        ) {
          console.log(ltsaData);
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
      header={<MapSlideBarHeader ltsaData={ltsaData} propertyDetails={propertyDetails} />}
    >
      <InventoryTabs PropertyView={<></>} LtsaView={<LtsaTabView ltsaData={ltsaData} />} />
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;
