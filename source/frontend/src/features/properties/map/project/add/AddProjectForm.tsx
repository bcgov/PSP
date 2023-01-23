import { Input, Select, SelectOption } from 'components/common/form';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Container } from 'react-bootstrap';
import styled from 'styled-components';

import { ProjectForm } from './models';
import * as Styled from './styles';

export interface IAddProjectFormProps {
  /** Initial values of the form */
  initialValues: ProjectForm;
  projectStatusOptions: SelectOption[];
  projectRegionOptions: SelectOption[];
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => void | Promise<any>;
}

export const AddProjectForm = React.forwardRef<FormikProps<ProjectForm>, IAddProjectFormProps>(
  (props, ref) => {
    const {
      initialValues,
      projectStatusOptions,
      projectRegionOptions,
      validationSchema,
      onSubmit,
    } = props;

    const handleSubmit = (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
      onSubmit(values, formikHelpers);
    };

    return (
      <Formik<ProjectForm>
        enableReinitialize
        innerRef={ref}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {formikProps => (
          <>
            <Container>
              <Section>
                <SectionField label="Project name" required={true}>
                  <Input field="projectName" />
                </SectionField>
                <SectionField label="Project number" required={true}>
                  <Input field="projectNumber" placeholder="if known" />
                </SectionField>
                <SectionField label="Status">
                  <Select
                    field="projectStatusType"
                    options={projectStatusOptions}
                    placeholder="Select..."
                  />
                </SectionField>
                <SectionField label="MoTI region" required={true}>
                  <Select field="region" options={projectRegionOptions} placeholder="Select..." />
                </SectionField>
                <SectionField label="Project summary">
                  <Styled.MediumTextArea field="summary" />
                </SectionField>
              </Section>
            </Container>
          </>
        )}
      </Formik>
    );
  },
);

export default AddProjectForm;
