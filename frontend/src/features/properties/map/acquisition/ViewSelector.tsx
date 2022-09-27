import { FormikProps } from 'formik';
import { InventoryTabNames, InventoryTabs } from 'features/mapSideBar/tabs/InventoryTabs';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';

import { PropertyFileContainer } from '../shared/detail/PropertyFileContainer';
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
  selectedIndex: number;
}

export const ViewSelector = React.forwardRef<FormikProps<any>, IViewSelectorProps>(
  (props, formikRef) => {
  if (props.selectedIndex === 0) {
    // render edit forms
    if (props.isEditing && !!props.acquisitionFile) {
      switch (props.activeEditForm) {
        case EditFormNames.acquisitionSummary:
          return (
            <UpdateAcquisitionContainer
              ref={formikRef}
              acquisitionFile={props.acquisitionFile}
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
    const properties = props.acquisitionFile?.fileProperties || [];
    const selectedPropertyIndex = props.selectedIndex - 1;
    const acquisitionFileProperty = properties[selectedPropertyIndex];
    acquisitionFileProperty.file = props.acquisitionFile;
    return (
      <PropertyFileContainer
        setEditFileProperty={() =>
          props.setContainerState({
            isEditing: true,
            activeEditForm: EditFormNames.propertyDetails,
          })
        }
        fileProperty={acquisitionFileProperty}
        defaultTab={InventoryTabNames.property}
        customTabs={[]}
        View={InventoryTabs}
      />
    );
  }
};

export default ViewSelector;
