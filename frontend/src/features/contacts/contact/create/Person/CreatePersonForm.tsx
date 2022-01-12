import { AsyncTypeahead, Button, Input } from 'components/common/form';
import { FormSection } from 'components/common/form/styles';
import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import GenericModal from 'components/common/GenericModal';
import { Stack } from 'components/common/Stack/Stack';
import { CountryCodes } from 'constants/countryCodes';
import { Address, ContactEmailList, ContactPhoneList } from 'features/contacts/contact/create';
import { personCreateFormToApiPerson } from 'features/contacts/contactUtils';
import {
  Formik,
  FormikHelpers,
  FormikProps,
  getIn,
  validateYupSchema,
  yupToFormErrors,
} from 'formik';
import { useApiAutocomplete } from 'hooks/pims-api/useApiAutocomplete';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IAutocompletePrediction } from 'interfaces';
import { defaultCreatePerson, ICreatePersonForm } from 'interfaces/ICreateContact';
import { useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import useAddressHelpers from '../address/useAddressHelpers';
import { PadBox } from '../styles';
import CommentNotes from './comments/CommentNotes';
import * as Styled from './styles';
import { hasAddress, hasEmail, hasPhoneNumber, validationSchema } from './validation';

export interface ICreatePersonFormProps {}

/**
 * Formik-connected form to Create Individual Contacts
 */
export const CreatePersonForm: React.FunctionComponent<ICreatePersonFormProps> = props => {
  const history = useHistory();
  const { postPerson } = useApiContacts();

  // validation needs to be adjusted when country == OTHER
  const { countries } = useAddressHelpers();
  const otherCountryId = useMemo(
    () => countries.find(c => c.code === CountryCodes.Other)?.value?.toString(),
    [countries],
  );

  const onValidate = (values: ICreatePersonForm) => {
    try {
      validateYupSchema(values, validationSchema, true, { otherCountry: otherCountryId });
      // combine yup schema validation with custom rules
      const errors = {} as any;
      if (!hasEmail(values) && !hasPhoneNumber(values) && !hasAddress(values)) {
        errors.needsContactMethod =
          'Contacts must have a minimum of one method of contact to be saved. (ex: email,phone or address)';
      }
      return errors;
    } catch (err) {
      return yupToFormErrors(err);
    }
  };

  const onSubmit = async (
    values: ICreatePersonForm,
    { resetForm, setSubmitting }: FormikHelpers<ICreatePersonForm>,
  ) => {
    try {
      await postPerson(personCreateFormToApiPerson(values));
      setSubmitting(false);
      toast.info('Contact added successfully');
      history.push('/contact/list');
    } catch (error) {
      toast.error('Failed to add contact', { autoClose: 7000 });
    }
  };

  return (
    <Formik
      component={CreatePersonComponent}
      initialValues={defaultCreatePerson}
      enableReinitialize
      validate={onValidate}
      onSubmit={onSubmit}
    />
  );
};

export default CreatePersonForm;

/**
 * Sub-component that is wrapped by Formik
 */
const CreatePersonComponent: React.FC<FormikProps<ICreatePersonForm>> = ({
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

  // organization type-ahead state
  const { getOrganizationPredictions } = useApiAutocomplete();
  const [isTypeaheadLoading, setIsTypeaheadLoading] = useState(false);
  const [matchedOrgs, setMatchedOrgs] = useState<IAutocompletePrediction[]>([]);

  // fetch autocomplete suggestions from server
  const handleTypeaheadSearch = async (query: string) => {
    try {
      setIsTypeaheadLoading(true);
      const { data } = await getOrganizationPredictions(query);
      setMatchedOrgs(data.predictions);
      setIsTypeaheadLoading(false);
    } catch (e) {
      setMatchedOrgs([]);
      toast.error('Failed to get autocomplete results for supplied organization', {
        autoClose: 7000,
      });
    } finally {
      setIsTypeaheadLoading(false);
    }
  };

  const onCancel = () => {
    if (dirty) {
      setShowConfirmation(true);
    } else {
      history.push('/contact/list');
    }
  };

  return (
    <>
      {/* Router-based confirmation popup when user tries to navigate away and form has unsaved changes */}
      <UnsavedChangesPrompt />

      {/* Confirmation popup when Cancel button is clicked */}
      <GenericModal
        title="Unsaved Changes"
        message="Confirm cancel adding this contact? Changes will not be saved."
        cancelButtonText="No"
        okButtonText="Confirm"
        display={showConfirmation}
        handleOk={() => {
          resetForm({ values: initialValues });
          // need a timeout here to give the form time to reset before navigating away
          // or else the router guard prompt will also be shown
          setTimeout(() => history.push('/contact/list'), 100);
        }}
        handleCancel={() => setShowConfirmation(false)}
      />

      <Styled.CreatePersonLayout>
        <Styled.Form id="createForm">
          <Stack gap={1.6}>
            <FormSection>
              <Row>
                <Col md={4}>
                  <Input field="firstName" label="First Name" required />
                </Col>
                <Col md={3}>
                  <Input field="middleNames" label="Middle" />
                </Col>
                <Col>
                  <Input field="surname" label="Last Name" required />
                </Col>
              </Row>
              <Row>
                <Col md={7}>
                  <Input field="preferredName" label="Preferred Name" />
                </Col>
                <Col></Col>
              </Row>
            </FormSection>

            <FormSection>
              <Styled.H2>Organization</Styled.H2>
              <Row>
                <Col md={7}>
                  <AsyncTypeahead
                    field="organizationId"
                    label="Link to an existing organization"
                    labelKey="text"
                    isLoading={isTypeaheadLoading}
                    options={matchedOrgs}
                    onSearch={handleTypeaheadSearch}
                  />
                </Col>
              </Row>
            </FormSection>

            <FormSection>
              <Styled.H2>Contact info</Styled.H2>
              <Styled.SummaryText
                $direction="row"
                alignItems="flex-start"
                variant={getIn(errors, 'needsContactMethod') ? 'error' : 'text'}
                gap="0.5rem"
              >
                <AiOutlineExclamationCircle size="1.8rem" className="mt-2" />
                <p>
                  Contacts must have a minimum of one method of contact to be saved. <br />
                  <em>(ex: email,phone or address)</em>
                </p>
              </Styled.SummaryText>
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
          </Stack>
        </Styled.Form>
      </Styled.CreatePersonLayout>

      <PadBox className="w-100">
        <Stack $direction="row" justifyContent="flex-end" alignItems="stretch" gap={2}>
          {Object.keys(touched).length > 0 && Object.keys(errors).length > 0 ? (
            <Styled.ErrorMessage gap="0.5rem" className="mr-3" alignItems="center">
              <AiOutlineExclamationCircle size="1.8rem" className="mt-1" />
              <span>Please complete required fields</span>
            </Styled.ErrorMessage>
          ) : null}
          <Button variant="secondary" onClick={onCancel}>
            Cancel
          </Button>
          <Button onClick={submitForm}>Save</Button>
        </Stack>
      </PadBox>
    </>
  );
};
