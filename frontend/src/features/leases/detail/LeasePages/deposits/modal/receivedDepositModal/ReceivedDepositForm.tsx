import { FastCurrencyInput, TextArea } from 'components/common/form';
import { InlineFastDatePicker, InlineInput, InlineSelect } from 'components/common/form/styles';
import * as API from 'constants/API';
import { Formik, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { FormLeaseDeposit } from 'interfaces';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ReceivedDepositYupSchema } from './ReceivedDepositYupSchema';

export interface IReceivedDepositFormProps {
  formikRef: React.Ref<FormikProps<FormLeaseDeposit>>;
  onSave: (values: FormLeaseDeposit) => void;
  initialValues: FormLeaseDeposit;
}

export const ReceivedDepositForm: React.FunctionComponent<IReceivedDepositFormProps> = ({
  initialValues,
  formikRef,
  onSave,
}) => {
  const lookups = useLookupCodeHelpers();
  const depositTypeOptions = lookups.getOptionsByType(API.SECURITY_DEPOSIT_TYPES);

  return (
    <Formik
      innerRef={formikRef}
      enableReinitialize
      validationSchema={ReceivedDepositYupSchema}
      onSubmit={values => {
        onSave(values);
      }}
      initialValues={initialValues}
    >
      {formikProps => (
        <StyledFormBody className="px-4">
          <Row>
            <Col md="8">
              <InlineSelect
                label="Deposit type:"
                required
                field="depositTypeCode"
                options={depositTypeOptions}
                placeholder="Select"
                onChange={() => {
                  let depositTypeCode = formikProps.values?.depositTypeCode;
                  if (!!depositTypeCode && depositTypeCode !== 'OTHER') {
                    formikProps.setFieldValue('otherTypeDescription', '');
                  }
                }}
              />
              {formikProps.values?.depositTypeCode === 'OTHER' && (
                <InlineInput label="Describe other:" field="otherTypeDescription" required />
              )}
            </Col>
          </Row>
          <Row>
            <Col>
              <TextArea label="Description:" rows={4} field="description" />
            </Col>
          </Row>
          <Row>
            <Col>
              <FastCurrencyInput
                formikProps={formikProps}
                label="Depost Amount:"
                field="amountPaid"
                required
              />
            </Col>
            <Col>
              <InlineFastDatePicker
                formikProps={formikProps}
                label="Paid date:"
                field="depositDate"
                required
              />
            </Col>
          </Row>
        </StyledFormBody>
      )}
    </Formik>
  );
};

export default ReceivedDepositForm;

const StyledFormBody = styled.form`
  .form-group {
    flex-direction: column;
    .form-label {
      font-weight: bold;
    }
    textarea {
      resize: none;
    }
  }
`;
