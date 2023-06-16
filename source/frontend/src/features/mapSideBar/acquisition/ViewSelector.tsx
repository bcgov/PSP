import { FormikProps } from 'formik';
import React from 'react';

import { FileTypes } from '@/constants/fileTypes';
import { InventoryTabNames, InventoryTabs } from '@/features/mapSideBar/property/InventoryTabs';
import { UpdatePropertyDetailsContainer } from '@/features/mapSideBar/property/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import TakesUpdateContainer from '@/features/mapSideBar/property/tabs/takes/update/TakesUpdateContainer';
import TakesUpdateForm from '@/features/mapSideBar/property/tabs/takes/update/TakesUpdateForm';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { PropertyFileContainer } from '@/features/mapSideBar/shared/detail/PropertyFileContainer';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

import { AcquisitionContainerState } from './AcquisitionContainer';
import { EditFormType } from './EditFormNames';
import AcquisitionFileTabs from './tabs/AcquisitionFileTabs';
import { UpdateAgreementsContainer } from './tabs/agreement/update/UpdateAgreementsContainer';
import { UpdateAgreementsForm } from './tabs/agreement/update/UpdateAgreementsForm';
import { UpdateAcquisitionChecklistContainer } from './tabs/checklist/update/UpdateAcquisitionChecklistContainer';
import { UpdateAcquisitionChecklistForm } from './tabs/checklist/update/UpdateAcquisitionChecklistForm';
import { UpdateAcquisitionContainer } from './tabs/fileDetails/update/UpdateAcquisitionContainer';
import { UpdateAcquisitionForm } from './tabs/fileDetails/update/UpdateAcquisitionForm';
import UpdateStakeHolderContainer from './tabs/stakeholders/update/UpdateStakeHolderContainer';
import UpdateStakeHolderForm from './tabs/stakeholders/update/UpdateStakeHolderForm';

export interface IViewSelectorProps {
  acquisitionFile?: Api_AcquisitionFile;
  isEditing: boolean;
  activeEditForm?: EditFormType;
  selectedMenuIndex: number;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
  onSuccess: () => void;
  ref: React.RefObject<FormikProps<any>>;
}

export const ViewSelector = React.forwardRef<FormikProps<any>, IViewSelectorProps>(
  (props, formikRef) => {
    // render edit forms
    if (props.isEditing && !!props.acquisitionFile) {
      // File-based tabs
      if (props.selectedMenuIndex === 0) {
        switch (props.activeEditForm) {
          case EditFormType.ACQUISITION_CHECKLIST:
            return (
              <UpdateAcquisitionChecklistContainer
                formikRef={formikRef}
                acquisitionFile={props.acquisitionFile}
                onSuccess={props.onSuccess}
                View={UpdateAcquisitionChecklistForm}
              />
            );

          case EditFormType.ACQUISITION_SUMMARY:
            return (
              <UpdateAcquisitionContainer
                ref={formikRef}
                acquisitionFile={props.acquisitionFile}
                onSuccess={props.onSuccess}
                View={UpdateAcquisitionForm}
              />
            );

          case EditFormType.AGREEMENTS:
            return (
              <UpdateAgreementsContainer
                acquisitionFileId={props.acquisitionFile.id || -1}
                View={UpdateAgreementsForm}
                formikRef={formikRef}
                onSuccess={() =>
                  props.setContainerState({
                    isEditing: false,
                    activeEditForm: undefined,
                  })
                }
              />
            );

          case EditFormType.STAKEHOLDERS:
            return (
              <UpdateStakeHolderContainer
                View={UpdateStakeHolderForm}
                formikRef={formikRef}
                acquisitionFile={props.acquisitionFile}
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
        // Property-based tabs
        const propertyFile = getAcquisitionFileProperty(
          props.acquisitionFile,
          props.selectedMenuIndex,
        );
        switch (props.activeEditForm) {
          case EditFormType.PROPERTY_DETAILS:
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
          case EditFormType.TAKES:
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
                  activeEditForm: EditFormType.PROPERTY_DETAILS,
                  defaultPropertyTab: InventoryTabNames.property,
                })
              }
              setEditTakes={() =>
                props.setContainerState({
                  isEditing: true,
                  activeEditForm: EditFormType.TAKES,
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
