import { FormikProps } from 'formik/dist/types';
import { useRef } from 'react';

import GenericModal from '@/components/common/GenericModal';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';

import { FormLeasePayment } from '../../models';
import { PaymentForm } from './PaymentForm';

export interface IPaymentModalProps {
  initialValues?: FormLeasePayment;
  displayModal?: boolean;
  periods: ApiGen_Concepts_LeasePeriod[];
  onCancel: () => void;
  onSave: (values: FormLeasePayment) => void;
}

/**
 * Modal displaying form allowing add/update lease peridos. Save button triggers internal formik validation and submit.
 * @param {IPaymentModalProps} props
 */
export const PaymentModal: React.FunctionComponent<React.PropsWithChildren<IPaymentModalProps>> = ({
  initialValues,
  displayModal,
  periods,
  onCancel,
  onSave,
}) => {
  const formikRef = useRef<FormikProps<FormLeasePayment>>(null);
  return (
    <GenericModal
      variant="info"
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
          periods={periods}
        />
      }
    />
  );
};

export default PaymentModal;
