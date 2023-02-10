import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { FormikHelpers, FormikProps } from 'formik';
import { useProjectProvider } from 'hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_Project } from 'models/api/Project';
import React, { ForwardedRef } from 'react';
import { mapLookupCode } from 'utils/mapLookupCode';

import { ProjectForm } from '../models';

export interface IUpdateProjectContainerProps {
  project: Api_Project;
  View: React.FC<IUpdateProjectContainerViewProps>;
  onSuccess: () => void;
}

export interface IUpdateProjectContainerViewProps {
  initialValues: ProjectForm;
  projectStatusOptions: SelectOption[];
  projectRegionOptions: SelectOption[];
  formikRef: ForwardedRef<FormikProps<ProjectForm>>;
  onSubmit: (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => void | Promise<any>;
}

const UpdateProjectContainer = React.forwardRef<
  FormikProps<ProjectForm>,
  IUpdateProjectContainerProps
>((props, formikRef) => {
  const { project, View, onSuccess } = props;

  const {
    updateProject: { execute: updateProject },
  } = useProjectProvider();

  const { getOptionsByType, getByType } = useLookupCodeHelpers();

  const intialValues = ProjectForm.fromApi(project);
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);
  const regionTypeCodes = getByType(API.REGION_TYPES).map(c => mapLookupCode(c));

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
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <View
      formikRef={formikRef}
      projectRegionOptions={regionTypeCodes}
      projectStatusOptions={projectStatusTypeCodes}
      initialValues={intialValues}
      onSubmit={handleSubmit}
    />
  );
});

export default UpdateProjectContainer;
