import { Select } from 'components/common/form';
import { InlineCol } from 'components/common/form/styles';
import * as API from 'constants/API';
import { FormikProps } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { IAddFormLease } from 'interfaces';
import * as React from 'react';
import { Row } from 'react-bootstrap';

import { Spacer } from '../list/styles';
import * as Styled from './styles';

export interface ILeaseDatesSubFormProps {
  formikProps: FormikProps<IAddFormLease>;
}

export const LeaseDatesSubForm: React.FunctionComponent<ILeaseDatesSubFormProps> = ({
  formikProps,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const leaseStatusTypes = getOptionsByType(API.LEASE_STATUS_TYPES);
  return (
    <Row>
      <InlineCol>
        <Styled.StackedDatePicker
          formikProps={formikProps}
          label="Start Date"
          field="startDate"
          required
        />
        <Styled.StackedDatePicker
          formikProps={formikProps}
          label="Expiry Date (if known)"
          field="expiryDate"
        />
        <Spacer />
        <Select
          placeholder="Select Status"
          label="Status:"
          field="statusType"
          options={leaseStatusTypes}
        />
      </InlineCol>
    </Row>
  );
};

export default LeaseDatesSubForm;
