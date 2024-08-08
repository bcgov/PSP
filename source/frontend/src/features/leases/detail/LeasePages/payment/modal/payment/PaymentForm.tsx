import { Formik, FormikProps, validateYupSchema, yupToFormErrors } from 'formik';

import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';

import { FormLeasePayment } from '../../models';
import PaymentFormContent from './PaymentFormContent';
import { PaymentsYupSchema } from './PaymentsYupSchema';

export interface IPaymentFormProps {
  formikRef: React.Ref<FormikProps<FormLeasePayment>>;
  onSave: (values: FormLeasePayment) => void;
  initialValues?: FormLeasePayment;
  isReceived?: boolean;
  periods: ApiGen_Concepts_LeasePeriod[];
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
  periods,
}: IPaymentFormProps) => {
  const isGstEligible = false;

  const currentPeriod = periods.find(t => t.id === initialValues?.leasePeriodId);

  return (
    <Formik<FormLeasePayment>
      innerRef={formikRef}
      enableReinitialize
      validateOnChange={false}
      validate={values => {
        let errors = {};
        try {
          validateYupSchema(values, PaymentsYupSchema, true);
        } catch (err) {
          errors = yupToFormErrors(err);
        }
        if (values.amountTotal !== +values.amountGst + +values.amountPreTax) {
          return {
            ...errors,
            form: 'Expected payment amount and GST amount must sum to the total received',
          };
        }

        return errors;
      }}
      onSubmit={values => {
        onSave(values);
      }}
      initialValues={{
        ...initialValues,
        amountGst: isGstEligible ? initialValues?.amountGst ?? '' : '',
      }}
    >
      <PaymentFormContent
        periods={periods}
        isReceived={!!isReceived}
        isGstEligible={!!isGstEligible}
        isVariable={currentPeriod?.isVariable}
      />
    </Formik>
  );
};

export default PaymentForm;
