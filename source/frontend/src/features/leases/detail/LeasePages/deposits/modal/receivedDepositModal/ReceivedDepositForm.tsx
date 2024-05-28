import { Formik, FormikProps } from 'formik';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { FastCurrencyInput, Select, TextArea } from '@/components/common/form';
import { ContactInput } from '@/components/common/form/ContactInput';
import { InlineFastDatePicker, InlineInput } from '@/components/common/form/styles';
import { SectionField } from '@/components/common/Section/SectionField';
import { ContactManagerModal } from '@/components/contact/ContactManagerModal';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IContactSearchResult } from '@/interfaces';
import { isValidString } from '@/utils';

import { FormLeaseDeposit } from '../../models/FormLeaseDeposit';
import { ReceivedDepositYupSchema } from './ReceivedDepositYupSchema';

export interface IReceivedDepositFormProps {
  formikRef: React.Ref<FormikProps<FormLeaseDeposit>>;
  onSave: (values: FormLeaseDeposit) => void;
  initialValues: FormLeaseDeposit;
}

export const ReceivedDepositForm: React.FunctionComponent<
  React.PropsWithChildren<IReceivedDepositFormProps>
> = ({ initialValues, formikRef, onSave }) => {
  const initialContacts =
    initialValues.contactHolder !== undefined ? [initialValues.contactHolder] : [];
  const [selectedContacts, setSelectedContacts] = useState<IContactSearchResult[]>(initialContacts);

  const [showContactManager, setShowContactManager] = useState(false);

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
        <StyledFormBody>
          <Row>
            <Col>
              <SectionField label="Deposit type" labelWidth="4" contentWidth="5" required>
                <Select
                  field="depositTypeCode"
                  placeholder="Select..."
                  options={depositTypeOptions}
                  onChange={() => {
                    const depositTypeCode = formikProps.values?.depositTypeCode;
                    if (isValidString(depositTypeCode) && depositTypeCode !== 'OTHER') {
                      formikProps.setFieldValue('otherTypeDescription', '');
                    }
                  }}
                ></Select>
              </SectionField>
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
            <Col md="6">
              <FastCurrencyInput
                formikProps={formikProps}
                label="Deposit amount:"
                field="amountPaid"
                required
              />
            </Col>
            <Col md="6">
              <InlineFastDatePicker
                formikProps={formikProps}
                label="Paid date:"
                field="depositDate"
                required
              />
            </Col>
          </Row>
          <Row>
            <Col md="9">
              <ContactInput
                label="Deposit holder:"
                field="contactHolder"
                setShowContactManager={setShowContactManager}
                onClear={() => {
                  formikProps.setFieldValue('contactHolder', undefined);
                  setSelectedContacts([]);
                }}
                required
              />
            </Col>
          </Row>
          <Row>
            <Col md="12">
              <div style={{ marginTop: 24 }}>
                <p>Do you want to save it?</p>
              </div>
            </Col>
          </Row>
          <ContactManagerModal
            display={showContactManager}
            setDisplay={setShowContactManager}
            setSelectedRows={setSelectedContacts}
            selectedRows={selectedContacts}
            handleModalOk={() => {
              formikProps.setFieldValue('contactHolder', selectedContacts[0]);
              setShowContactManager(false);
            }}
            isSingleSelect
          ></ContactManagerModal>
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
