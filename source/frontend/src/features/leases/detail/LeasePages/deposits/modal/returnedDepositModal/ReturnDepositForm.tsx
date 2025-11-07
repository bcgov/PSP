import { Formik, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { FastCurrencyInput, FastDatePicker } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { SectionField } from '@/components/common/Section/SectionField';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
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
            <SectionField label="Termination or surrender date" labelWidth={{ xs: 12 }} required>
              <FastDatePicker formikProps={formikProps} field="terminationDate" required />
            </SectionField>
            <Row>
              <Col>
                <SectionField
                  label="Claims against deposit ($)"
                  labelWidth={{ xs: 12 }}
                  contentWidth={{ xs: 10 }}
                >
                  <FastCurrencyInput
                    formikProps={formikProps}
                    field="claimsAgainst"
                    className="mt-6"
                  />
                </SectionField>
              </Col>
              <Col>
                <SectionField
                  label="Returned amount ($) without interest"
                  labelWidth={{ xs: 12 }}
                  required
                >
                  <FastCurrencyInput formikProps={formikProps} field="returnAmount" required />
                </SectionField>
              </Col>
            </Row>
            <Row>
              <Col>
                <SectionField
                  label="Interest paid ($)"
                  labelWidth={{ xs: 12 }}
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
            <SectionField label="Returned date" labelWidth={{ xs: 12 }} required>
              <FastDatePicker formikProps={formikProps} field="returnDate" required />
            </SectionField>
            <SectionField label="Payee name" labelWidth={{ xs: 12 }} required>
              <ContactInputContainer
                field="contactHolder"
                View={ContactInputView}
                restrictContactType={RestrictContactType.ALL}
                displayErrorAsTooltip={false}
                required={true}
              />
            </SectionField>
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
