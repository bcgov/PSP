import * as CommonStyled from 'components/common/styles';
import { FormikProps } from 'formik';
import { IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import * as React from 'react';
import { useRef } from 'react';

import PaymentForm from './PaymentForm';

export interface IPaymentModalProps {
  initialValues?: IFormLeaseTerm;
  displayModal?: boolean;
  onCancel: () => void;
  onSave: (values: IFormLeaseTerm) => void;
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
  const formikRef = useRef<FormikProps<IFormLeaseTerm>>(null);
  return (
    <CommonStyled.PrimaryGenericModal
      title="Add a Term"
      display={displayModal}
      okButtonText="Save term"
      cancelButtonText="Cancel"
      handleCancel={onCancel}
      handleOk={() => formikRef?.current?.submitForm()}
      message={<PaymentForm formikRef={formikRef} initialValues={initialValues} onSave={onSave} />}
    />
  );
};

export default PaymentModal;
