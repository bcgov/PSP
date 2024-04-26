import { Formik, FormikProps } from 'formik';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { FastCurrencyInput } from '@/components/common/form';
import { ContactInput } from '@/components/common/form/ContactInput';
import { InlineFastDatePicker } from '@/components/common/form/styles';
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
              <Col className="align-items-end d-flex">
                <FastCurrencyInput
                  formikProps={formikProps}
                  label="Claims against deposit ($):"
                  field="claimsAgainst"
                />
              </Col>
              <Col>
                <FastCurrencyInput
                  formikProps={formikProps}
                  label="Returned amount ($) without interest:"
                  field="returnAmount"
                  required
                />
              </Col>
            </Row>
            <Row>
              <Col>
                <FastCurrencyInput
                  formikProps={formikProps}
                  label="Interest paid ($):"
                  field="interestPaid"
                  tooltip="This is the interest paid on the deposit, if any, for the entire term the deposit is held.​​​​​​​"
                />
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
            <Row>
              <Col>
                <ContactInput
                  label="Payee name:"
                  field="contactHolder"
                  setShowContactManager={setShowContactManager}
                  onClear={() => {
                    formikProps.setFieldValue('contactHolder', undefined);
                    setSelectedContacts([]);
                  }}
                />
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
  }
`;

export const SubHeaderSection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
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
