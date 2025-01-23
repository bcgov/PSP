import { Formik, FormikHelpers, FormikProps, getIn } from 'formik';
import { useCallback, useEffect, useMemo } from 'react';
import { Col } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { Button } from '@/components/common/buttons/Button';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import { Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { FlexBox } from '@/components/common/styles';
import { useAddressHelpers } from '@/features/contacts/contact/create/components';
import * as Styled from '@/features/contacts/contact/edit/styles';
import {
  IEditableOrganizationAddressForm,
  IEditablePersonForm,
} from '@/features/contacts/formModels';
import { usePersonDetail } from '@/features/contacts/hooks/usePersonDetail';
import useUpdateContact from '@/features/contacts/hooks/useUpdateContact';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { usePrevious } from '@/hooks/usePrevious';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { isValidId } from '@/utils';

import PersonSubForm from '../../Person/PersonSubForm';
import { onValidatePerson } from '../../utils/contactUtils';

/**
 * Formik-connected form to Update Individual Contacts
 */
export const UpdatePersonForm: React.FC<{ id: number }> = ({ id }) => {
  const history = useHistory();
  const { updatePerson } = useUpdateContact();

  // fetch person details from API for the supplied person's Id
  const { person } = usePersonDetail(id);
  const formPerson = useMemo(() => IEditablePersonForm.apiPersonToFormPerson(person), [person]);

  // validation needs to be adjusted when country == OTHER
  const { otherCountryId } = useAddressHelpers();

  const onSubmit = async (
    formPerson: IEditablePersonForm,
    { setSubmitting }: FormikHelpers<IEditablePersonForm>,
  ) => {
    try {
      const apiPerson = formPerson.formPersonToApiPerson();
      const personResponse = await updatePerson(apiPerson);
      const personId = personResponse?.id;

      if (isValidId(personId)) {
        history.push(`/contact/P${personId}`);
      }
    } finally {
      setSubmitting(false);
    }
  };

  const initialValues = formPerson ? formPerson : new IEditablePersonForm();

  return (
    <Formik<IEditablePersonForm>
      component={UpdatePersonComponent}
      initialValues={initialValues}
      validate={(values: IEditablePersonForm) => onValidatePerson(values, otherCountryId)}
      enableReinitialize
      onSubmit={onSubmit}
    />
  );
};

/**
 * Sub-component that is wrapped by Formik
 */
const UpdatePersonComponent: React.FC<
  React.PropsWithChildren<FormikProps<IEditablePersonForm>>
> = ({ values, errors, touched, dirty, submitForm, setFieldValue, isSubmitting }) => {
  const history = useHistory();
  const { getOrganization } = useApiContacts();
  const isPersonDisabled = getIn(values, 'isDisabled');

  const personId = getIn(values, 'id');
  const organizationId = getIn(values, 'organization.id');
  const useOrganizationAddress = getIn(values, 'useOrganizationAddress');
  const previousUseOrganizationAddress = usePrevious(useOrganizationAddress);

  const onCancel = () => {
    history.push(`/contact/P${personId}`);
  };

  const isContactMethodInvalid = useMemo(() => {
    if (isPersonDisabled === true) {
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
  }, [touched, errors, isPersonDisabled]);

  // update mailing address sub-form when "useOrganizationAddress" checkbox is toggled
  useEffect(() => {
    // toggle is on - set mailing address values to match organization address
    if (useOrganizationAddress === true && organizationId) {
      getOrganization(organizationId)
        .then(({ data }) => {
          const mailing = data.organizationAddresses?.find(
            a => a.addressUsageType?.id === ApiGen_CodeTypes_AddressUsageTypes.MAILING,
          );
          setFieldValue(
            'mailingAddress',
            IEditableOrganizationAddressForm.apiAddressToFormAddress(mailing),
          );
        })
        .catch(() => {
          setFieldValue(
            'mailingAddress',
            new IEditableOrganizationAddressForm(ApiGen_CodeTypes_AddressUsageTypes.MAILING),
          );
          toast.error('Failed to get organization address.');
        });
    }
  }, [useOrganizationAddress, organizationId, setFieldValue, getOrganization]);

  // toggle is off - clear out existing values
  useEffect(() => {
    if (previousUseOrganizationAddress === true && useOrganizationAddress === false) {
      setFieldValue(
        'mailingAddress',
        new IEditableOrganizationAddressForm(ApiGen_CodeTypes_AddressUsageTypes.MAILING),
      );
    }
  }, [previousUseOrganizationAddress, useOrganizationAddress, setFieldValue]);

  // uncheck the checkbox when organization field is cleared
  useEffect(() => {
    if (!organizationId) {
      setFieldValue('useOrganizationAddress', false);
    }
  }, [organizationId, setFieldValue]);

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
                label="Individual"
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

              <Styled.RowAligned className="align-items-center">
                <Col className="d-flex">
                  <span></span>
                </Col>
                <Col md="auto" className="d-flex ml-auto"></Col>
              </Styled.RowAligned>
            </Section>

            <PersonSubForm isContactMethodInvalid={isContactMethodInvalid} />
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

export default UpdatePersonForm;
