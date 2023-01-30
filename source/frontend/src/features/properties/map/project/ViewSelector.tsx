import { Api_Project } from 'models/api/Project';

import ProjectTabsContainer from './detail/ProjectTabsContainer';
import { ProjectContainerState, ProjectPageNames } from './ProjectContainer';
import { ProjectTabNames } from './ProjectTabs';

export interface IViewSelectorProps {
  project?: Api_Project;
  isEditing: boolean;
  setContainerState: (value: Partial<ProjectContainerState>) => void;
  setProject: (project: Api_Project) => void;
  activeTab?: ProjectTabNames;
  activeEditForm?: ProjectPageNames;
}

export const ViewSelector: React.FunctionComponent<IViewSelectorProps> = (props, formikRef) => {
  // if (props.isEditing && !!props.project) {
  //   return (
  //     <UpdateProjectContainer ref={formikRef}></UpdateProjectContainer>;
  //   )

  // }
  // render read-only views
  return (
    <ProjectTabsContainer
      project={props.project}
      setProject={props.setProject}
      setContainerState={props.setContainerState}
      activeTab={props.activeTab}
      isEditing={props.isEditing}
    />
  );
};

export default ViewSelector;
