import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Check, FastCurrencyInput, Select } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ISystemConstant } from '@/store/slices/systemConstants';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { formatMoney, round } from '@/utils';

import { FormLeasePeriod } from '../../models';

interface IVariablePeriodSubFormProps {
  frequencyField: string;
  paymentAmountField: string;
  isGstEligibleField: string;
  gstAmountField: string;
  gstConstant: ISystemConstant;
}

const VariablePeriodSubForm: React.FunctionComponent<IVariablePeriodSubFormProps> = ({
  frequencyField,
  paymentAmountField,
  isGstEligibleField,
  gstAmountField,
  gstConstant,
}) => {
  const formikProps = useFormikContext<FormLeasePeriod>();
  const lookups = useLookupCodeHelpers();
  const paymentFrequencyOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_FREQUENCY_TYPES);

  const isGstEligible = getIn(formikProps.values, isGstEligibleField);
  const paymentAmount = getIn(formikProps.values, paymentAmountField) ?? 0;
  const setFieldValue = formikProps.setFieldValue;
  const touched = getIn(formikProps.touched, paymentAmountField);
  const onGstChange = useCallback(
    (field: string, values: boolean) => {
      if (values === true) {
        const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : 5;
        const calculated = round(paymentAmount * (gstDecimal / 100));
        setFieldValue(gstAmountField, calculated);
      } else {
        setFieldValue(gstAmountField, '');
      }
    },
    [gstConstant, paymentAmount, setFieldValue, gstAmountField],
  );

  useEffect(() => {
    if (touched) {
      onGstChange('', isGstEligible);
    }
  }, [isGstEligible, paymentAmount, onGstChange, touched]);

  const calculateTotal = (amount: NumberFieldValue, gstAmount: NumberFieldValue): number => {
    const total = Number(amount) + Number(gstAmount);
    return isNaN(total) ? 0 : total;
  };

  const initialGstAmount = getIn(formikProps.initialValues, gstAmountField) ?? '';
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
            label="Payment (before tax)"
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
            handleChange={onGstChange}
          />
          {initialGstAmount !== getIn(formikProps.values, gstAmountField) &&
            getIn(formikProps.values, isGstEligibleField) === false && (
              <StyledRedCol className="pt-4">
                You have selected to remove subject to GST. GST amount previously added will be
                removed.
              </StyledRedCol>
            )}
        </Col>
      </Row>
      {getIn(formikProps.values, isGstEligibleField) === true && (
        <Row>
          <Col xs="6">
            <FastCurrencyInput
              formikProps={formikProps}
              label="GST Amount"
              field={gstAmountField}
            />
          </Col>
        </Row>
      )}
      <SectionField label="Total Payment" labelWidth="auto">
        {formatMoney(
          calculateTotal(
            getIn(formikProps.values, paymentAmountField),
            getIn(formikProps.values, gstAmountField),
          ),
        )}
      </SectionField>
    </>
  );
};

export default VariablePeriodSubForm;

const StyledRedCol = styled(Col)`
  color: red;
`;
