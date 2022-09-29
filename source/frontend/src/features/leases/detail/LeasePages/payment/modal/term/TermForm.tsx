import { Check, FastCurrencyInput, FastDatePicker, Input, Select } from 'components/common/form';
import * as API from 'constants/API';
import { LeaseTermStatusTypes } from 'constants/index';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { Formik, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { defaultFormLeaseTerm, IFormLeaseTerm } from 'interfaces/ILeaseTerm';
import * as React from 'react';
import { useContext } from 'react';

import * as Styled from '../../styles';
import { StyledFormBody } from '../../styles';
import { LeaseTermSchema } from './TermsYupSchema';

export interface ITermFormProps {
  formikRef: React.Ref<FormikProps<IFormLeaseTerm>>;
  onSave: (values: IFormLeaseTerm) => void;
  initialValues?: IFormLeaseTerm;
}

/**
 * Internal Form intended to be displayed within a modal window. will be initialized with default values (including NEXER status)
 * if not initialValues provided. Otherwise will display the passed lease term. Save/Cancel triggered externally via passed formikRef.
 * @param {ITermFormProps} props
 */
export const TermForm: React.FunctionComponent<ITermFormProps> = ({
  initialValues,
  formikRef,
  onSave,
}) => {
  const { lease } = useContext(LeaseStateContext);
  const lookups = useLookupCodeHelpers();
  const paymentFrequencyOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_FREQUENCY_TYPES);
  const leaseTermStatusOptions = lookups.getOptionsByType(API.LEASE_TERM_STATUS_TYPES);
  return (
    <Formik
      innerRef={formikRef}
      enableReinitialize
      validationSchema={LeaseTermSchema}
      onSubmit={values => {
        onSave(values);
      }}
      initialValues={{
        ...defaultFormLeaseTerm,
        ...initialValues,
        leaseId: lease?.id,
        statusTypeCode: initialValues?.statusTypeCode?.id
          ? initialValues?.statusTypeCode
          : { id: LeaseTermStatusTypes.NOT_EXERCISED },
        leaseRowVersion: lease?.rowVersion,
      }}
    >
      {formikProps => (
        <StyledFormBody>
          <Styled.FlexRowDiv>
            <FastDatePicker
              required
              label="Start date:"
              field="startDate"
              formikProps={formikProps}
            />
            <FastDatePicker label="End date:" field="expiryDate" formikProps={formikProps} />
          </Styled.FlexRowDiv>
          <Select
            placeholder="Select"
            label="Payment frequency:"
            field="leasePmtFreqTypeCode.id"
            options={paymentFrequencyOptions}
          />
          <FastCurrencyInput
            formikProps={formikProps}
            label="Agreed payment ($)"
            field="paymentAmount"
          />
          <Input
            label="Payments due"
            field="paymentDueDate"
            tooltip={`Arrangement for payments, such as "1st of each month" or "1st & 15th" etc`}
          />
          <Check
            label="Subject to GST?"
            field="isGstEligible"
            radioLabelOne="Y"
            radioLabelTwo="N"
            type="radio"
          />
          <Select label="Term Status" field="statusTypeCode.id" options={leaseTermStatusOptions} />
        </StyledFormBody>
      )}
    </Formik>
  );
};

export default TermForm;
