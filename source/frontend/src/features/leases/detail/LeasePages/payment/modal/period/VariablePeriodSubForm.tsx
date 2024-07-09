import { useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { Check, FastCurrencyInput, Select } from '@/components/common/form';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

interface IVariablePeriodSubFormProps {
  frequencyField: string;
  paymentAmountField: string;
  isGstEligibleField: string;
}

const VariablePeriodSubForm: React.FunctionComponent<IVariablePeriodSubFormProps> = ({
  frequencyField,
  paymentAmountField,
  isGstEligibleField,
}) => {
  const formikProps = useFormikContext();
  const lookups = useLookupCodeHelpers();
  const paymentFrequencyOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_FREQUENCY_TYPES);
  return (
    <>
      <Row>
        <Col md={6}>
          <Select
            placeholder="Select"
            label="Payment frequency:"
            field={`${frequencyField}.id`}
            options={paymentFrequencyOptions}
          />
        </Col>
      </Row>
      <Row>
        <Col>
          <FastCurrencyInput
            formikProps={formikProps}
            label="Agreed payment ($)"
            field={paymentAmountField}
          />
        </Col>
        <Col>
          <Check
            label="Subject to GST?"
            field={isGstEligibleField}
            radioLabelOne="Y"
            radioLabelTwo="N"
            type="radio"
          />
        </Col>
      </Row>
    </>
  );
};

export default VariablePeriodSubForm;
