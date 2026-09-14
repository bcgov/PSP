import { Formik, FormikProps, getIn } from 'formik';
import { useRef, useState } from 'react';
import { Form } from 'react-bootstrap';

import { Button } from '@/components/common/buttons/Button';
import { ContactTypeSelector } from '@/features/contacts';
import { useAddressHelpers } from '@/features/contacts/contact/create/components';
import { hasEmail, hasPhoneNumber } from '@/features/contacts/contact/create/validation';
import * as Styled from '@/features/contacts/contact/edit/styles';
import OrganizationSubForm from '@/features/contacts/contact/Organization/OrganizationSubForm';
import PersonContactDetailsSection from '@/features/contacts/contact/Person/PersonContactDetailsSection';
import PersonContactInfoSection from '@/features/contacts/contact/Person/PersonContactInfoSection';
import {
  onValidateOrganization,
  onValidatePerson,
} from '@/features/contacts/contact/utils/contactUtils';
import { IEditableOrganizationForm, IEditablePersonForm } from '@/features/contacts/formModels';
import useAddContact from '@/features/contacts/hooks/useAddContact';
import { ContactTypes } from '@/features/contacts/interfaces';
import { fromApiOrganization, fromApiPerson, IContactSearchResult } from '@/interfaces';

export interface IContactCreateFormProps {
  onSaved: (contact: IContactSearchResult) => void;
  onCancel: () => void;
}

export const ContactCreateForm: React.FC<IContactCreateFormProps> = ({ onSaved, onCancel }) => {
  const initialContactType = ContactTypes.INDIVIDUAL;
  const [selectedType, setSelectedType] = useState(initialContactType);
  const personRef = useRef<FormikProps<IEditablePersonForm>>(null);
  const organizationRef = useRef<FormikProps<IEditableOrganizationForm>>(null);
  const { otherCountryId } = useAddressHelpers();
  const { addPerson, addOrganization } = useAddContact();

  const validatePerson = (values: IEditablePersonForm) => ({
    ...onValidatePerson(values, otherCountryId),
    ...(!hasEmail(values) && !hasPhoneNumber(values)
      ? { needsContactMethod: 'Add at least one email or phone number.' }
      : {}),
  });

  const validateOrganization = (values: IEditableOrganizationForm) => ({
    ...onValidateOrganization(values, otherCountryId),
    ...(!hasEmail(values) && !hasPhoneNumber(values)
      ? { needsContactMethod: 'Add at least one email or phone number.' }
      : {}),
  });

  const savePerson = async (values: IEditablePersonForm) => {
    const response = await addPerson(values.formPersonToApiPerson(), () => undefined);
    if (response) {
      onSaved(fromApiPerson(response));
    }
  };

  const saveOrganization = async (values: IEditableOrganizationForm) => {
    const response = await addOrganization(
      values.formOrganizationToApiOrganization(),
      () => undefined,
    );
    if (response) {
      onSaved(fromApiOrganization(response));
    }
  };

  const form =
    selectedType === ContactTypes.INDIVIDUAL ? (
      <Formik<IEditablePersonForm>
        key="person"
        innerRef={personRef}
        initialValues={new IEditablePersonForm()}
        validate={validatePerson}
        onSubmit={savePerson}
      >
        {formikProps => (
          <Form onSubmit={formikProps.handleSubmit}>
            <PersonContactDetailsSection />
            <PersonContactInfoSection
              isContactMethodInvalid={!!getIn(formikProps.errors, 'needsContactMethod')}
            />
            <Styled.ButtonGroup>
              <Button type="button" variant="secondary" onClick={onCancel}>
                Cancel
              </Button>
              <Button type="submit">Save</Button>
            </Styled.ButtonGroup>
          </Form>
        )}
      </Formik>
    ) : (
      <Formik<IEditableOrganizationForm>
        key="organization"
        innerRef={organizationRef}
        initialValues={new IEditableOrganizationForm()}
        validate={validateOrganization}
        onSubmit={saveOrganization}
      >
        {formikProps => (
          <Form onSubmit={formikProps.handleSubmit}>
            <OrganizationSubForm
              isContactMethodInvalid={!!getIn(formikProps.errors, 'needsContactMethod')}
            />
            <Styled.ButtonGroup>
              <Button type="button" variant="secondary" onClick={onCancel}>
                Cancel
              </Button>
              <Button type="submit">Save</Button>
            </Styled.ButtonGroup>
          </Form>
        )}
      </Formik>
    );

  return (
    <>
      <ContactTypeSelector contactType={selectedType} setContactType={setSelectedType} />

      {form}
    </>
  );
};

export default ContactCreateForm;
