import { Formik, FormikHelpers, FormikProps, getIn } from 'formik';
import { useEffect, useMemo, useRef, useState } from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { Button } from '@/components/common/buttons/Button';
import { UnsavedChangesPrompt } from '@/components/common/form/UnsavedChangesPrompt';
import { FlexBox } from '@/components/common/styles';
import {
  CancelConfirmationModal,
  DuplicateContactModal,
  useAddressHelpers,
} from '@/features/contacts/contact/create/components';
import * as Styled from '@/features/contacts/contact/create/styles';
import { IEditablePersonAddressForm, IEditablePersonForm } from '@/features/contacts/formModels';
import useAddContact from '@/features/contacts/hooks/useAddContact';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { usePrevious } from '@/hooks/usePrevious';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { isValidId } from '@/utils';

import PersonSubForm from '../../Person/PersonSubForm';
import { onValidatePerson } from '../../utils/contactUtils';

/**
 * Formik-connected form to Create Individual Contacts
 */
export const CreatePersonForm: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const history = useHistory();
  const { addPerson } = useAddContact();

  const [showDuplicateModal, setShowDuplicateModal] = useState(false);
  const [allowDuplicate, setAllowDuplicate] = useState(false);

  // validation needs to be adjusted when country == OTHER
  const { otherCountryId } = useAddressHelpers();

  const formikRef = useRef<FormikProps<IEditablePersonForm>>(null);

  const onSubmit = async (
    formPerson: IEditablePersonForm,
    { setSubmitting }: FormikHelpers<IEditablePersonForm>,
  ) => {
    try {
      setShowDuplicateModal(false);
      const newPerson = formPerson.formPersonToApiPerson();
      const personResponse = await addPerson(newPerson, setShowDuplicateModal, allowDuplicate);

      if (isValidId(personResponse?.id)) {
        history.push(`/contact/P${personResponse?.id}`);
      }
    } finally {
      setSubmitting(false);
    }
  };

  const saveDuplicate = async () => {
    setAllowDuplicate(true);
    if (formikRef.current) {
      formikRef.current.handleSubmit();
    }
  };

  return (
    <>
      <Formik
        component={CreatePersonComponent}
        initialValues={new IEditablePersonForm()}
        enableReinitialize
        validate={(values: IEditablePersonForm) => onValidatePerson(values, otherCountryId)}
        onSubmit={onSubmit}
        innerRef={formikRef}
      />
      <DuplicateContactModal
        variant="warning"
        display={showDuplicateModal}
        handleOk={() => saveDuplicate()}
        handleCancel={() => {
          setAllowDuplicate(false);
          setShowDuplicateModal(false);
        }}
      ></DuplicateContactModal>
    </>
  );
};

export default CreatePersonForm;

/**
 * Sub-component that is wrapped by Formik
 */
const CreatePersonComponent: React.FC<FormikProps<IEditablePersonForm>> = ({
  values,
  errors,
  touched,
  dirty,
  resetForm,
  submitForm,
  setFieldValue,
  initialValues,
}) => {
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
      history.push('/contact/list');
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
          const mailing = data.organizationAddresses?.find(
            a => a.addressUsageType?.id === ApiGen_CodeTypes_AddressUsageTypes.MAILING,
          );

          if (mailing) {
            const personMailinAddress = IEditablePersonAddressForm.apiOrgAddressToFormAddress(
              personId,
              mailing,
            );

            personMailinAddress.personAddressId = 0;
            setFieldValue('mailingAddress', personMailinAddress);
          }
        })
        .catch(() => {
          setFieldValue(
            'mailingAddress',
            new IEditablePersonAddressForm(ApiGen_CodeTypes_AddressUsageTypes.MAILING),
          );
          toast.error('Failed to get organization address.');
        });
    }
  }, [useOrganizationAddress, organizationId, setFieldValue, getOrganization, personId]);

  // toggle is off - clear out existing values
  useEffect(() => {
    if (previousUseOrganizationAddress === true && useOrganizationAddress === false) {
      setFieldValue(
        'mailingAddress',
        new IEditablePersonAddressForm(ApiGen_CodeTypes_AddressUsageTypes.MAILING),
      );
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
      <UnsavedChangesPrompt />

      {/* Confirmation popup when Cancel button is clicked */}
      <CancelConfirmationModal
        variant="info"
        display={showConfirmation}
        setDisplay={setShowConfirmation}
        handleOk={() => {
          resetForm({ values: initialValues });
          // need a timeout here to give the form time to reset before navigating away
          // or else the router guard prompt will also be shown
          setTimeout(() => history.push('/contact/list'), 100);
        }}
      />

      <Styled.CreateFormLayout>
        <Styled.Form id="createForm">
          <FlexBox column>
            <PersonSubForm isContactMethodInvalid={isContactMethodInvalid} />
          </FlexBox>
        </Styled.Form>
      </Styled.CreateFormLayout>

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
