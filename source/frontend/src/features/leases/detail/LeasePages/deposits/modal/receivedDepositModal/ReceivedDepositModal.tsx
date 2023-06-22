import { FormikProps } from 'formik';
import * as React from 'react';
import { useRef } from 'react';

import * as CommonStyled from '@/components/common/styles';

import { FormLeaseDeposit } from '../../models/FormLeaseDeposit';
import { ReceivedDepositForm } from './ReceivedDepositForm';

export interface IReceivedDepositModalProps {
  initialValues: FormLeaseDeposit;
  display: boolean;
  onCancel: () => void;
  onSave: (values: FormLeaseDeposit) => void;
}

/**
 * Modal displaying form allowing add/update lease deposits. Save button triggers internal formik validation and submit.
 * @param {IReceivedDepositModalProps} props
 */
export const ReceivedDepositModal: React.FunctionComponent<
  React.PropsWithChildren<IReceivedDepositModalProps>
> = ({ initialValues, display, onCancel, onSave }) => {
  const formikRef = useRef<FormikProps<FormLeaseDeposit>>(null);
  const modalTitle = initialValues.id === undefined ? 'Add a Deposit' : 'Edit Deposit';
  return (
    <CommonStyled.PrimaryGenericModal
      title={modalTitle}
      display={display}
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleCancel={onCancel}
      handleOk={() => {
        formikRef?.current?.submitForm();
      }}
      message={
        <ReceivedDepositForm formikRef={formikRef} initialValues={initialValues} onSave={onSave} />
      }
    />
  );
};

export default ReceivedDepositModal;
