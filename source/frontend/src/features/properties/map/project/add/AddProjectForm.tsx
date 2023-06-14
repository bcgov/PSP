import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { Form, Input, Select, SelectOption, TextArea } from '@/components/common/form';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { ProjectForm } from '../models';
import ProductsArrayForm from './ProductsArrayForm';

export interface IAddProjectFormProps {
  isCreating?: boolean;
  /** Initial values of the form */
  initialValues: ProjectForm;
  projectStatusOptions: SelectOption[];
  businessFunctionOptions: SelectOption[];
  costTypeOptions: SelectOption[];
  workActivityOptions: SelectOption[];
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => void | Promise<any>;
}

const AddProjectForm = React.forwardRef<FormikProps<ProjectForm>, IAddProjectFormProps>(
  (props, formikRef) => {
    const {
      initialValues,
      projectStatusOptions,
      businessFunctionOptions,
      costTypeOptions,
      workActivityOptions,
      validationSchema,
      onSubmit,
    } = props;

    const handleSubmit = async (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
      await onSubmit(values, formikHelpers);
      formikHelpers.setSubmitting(false);
    };

    return (
      <Formik<ProjectForm>
        enableReinitialize
        innerRef={formikRef}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {formikProps => (
          <StyledFormWrapper>
            <Form>
              <Section header="Project Details">
                {props.isCreating === true && (
                  <StyledRow className="no-gutters py-4 mb-5">
                    <Col>
                      <p>
                        Before creating a project, <Link to={'/project/list'}>do a search</Link> to
                        ensure the the project you're creating doesn't already exist.
                      </p>
                    </Col>
                  </StyledRow>
                )}
                <SectionField label="Project name" required labelWidth="2">
                  <Input field="projectName" />
                </SectionField>
                <SectionField label="Project number" labelWidth="2">
                  <Input field="projectNumber" placeholder="if known" />
                </SectionField>
                <SectionField label="Status" labelWidth="2" required>
                  <Select
                    field="projectStatusType"
                    options={projectStatusOptions}
                    placeholder="Select..."
                  />
                </SectionField>
                <SectionField label="MoTI region" required labelWidth="2">
                  <UserRegionSelectContainer field="region" placeholder="Select region..." />
                </SectionField>
                <SectionField label="Project summary" labelWidth="12">
                  <MediumTextArea field="summary" />
                </SectionField>
              </Section>
              <Section header="Associated Codes">
                <SectionField label="Cost type" labelWidth="2">
                  <Select field="costTypeCode" options={costTypeOptions} placeholder="Select..." />
                </SectionField>
                <SectionField label="Work activity" labelWidth="2">
                  <Select
                    field="workActivityCode"
                    options={workActivityOptions}
                    placeholder="Select..."
                  />
                </SectionField>
                <SectionField label="Business function" labelWidth="2">
                  <Select
                    field="businessFunctionCode"
                    options={businessFunctionOptions}
                    placeholder="Select..."
                  />
                </SectionField>
              </Section>
              <ProductsArrayForm formikProps={formikProps} field="products" />
            </Form>
          </StyledFormWrapper>
        )}
      </Formik>
    );
  },
);

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
