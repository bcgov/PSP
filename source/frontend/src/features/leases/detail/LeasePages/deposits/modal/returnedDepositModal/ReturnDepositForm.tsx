import { Formik, FormikProps } from 'formik';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { FastCurrencyInput, FastDatePicker } from '@/components/common/form';
import { ContactInput } from '@/components/common/form/ContactInput';
import { SectionField } from '@/components/common/Section/SectionField';
import { ContactManagerModal } from '@/components/contact/ContactManagerModal';
import { IContactSearchResult } from '@/interfaces';
import { formatMoney } from '@/utils';

import { FormLeaseDepositReturn } from '../../models/FormLeaseDepositReturn';
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
export const ReturnDepositForm: React.FunctionComponent<
  React.PropsWithChildren<IReturnDepositFormProps>
> = ({ initialValues, formikRef, onSave }) => {
  const initialContacts =
    initialValues.contactHolder !== undefined ? [initialValues.contactHolder] : [];
  const [selectedContacts, setSelectedContacts] = useState<IContactSearchResult[]>(initialContacts);

  const [showContactManager, setShowContactManager] = useState(false);

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
          <SubHeaderSection className="pt-3 mb-4 pl-3">
            <SectionField label="Deposit type">{typeDescription}</SectionField>
            <SectionField label="Deposit amount">
              {formatMoney(initialValues.parentDepositAmount)}
            </SectionField>
          </SubHeaderSection>
          <form className="mx-3">
            <SectionField label="Termination or surrender date" labelWidth="12" required>
              <FastDatePicker formikProps={formikProps} field="terminationDate" required />
            </SectionField>
            <Row>
              <Col>
                <SectionField label="Claims against deposit ($)" labelWidth="12" contentWidth="10">
                  <FastCurrencyInput
                    formikProps={formikProps}
                    field="claimsAgainst"
                    className="mt-6"
                  />
                </SectionField>
              </Col>
              <Col>
                <SectionField label="Returned amount ($) without interest" labelWidth="12" required>
                  <FastCurrencyInput formikProps={formikProps} field="returnAmount" required />
                </SectionField>
              </Col>
            </Row>
            <Row>
              <Col>
                <SectionField
                  label="Interest paid ($)"
                  labelWidth="12"
                  tooltip="This is the interest paid on the deposit, if any, for the entire period the deposit is held.​​​​​​​"
                >
                  <FastCurrencyInput formikProps={formikProps} field="interestPaid" />
                </SectionField>
              </Col>
              <Col>
                <StyledReturnInfoContainer>
                  <StyledReturningDepositLink
                    target="_blank"
                    rel="noopener noreferrer"
                    href="https://www2.gov.bc.ca/gov/content/housing-tenancy/residential-tenancies/ending-a-tenancy/returning-deposits"
                  >
                    Returning deposits in BC
                  </StyledReturningDepositLink>
                  <FaExternalLinkAltIcon />
                </StyledReturnInfoContainer>
              </Col>
            </Row>
            <SectionField label="Returned date" labelWidth="12" required>
              <FastDatePicker formikProps={formikProps} field="returnDate" required />
            </SectionField>
            <SectionField label="Payee name" labelWidth="12" required>
              <ContactInput
                field="contactHolder"
                setShowContactManager={setShowContactManager}
                onClear={() => {
                  formikProps.setFieldValue('contactHolder', undefined);
                  setSelectedContacts([]);
                }}
              />
            </SectionField>
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
          </form>
        </>
      )}
    </Formik>
  );
};

export default ReturnDepositForm;

export const SubHeaderSection = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledReturningDepositLink = styled.a`
  font-size: 1.3rem;
`;

const StyledReturnInfoContainer = styled.p`
  margin-top: 3.5rem;
`;

const FaExternalLinkAltIcon = styled(FaExternalLinkAlt)`
  height: 0.7em;
  margin-left: 1rem;
`;
