import { Formik, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';

import {
  Check,
  FastCurrencyInput,
  FastDatePicker,
  Input,
  Select,
  SelectOption,
} from '@/components/common/form';
import { LeaseTermStatusTypes } from '@/constants';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { toTypeCodeNullable } from '@/utils/formUtils';

import { defaultFormLeasePeriod, FormLeasePeriod } from '../../models';
import { StyledFormBody } from '../../styles';
import { LeasePeriodSchema } from './PeriodsYupSchema';

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
  const lookups = useLookupCodeHelpers();
  const paymentFrequencyOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_FREQUENCY_TYPES);
  const leaseTermStatusOptions = lookups.getOptionsByType(API.LEASE_TERM_STATUS_TYPES);
  const flexiblePeriodOptions: SelectOption[] = [
    { label: 'Fixed', value: 0 },
    { label: 'Flexible', value: 1 },
  ];
  return (
    <Formik<FormLeaseTerm>
      innerRef={formikRef}
      enableReinitialize
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
      {formikProps => (
        <StyledFormBody>
          <Row>
            <Select
              label="Period duration:"
              field="isFlexible"
              tooltip="Fixed Payment Period Duration has end date. Select Flexible payment period duration to track hold over payments."
              options={flexiblePeriodOptions}
            />
          </Row>
          <Row>
            <Col>
              <FastDatePicker
                required
                label="Start date:"
                field="startDate"
                formikProps={formikProps}
              />
            </Col>
            <Col>
              <FastDatePicker
                label={formikProps.values.isFlexible ? 'End date (Anticipated):' : 'End date:'}
                field="expiryDate"
                formikProps={formikProps}
              />
            </Col>
          </Row>
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
                label="Payments due"
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
            <Col>
              <Select
                label="Period Status"
                field="statusTypeCode.id"
                options={leasePeriodStatusOptions}
              />
            </Col>
            <Col></Col>
          </Row>
          <Row>
            <Col>
              <div style={{ marginTop: 24 }}>
                <p>Do you want to save it?</p>
              </div>
            </Col>
          </Row>
        </StyledFormBody>
      )}
    </Formik>
  );
};

export default PeriodForm;
