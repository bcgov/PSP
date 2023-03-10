import axios from 'axios';
import * as API from 'constants/API';
import { FormikHelpers, FormikProps } from 'formik';
import { useProjectProvider } from 'hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_Project } from 'models/api/Project';
import React from 'react';
import { toast } from 'react-toastify';

import { AddProjectYupSchema } from '../add/AddProjectFileYupSchema';
import { IAddProjectFormProps } from '../add/AddProjectForm';
import { ProjectForm } from '../models';

export interface IUpdateProjectContainerProps {
  project: Api_Project;
  View: React.ForwardRefExoticComponent<
    IAddProjectFormProps & React.RefAttributes<FormikProps<ProjectForm>>
  >;
  onSuccess: () => void;
}

const UpdateProjectContainer = React.forwardRef<
  FormikProps<ProjectForm>,
  IUpdateProjectContainerProps
>((props, formikRef) => {
  const { project, View, onSuccess } = props;

  const {
    updateProject: { execute: updateProject },
  } = useProjectProvider();

  const { getOptionsByType } = useLookupCodeHelpers();

  const initialValues = ProjectForm.fromApi(project);
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);

  const handleSubmit = async (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
    try {
      formikHelpers?.setSubmitting(true);
      const updatedProject = values.toApi();
      const response = await updateProject(updatedProject);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    } catch (e) {
      if (axios.isAxiosError(e) && e.response?.status === 409) {
        toast.error(e.response.data as any);
      } else {
        toast.error('Failed to update project.');
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <View
      ref={formikRef}
      validationSchema={AddProjectYupSchema}
      projectStatusOptions={projectStatusTypeCodes}
      initialValues={initialValues}
      onSubmit={handleSubmit}
    />
  );
});

export default UpdateProjectContainer;
