import { FormikProps } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { FastDatePicker, ProjectSelector, Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';

import { LeaseFormModel } from '../models';

export interface ILeaseDetailsSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

export const LeaseDetailSubForm: React.FunctionComponent<ILeaseDetailsSubFormProps> = ({
  formikProps,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const leaseStatusTypes = getOptionsByType(API.LEASE_STATUS_TYPES);
  const paymentReceivableTypes = getOptionsByType(API.LEASE_PAYMENT_RECEIVABLE_TYPES);
  return (
    <Section>
      <SectionField label="Ministry project" labelWidth="2">
        <ProjectSelector field="project" />
      </SectionField>
      <SectionField label="Status" labelWidth="2" contentWidth="4" required>
        <Select
          placeholder="Select Status"
          field="statusTypeCode"
          options={leaseStatusTypes}
          required
        />
      </SectionField>
      <SectionField label="Account type" labelWidth="2" contentWidth="5" required>
        <Select field="paymentReceivableTypeCode" options={paymentReceivableTypes} />
      </SectionField>
      <Row>
        <Col>
          <SectionField label="Start date" required>
            <FastDatePicker formikProps={formikProps} field="startDate" required />
          </SectionField>
        </Col>
        <Col>
          <SectionField label="Expiry date">
            <FastDatePicker formikProps={formikProps} field="expiryDate" />
          </SectionField>
        </Col>
      </Row>
    </Section>
  );
};

export default LeaseDetailSubForm;
