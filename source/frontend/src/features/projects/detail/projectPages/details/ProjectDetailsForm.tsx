import { ProjectStateContext } from 'features/projects/context/ProjectContext';
import { Formik } from 'formik';
import { defaultProjectForm } from 'interfaces/IProject';
import { noop } from 'lodash';
import React from 'react';

import { ProjectDetailInformation } from './ProjectDetailInformation';

export interface IProjectDetailsFormProps {
  isEditing?: boolean;
}

const ProjectDetailsForm: React.FunctionComponent<
  React.PropsWithChildren<IProjectDetailsFormProps>
> = () => {
  const { project } = React.useContext(ProjectStateContext);

  console.log(project);
  return (
    <Formik
      initialValues={{ ...defaultProjectForm, ...project }}
      enableRenitialize={true}
      onSubmit={noop}
    >
      <ProjectDetailInformation />
    </Formik>
  );
};

export default ProjectDetailsForm;
