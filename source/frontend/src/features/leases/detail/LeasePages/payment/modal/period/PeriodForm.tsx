import { Formik, FormikProps } from 'formik';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import {
  Check,
  FastCurrencyInput,
  FastDatePicker,
  Input,
  Select,
  SelectOption,
} from '@/components/common/form';
import { RadioGroup } from '@/components/common/form/RadioGroup';
import GenericModal from '@/components/common/GenericModal';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as API from '@/constants/API';
import { LeasePeriodStatusTypes } from '@/constants/leaseStatusTypes';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { toTypeCodeNullable } from '@/utils/formUtils';

import { defaultFormLeasePeriod, FormLeasePeriod } from '../../models';
import { StyledFormBody } from '../../styles';
import { LeasePeriodSchema } from './PeriodsYupSchema';
import VariablePeriodSubForm from './VariablePeriodSubForm';

export interface IPeriodFormProps {
  formikRef: React.Ref<FormikProps<FormLeasePeriod>>;
  onSave: (values: FormLeasePeriod) => void;
  initialValues?: FormLeasePeriod;
  lease: ApiGen_Concepts_Lease | undefined;
}

/**
 * Internal Form intended to be displayed within a modal window. will be initialized with default values (including NEXER status)
 * if not initialValues provided. Otherwise will display the passed lease period. Save/Cancel triggered externally via passed formikRef.
 * @param {IPeriodFormProps} props
 */
export const PeriodForm: React.FunctionComponent<React.PropsWithChildren<IPeriodFormProps>> = ({
  initialValues,
  formikRef,
  onSave,
  lease,
}) => {
  const [displayWarningModal, setDisplayWarningModal] = useState(false);
  const lookups = useLookupCodeHelpers();
  const paymentFrequencyOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_FREQUENCY_TYPES);
  const leasePeriodStatusOptions = lookups.getOptionsByType(API.LEASE_PERIOD_STATUS_TYPES);
  const flexiblePeriodOptions: SelectOption[] = [
    { label: 'Fixed', value: 'false' },
    { label: 'Flexible', value: 'true' },
  ];
  const variableRadioGroupValues = [
    {
      radioValue: 'false',
      radioLabel: 'Predetermined',
    },
    {
      radioValue: 'true',
      radioLabel: 'Variable',
    },
  ];

  return (
    <>
      <Formik<FormLeasePeriod>
        innerRef={formikRef}
        validationSchema={LeasePeriodSchema}
        onSubmit={values => {
          onSave(values);
        }}
        initialValues={{
          ...defaultFormLeasePeriod,
          ...initialValues,
          leaseId: lease?.id ?? 0,
          statusTypeCode: initialValues?.statusTypeCode?.id
            ? initialValues?.statusTypeCode
            : toTypeCodeNullable(LeasePeriodStatusTypes.NOT_EXERCISED),
        }}
      >
        {formikProps => {
          return (
            <StyledFormBody>
              <Row>
                <Col md={6}>
                  <SectionField
                    label="Select payment type"
                    labelWidth="12"
                    tooltip="Predetermined Payment Period only accept fixed payment amounts. Select Variable payment type to track variable payments."
                  >
                    <StyledRadioGroup
                      field="isVariable"
                      disabled={!!initialValues?.id}
                      radioValues={variableRadioGroupValues}
                      flexDirection="row"
                    />
                  </SectionField>
                </Col>
              </Row>
              <Row>
                <Col md={6}>
                  <Select
                    label="Period duration:"
                    field="isFlexible"
                    tooltip="Fixed Payment Period Duration has end date. Select Flexible payment period duration to track hold over payments."
                    options={flexiblePeriodOptions}
                  />
                </Col>
              </Row>
              <Row>
                <Col>
                  <FastDatePicker
                    required
                    label="Start date:"
                    field="startDate"
                    formikProps={formikProps}
                    tooltip="Start Date: The start date defined for the period."
                  />
                </Col>
                <Col>
                  <FastDatePicker
                    label={
                      formikProps.values.isFlexible === 'true'
                        ? 'End date (Anticipated):'
                        : 'End date:'
                    }
                    field="expiryDate"
                    formikProps={formikProps}
                    tooltip="End Date: The end date specified for the period."
                    required={formikProps.values.isFlexible === 'false'}
                  />
                </Col>
              </Row>
              {formikProps.values?.isVariable === 'false' ? (
                <>
                  <Row>
                    <Col>
                      <Select
                        placeholder="Select"
                        label="Payment frequency:"
                        field="leasePmtFreqTypeCode.id"
                        options={paymentFrequencyOptions}
                      />
                    </Col>
                    <Col>
                      <FastCurrencyInput
                        formikProps={formikProps}
                        label="Agreed payment ($)"
                        field="paymentAmount"
                      />
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <Input
                        label="Payment due"
                        field="paymentDueDateStr"
                        tooltip={`Arrangement for payments, such as "1st of each month" or "1st & 15th" etc`}
                      />
                    </Col>
                    <Col>
                      <Check
                        label="Subject to GST?"
                        field="isGstEligible"
                        radioLabelOne="Y"
                        radioLabelTwo="N"
                        type="radio"
                      />
                    </Col>
                  </Row>
                  <Row>
                    <Col md={6}>
                      <Select
                        label="Term status"
                        field="statusTypeCode.id"
                        options={leasePeriodStatusOptions}
                      />
                    </Col>
                  </Row>
                </>
              ) : (
                <>
                  <Row>
                    <Col md={6}>
                      <Input
                        label="Payment due"
                        field="paymentDueDateStr"
                        tooltip={`Arrangement for payments, such as "1st of each month" or "1st & 15th" etc`}
                      />
                    </Col>
                  </Row>
                  <Row>
                    <Col md={6}>
                      <Select
                        label="Term status"
                        field="statusTypeCode.id"
                        options={leasePeriodStatusOptions}
                      />
                    </Col>
                  </Row>
                  <StyledSection
                    isCollapsable
                    initiallyExpanded
                    header={
                      <>
                        Add Base Rent
                        <TooltipIcon
                          toolTipId="base-rent-tooltip"
                          toolTip="Fixed Amount of Rent per Payment Payment Period, excluding Operating Expenses."
                        />
                      </>
                    }
                  >
                    <VariablePeriodSubForm
                      frequencyField="leasePmtFreqTypeCode"
                      isGstEligibleField="isGstEligible"
                      paymentAmountField="paymentAmount"
                    />
                  </StyledSection>
                  <StyledSection
                    isCollapsable
                    initiallyExpanded
                    header={
                      <>
                        Add Additional Rent
                        <TooltipIcon
                          toolTipId="additional-rent-tooltip"
                          toolTip="Operating Expenses and Taxes Payable by the Tenant."
                        />
                      </>
                    }
                  >
                    <VariablePeriodSubForm
                      frequencyField="additionalRentFreqTypeCode"
                      isGstEligibleField="isAdditionalRentGstEligible"
                      paymentAmountField="additionalRentPaymentAmount"
                    />
                  </StyledSection>
                  <StyledSection
                    isCollapsable
                    initiallyExpanded
                    header={
                      <>
                        Add Variable Rent
                        <TooltipIcon
                          toolTipId="variable-rent-tooltip"
                          toolTip="Percentage Rent payable by Tenant."
                        />
                      </>
                    }
                  >
                    <VariablePeriodSubForm
                      frequencyField="variableRentFreqTypeCode"
                      isGstEligibleField="isVariableRentGstEligible"
                      paymentAmountField="variableRentPaymentAmount"
                    />
                  </StyledSection>
                </>
              )}
              <Row>
                <Col>
                  <div style={{ marginTop: 24 }}>
                    <p>Do you want to save it?</p>
                  </div>
                </Col>
              </Row>
            </StyledFormBody>
          );
        }}
      </Formik>
      <GenericModal
        title="Warning"
        message="You are changing the period duration from flexible to fixed. Your end date will no longer be anticipated."
        okButtonText="Ok"
        variant="info"
        display={displayWarningModal}
        setDisplay={setDisplayWarningModal}
      />
    </>
  );
};

const StyledSection = styled(Section)`
  padding-left: 0;
  margin-left: 0;
  svg.tooltip-icon {
    max-width: 1rem;
    max-height: 1rem;
  }
`;

const StyledRadioGroup = styled(RadioGroup)`
  &.form-group {
    padding-top: 0.5rem;
    padding-bottom: 1rem;
  }
  .radio-group div:first-child .form-check {
    padding-left: 0;
  }
`;

export default PeriodForm;
