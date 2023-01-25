import { Api_Project } from 'models/api/Project';

import ProjectTabsContainer from './detail/ProjectTabsContainer';
import { ProjectContainerState, ProjectPageNames, projectPages } from './ProjectContainer';
import { ProjectTabNames } from './ProjectTabs';

export interface IViewSelectorProps {
  project?: Api_Project;
  isEditing: boolean;
  setContainerState: (value: Partial<ProjectContainerState>) => void;
  setProject: (project: Api_Project) => void;
  activeTab?: ProjectTabNames;
  activeEditForm?: ProjectPageNames;
}

export const ViewSelector: React.FunctionComponent<IViewSelectorProps> = props => {
  if (props.isEditing && !!props.project && props.activeEditForm) {
    const activeProjectPage = projectPages.get(props.activeEditForm);
    if (!activeProjectPage) {
      throw Error('Project page not found');
    }
    const Component = activeProjectPage.component;

    return (
      <Component
        isEditing={props.isEditing}
        onEdit={(isEditing: boolean) => props.setContainerState({ isEditing: isEditing })}
      />
    );
  } else {
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
  }
};

export default ViewSelector;
