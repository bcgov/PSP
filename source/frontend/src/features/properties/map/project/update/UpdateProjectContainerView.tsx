import { Input, Select, TextArea } from 'components/common/form';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikProps } from 'formik';
import React from 'react';
import { Container } from 'react-bootstrap';
import styled from 'styled-components';

import { AddProjectYupSchema } from '../add/AddProjectFileYupSchema';
import { ProjectForm } from '../models';
import { IUpdateProjectContainerViewProps } from './UpdateProjectContainer';

const UpdateProjectContainerView = React.forwardRef<
  FormikProps<ProjectForm>,
  IUpdateProjectContainerViewProps
>((props, formikRef) => {
  const { initialValues, projectRegionOptions, projectStatusOptions, onSubmit } = props;

  return (
    <Formik<ProjectForm>
      enableReinitialize
      innerRef={props.formikRef}
      initialValues={initialValues}
      validationSchema={AddProjectYupSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <>
          <Container>
            <Section>
              <SectionField label="Project name" required={true}>
                <Input field="projectName" />
              </SectionField>
              <SectionField label="Project number">
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
                <MediumTextArea field="summary" />
              </SectionField>
            </Section>
          </Container>
        </>
      )}
    </Formik>
  );
});

export default UpdateProjectContainerView;

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;
