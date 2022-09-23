import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { Formik, FormikProps } from 'formik';
import { defaultFormLeasePayment, IFormLeasePayment } from 'interfaces';
import * as React from 'react';
import { useContext } from 'react';

import { isActualGstEligible } from '../../TermPaymentsContainer';
import PaymentFormContent from './PaymentFormContent';
import { PaymentsYupSchema } from './PaymentsYupSchema';

export interface IPaymentFormProps {
  formikRef: React.Ref<FormikProps<IFormLeasePayment>>;
  onSave: (values: IFormLeasePayment) => void;
  initialValues?: IFormLeasePayment;
  isReceived?: boolean;
}

/**
 * Internal Form intended to be displayed within a modal window. will be initialized with default values
 * if not initialValues provided. Otherwise will display the passed lease payment. Save/Cancel triggered externally via passed formikRef.
 * @param {IPaymentFormProps} props
 */
export const PaymentForm: React.FunctionComponent<IPaymentFormProps> = ({
  initialValues,
  formikRef,
  onSave,
  isReceived,
}: IPaymentFormProps) => {
  const { lease } = useContext(LeaseStateContext);
  let isGstEligible = false;
  if (initialValues?.leaseTermId) {
    isGstEligible = isActualGstEligible(initialValues?.leaseTermId, lease?.terms ?? []);
  }

  return (
    <Formik
      innerRef={formikRef}
      enableReinitialize
      validationSchema={PaymentsYupSchema}
      onSubmit={values => {
        onSave(values);
      }}
      initialValues={{
        ...defaultFormLeasePayment,
        ...initialValues,
        leasePaymentMethodType: { id: 'CHEQ' },
        amountGst: isGstEligible ? initialValues?.amountGst ?? '' : '',
      }}
    >
      <PaymentFormContent isReceived={isReceived} isGstEligible={isGstEligible} />
    </Formik>
  );
};

export default PaymentForm;
