import { Input, Select, TextArea } from 'components/common/form';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import { useProjectProvider } from 'hooks/providers/useProjectProvider';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_Project } from 'models/api/Project';
import React from 'react';
import { Container } from 'react-bootstrap';
import styled from 'styled-components';
import { mapLookupCode } from 'utils/mapLookupCode';

import { AddProjectYupSchema } from '../add/AddProjectFileYupSchema';
import { ProjectForm } from '../add/models';

export interface IUpdateProjectContainerProps {
  project: Api_Project;
  onSuccess: () => void;
}

const UpdateProjectContainer = React.forwardRef<
  FormikProps<ProjectForm>,
  IUpdateProjectContainerProps
>((props, formikRef) => {
  const { project, onSuccess } = props;
  const {
    updateProject: { execute: updateProject },
  } = useProjectProvider();

  const { getOptionsByType, getByType } = useLookupCodeHelpers();

  const intialValues = ProjectForm.fromApi(project);
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);
  const regionTypeCodes = getByType(API.REGION_TYPES).map(c => mapLookupCode(c));

  const handleSubmit = async (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
    try {
      const updatedProject = values.toApi();
      const response = await updateProject(updatedProject);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <Formik<ProjectForm>
      enableReinitialize
      innerRef={formikRef}
      initialValues={intialValues}
      validationSchema={AddProjectYupSchema}
      onSubmit={handleSubmit}
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
                  options={projectStatusTypeCodes}
                  placeholder="Select..."
                />
              </SectionField>
              <SectionField label="MoTI region" required={true}>
                <Select field="region" options={regionTypeCodes} placeholder="Select..." />
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

export default UpdateProjectContainer;

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;
