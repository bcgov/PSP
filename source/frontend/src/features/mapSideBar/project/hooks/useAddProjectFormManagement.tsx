import { FormikHelpers } from 'formik';
import { useCallback } from 'react';

import * as API from '@/constants/API';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { Api_Project } from '@/models/api/Project';

import { AddProjectYupSchema } from '../add/AddProjectFileYupSchema';
import { ProjectForm } from '../models';

export interface IUseAddProjectFormProps {
  onSuccess?: (project: Api_Project) => void;
  initialForm?: ProjectForm;
}

export function useAddProjectForm(props: IUseAddProjectFormProps) {
  const { onSuccess } = props;
  const { addProject } = useProjectProvider();
  const { getPublicByType } = useLookupCodeHelpers();
  const initialValues = props.initialForm ?? new ProjectForm();
  if (!initialValues.projectStatusType) {
    initialValues.projectStatusType =
      getPublicByType(API.PROJECT_STATUS_TYPES)
        .find(s => s.id === 'AC')
        ?.id?.toString() ?? '';
  }

  // save handler
  const handleSubmit = useCallback(
    async (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
      const project = values.toApi();
      const response = await addProject.execute(project);

      if (!!response?.id) {
        formikHelpers.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess(response);
        }
      }
    },
    [addProject, onSuccess],
  );

  return {
    handleSubmit,
    initialValues: initialValues,
    validationSchema: AddProjectYupSchema,
  };
}
