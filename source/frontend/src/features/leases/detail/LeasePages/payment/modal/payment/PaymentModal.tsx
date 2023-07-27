import { FormikProps } from 'formik';
import * as React from 'react';
import { useRef } from 'react';

import * as CommonStyled from '@/components/common/styles';
import { Api_LeaseTerm } from '@/models/api/LeaseTerm';

import { FormLeasePayment } from '../../models';
import { PaymentForm } from './PaymentForm';

export interface IPaymentModalProps {
  initialValues?: FormLeasePayment;
  displayModal?: boolean;
  terms: Api_LeaseTerm[];
  onCancel: () => void;
  onSave: (values: FormLeasePayment) => void;
}

/**
 * Modal displaying form allowing add/update lease terms. Save button triggers internal formik validation and submit.
 * @param {IPaymentModalProps} props
 */
export const PaymentModal: React.FunctionComponent<React.PropsWithChildren<IPaymentModalProps>> = ({
  initialValues,
  displayModal,
  terms,
  onCancel,
  onSave,
}) => {
  const formikRef = useRef<FormikProps<FormLeasePayment>>(null);
  return (
    <CommonStyled.PrimaryGenericModal
      title="Payment details"
      display={displayModal}
      okButtonText="Save payment"
      cancelButtonText="Cancel"
      handleCancel={onCancel}
      handleOk={() => formikRef?.current?.submitForm()}
      message={
        <PaymentForm
          formikRef={formikRef}
          initialValues={initialValues}
          onSave={onSave}
          terms={terms}
        />
      }
    />
  );
};

export default PaymentModal;
