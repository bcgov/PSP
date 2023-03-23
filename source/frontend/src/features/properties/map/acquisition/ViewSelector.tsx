import { FileTypes } from 'constants/fileTypes';
import { FileTabNames } from 'features/mapSideBar/tabs/FileTabs';
import { InventoryTabNames, InventoryTabs } from 'features/mapSideBar/tabs/InventoryTabs';
import { UpdatePropertyDetailsContainer } from 'features/mapSideBar/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import TakesUpdateContainer from 'features/mapSideBar/tabs/takes/update/TakesUpdateContainer';
import TakesUpdateForm from 'features/mapSideBar/tabs/takes/update/TakesUpdateForm';
import { FormikProps } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';

import { PropertyFileContainer } from '../shared/detail/PropertyFileContainer';
import { AcquisitionContainerState } from './AcquisitionContainer';
import AcquisitionFileTabs from './detail/AcquisitionFileTabs';
import { EditFormNames } from './EditFormNames';
import { UpdateAcquisitionChecklistContainer } from './update/checklist/UpdateAcquisitionChecklistContainer';
import { UpdateAcquisitionChecklistForm } from './update/checklist/UpdateAcquisitionChecklistForm';
import { UpdateAcquisitionContainer } from './update/summary/UpdateAcquisitionContainer';

export interface IViewSelectorProps {
  acquisitionFile?: Api_AcquisitionFile;
  isEditing: boolean;
  activeEditForm?: EditFormNames;
  selectedMenuIndex: number;
  defaultFileTab: FileTabNames;
  defaultPropertyTab: InventoryTabNames;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
  onSuccess: () => void;
  ref: React.RefObject<FormikProps<any>>;
}

export const ViewSelector = React.forwardRef<FormikProps<any>, IViewSelectorProps>(
  (props, formikRef) => {
    // render edit forms
    if (props.isEditing && !!props.acquisitionFile) {
      const propertyFile = getAcquisitionFileProperty(
        props.acquisitionFile,
        props.selectedMenuIndex,
      );
      switch (props.activeEditForm) {
        case EditFormNames.acquisitionChecklist:
          return (
            <UpdateAcquisitionChecklistContainer
              formikRef={formikRef}
              acquisitionFile={props.acquisitionFile}
              onSuccess={props.onSuccess}
              View={UpdateAcquisitionChecklistForm}
            />
          );

        case EditFormNames.acquisitionSummary:
          return (
            <UpdateAcquisitionContainer
              ref={formikRef}
              acquisitionFile={props.acquisitionFile}
              onSuccess={props.onSuccess}
            />
          );

        case EditFormNames.propertyDetails:
          if (propertyFile?.property?.id === undefined) {
            throw Error('Cannot edit property without a valid id');
          }
          return (
            <UpdatePropertyDetailsContainer
              id={propertyFile?.property?.id}
              onSuccess={props.onSuccess}
              ref={formikRef}
            />
          );
        case EditFormNames.takes:
          return (
            <TakesUpdateContainer
              fileProperty={propertyFile}
              View={TakesUpdateForm}
              ref={formikRef}
              onSuccess={() =>
                props.setContainerState({
                  isEditing: false,
                  activeEditForm: undefined,
                })
              }
            />
          );

        default:
          throw Error('Active edit form not defined');
      }
    } else {
      // render read-only views
      if (props.selectedMenuIndex === 0) {
        return (
          <AcquisitionFileTabs
            acquisitionFile={props.acquisitionFile}
            defaultTab={props.defaultFileTab}
            setContainerState={props.setContainerState}
          />
        );
      } else {
        if (!!props.acquisitionFile) {
          return (
            <PropertyFileContainer
              setEditFileProperty={() =>
                props.setContainerState({
                  isEditing: true,
                  activeEditForm: EditFormNames.propertyDetails,
                  defaultPropertyTab: InventoryTabNames.property,
                })
              }
              setEditTakes={() =>
                props.setContainerState({
                  isEditing: true,
                  activeEditForm: EditFormNames.takes,
                  defaultPropertyTab: InventoryTabNames.takes,
                })
              }
              fileProperty={getAcquisitionFileProperty(
                props.acquisitionFile,
                props.selectedMenuIndex,
              )}
              defaultTab={props.defaultPropertyTab}
              customTabs={[]}
              View={InventoryTabs}
              fileContext={FileTypes.Acquisition}
            />
          );
        } else {
          return null;
        }
      }
    }
  },
);

const getAcquisitionFileProperty = (
  acquisitionFile: Api_AcquisitionFile,
  selectedMenuIndex: number,
) => {
  const properties = acquisitionFile?.fileProperties || [];
  const selectedPropertyIndex = selectedMenuIndex - 1;
  const acquisitionFileProperty = properties[selectedPropertyIndex];
  if (!!acquisitionFileProperty.file) {
    acquisitionFileProperty.file = acquisitionFile;
  }
  return acquisitionFileProperty;
};

export default ViewSelector;
