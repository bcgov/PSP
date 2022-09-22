import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';

import { AcquisitionContainerState } from './AcquisitionContainer';
import AcquisitionFileTabs from './detail/AcquisitionFileTabs';

export interface IViewSelectorProps {
  acquisitionFile?: Api_AcquisitionFile;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
}

export const ViewSelector: React.FunctionComponent<IViewSelectorProps> = props => {
  return (
    <AcquisitionFileTabs
      acquisitionFile={props.acquisitionFile}
      setContainerState={props.setContainerState}
    />
  );
};

export default ViewSelector;
