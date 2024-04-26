import { FormikProps } from 'formik/dist/types';
import { useRef } from 'react';

import GenericModal from '@/components/common/GenericModal';
import { isValidId } from '@/utils';

import { FormLeaseDepositReturn } from '../../models/FormLeaseDepositReturn';
import { ReturnDepositForm } from './ReturnDepositForm';

export interface IReturnedDepositModalProps {
  initialValues?: FormLeaseDepositReturn;
  display: boolean;
  onCancel: () => void;
  onSave: (values: FormLeaseDepositReturn) => void;
}

/**
 * Modal displaying form allowing returned deposits modifications. Save button triggers internal formik validation and submit.
 * @param {IReturnedDepositModalProps} props
 */
export const ReturnedDepositModal: React.FunctionComponent<
  React.PropsWithChildren<IReturnedDepositModalProps>
> = ({ initialValues, display, onCancel, onSave }) => {
  const formikRef = useRef<FormikProps<FormLeaseDepositReturn>>(null);
  const modalTitle = !isValidId(initialValues?.id) ? 'Return a Deposit' : 'Edit a Deposit Return';
  return (
    <GenericModal
      variant="info"
      title={modalTitle}
      display={display}
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleCancel={onCancel}
      handleOk={() => {
        formikRef?.current?.submitForm();
      }}
      message={
        initialValues && (
          <ReturnDepositForm formikRef={formikRef} initialValues={initialValues} onSave={onSave} />
        )
      }
    />
  );
};

export default ReturnedDepositModal;
