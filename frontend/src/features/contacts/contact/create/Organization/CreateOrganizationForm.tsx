import { Button } from 'components/common/buttons/Button';
import { Input } from 'components/common/form';
import { FormSection } from 'components/common/form/styles';
import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import { FlexBox } from 'components/common/styles';
import { CountryCodes } from 'constants/countryCodes';
import {
  Address,
  CancelConfirmationModal,
  CommentNotes,
  ContactEmailList,
  ContactPhoneList,
  DuplicateContactModal,
  useAddressHelpers,
} from 'features/contacts/contact/create/components';
import * as Styled from 'features/contacts/contact/create/styles';
import {
  hasAddress,
  hasEmail,
  hasPhoneNumber,
  OrganizationValidationSchema,
} from 'features/contacts/contact/create/validation';
import { formOrganizationToApiOrganization } from 'features/contacts/contactUtils';
import useAddContact from 'features/contacts/hooks/useAddContact';
import {
  Formik,
  FormikHelpers,
  FormikProps,
  getIn,
  validateYupSchema,
  yupToFormErrors,
} from 'formik';
import { defaultCreateOrganization, IEditableOrganizationForm } from 'interfaces/editable-contact';
import { useMemo, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';

/**
 * Formik-connected form to Create Organizational Contacts
 */
export const CreateOrganizationForm: React.FunctionComponent = () => {
  const history = useHistory();
  const { addOrganization } = useAddContact();

  const [showDuplicateModal, setShowDuplicateModal] = useState(false);
  const [allowDuplicate, setAllowDuplicate] = useState(false);

  // validation needs to be adjusted when country == OTHER
  const { countries } = useAddressHelpers();
  const otherCountryId = useMemo(
    () => countries.find(c => c.code === CountryCodes.Other)?.value?.toString(),
    [countries],
  );

  const formikRef = useRef<FormikProps<IEditableOrganizationForm>>(null);
  const onValidate = (values: IEditableOrganizationForm) => {
    const errors = {} as any;
    try {
      // combine yup schema validation with custom rules
      if (!hasEmail(values) && !hasPhoneNumber(values) && !hasAddress(values)) {
        errors.needsContactMethod =
          'Contacts must have a minimum of one method of contact to be saved. (ex: email,phone or address)';
      }
      validateYupSchema(values, OrganizationValidationSchema, true, {
        otherCountry: otherCountryId,
      });
      return errors;
    } catch (err) {
      return { ...errors, ...yupToFormErrors(err) };
    }
  };

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
        validate={onValidate}
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
            <FormSection>
              <Row>
                <Col>
                  <Input field="name" label="Organization Name" required />
                </Col>
              </Row>
              <Row>
                <Col>
                  <Input field="alias" label="Alias" />
                </Col>
              </Row>
              <Row>
                <Col md={6}>
                  <Input field="incorporationNumber" label="Incorporation Number" />
                </Col>
                <Col></Col>
              </Row>
            </FormSection>

            <FormSection>
              <Styled.H2>Contact info</Styled.H2>
              <Styled.SectionMessage
                appearance={isContactMethodInvalid ? 'error' : 'information'}
                gap="0.5rem"
              >
                <AiOutlineExclamationCircle size="1.8rem" className="mt-2" />
                <p>
                  Contacts must have a minimum of one method of contact to be saved. <br />
                  <em>(ex: email,phone or address)</em>
                </p>
              </Styled.SectionMessage>
              <ContactEmailList
                field="emailContactMethods"
                contactEmails={values.emailContactMethods}
              />
              <br />
              <ContactPhoneList
                field="phoneContactMethods"
                contactPhones={values.phoneContactMethods}
              />
            </FormSection>

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
