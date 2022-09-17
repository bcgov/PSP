import { FormikProps } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';

import { AcquisitionContainerState } from './AcquisitionContainer';
import AcquisitionSummaryTabs from './detail/AcquisitionSummaryTabs';
import { EditFormNames } from './EditFormNames';

export interface IViewSelectorProps {
  acquisitionFile?: Api_AcquisitionFile;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;

  // selectedIndex: number;
  // setFormikRef: (ref: React.RefObject<FormikProps<any>> | undefined) => void;
  // onSuccess: () => void;
}

export const ViewSelector: React.FunctionComponent<IViewSelectorProps> = props => {
  return (
    <AcquisitionSummaryTabs
      acquisitionFile={props.acquisitionFile}
      setContainerState={props.setContainerState}
    />
  );
};

export default ViewSelector;
