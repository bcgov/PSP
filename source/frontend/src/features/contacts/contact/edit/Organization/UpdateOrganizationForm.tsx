import { Formik, FormikHelpers, FormikProps, getIn } from 'formik';
import React, { useCallback, useMemo } from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { Link, useHistory } from 'react-router-dom';

import { Button } from '@/components/common/buttons/Button';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import { Select, TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { FlexBox } from '@/components/common/styles';
import { Address, useAddressHelpers } from '@/features/contacts/contact/create/components';
import * as Styled from '@/features/contacts/contact/edit/styles';
import { IEditableOrganizationForm } from '@/features/contacts/formModels';
import { useOrganizationDetail } from '@/features/contacts/hooks/useOrganizationDetail';
import useUpdateContact from '@/features/contacts/hooks/useUpdateContact';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { isValidId } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { OrganizationSubForm } from '../../Organization/OrganizationSubForm';
import { onValidateOrganization } from '../../utils/contactUtils';

/**
 * Formik-connected form to Update Organizational Contacts
 */
export const UpdateOrganizationForm: React.FC<{ id: number }> = ({ id }) => {
  const history = useHistory();
  const { updateOrganization } = useUpdateContact();
  const { otherCountryId } = useAddressHelpers();

  const onSubmit = async (
    formOrganization: IEditableOrganizationForm,
    { setSubmitting }: FormikHelpers<IEditableOrganizationForm>,
  ) => {
    try {
      const apiOrganization = formOrganization.formOrganizationToApiOrganization();
      const organizationResponse = await updateOrganization(apiOrganization);
      const organizationId = organizationResponse?.id;

      if (isValidId(organizationId)) {
        history.push(`/contact/O${organizationId}`);
      }
    } finally {
      setSubmitting(false);
    }
  };

  // fetch organization details from API for the supplied Id
  const { organization } = useOrganizationDetail(id);
  const formOrganization = useMemo(
    () => IEditableOrganizationForm.apiOrganizationToFormOrganization(organization),
    [organization],
  );

  const initialValues = formOrganization ? formOrganization : new IEditableOrganizationForm();

  return (
    <Formik<IEditableOrganizationForm>
      component={UpdateOrganization}
      initialValues={initialValues}
      validate={(values: IEditableOrganizationForm) =>
        onValidateOrganization(values, otherCountryId)
      }
      enableReinitialize
      onSubmit={onSubmit}
    />
  );
};

/**
 * Sub-component that is wrapped by Formik
 */
const UpdateOrganization: React.FC<FormikProps<IEditableOrganizationForm>> = ({
  values,
  errors,
  touched,
  dirty,
  submitForm,
  isSubmitting,
}) => {
  const history = useHistory();

  const organizationId = getIn(values, 'id');
  const persons = getIn(values, 'persons') as Partial<ApiGen_Concepts_Person>[];
  const isOrganizationDisabled = getIn(values, 'isDisabled');

  const onCancel = () => {
    history.push(`/contact/O${organizationId}`);
  };

  const isContactMethodInvalid = useMemo(() => {
    if (isOrganizationDisabled === true) {
      return false;
    }
    return (
      !!touched.phoneContactMethods &&
      !!touched.emailContactMethods &&
      (!!touched.mailingAddress?.streetAddress1 ||
        !!touched.propertyAddress?.streetAddress1 ||
        !!touched.billingAddress?.streetAddress1) &&
      getIn(errors, 'needsContactMethod')
    );
  }, [touched, errors, isOrganizationDisabled]);

  const checkState = useCallback(() => {
    return dirty && !isSubmitting;
  }, [dirty, isSubmitting]);

  return (
    <>
      <Styled.ScrollingFormLayout>
        <Styled.Form id="updateForm">
          <FlexBox column>
            <Section className="py-2">
              <SectionField
                label="Organization"
                contentWidth="auto"
                className="py-3"
                valueClassName="ml-auto"
              >
                <Select
                  className="mb-0"
                  field="isDisabled"
                  options={[
                    { label: 'Inactive', value: 'true' },
                    { label: 'Active', value: 'false' },
                  ]}
                ></Select>
              </SectionField>
            </Section>
            <OrganizationSubForm isContactMethodInvalid={isContactMethodInvalid} />
            <Section header="Individual Contacts">
              <SectionField
                label="Connected to this organization"
                labelWidth="auto"
                tooltip="To unlink a contact from this organization, or edit a contact's information, click on the name and unlink from the individual contact page"
              >
                {persons &&
                  persons.map((person, index: number) =>
                    person?.id ? (
                      <Styled.ContactLink key={'organization-person-' + person.id + '-contact'}>
                        <Link
                          to={'/contact/P' + person.id}
                          data-testid={`contact-organization-person-${index}`}
                          key={`org-person-${index}`}
                          className="d-block"
                        >
                          {
                            formatApiPersonNames(
                              person as ApiGen_Concepts_Person,
                            ) /**cast is safe due to id check above*/
                          }
                        </Link>
                      </Styled.ContactLink>
                    ) : null,
                  )}
              </SectionField>
            </Section>
            <Section header="Mailing Address" isCollapsable initiallyExpanded>
              <Address namespace="mailingAddress" />
            </Section>
            <Section header="Property Address" isCollapsable initiallyExpanded>
              <Address namespace="propertyAddress" />
            </Section>
            <Section header="Billing Address" isCollapsable initiallyExpanded>
              <Address namespace="billingAddress" />
            </Section>
            <Section header="Comments">
              <TextArea rows={5} field="comment" />
            </Section>
          </FlexBox>
        </Styled.Form>
      </Styled.ScrollingFormLayout>

      <Styled.ButtonGroup>
        {Object.keys(touched).length > 0 && Object.keys(errors).length > 0 ? (
          <Styled.ErrorMessage gap="0.5rem" className="mr-3">
            <AiOutlineExclamationCircle size="1.8rem" className="mt-1" />
            <span>Please complete required fields</span>
          </Styled.ErrorMessage>
        ) : null}
        <Button variant="secondary" onClick={onCancel}>
          Cancel
        </Button>
        <Button onClick={submitForm}>Save</Button>
      </Styled.ButtonGroup>
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </>
  );
};

export default UpdateOrganizationForm;
