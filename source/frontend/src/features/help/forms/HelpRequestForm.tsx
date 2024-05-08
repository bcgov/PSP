import { Formik } from 'formik';
import noop from 'lodash/noop';
import Form from 'react-bootstrap/Form';
import styled from 'styled-components';

import { Input, TextArea } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';

import { IHelpForm } from '../interfaces';

interface HelpRequestFormProps {
  /** Form values that should overwrite the default form values */
  formValues: IHelpForm;
  /** Call this function whenever the form fields are updated to keep the mailto in sync with this form */
  setMailto: (mailto: { subject: string; body: string; email: string }) => void;
}

interface IHelpRequestForm extends IHelpForm {
  description: string;
}

const defaultHelpFormValues: IHelpRequestForm = {
  user: '',
  email: '',
  page: '',
  description: '',
};

/**
 * Form allowing user to request a feature. The state of this form is synchronized with the parent's mailto.
 */
const HelpRequestForm: React.FunctionComponent<React.PropsWithChildren<HelpRequestFormProps>> = ({
  formValues,
  setMailto,
}) => {
  const initialValues = { ...defaultHelpFormValues, ...formValues };

  return (
    <Container>
      <Formik
        initialValues={initialValues}
        onSubmit={noop}
        validateOnMount={true}
        validate={(values: any) => {
          const mailto = {
            subject: `Help Desk Request - ${formValues.page}`,
            body: values.description,
            email: values.email,
          };
          setMailto(mailto);
        }}
      >
        <Form>
          <SectionField label="Name" labelWidth="2">
            <Input field="user" />
          </SectionField>
          <SectionField label="Email" labelWidth="2">
            <Input field="email" />
          </SectionField>
          <SectionField label="Description" labelWidth="12">
            <TextArea field="description" placeholder="Question, suggestion, bug..." />
          </SectionField>
        </Form>
      </Formik>
    </Container>
  );
};

const Container = styled.div`
  margin-left: 0;
  overflow: hidden;
`;

export default HelpRequestForm;
