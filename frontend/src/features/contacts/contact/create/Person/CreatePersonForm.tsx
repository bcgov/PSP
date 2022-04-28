import { Button } from 'components/common/buttons/Button';
import { AsyncTypeahead, Check, Input } from 'components/common/form';
import { FormSection } from 'components/common/form/styles';
import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import { FlexBox } from 'components/common/styles';
import { AddressTypes } from 'constants/addressTypes';
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
  PersonValidationSchema,
} from 'features/contacts/contact/create/validation';
import {
  apiAddressToFormAddress,
  formPersonToApiPerson,
  getApiMailingAddress,
} from 'features/contacts/contactUtils';
import useAddContact from 'features/contacts/hooks/useAddContact';
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
import { usePrevious } from 'hooks/usePrevious';
import { IAutocompletePrediction } from 'interfaces';
import {
  defaultCreatePerson,
  getDefaultAddress,
  IEditablePersonForm,
} from 'interfaces/editable-contact';
import { useEffect, useMemo, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

/**
 * Formik-connected form to Create Individual Contacts
 */
export const CreatePersonForm: React.FunctionComponent = () => {
  const history = useHistory();
  const { addPerson } = useAddContact();

  const [showDuplicateModal, setShowDuplicateModal] = useState(false);
  const [allowDuplicate, setAllowDuplicate] = useState(false);

  // validation needs to be adjusted when country == OTHER
  const { countries } = useAddressHelpers();
  const otherCountryId = useMemo(
    () => countries.find(c => c.code === CountryCodes.Other)?.value?.toString(),
    [countries],
  );

  const formikRef = useRef<FormikProps<IEditablePersonForm>>(null);

  const onValidate = (values: IEditablePersonForm) => {
    const errors = {} as any;
    try {
      // combine yup schema validation with custom rules
      if (!hasEmail(values) && !hasPhoneNumber(values) && !hasAddress(values)) {
        errors.needsContactMethod =
          'Contacts must have a minimum of one method of contact to be saved. (ex: email,phone or address)';
      }
      validateYupSchema(values, PersonValidationSchema, true, { otherCountry: otherCountryId });

      return errors;
    } catch (err) {
      return { ...errors, ...yupToFormErrors(err) };
    }
  };

  const onSubmit = async (
    formPerson: IEditablePersonForm,
    { setSubmitting }: FormikHelpers<IEditablePersonForm>,
  ) => {
    try {
      setShowDuplicateModal(false);
      let newPerson = formPersonToApiPerson(formPerson);
      const personResponse = await addPerson(newPerson, setShowDuplicateModal, allowDuplicate);

      if (!!personResponse?.id) {
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
        initialValues={defaultCreatePerson}
        enableReinitialize
        validate={onValidate}
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

  const organizationId = getIn(values, 'organization.id');
  const useOrganizationAddress = getIn(values, 'useOrganizationAddress');
  const previousUseOrganizationAddress = usePrevious(useOrganizationAddress);

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
          setTimeout(() => history.push('/contact/list'), 100);
        }}
      />

      <Styled.CreateFormLayout>
        <Styled.Form id="createForm">
          <FlexBox column gap="1.6rem">
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
                    field="organization"
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
              <Styled.SectionMessage
                appearance={
                  isContactMethodInvalid && getIn(errors, 'needsContactMethod')
                    ? 'error'
                    : 'information'
                }
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
              <Check
                field="useOrganizationAddress"
                postLabel="Use mailing address from organization"
                disabled={!organizationId}
              />
              <Address namespace="mailingAddress" disabled={useOrganizationAddress} />
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
