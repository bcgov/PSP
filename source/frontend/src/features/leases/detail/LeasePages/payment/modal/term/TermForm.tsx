import { Formik, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { Check, FastCurrencyInput, FastDatePicker, Input, Select } from '@/components/common/form';
import { LeaseTermStatusTypes } from '@/constants';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { toTypeCodeNullable } from '@/utils/formUtils';

import { defaultFormLeaseTerm, FormLeaseTerm } from '../../models';
import { StyledFormBody } from '../../styles';
import { LeaseTermSchema } from './TermsYupSchema';

export interface ITermFormProps {
  formikRef: React.Ref<FormikProps<FormLeaseTerm>>;
  onSave: (values: FormLeaseTerm) => void;
  initialValues?: FormLeaseTerm;
  lease: ApiGen_Concepts_Lease | undefined;
}

/**
 * Internal Form intended to be displayed within a modal window. will be initialized with default values (including NEXER status)
 * if not initialValues provided. Otherwise will display the passed lease term. Save/Cancel triggered externally via passed formikRef.
 * @param {ITermFormProps} props
 */
export const TermForm: React.FunctionComponent<React.PropsWithChildren<ITermFormProps>> = ({
  initialValues,
  formikRef,
  onSave,
  lease,
}) => {
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
        leaseId: lease?.id ?? 0,
        statusTypeCode: initialValues?.statusTypeCode?.id
          ? initialValues?.statusTypeCode
          : toTypeCodeNullable(LeaseTermStatusTypes.NOT_EXERCISED),
      }}
    >
      {formikProps => (
        <StyledFormBody>
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
              <FastDatePicker label="End date:" field="expiryDate" formikProps={formikProps} />
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
                label="Term Status"
                field="statusTypeCode.id"
                options={leaseTermStatusOptions}
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

export default TermForm;
