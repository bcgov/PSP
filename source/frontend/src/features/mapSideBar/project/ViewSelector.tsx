import { FormikProps } from 'formik';
import React from 'react';

import { Api_Project } from '@/models/api/Project';

import AddProjectForm from './add/AddProjectForm';
import { ProjectContainerState, ProjectPageNames } from './ProjectContainer';
import UpdateProjectContainer from './tabs/projectDetails/update/UpdateProjectContainer';
import { ProjectTabNames } from './tabs/ProjectTabs';
import ProjectTabsContainer from './tabs/ProjectTabsContainer';

export interface IViewSelectorProps {
  project?: Api_Project;
  isEditing: boolean;
  setContainerState: (value: Partial<ProjectContainerState>) => void;
  setProject: (project: Api_Project) => void;
  activeTab?: ProjectTabNames;
  activeEditForm?: ProjectPageNames;
  onSuccess: () => void;
}

export const ViewSelector = React.forwardRef<FormikProps<any>, IViewSelectorProps>(
  (props, formikRef) => {
    if (props.isEditing && !!props.project) {
      return (
        <UpdateProjectContainer
          ref={formikRef}
          project={props.project}
          View={AddProjectForm}
          onSuccess={props.onSuccess}
        />
      );
    }

    // render read-only views
    return (
      <ProjectTabsContainer
        project={props.project}
        setProject={props.setProject}
        setContainerState={props.setContainerState}
        activeTab={props.activeTab}
      />
    );
  },
);

export default ViewSelector;
