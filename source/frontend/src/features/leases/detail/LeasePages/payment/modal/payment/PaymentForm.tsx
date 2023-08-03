import { Formik, FormikProps } from 'formik';

import { Api_LeaseTerm } from '@/models/api/LeaseTerm';

import { defaultFormLeasePayment, FormLeasePayment, FormLeaseTerm } from '../../models';
import { isActualGstEligible } from '../../TermPaymentsContainer';
import PaymentFormContent from './PaymentFormContent';
import { PaymentsYupSchema } from './PaymentsYupSchema';

export interface IPaymentFormProps {
  formikRef: React.Ref<FormikProps<FormLeasePayment>>;
  onSave: (values: FormLeasePayment) => void;
  initialValues?: FormLeasePayment;
  isReceived?: boolean;
  terms: Api_LeaseTerm[];
}

/**
 * Internal Form intended to be displayed within a modal window. will be initialized with default values
 * if not initialValues provided. Otherwise will display the passed lease payment. Save/Cancel triggered externally via passed formikRef.
 * @param {IPaymentFormProps} props
 */
export const PaymentForm: React.FunctionComponent<React.PropsWithChildren<IPaymentFormProps>> = ({
  initialValues,
  formikRef,
  onSave,
  isReceived,
  terms,
}: IPaymentFormProps) => {
  let isGstEligible = false;
  if (initialValues?.leaseTermId) {
    isGstEligible = isActualGstEligible(
      initialValues?.leaseTermId,
      terms?.map(t => FormLeaseTerm.fromApi(t)) ?? [],
    );
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
      <PaymentFormContent isReceived={!!isReceived} isGstEligible={!!isGstEligible} />
    </Formik>
  );
};

export default PaymentForm;
