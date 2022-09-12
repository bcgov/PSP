import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { ModalContext } from 'contexts/modalContext';
import { dequal } from 'dequal';
import { getCancelModalProps } from 'hooks/useModalContext';
import { Api_Activity } from 'models/api/Activity';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import * as React from 'react';
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
  onSave: () => Promise<Api_Activity | undefined>;
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
  const { setModalProps, setDisplayModal } = useContext(ModalContext);
  return (
    <GenericModal
      modalSize={ModalSize.MEDIUM}
      title="Related properties"
      display={display}
      setDisplay={setDisplay}
      closeButton={true}
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleCancel={() => {
        if (
          !dequal(
            originalSelectedProperties.map(op => op.id),
            selectedFileProperties.map(fp => fp.id),
          )
        ) {
          setModalProps({
            ...getCancelModalProps(),
            display: true,
            handleOk: () => {
              setSelectedFileProperties(originalSelectedProperties);
              setDisplay(false);
              setDisplayModal(false);
            },
          });
        } else {
          setDisplay(false);
          setDisplayModal(false);
        }
      }}
      handleOk={async () => {
        if (activityModel) {
          activityModel.actInstPropFiles =
            selectedFileProperties.map(p => ({
              activityId: activityModel?.id,
              propertyFileId: p?.id,
            })) ?? [];
          const updatedActivity = await onSave();
          // if the activity was updated successfully, hide the modal.
          if (!!updatedActivity) {
            setDisplay(false);
          }
        }
      }}
      message={
        <PropertyActivityTable
          fileProperties={allProperties ?? []}
          selectedFileProperties={selectedFileProperties}
          setSelectedFileProperties={setSelectedFileProperties}
        />
      }
    />
  );
};

export default ActivityPropertyModal;
