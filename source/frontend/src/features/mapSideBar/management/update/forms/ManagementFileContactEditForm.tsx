import { Formik, FormikProps } from 'formik';
import { forwardRef, useCallback, useEffect, useState } from 'react';

import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { Input } from '@/components/common/form/Input';
import { Select, SelectOption } from '@/components/common/form/Select';
import { TextArea } from '@/components/common/form/TextArea';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { formatContactSearchResult } from '@/utils/contactUtils';
import { formatApiPersonNames } from '@/utils/personUtils';
import { exists, isValidId } from '@/utils/utils';

import { ManagementFileContactFormModel } from '../../models/ManagementFileContactFormModel';
import { ManagementFileContactFormYupSchema } from '../../models/ManagementFileContactFormYupSchema';

export interface IManagementFileContactEditFormProps {
  isLoading: boolean;
  managementFileContact: ManagementFileContactFormModel;
  onSave: (apiModel: ApiGen_Concepts_ManagementFileContact) => void;
}

const ManagementFileContactEditForm = forwardRef<
  FormikProps<ManagementFileContactFormModel>,
  IManagementFileContactEditFormProps
>((props, ref) => {
  const {
    getOrganizationDetail: { execute: fetchOrganization, loading: getOrganizationLoading },
  } = useOrganizationRepository();

  const [primaryContactOptions, setPrimaryContactOptions] = useState<SelectOption[] | null>([]);

  const getOrganizationInfo = useCallback(
    async (orgId: number) => {
      const org = await fetchOrganization(orgId);
      if (org && org.organizationPersons) {
        const orgContacts =
          org?.organizationPersons?.map((orgPerson: ApiGen_Concepts_PersonOrganization) => {
            return {
              label: `${formatApiPersonNames(orgPerson.person)}`,
              value: orgPerson.personId ?? '',
            };
          }) ?? [];

        setPrimaryContactOptions(orgContacts);
      }
    },
    [fetchOrganization],
  );

  useEffect(() => {
    if (isValidId(props.managementFileContact?.contact?.organizationId)) {
      getOrganizationInfo(props.managementFileContact?.contact?.organizationId);
    }
  }, [getOrganizationInfo, props.managementFileContact?.contact?.organizationId]);

  const saveContact = (values: ManagementFileContactFormModel) => {
    props.onSave(values.toApi());
  };

  const handleContactSelected = (
    contact: IContactSearchResult,
    formikProps: FormikProps<ManagementFileContactFormModel>,
  ): void => {
    formikProps.setFieldValue('contact', contact);
    if (exists(contact.organization?.id)) {
      getOrganizationInfo(contact.organization?.id);
    }
  };

  return (
    <StyledFormWrapper>
      <StyledSummarySection>
        <LoadingBackdrop show={props.isLoading || getOrganizationLoading} />
        {props.managementFileContact && (
          <Formik<ManagementFileContactFormModel>
            enableReinitialize
            innerRef={ref}
            validationSchema={ManagementFileContactFormYupSchema}
            initialValues={props.managementFileContact}
            validateOnChange={false}
            onSubmit={saveContact}
          >
            {formikProps => (
              <Section header="Contact Details">
                <SectionField label="Contact" contentWidth={{ xs: 7 }} required>
                  {!isValidId(formikProps.values.id) && (
                    <ContactInputContainer
                      field="contact"
                      View={ContactInputView}
                      onContactSelected={contact => handleContactSelected(contact, formikProps)}
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

                <SectionField label="Purpose description" contentWidth={{ xs: 12 }} required>
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

export default ManagementFileContactEditForm;
