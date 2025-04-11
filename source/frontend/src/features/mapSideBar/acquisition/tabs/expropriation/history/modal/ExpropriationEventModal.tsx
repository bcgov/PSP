import { FormikProps } from 'formik';
import React, { useRef } from 'react';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';

import { ExpropriationEventFormModel } from '../models';
import { ExpropriationEventForm } from './ExpropriationEventForm';

export interface IExpropriationEventModalProps {
  acquisitionFileId: number;
  display?: boolean;
  initialValues?: ExpropriationEventFormModel;
  onSave: (values: ExpropriationEventFormModel) => void;
  onCancel: () => void;
}

export const ExpropriationEventModal: React.FunctionComponent<IExpropriationEventModalProps> = ({
  acquisitionFileId,
  display,
  initialValues,
  onSave,
  onCancel,
}) => {
  const formikRef = useRef<FormikProps<ExpropriationEventFormModel>>(null);
  return (
    <GenericModal
      variant="info"
      title="Expropriation Date History"
      display={display}
      modalSize={ModalSize.LARGE}
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleOk={() => {
        formikRef?.current?.submitForm();
      }}
      handleCancel={onCancel}
      message={
        <ExpropriationEventForm
          formikRef={formikRef}
          initialValues={initialValues ?? new ExpropriationEventFormModel(acquisitionFileId)}
          onSave={onSave}
        />
      }
    ></GenericModal>
  );
};

export default ExpropriationEventModal;
