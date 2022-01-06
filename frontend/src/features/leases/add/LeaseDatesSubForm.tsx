import { InlineCol } from 'components/common/form/styles';
import { FormikProps } from 'formik';
import { IAddFormLease } from 'interfaces';
import * as React from 'react';
import { Row } from 'react-bootstrap';

import * as Styled from './styles';

export interface ILeaseDatesSubFormProps {
  formikProps: FormikProps<IAddFormLease>;
}

export const LeaseDatesSubForm: React.FunctionComponent<ILeaseDatesSubFormProps> = ({
  formikProps,
}) => {
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
      </InlineCol>
    </Row>
  );
};

export default LeaseDatesSubForm;
