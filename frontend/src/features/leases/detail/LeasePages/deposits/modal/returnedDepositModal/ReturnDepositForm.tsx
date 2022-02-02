import { FastCurrencyInput } from 'components/common/form';
import { InlineFastDatePicker } from 'components/common/form/styles';
import { Formik, FormikProps } from 'formik';
import { FormLeaseDepositReturn } from 'interfaces';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { formatMoney } from 'utils';

import { ReturnDepositYupSchema } from './ReturnDepositYupSchema';

export interface IReturnDepositFormProps {
  formikRef: React.Ref<FormikProps<FormLeaseDepositReturn>>;
  onSave: (values: FormLeaseDepositReturn) => void;
  initialValues: FormLeaseDepositReturn;
}

/**
 * Internal Form intended to be displayed within a modal window.
 * @param {IReturnDepositFormProps} props
 */
export const ReturnDepositForm: React.FunctionComponent<IReturnDepositFormProps> = ({
  initialValues,
  formikRef,
  onSave,
}) => {
  const typeDescription =
    initialValues.depositTypeCode === 'OTHER'
      ? 'Other - ' + initialValues.parentDepositOtherDescription
      : initialValues.depositTypeDescription;

  return (
    <Formik
      innerRef={formikRef}
      enableReinitialize
      validationSchema={ReturnDepositYupSchema}
      onSubmit={values => {
        onSave(values);
      }}
      initialValues={initialValues}
    >
      {formikProps => (
        <>
          <SubHeaderSection className="py-3 mb-4 pl-3">
            <Row className="pb-3">
              <Col md="4">
                <strong>Deposit type:</strong>
              </Col>
              <Col>{typeDescription}</Col>
            </Row>
            <Row>
              <Col md="4">
                <strong>Deposit amount:</strong>
              </Col>
              <Col>{formatMoney(initialValues.parentDepositAmount)}</Col>
            </Row>
          </SubHeaderSection>
          <StyledFormBody className="mx-3">
            <Row>
              <Col>
                <InlineFastDatePicker
                  formikProps={formikProps}
                  label="Termination or surrender date:"
                  field="terminationDate"
                  required
                />
              </Col>
            </Row>
            <Row>
              <Col>
                <FastCurrencyInput
                  formikProps={formikProps}
                  label="Claims against deposit ($):"
                  field="claimsAgainst"
                />
              </Col>
              <Col>
                <FastCurrencyInput
                  formikProps={formikProps}
                  label="Returned amount ($):"
                  field="returnAmount"
                  required
                />
              </Col>
            </Row>
            <Row>
              <Col>
                <InlineFastDatePicker
                  formikProps={formikProps}
                  label="Returned date:"
                  field="returnDate"
                  required
                />
              </Col>
            </Row>
          </StyledFormBody>
        </>
      )}
    </Formik>
  );
};

export default ReturnDepositForm;

const StyledFormBody = styled.form`
  .form-group {
    flex-direction: column;
    .form-label {
      font-weight: bold;
    }
    input {
      width: 90%;
    }
  }
`;

export const SubHeaderSection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;
