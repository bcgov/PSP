import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { noop } from 'lodash';
import * as React from 'react';
import { MdTopic } from 'react-icons/md';

import AddResearchForm from './AddResearchForm';

export interface IAddResearchViewProps {}

export const AddResearchSideBar: React.FunctionComponent = () => {
  return (
    <MapSideBarLayout
      showSideBar={true}
      setShowSideBar={noop}
      title="Create Research File"
      icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
    >
      <AddResearchForm />
    </MapSideBarLayout>
  );
};

export default AddResearchSideBar;
