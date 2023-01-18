import { FormikHelpers } from 'formik';
import { Api_Project } from 'models/api/Project';
import { useCallback } from 'react';

import { ProjectForm } from '../add/models';
import { useProjectProvider } from './useProjectProvider';

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
    // validationSchema: AddAcquisitionFileYupSchema,
  };
}
