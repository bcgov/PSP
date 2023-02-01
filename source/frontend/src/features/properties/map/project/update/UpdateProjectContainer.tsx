import { Input, Select, SelectOption, TextArea } from 'components/common/form';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Container } from 'react-bootstrap';
import styled from 'styled-components';

import { ProjectForm } from '../add/models';

export interface IUpdateProjectContainerProps {
  initialValues: ProjectForm;
  projectStatusOptions: SelectOption[];
  projectRegionOptions: SelectOption[];
  validationSchema?: any | (() => any);
  onSubmit: (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => void | Promise<any>;
}

const UpdateProjectContainer = React.forwardRef<FormikProps<any>, IUpdateProjectContainerProps>(
  (props, formikRef) => {
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
        innerRef={formikRef}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {formikProps => (
          <>
            <Container>
              <Section>
                <SectionField label="Project name" required>
                  <Input field="projectName" />
                </SectionField>
                <SectionField label="Project number">
                  <Input field="projectNumber" placeholder="if known" />
                </SectionField>
                <SectionField label="Status" required>
                  <Select
                    field="projectStatusType"
                    options={projectStatusOptions}
                    placeholder="Select..."
                  />
                </SectionField>
                <SectionField label="MoTI region" required>
                  <Select field="region" options={projectRegionOptions} placeholder="Select..." />
                </SectionField>
                <SectionField label="Project summary">
                  <MediumTextArea field="summary" />
                </SectionField>
              </Section>
            </Container>
          </>
        )}
      </Formik>
    );
  },
);

export default UpdateProjectContainer;

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;
