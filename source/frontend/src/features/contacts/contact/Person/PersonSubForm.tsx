import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';

import { AsyncTypeahead, Check, Input } from '@/components/common/form';
import { FormSection } from '@/components/common/form/styles';
import * as Styled from '@/features/contacts/contact/edit/styles';
import { usePersonOrganizationTypeahead } from '@/features/contacts/hooks/usePersonOrganizationTypeahead';
import { IEditablePersonForm } from '@/interfaces/editable-contact';

import { Address, CommentNotes, ContactEmailList, ContactPhoneList } from '../create/components';

interface IPersonSubFormProps {
  isContactMethodInvalid?: boolean;
}

const PersonSubForm: React.FunctionComponent<React.PropsWithChildren<IPersonSubFormProps>> = ({
  isContactMethodInvalid,
}) => {
  const { handleTypeaheadSearch, isTypeaheadLoading, matchedOrgs } =
    usePersonOrganizationTypeahead();
  const { values, errors } = useFormikContext<IEditablePersonForm>();
  const organizationId = getIn(values, 'organization.id');
  const useOrganizationAddress = getIn(values, 'useOrganizationAddress');

  return (
    <>
      <FormSection>
        <Row>
          <Col md={4}>
            <Input field="firstName" label="First Name" required />
          </Col>
          <Col md={3}>
            <Input field="middleNames" label="Middle" />
          </Col>
          <Col>
            <Input field="surname" label="Last Name" required />
          </Col>
        </Row>
        <Row>
          <Col md={7}>
            <Input field="preferredName" label="Preferred Name" />
          </Col>
          <Col></Col>
        </Row>
      </FormSection>
      <FormSection>
        <Styled.H2>Organization</Styled.H2>
        <Row>
          <Col md={7}>
            <AsyncTypeahead
              field="organization"
              label="Link to an existing organization"
              labelKey="text"
              isLoading={isTypeaheadLoading}
              options={matchedOrgs}
              onSearch={handleTypeaheadSearch}
            />
          </Col>
        </Row>
      </FormSection>
      <FormSection>
        <Styled.H2>Contact info</Styled.H2>
        <Styled.SectionMessage
          appearance={
            isContactMethodInvalid && getIn(errors, 'needsContactMethod') ? 'error' : 'information'
          }
          gap="0.5rem"
        >
          <AiOutlineExclamationCircle size="1.8rem" className="mt-2" />
          <p>
            Contacts must have a minimum of one method of contact to be saved. <br />
            <em>(ex: email,phone or address)</em>
          </p>
        </Styled.SectionMessage>
        <ContactEmailList field="emailContactMethods" contactEmails={values.emailContactMethods} />
        <br />
        <ContactPhoneList field="phoneContactMethods" contactPhones={values.phoneContactMethods} />
      </FormSection>
      <FormSection>
        <Styled.H2>Address</Styled.H2>
        <Styled.H3>Mailing Address</Styled.H3>
        <Check
          field="useOrganizationAddress"
          postLabel="Use mailing address from organization"
          disabled={!organizationId}
        />
        <Address namespace="mailingAddress" disabled={useOrganizationAddress} />
      </FormSection>
      <FormSection>
        <Styled.H3>Property Address</Styled.H3>
        <Address namespace="propertyAddress" />
      </FormSection>
      <FormSection>
        <Styled.H3>Billing Address</Styled.H3>
        <Address namespace="billingAddress" />
      </FormSection>
      <FormSection>
        <CommentNotes />
      </FormSection>
    </>
  );
};

export default PersonSubForm;
