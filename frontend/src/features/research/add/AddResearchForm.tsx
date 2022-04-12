import { InlineInput } from 'components/common/form/styles';
import { Formik } from 'formik';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { Button, Col, Row } from 'react-bootstrap';
import { Prompt, useHistory } from 'react-router-dom';
import styled from 'styled-components';

import { useAddResearch } from '../hooks/useAddResearch';
import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import { ResearchForm } from './models';
import ResearchProperties from './ResearchProperties';

const AddResearchForm: React.FunctionComponent = () => {
  const initialForm = new ResearchForm();
  const history = useHistory();

  const { addResearchFile } = useAddResearch();

  const submitForm = async (researchFile: Api_ResearchFile) => {
    const response = await addResearchFile(researchFile);
    if (!!response?.name) {
      history.push(`/research/${response?.name}`);
    }
  };

  const onCancel = () => {
    history.push(`/`);
  };
  return (
    <Formik<ResearchForm>
      initialValues={initialForm}
      onSubmit={async (values: ResearchForm, formikHelpers) => {
        const researchFile: Api_ResearchFile = values.toApi();
        formikHelpers.setSubmitting(false);
        submitForm(researchFile);
      }}
      validationSchema={AddResearchFileYupSchema}
    >
      {formikProps => (
        <StyledFormWrapper>
          <Row className="py-4 no-gutters">
            <Col xs="auto" className="pr-5">
              <strong>Name this research file:</strong>
            </Col>
            <Col xs="auto">
              <LargeInlineInput field="name" />A unique file number will be generated for this
              research file on save.
            </Col>
          </Row>
          <ResearchProperties />
          <Row className="justify-content-end mt-auto no-gutters pt-3">
            <Col xs="auto" className="pr-4">
              <Button variant="secondary" onClick={onCancel}>
                Cancel
              </Button>
            </Col>
            <Col xs="auto">
              <Button
                disabled={formikProps.isSubmitting}
                onClick={async () => {
                  formikProps.setSubmitting(true);
                  formikProps.submitForm();
                }}
              >
                Save
              </Button>
            </Col>
          </Row>

          <Prompt
            when={formikProps.dirty && formikProps.submitCount === 0}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
        </StyledFormWrapper>
      )}
    </Formik>
  );
};

export default AddResearchForm;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;

const LargeInlineInput = styled(InlineInput)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;
