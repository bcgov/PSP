import { Formik, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { FastCurrencyInput, FastDatePicker, Select, TextArea } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { InlineInput } from '@/components/common/form/styles';
import { SectionField } from '@/components/common/Section/SectionField';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_CodeTypes_LeaseSecurityDepositTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseSecurityDepositTypes';
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
      validateOnChange={false}
    >
      {formikProps => (
        <form className="mx-3">
          <SectionField
            label="Deposit type"
            labelWidth={{ xs: 3 }}
            contentWidth={{ xs: 5 }}
            required
          >
            <Select
              field="depositTypeCode"
              placeholder="Select..."
              options={depositTypeOptions}
              required
              onChange={() => {
                const depositTypeCode = formikProps.values?.depositTypeCode;
                if (
                  isValidString(depositTypeCode) &&
                  depositTypeCode !== ApiGen_CodeTypes_LeaseSecurityDepositTypes.OTHER
                ) {
                  formikProps.setFieldValue('otherTypeDescription', '');
                }
              }}
            ></Select>
          </SectionField>
          {formikProps.values?.depositTypeCode ===
            ApiGen_CodeTypes_LeaseSecurityDepositTypes.OTHER && (
            <SectionField label="Describe other" labelWidth={{ xs: 3 }} required>
              <InlineInput field="otherTypeDescription" required />
            </SectionField>
          )}
          <SectionField label="Description" labelWidth={{ xs: 12 }}>
            <TextArea rows={4} field="description" />
          </SectionField>
          <Row>
            <Col xs="6">
              <SectionField label="Deposit amount" labelWidth={{ xs: 12 }}>
                <FastCurrencyInput formikProps={formikProps} field="amountPaid" />
              </SectionField>
            </Col>
            <Col xs="6">
              <SectionField label="Paid date" labelWidth={{ xs: 12 }}>
                <FastDatePicker formikProps={formikProps} field="depositDate" />
              </SectionField>
            </Col>
          </Row>
          <SectionField
            label="Deposit holder"
            labelWidth={{ xs: 12 }}
            contentWidth={{ xs: 9 }}
            required
          >
            <ContactInputContainer
              field="contactHolder"
              View={ContactInputView}
              restrictContactType={RestrictContactType.ALL}
              displayErrorAsTooltip={false}
              required={true}
            />
          </SectionField>
          <div style={{ marginTop: 24 }}>
            <p>Do you want to save it?</p>
          </div>
        </form>
      )}
    </Formik>
  );
};

export default ReceivedDepositForm;
