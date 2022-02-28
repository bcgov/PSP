import { FormikProps, FormikValues } from 'formik';
import * as React from 'react';

import useMapSideBarQueryParams from './hooks/useMapSideBarQueryParams';
import MapSideBarLayout from './layout/MapSideBarLayout';
import { InventoryTabs } from './tabs/InventoryTabs';

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
const MotiInventoryContainer: React.FunctionComponent = () => {
  const formikRef = React.useRef<FormikProps<FormikValues>>();
  const { showSideBar, setShowSideBar } = useMapSideBarQueryParams(formikRef);

  return (
    <MapSideBarLayout
      title="Property Information"
      show={showSideBar}
      setShowSideBar={setShowSideBar}
      hidePolicy={true}
    >
      <InventoryTabs PropertyForm={<></>} />
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;
