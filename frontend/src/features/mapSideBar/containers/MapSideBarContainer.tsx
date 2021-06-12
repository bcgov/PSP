import MotiInventoryContainer from 'features/moti/mapSideBar/containers/MotiInventoryContainer';
import * as React from 'react';
import { FeatureVisible } from 'tenants';

import PimsInventoryContainer from './PimsInventoryContainer';

/**
 * Conditionally display the correct side bar contents based on the tenant.
 */
const MapSideBarContainer: React.FunctionComponent = () => {
  return (
    <>
      <FeatureVisible tenant="MOTI">
        <MotiInventoryContainer />
      </FeatureVisible>
      <FeatureVisible tenant="CITZ">
        <PimsInventoryContainer />
      </FeatureVisible>
    </>
  );
};

export default MapSideBarContainer;
