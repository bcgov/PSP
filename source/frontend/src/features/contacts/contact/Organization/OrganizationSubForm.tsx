import { useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';

import { Input } from '@/components/common/form';
import { FormSection } from '@/components/common/form/styles';
import * as Styled from '@/features/contacts/contact/edit/styles';
import { IEditableOrganizationForm } from '@/interfaces/editable-contact';

import { ContactEmailList, ContactPhoneList } from '../create/components';

interface IOrganizationSubFormProps {
  isContactMethodInvalid: boolean;
}

export const OrganizationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IOrganizationSubFormProps>
> = ({ isContactMethodInvalid }) => {
  const { values } = useFormikContext<IEditableOrganizationForm>();

  return (
    <>
      <FormSection>
        <Row>
          <Col>
            <Input field="name" label="Organization Name" required />
          </Col>
        </Row>
        <Row>
          <Col>
            <Input field="alias" label="Alias" />
          </Col>
        </Row>
        <Row>
          <Col md={6}>
            <Input field="incorporationNumber" label="Incorporation Number" />
          </Col>
          <Col></Col>
        </Row>
      </FormSection>
      <FormSection>
        <Styled.H2>Contact info</Styled.H2>
        <Styled.SectionMessage
          appearance={isContactMethodInvalid ? 'error' : 'information'}
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
    </>
  );
};

export default OrganizationSubForm;
