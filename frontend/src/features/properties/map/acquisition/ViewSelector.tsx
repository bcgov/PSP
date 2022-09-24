import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';

import { AcquisitionContainerState } from './AcquisitionContainer';
import AcquisitionFileTabs from './detail/AcquisitionFileTabs';
import { EditFormNames } from './EditFormNames';
import { UpdateAcquisitionContainer } from './update/summary/UpdateAcquisitionContainer';

export interface IViewSelectorProps {
  acquisitionFile?: Api_AcquisitionFile;
  isEditing: boolean;
  activeEditForm?: EditFormNames;
  selectedMenuIndex: number;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
  onSuccess: () => void;
}

export const ViewSelector: React.FunctionComponent<IViewSelectorProps> = props => {
  // render edit forms
  if (props.isEditing && !!props.acquisitionFile) {
    switch (props.activeEditForm) {
      case EditFormNames.acquisitionSummary:
        return (
          <UpdateAcquisitionContainer
            acquisitionFile={props.acquisitionFile}
            setContainerState={props.setContainerState}
            onSuccess={props.onSuccess}
          />
        );

      case EditFormNames.propertyDetails:
        return <></>;

      default:
        throw Error('Active edit form not defined');
    }
  } else {
    // render read-only views
    if (props.selectedMenuIndex === 0) {
      return (
        <AcquisitionFileTabs
          acquisitionFile={props.acquisitionFile}
          setContainerState={props.setContainerState}
        />
      );
    } else {
      return <></>;
    }
  }
};

export default ViewSelector;
