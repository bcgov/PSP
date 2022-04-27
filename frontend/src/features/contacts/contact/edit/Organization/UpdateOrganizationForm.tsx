import { Button } from 'components/common/buttons/Button';
import { Input, Select } from 'components/common/form';
import { FormSection } from 'components/common/form/styles';
import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import { FlexBox } from 'components/common/styles';
import TooltipIcon from 'components/common/TooltipIcon';
import {
  Address,
  CancelConfirmationModal,
  CommentNotes,
  ContactEmailList,
  ContactPhoneList,
  useAddressHelpers,
} from 'features/contacts/contact/create/components';
import {
  hasAddress,
  hasEmail,
  hasPhoneNumber,
  OrganizationValidationSchema,
} from 'features/contacts/contact/create/validation';
import * as Styled from 'features/contacts/contact/edit/styles';
import {
  apiOrganizationToFormOrganization,
  formOrganizationToApiOrganization,
} from 'features/contacts/contactUtils';
import { useOrganizationDetail } from 'features/contacts/hooks/useOrganizationDetail';
import useUpdateContact from 'features/contacts/hooks/useUpdateContact';
import {
  Formik,
  FormikHelpers,
  FormikProps,
  getIn,
  validateYupSchema,
  yupToFormErrors,
} from 'formik';
import { defaultCreateOrganization, IEditableOrganizationForm } from 'interfaces/editable-contact';
import { IContactPerson } from 'interfaces/IContact';
import { useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { Link, useHistory } from 'react-router-dom';

/**
 * Formik-connected form to Update Organizational Contacts
 */
export const UpdateOrganizationForm: React.FC<{ id: number }> = ({ id }) => {
  const history = useHistory();
  const { updateOrganization } = useUpdateContact();
  const { otherCountryId } = useAddressHelpers();

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
      const apiOrganization = formOrganizationToApiOrganization(formOrganization);
      const organizationResponse = await updateOrganization(apiOrganization);
      const organizationId = organizationResponse?.id;

      if (!!organizationId) {
        history.push(`/contact/O${organizationId}`);
      }
    } finally {
      setSubmitting(false);
    }
  };

  // fetch organization details from API for the supplied Id
  const { organization } = useOrganizationDetail(id);
  const formOrganization = useMemo(() => apiOrganizationToFormOrganization(organization), [
    organization,
  ]);

  const initialValues = !!formOrganization
    ? { ...defaultCreateOrganization, ...formOrganization }
    : defaultCreateOrganization;

  return (
    <Formik
      component={UpdateOrganization}
      initialValues={initialValues}
      validate={onValidate}
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
  resetForm,
  submitForm,
  initialValues,
}) => {
  const history = useHistory();
  const [showConfirmation, setShowConfirmation] = useState(false);

  const organizationId = getIn(values, 'id');
  const persons = getIn(values, 'persons') as Partial<IContactPerson>[];

  const onCancel = () => {
    if (dirty) {
      setShowConfirmation(true);
    } else {
      history.push(`/contact/O${organizationId}`);
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
          setTimeout(() => history.push(`/contact/O${organizationId}`), 100);
        }}
      />

      <Styled.ScrollingFormLayout>
        <Styled.Form id="updateForm">
          <FlexBox column gap="1.6rem">
            <FormSection className="py-2">
              <Styled.RowAligned className="align-items-center">
                <Col className="d-flex">
                  <span>Organization</span>
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

            <FormSection className="position-relative">
              <Styled.TopRightCorner>
                <TooltipIcon
                  placement="left"
                  toolTipId="individual-contacts"
                  toolTip="To unlink a contact from this organization, or edit a contact's information, click on the name and unlink from the individual contact page."
                />
              </Styled.TopRightCorner>
              <Styled.H2>Individual Contacts</Styled.H2>
              <Styled.H3>Connected to this organization:</Styled.H3>
              <Styled.RowAligned>
                <Col>
                  {persons &&
                    persons.map((person, index: number) => (
                      <>
                        <Link
                          to={'/contact/P' + person.id}
                          data-testid={`contact-organization-person-${index}`}
                          key={`org-person-${index}`}
                        >
                          {person.fullName}
                        </Link>
                        <br />
                      </>
                    ))}
                </Col>
              </Styled.RowAligned>
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

export default UpdateOrganizationForm;
