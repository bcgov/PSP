import MapSideBarLayout from 'features/mapSideBar/components/MapSideBarLayout';
import { useQueryParamSideBar } from 'features/mapSideBar/hooks/useQueryParamSideBar';
import { FormikValues } from 'formik';
import { noop } from 'lodash';
import * as React from 'react';

import SubmitPropertySelector from '../SubmitPropertySelector';

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
const MotiInventoryContainer: React.FunctionComponent = () => {
  const formikRef = React.useRef<FormikValues>();
  const { showSideBar, setShowSideBar, size } = useQueryParamSideBar(formikRef);

  return (
    <MapSideBarLayout
      title="Add to Inventory"
      show={showSideBar}
      setShowSideBar={setShowSideBar}
      size={size}
      hidePolicy={true}
    >
      <SubmitPropertySelector addProperty={noop} />
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;
