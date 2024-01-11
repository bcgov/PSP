import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';

import { AsyncTypeahead, Check, Input } from '@/components/common/form';
import { FormSection } from '@/components/common/form/styles';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
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
      <Section header="Contact Details">
        <SectionField label="First name" required>
          <Input field="firstName" />
        </SectionField>
        <SectionField label="Middle">
          <Input field="middleNames" />
        </SectionField>
        <SectionField label="Last name" required>
          <Input field="surname" />
        </SectionField>
        <SectionField label="Preferred name">
          <Input field="preferredName" />
        </SectionField>
      </Section>
      <Section header="Organization">
        <SectionField label="Link to an existing organization">
          <AsyncTypeahead
            field="organization"
            labelKey="text"
            isLoading={isTypeaheadLoading}
            options={matchedOrgs}
            onSearch={handleTypeaheadSearch}
          />
        </SectionField>
      </Section>
      <Section header="Contact Info">
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
        <SectionField label="Email">
          <ContactEmailList
            field="emailContactMethods"
            contactEmails={values.emailContactMethods}
          />
        </SectionField>
        <SectionField label="Phone" className="mt-3">
          <ContactPhoneList
            field="phoneContactMethods"
            contactPhones={values.phoneContactMethods}
          />
        </SectionField>
      </Section>
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
