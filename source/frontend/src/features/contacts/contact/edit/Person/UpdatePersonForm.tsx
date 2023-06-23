import { Formik, FormikHelpers, FormikProps, getIn } from 'formik';
import { useEffect, useMemo, useState } from 'react';
import { Col } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { Button } from '@/components/common/buttons/Button';
import { Select } from '@/components/common/form';
import { FormSection } from '@/components/common/form/styles';
import { UnsavedChangesPrompt } from '@/components/common/form/UnsavedChangesPrompt';
import { FlexBox } from '@/components/common/styles';
import { AddressTypes } from '@/constants/addressTypes';
import {
  CancelConfirmationModal,
  useAddressHelpers,
} from '@/features/contacts/contact/create/components';
import * as Styled from '@/features/contacts/contact/edit/styles';
import {
  apiAddressToFormAddress,
  apiPersonToFormPerson,
  formPersonToApiPerson,
  getApiMailingAddress,
} from '@/features/contacts/contactUtils';
import { usePersonDetail } from '@/features/contacts/hooks/usePersonDetail';
import useUpdateContact from '@/features/contacts/hooks/useUpdateContact';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { usePrevious } from '@/hooks/usePrevious';
import {
  defaultCreatePerson,
  getDefaultAddress,
  IEditablePersonForm,
} from '@/interfaces/editable-contact';

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
  const formPerson = useMemo(() => apiPersonToFormPerson(person), [person]);

  // validation needs to be adjusted when country == OTHER
  const { otherCountryId } = useAddressHelpers();

  const onSubmit = async (
    formPerson: IEditablePersonForm,
    { setSubmitting }: FormikHelpers<IEditablePersonForm>,
  ) => {
    try {
      let apiPerson = formPersonToApiPerson(formPerson);
      const personResponse = await updatePerson(apiPerson);
      const personId = personResponse?.id;

      if (!!personId) {
        history.push(`/contact/P${personId}`);
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Formik
      component={UpdatePersonComponent}
      initialValues={
        !!formPerson
          ? {
              ...defaultCreatePerson,
              ...formPerson,
              mailingAddress: {
                ...defaultCreatePerson.mailingAddress,
                ...formPerson.mailingAddress,
              },
              propertyAddress: {
                ...defaultCreatePerson.propertyAddress,
                ...formPerson.propertyAddress,
              },
              billingAddress: {
                ...defaultCreatePerson.billingAddress,
                ...formPerson.billingAddress,
              },
            }
          : defaultCreatePerson
      }
      enableReinitialize
      validate={(values: IEditablePersonForm) => onValidatePerson(values, otherCountryId)}
      onSubmit={onSubmit}
    />
  );
};

/**
 * Sub-component that is wrapped by Formik
 */
const UpdatePersonComponent: React.FC<
  React.PropsWithChildren<FormikProps<IEditablePersonForm>>
> = ({ values, errors, touched, dirty, resetForm, submitForm, setFieldValue, initialValues }) => {
  const history = useHistory();
  const { getOrganization } = useApiContacts();
  const [showConfirmation, setShowConfirmation] = useState(false);

  const personId = getIn(values, 'id');
  const organizationId = getIn(values, 'organization.id');
  const useOrganizationAddress = getIn(values, 'useOrganizationAddress');
  const previousUseOrganizationAddress = usePrevious(useOrganizationAddress);

  const onCancel = () => {
    if (dirty) {
      setShowConfirmation(true);
    } else {
      history.push(`/contact/P${personId}`);
    }
  };

  const isContactMethodInvalid = useMemo(() => {
    return (
      !!touched.phoneContactMethods &&
      !!touched.emailContactMethods &&
      (!!touched.mailingAddress?.streetAddress1 ||
        !!touched.propertyAddress?.streetAddress1 ||
        !!touched.billingAddress?.streetAddress1) &&
      getIn(errors, 'needsContactMethod')
    );
  }, [touched, errors]);

  // update mailing address sub-form when "useOrganizationAddress" checkbox is toggled
  useEffect(() => {
    // toggle is on - set mailing address values to match organization address
    if (useOrganizationAddress === true && organizationId) {
      getOrganization(organizationId)
        .then(({ data }) => {
          const mailing = getApiMailingAddress(data);
          setFieldValue('mailingAddress', apiAddressToFormAddress(mailing));
        })
        .catch(() => {
          setFieldValue('mailingAddress', getDefaultAddress(AddressTypes.Mailing));
          toast.error('Failed to get organization address.');
        });
    }
  }, [useOrganizationAddress, organizationId, setFieldValue, getOrganization]);

  // toggle is off - clear out existing values
  useEffect(() => {
    if (previousUseOrganizationAddress === true && useOrganizationAddress === false) {
      setFieldValue('mailingAddress', getDefaultAddress(AddressTypes.Mailing));
    }
  }, [previousUseOrganizationAddress, useOrganizationAddress, setFieldValue]);

  // uncheck the checkbox when organization field is cleared
  useEffect(() => {
    if (!organizationId) {
      setFieldValue('useOrganizationAddress', false);
    }
  }, [organizationId, setFieldValue]);

  return (
    <>
      {/* Router-based confirmation popup when user tries to navigate away and form has unsaved changes */}
      <UnsavedChangesPrompt />

      {/* Confirmation popup when Cancel button is clicked */}
      <CancelConfirmationModal
        display={showConfirmation}
        setDisplay={setShowConfirmation}
        handleOk={() => {
          resetForm({ values: initialValues });
          // need a timeout here to give the form time to reset before navigating away
          // or else the router guard prompt will also be shown
          setTimeout(() => history.push(`/contact/P${personId}`), 100);
        }}
      />

      <Styled.ScrollingFormLayout>
        <Styled.Form id="updateForm">
          <FlexBox column gap="1.6rem">
            <FormSection className="py-2">
              <Styled.RowAligned className="align-items-center">
                <Col className="d-flex">
                  <span>Individual</span>
                </Col>
                <Col md="auto" className="d-flex ml-auto">
                  <Select
                    className="mb-0"
                    field="isDisabled"
                    options={[
                      { label: 'Inactive', value: 'true' },
                      { label: 'Active', value: 'false' },
                    ]}
                  ></Select>
                </Col>
              </Styled.RowAligned>
            </FormSection>

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
    </>
  );
};

export default UpdatePersonForm;
