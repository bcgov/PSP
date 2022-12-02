import { FastDatePicker, Select } from 'components/common/form';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseFormModel } from '../models';
import * as Styled from './styles';

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
          <SectionField label="Start Date" required>
            <FastDatePicker formikProps={formikProps} field="startDate" required />
          </SectionField>
        </Col>
        <Col>
          <SectionField label="Expiry Date">
            <FastDatePicker formikProps={formikProps} field="expiryDate" />
          </SectionField>
        </Col>
      </Row>
      <SectionField label="Description">
        <Styled.MediumTextArea field="description" />
      </SectionField>
    </Section>
  );
};

export default LeaseDetailSubForm;
