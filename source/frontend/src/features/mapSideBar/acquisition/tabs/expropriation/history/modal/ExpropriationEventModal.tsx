import { FormikProps } from 'formik';
import React, { useRef } from 'react';
import { FaCalendarAlt } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';

import { ExpropriationEventFormModel } from '../models';
import { ExpropriationEventForm } from './ExpropriationEventForm';

export interface IExpropriationEventModalProps {
  acquisitionFileId: number;
  display?: boolean;
  initialValues?: ExpropriationEventFormModel;
  payeeOptions: PayeeOption[];
  onSave: (values: ExpropriationEventFormModel) => void;
  onCancel: () => void;
}

export const ExpropriationEventModal: React.FunctionComponent<IExpropriationEventModalProps> = ({
  acquisitionFileId,
  display,
  initialValues,
  payeeOptions,
  onSave,
  onCancel,
}) => {
  const formikRef = useRef<FormikProps<ExpropriationEventFormModel>>(null);
  return (
    <GenericModal
      variant="info"
      title="Expropriation Date History"
      headerIcon={<FaCalendarAlt size={22} />}
      display={display}
      modalSize={ModalSize.MEDIUM}
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
          payeeOptions={payeeOptions}
          onSave={onSave}
        />
      }
    ></GenericModal>
  );
};

export default ExpropriationEventModal;
