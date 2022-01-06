import { AsyncTypeahead, Button, Input } from 'components/common/form';
import { FormSection } from 'components/common/form/styles';
import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import { Stack } from 'components/common/Stack/Stack';
import { Address, ContactEmailList, ContactPhoneList } from 'features/contacts/contact/create';
import { personCreateFormToApiPerson } from 'features/contacts/contactUtils';
import { Formik } from 'formik';
import { useApiAutocomplete } from 'hooks/pims-api/useApiAutocomplete';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { IAutocompletePrediction } from 'interfaces';
import { defaultCreatePerson, ICreatePersonForm } from 'interfaces/ICreateContact';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';

import { PadBox } from '../styles';
import CommentNotes from './comments/CommentNotes';
import * as Styled from './styles';

export interface ICreatePersonFormProps {}

export const CreatePersonForm: React.FunctionComponent<ICreatePersonFormProps> = props => {
  const { postPerson } = useApiContacts();
  const { goBack } = useHistory();

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

  return (
    <Formik
      initialValues={defaultCreatePerson}
      enableReinitialize
      onSubmit={async values => {
        try {
          await postPerson(personCreateFormToApiPerson(values));
          toast.info('Contact added successfully');
          goBack();
        } catch (error) {
          toast.error('Failed to add contact', { autoClose: 7000 });
        }
      }}
    >
      {({ values }) => (
        <>
          {/* Show confirmation dialog when user tries to navigate away and form has unsaved changes */}
          <UnsavedChangesPrompt />

          <Styled.Form id="createForm">
            <Styled.CreatePersonLayout>
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
                  <Styled.SummaryText>
                    Contacts must have a minimum of one method of contact to be saved. <br />
                    <em>(ex: email,phone or address)</em>
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

                <PadBox className="w-100">
                  <Stack $direction="row" justifyContent="flex-end" gap={2}>
                    <Button variant="secondary">Cancel</Button>
                    <Button type="submit">Save</Button>
                  </Stack>
                </PadBox>
              </Stack>
            </Styled.CreatePersonLayout>
          </Styled.Form>
        </>
      )}
    </Formik>
  );
};

export default CreatePersonForm;
