import { Button, Input } from 'components/common/form';
import { FormSection } from 'components/common/form/styles';
import { Stack } from 'components/common/Stack/Stack';
import { Formik } from 'formik';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { defaultCreatePerson } from 'interfaces/ICreateContact';
import { Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';

import { PadBox } from '../styles';
import Address from './address/Address';
import CommentNotes from './comments/CommentNotes';
import * as Styled from './styles';

export interface ICreatePersonFormProps {}

export const CreatePersonForm: React.FunctionComponent<ICreatePersonFormProps> = props => {
  const { postPerson } = useApiContacts();
  const { goBack } = useHistory();

  return (
    <Formik
      initialValues={defaultCreatePerson}
      enableReinitialize
      onSubmit={async values => {
        try {
          await postPerson(values);
          toast.info('Contact added successfully');
          goBack();
        } catch (error) {
          toast.error('Failed to add contact', { autoClose: 7000 });
        }
      }}
    >
      {() => (
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
                <Styled.H3>Link to an existing organization</Styled.H3>
              </FormSection>

              <FormSection>
                <Styled.H2>Contact info</Styled.H2>
                <Styled.SummaryText>
                  Contacts must have a minimum of one method of contact to be saved. (ex: email,
                  phone or address)
                </Styled.SummaryText>
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
      )}
    </Formik>
  );
};

export default CreatePersonForm;
