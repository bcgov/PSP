import { Formik, FormikHelpers, FormikProps, getIn } from 'formik';
import { useMemo, useRef, useState } from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';

import { Button } from '@/components/common/buttons/Button';
import { FormSection } from '@/components/common/form/styles';
import { UnsavedChangesPrompt } from '@/components/common/form/UnsavedChangesPrompt';
import { FlexBox } from '@/components/common/styles';
import {
  Address,
  CancelConfirmationModal,
  CommentNotes,
  DuplicateContactModal,
  useAddressHelpers,
} from '@/features/contacts/contact/create/components';
import * as Styled from '@/features/contacts/contact/create/styles';
import { formOrganizationToApiOrganization } from '@/features/contacts/contactUtils';
import useAddContact from '@/features/contacts/hooks/useAddContact';
import {
  defaultCreateOrganization,
  IEditableOrganizationForm,
} from '@/interfaces/editable-contact';

import OrganizationSubForm from '../../Organization/OrganizationSubForm';
import { onValidateOrganization } from '../../utils/contactUtils';

/**
 * Formik-connected form to Create Organizational Contacts
 */
export const CreateOrganizationForm: React.FunctionComponent<unknown> = () => {
  const history = useHistory();
  const { addOrganization } = useAddContact();

  const [showDuplicateModal, setShowDuplicateModal] = useState(false);
  const [allowDuplicate, setAllowDuplicate] = useState(false);

  // validation needs to be adjusted when country == OTHER
  const { otherCountryId } = useAddressHelpers();

  const formikRef = useRef<FormikProps<IEditableOrganizationForm>>(null);

  const onSubmit = async (
    formOrganization: IEditableOrganizationForm,
    { setSubmitting }: FormikHelpers<IEditableOrganizationForm>,
  ) => {
    try {
      setShowDuplicateModal(false);
      let newOrganization = formOrganizationToApiOrganization(formOrganization);

      const organizationResponse = await addOrganization(
        newOrganization,
        setShowDuplicateModal,
        allowDuplicate,
      );

      if (!!organizationResponse?.id) {
        history.push(`/contact/O${organizationResponse?.id}`);
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
        component={CreateOrganizationComponent}
        initialValues={defaultCreateOrganization}
        validate={(values: IEditableOrganizationForm) =>
          onValidateOrganization(values, otherCountryId)
        }
        enableReinitialize
        onSubmit={onSubmit}
        innerRef={formikRef}
      />
      <DuplicateContactModal
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

export default CreateOrganizationForm;

/**
 * Sub-component that is wrapped by Formik
 */
const CreateOrganizationComponent: React.FC<FormikProps<IEditableOrganizationForm>> = ({
  values,
  errors,
  touched,
  dirty,
  resetForm,
  submitForm,
  initialValues,
}) => {
  const history = useHistory();
  const [showConfirmation, setShowConfirmation] = useState(false);

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
          setTimeout(() => history.push('/contact/list'), 100);
        }}
      />

      <Styled.CreateFormLayout>
        <Styled.Form id="createForm">
          <FlexBox column gap="1.6rem">
            <OrganizationSubForm isContactMethodInvalid={isContactMethodInvalid} />

            <FormSection>
              <Styled.H2>Address</Styled.H2>
              <Styled.H3>Mailing Address</Styled.H3>
              <Address namespace="mailingAddress" />
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
