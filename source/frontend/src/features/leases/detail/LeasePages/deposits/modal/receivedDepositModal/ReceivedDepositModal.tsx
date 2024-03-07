import { FormikProps } from 'formik';
import * as React from 'react';
import { useRef } from 'react';
import { FaDollarSign } from 'react-icons/fa';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { isValidId } from '@/utils';

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
  const modalTitle = !isValidId(initialValues.id) ? 'Add a Deposit' : 'Edit Deposit';
  return (
    <GenericModal
      variant="info"
      modalSize={ModalSize.LARGE}
      title={modalTitle}
      display={display}
      okButtonText="Yes"
      cancelButtonText="No"
      headerIcon={<FaDollarSign size={22} />}
      handleCancel={onCancel}
      handleOk={() => {
        formikRef?.current?.submitForm();
      }}
      message={
        <>
          <ReceivedDepositForm
            formikRef={formikRef}
            initialValues={initialValues}
            onSave={onSave}
          />
        </>
      }
    />
  );
};

export default ReceivedDepositModal;
