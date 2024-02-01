import { FormikHelpers } from 'formik';
import { useCallback } from 'react';

import * as API from '@/constants/API';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId } from '@/utils';

import { AddProjectYupSchema } from '../add/AddProjectFileYupSchema';
import { ProjectForm } from '../models';

export interface IUseAddProjectFormProps {
  onSuccess?: (project: ApiGen_Concepts_Project) => void;
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
    async (
      values: ProjectForm,
      formikHelpers: FormikHelpers<ProjectForm>,
      userOverrideCodes: UserOverrideCode[],
    ) => {
      const project = values.toApi();
      const response = await addProject.execute(project, userOverrideCodes);

      if (exists(response) && isValidId(response?.id)) {
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
