import { FormikHelpers } from 'formik';
import { useProjectProvider } from 'hooks/providers/useProjectProvider';
import { Api_Project } from 'models/api/Project';
import { useCallback } from 'react';

import { AddProjectYupSchema } from '../add/AddProjectFileYupSchema';
import { ProjectForm } from '../add/models';

export interface IUseAddProjectFormManagementProps {
  /** Optional - callback to execute after acquisition file has been added to the datastore */
  onSuccess?: (project: Api_Project) => void;
  initialForm?: ProjectForm;
}

export function useAddProjectFormManagement(props: IUseAddProjectFormManagementProps) {
  const { onSuccess } = props;
  const { addProject } = useProjectProvider();

  // save handler
  const handleSubmit = useCallback(
    async (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
      const project = values.toApi();
      const response = await addProject.execute(project);
      formikHelpers?.setSubmitting(false);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess(response);
        }
      }
    },
    [addProject, onSuccess],
  );

  return {
    handleSubmit,
    initialValues: props.initialForm ?? new ProjectForm(),
    validationSchema: AddProjectYupSchema,
  };
}
