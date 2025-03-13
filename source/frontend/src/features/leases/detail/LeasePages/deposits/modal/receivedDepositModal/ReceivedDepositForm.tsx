import { Formik, FormikProps } from 'formik';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { FastCurrencyInput, FastDatePicker, Select, TextArea } from '@/components/common/form';
import { ContactInput } from '@/components/common/form/ContactInput';
import { InlineInput } from '@/components/common/form/styles';
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
        <form className="mx-3">
          <SectionField label="Deposit type" labelWidth="3" contentWidth="5" required>
            <Select
              field="depositTypeCode"
              placeholder="Select..."
              options={depositTypeOptions}
              required
              onChange={() => {
                const depositTypeCode = formikProps.values?.depositTypeCode;
                if (isValidString(depositTypeCode) && depositTypeCode !== 'OTHER') {
                  formikProps.setFieldValue('otherTypeDescription', '');
                }
              }}
            ></Select>
          </SectionField>
          {formikProps.values?.depositTypeCode === 'OTHER' && (
            <SectionField label="Describe other" labelWidth="3" required>
              <InlineInput field="otherTypeDescription" required />
            </SectionField>
          )}
          <SectionField label="Description" labelWidth="12" required>
            <TextArea rows={4} field="description" required />
          </SectionField>
          <Row>
            <Col xs="6">
              <SectionField label="Deposit amount" labelWidth="12" required>
                <FastCurrencyInput formikProps={formikProps} field="amountPaid" required />
              </SectionField>
            </Col>
            <Col xs="6">
              <SectionField label="Paid date" labelWidth="12" required>
                <FastDatePicker formikProps={formikProps} field="depositDate" required />
              </SectionField>
            </Col>
          </Row>
          <SectionField label="Deposit holder" labelWidth="12" contentWidth="9" required>
            <ContactInput
              field="contactHolder"
              setShowContactManager={setShowContactManager}
              onClear={() => {
                formikProps.setFieldValue('contactHolder', undefined);
                setSelectedContacts([]);
              }}
              required
            />
          </SectionField>
          <div style={{ marginTop: 24 }}>
            <p>Do you want to save it?</p>
          </div>
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
          />
        </form>
      )}
    </Formik>
  );
};

export default ReceivedDepositForm;
