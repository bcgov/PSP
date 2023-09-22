import { Formik, FormikProps } from 'formik';
import React, { useMemo, useState } from 'react';
import styled from 'styled-components';

import { Input, Select, SelectOption, TextArea } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { IContactSearchResult } from '@/interfaces';
import { Api_OrganizationPerson } from '@/models/api/Organization';
import { Api_PropertyContact } from '@/models/api/Property';
import { formatApiPersonNames, formatContactSearchResult } from '@/utils/personUtils';

import { PropertyContactFormModel } from './models';

export interface IPropertyContactEditFormProps {
  isLoading: boolean;
  propertyContact: Api_PropertyContact;
  onSave: (apiModel: Api_PropertyContact) => void;
}

export const PropertyContactEditForm = React.forwardRef<
  FormikProps<any>,
  IPropertyContactEditFormProps
>((props, ref) => {
  const {
    getOrganizationDetail: {
      execute: fetchOrganization,
      response: organization,
      loading: getOrganizationLoading,
    },
  } = useOrganizationRepository();

  const [contact, setContact] = useState<IContactSearchResult | null>(null);

  React.useEffect(() => {
    if (contact?.organizationId) {
      fetchOrganization(contact.organizationId);
    }
  }, [contact, fetchOrganization]);

  const initialForm = useMemo(() => {
    const initialmodel = PropertyContactFormModel.fromApi(props.propertyContact);
    setContact(initialmodel.contact ?? null);
    return initialmodel;
  }, [props.propertyContact]);

  const primaryContactOptions: SelectOption[] = useMemo(() => {
    if (contact?.organizationId) {
      return (
        organization?.organizationPersons?.map((orgPerson: Api_OrganizationPerson) => {
          return {
            label: `${formatApiPersonNames(orgPerson.person)}`,
            value: orgPerson.personId ?? '',
          };
        }) ?? []
      );
    } else {
      return [];
    }
  }, [contact, organization?.organizationPersons]);

  const saveContact = (values: PropertyContactFormModel) => {
    props.onSave(values.toApi());
  };

  return (
    <Section header="Contact Details">
      <LoadingBackdrop show={props.isLoading || getOrganizationLoading} />
      {initialForm !== undefined && (
        <Formik<PropertyContactFormModel>
          enableReinitialize
          innerRef={ref}
          //validationSchema={UpdatePropertyDetailsYupSchema}
          initialValues={initialForm}
          onSubmit={saveContact}
        >
          {formikProps => (
            <StyledFormWrapper>
              <SectionField label="Contact" contentWidth="7">
                {formikProps.values.id === 0 && (
                  <ContactInputContainer
                    field={`contact`}
                    View={ContactInputView}
                    onContactSelected={contact => {
                      setContact(contact);
                    }}
                  />
                )}
                {formikProps.values.id !== 0 && (
                  <Input
                    field={`contact`}
                    disabled
                    value={
                      formikProps.values.contact !== undefined
                        ? formatContactSearchResult(formikProps.values.contact)
                        : ''
                    }
                  />
                )}
              </SectionField>
              {formikProps.values.contact?.organizationId !== undefined && (
                <SectionField label="Primary contact" contentWidth="7">
                  {primaryContactOptions.length > 0 ? (
                    <Select
                      options={primaryContactOptions}
                      field={`primaryContactId`}
                      placeholder="Select a primary contact"
                    />
                  ) : (
                    'No contacts available'
                  )}
                </SectionField>
              )}
              <SectionField label="Purpose description" labelWidth="12">
                <TextArea field="purposeDescription" />
              </SectionField>
            </StyledFormWrapper>
          )}
        </Formik>
      )}
    </Section>
  );
});

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
