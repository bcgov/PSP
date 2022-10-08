import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { ModalContext } from 'contexts/modalContext';
import { dequal } from 'dequal';
import { getCancelModalProps } from 'hooks/useModalContext';
import { Api_Activity } from 'models/api/Activity';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import React from 'react';
import { useContext } from 'react';

import PropertyActivityTable from '../properties/PropertyActivityTable';
import { ActivityModel } from './models';

export interface IActivityPropertyModalProps {
  display: boolean;
  setDisplay: (display: boolean) => void;
  activityModel?: ActivityModel;
  allProperties: Api_PropertyFile[];
  originalSelectedProperties: Api_PropertyFile[];
  selectedFileProperties: Api_PropertyFile[];
  setSelectedFileProperties: (properties: Api_PropertyFile[]) => void;
  onSave: (activity: ActivityModel) => Promise<Api_Activity | undefined>;
}

export const ActivityPropertyModal: React.FunctionComponent<IActivityPropertyModalProps> = ({
  display,
  setDisplay,
  activityModel,
  allProperties,
  originalSelectedProperties,
  selectedFileProperties,
  setSelectedFileProperties,
  onSave,
}) => {
  const { setModalContent, setDisplayModal } = useContext(ModalContext);

  const handleCancel = () => {
    if (
      !dequal(
        originalSelectedProperties.map(op => op.id),
        selectedFileProperties.map(fp => fp.id),
      )
    ) {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          setSelectedFileProperties(originalSelectedProperties);
          setDisplay(false);
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    } else {
      setDisplay(false);
      setDisplayModal(false);
    }
  };

  const handleOk = async () => {
    if (activityModel && allProperties?.length > 0) {
      activityModel.actInstPropFiles =
        selectedFileProperties.map(p => ({
          activityId: activityModel?.id,
          propertyFileId: p?.id,
        })) ?? [];
      const updatedActivity = await onSave(activityModel);
      // if the activity was updated successfully, hide the modal.
      if (!!updatedActivity) {
        setDisplay(false);
      }
    }
  };

  return (
    <GenericModal
      modalSize={ModalSize.MEDIUM}
      title="Related properties"
      display={display}
      setDisplay={setDisplay}
      closeButton={true}
      okButtonText={allProperties?.length > 0 ? 'Save' : 'Ok'}
      cancelButtonText={allProperties?.length > 0 ? 'Cancel' : undefined}
      handleCancel={allProperties?.length > 0 ? handleCancel : undefined}
      handleOk={allProperties?.length > 0 ? handleOk : undefined}
      message={
        <ModalContent
          allProperties={allProperties}
          selectedFileProperties={selectedFileProperties}
          setSelectedFileProperties={setSelectedFileProperties}
        />
      }
    />
  );
};

export default ActivityPropertyModal;

interface IModalContentProps {
  allProperties: Api_PropertyFile[];
  selectedFileProperties: Api_PropertyFile[];
  setSelectedFileProperties: (properties: Api_PropertyFile[]) => void;
}

const ModalContent: React.FC<IModalContentProps> = ({
  allProperties,
  selectedFileProperties,
  setSelectedFileProperties,
}) => {
  if (allProperties?.length > 0) {
    return (
      <PropertyActivityTable
        fileProperties={allProperties ?? []}
        selectedFileProperties={selectedFileProperties}
        setSelectedFileProperties={setSelectedFileProperties}
      />
    );
  } else {
    return <>To link activity to one or more properties, add properties to the parent file first</>;
  }
};
