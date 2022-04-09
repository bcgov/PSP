import { mdiFolderText } from '@mdi/js';
import Icon from '@mdi/react';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { noop } from 'lodash';
import * as React from 'react';

import AddResearchForm from './AddResearchForm';

export interface IAddResearchViewProps {}

export const AddResearchSideBar: React.FunctionComponent = () => {
  return (
    <MapSideBarLayout
      showSideBar={true}
      setShowSideBar={noop}
      title="Create Research File"
      icon={<Icon path={mdiFolderText} title="User Profile" size={2} className="mr-2" />}
    >
      <AddResearchForm />
    </MapSideBarLayout>
  );
};

export default AddResearchSideBar;
