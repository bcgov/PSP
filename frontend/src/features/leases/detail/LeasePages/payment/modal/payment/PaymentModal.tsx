import * as CommonStyled from 'components/common/styles';
import { FormikProps } from 'formik';
import { IFormLeasePayment } from 'interfaces';
import * as React from 'react';
import { useRef } from 'react';

import { PaymentForm } from './PaymentForm';

export interface IPaymentModalProps {
  initialValues?: IFormLeasePayment;
  displayModal?: boolean;
  onCancel: () => void;
  onSave: (values: IFormLeasePayment) => void;
}

/**
 * Modal displaying form allowing add/update lease terms. Save button triggers internal formik validation and submit.
 * @param {IPaymentModalProps} props
 */
export const PaymentModal: React.FunctionComponent<IPaymentModalProps> = ({
  initialValues,
  displayModal,
  onCancel,
  onSave,
}) => {
  const formikRef = useRef<FormikProps<IFormLeasePayment>>(null);
  return (
    <CommonStyled.PrimaryGenericModal
      title="Payment details"
      display={displayModal}
      okButtonText="Save payment"
      cancelButtonText="Cancel"
      handleCancel={onCancel}
      handleOk={() => formikRef?.current?.submitForm()}
      message={<PaymentForm formikRef={formikRef} initialValues={initialValues} onSave={onSave} />}
    />
  );
};

export default PaymentModal;
