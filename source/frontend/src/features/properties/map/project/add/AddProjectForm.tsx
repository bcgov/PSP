import { Form, Input, Select, SelectOption, TextArea } from 'components/common/form';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { ProjectForm } from './models';
import ProductsArrayForm from './ProductsArrayForm';

export interface IAddProjectFormProps {
  formikRef: React.RefObject<FormikProps<ProjectForm>>;
  /** Initial values of the form */
  initialValues: ProjectForm;
  projectStatusOptions: SelectOption[];
  projectRegionOptions: SelectOption[];
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => void | Promise<any>;
}

export const AddProjectForm: React.FC<IAddProjectFormProps> = props => {
  const { initialValues, projectStatusOptions, projectRegionOptions, validationSchema, onSubmit } =
    props;

  const handleSubmit = async (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
    await onSubmit(values, formikHelpers);
    formikHelpers.setSubmitting(false);
  };

  return (
    <Formik<ProjectForm>
      enableReinitialize
      innerRef={props.formikRef}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      {formikProps => (
        <StyledFormWrapper>
          <Form>
            <Section>
              <StyledRow className="no-gutters py-4 mb-5">
                <Col>
                  <p>
                    Before creating a project, <Link to={'/project/list'}>do a search</Link> to
                    ensure the the project you're creating doesn't already exist.
                  </p>
                </Col>
              </StyledRow>
              <SectionField label="Project name" required labelWidth="2">
                <Input field="projectName" />
              </SectionField>
              <SectionField label="Project number" labelWidth="2">
                <Input field="projectNumber" placeholder="if known" />
              </SectionField>
              <SectionField label="Status" labelWidth="2">
                <Select
                  field="projectStatusType"
                  options={projectStatusOptions}
                  placeholder="Select..."
                />
              </SectionField>
              <SectionField label="MoTI region" required labelWidth="2">
                <Select field="region" options={projectRegionOptions} placeholder="Select..." />
              </SectionField>
              <SectionField label="Project summary" labelWidth="12">
                <MediumTextArea field="summary" />
              </SectionField>
            </Section>
            <ProductsArrayForm formikProps={formikProps} field="products" />
          </Form>
        </StyledFormWrapper>
      )}
    </Formik>
  );
};

export default AddProjectForm;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;

const StyledRow = styled(Row)`
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
  text-align: left;
`;

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;
