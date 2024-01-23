import { Formik, FormikProps } from 'formik';
import React, { useMemo, useState } from 'react';
import styled from 'styled-components';

import { Input, Select, SelectOption, TextArea } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { formatContactSearchResult } from '@/features/contacts/contactUtils';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_OrganizationPerson } from '@/models/api/generated/ApiGen_Concepts_OrganizationPerson';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';
import { isValidId } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { PropertyContactFormModel } from './models';
import { PropertyContactEditFormYupSchema } from './validation';

export interface IPropertyContactEditFormProps {
  isLoading: boolean;
  propertyContact: ApiGen_Concepts_PropertyContact;
  onSave: (apiModel: ApiGen_Concepts_PropertyContact) => void;
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
    const initialModel = PropertyContactFormModel.fromApi(props.propertyContact);
    setContact(initialModel.contact ?? null);
    return initialModel;
  }, [props.propertyContact]);

  const primaryContactOptions: SelectOption[] = useMemo(() => {
    if (contact?.organizationId) {
      return (
        organization?.organizationPersons?.map((orgPerson: ApiGen_Concepts_OrganizationPerson) => {
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
    <StyledFormWrapper>
      <StyledSummarySection>
        <LoadingBackdrop show={props.isLoading || getOrganizationLoading} />
        {initialForm !== undefined && (
          <Formik<PropertyContactFormModel>
            enableReinitialize
            innerRef={ref}
            validationSchema={PropertyContactEditFormYupSchema}
            initialValues={initialForm}
            onSubmit={saveContact}
          >
            {formikProps => (
              <Section header="Contact Details">
                <SectionField label="Contact" contentWidth="7" required>
                  {!isValidId(formikProps.values.id) && (
                    <ContactInputContainer
                      field="contact"
                      View={ContactInputView}
                      onContactSelected={contact => {
                        setContact(contact);
                      }}
                    />
                  )}
                  {isValidId(formikProps.values.id) && (
                    <Input
                      field="contact"
                      value={
                        formikProps.values.contact !== undefined
                          ? formatContactSearchResult(formikProps.values.contact)
                          : ''
                      }
                      disabled
                    />
                  )}
                </SectionField>
                {!isValidId(formikProps.values.contact?.personId) && (
                  <SectionField label="Primary contact">
                    {primaryContactOptions.length > 1 ? (
                      <Select
                        options={primaryContactOptions}
                        field={`primaryContactId`}
                        placeholder="Select a primary contact"
                      />
                    ) : primaryContactOptions.length > 0 ? (
                      primaryContactOptions[0].label
                    ) : (
                      'No contacts available'
                    )}
                  </SectionField>
                )}
                <SectionField label="Purpose description" contentWidth="12" required>
                  <TextArea field="purposeDescription" />
                </SectionField>
              </Section>
            )}
          </Formik>
        )}
      </StyledSummarySection>
    </StyledFormWrapper>
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
