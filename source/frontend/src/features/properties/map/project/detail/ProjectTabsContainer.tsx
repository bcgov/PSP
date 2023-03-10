import Claims from 'constants/claims';
import { NoteTypes } from 'constants/noteTypes';
import NoteListView from 'features/notes/list/NoteListView';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Project } from 'models/api/Project';

import { ProjectContainerState, ProjectPageNames } from '../ProjectContainer';
import { ProjectTabNames, ProjectTabs, ProjectTabView } from '../ProjectTabs';
import ProjectSummaryView from './ProjectSummaryView';

export interface IProjectTabsContainerProps {
  project?: Api_Project;
  setProject: (project: Api_Project) => void;
  setContainerState: (value: Partial<ProjectContainerState>) => void;
  isEditing: boolean;
  onEdit?: () => {};
  activeTab?: ProjectTabNames;
}

export interface IProjectTabsProps {
  defaultTabKey: ProjectTabNames;
  tabViews: ProjectTabView[];
  activeTab: ProjectTabNames;
  setActiveTab: (tab: ProjectTabNames) => void;
}

const ProjectTabsContainer: React.FC<IProjectTabsContainerProps> = ({
  project,
  setContainerState,
  isEditing,
  activeTab,
}) => {
  const tabViews: ProjectTabView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  if (project?.id && hasClaim(Claims.PROJECT_VIEW)) {
    tabViews.push({
      content: (
        <ProjectSummaryView
          project={project}
          onEdit={() =>
            setContainerState({
              isEditing: true,
              activeEditForm: ProjectPageNames.DETAILS,
            })
          }
        />
      ),
      key: ProjectTabNames.projectDetails,
      name: 'Project details',
    });
  }

  if (project?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: <NoteListView type={NoteTypes.Project} entityId={project?.id} />,
      key: ProjectTabNames.notes,
      name: 'Notes',
    });
  }

  var defaultTab = ProjectTabNames.projectDetails;

  return (
    <ProjectTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab ?? defaultTab}
      setActiveTab={(tab: ProjectTabNames) => setContainerState({ activeTab: tab })}
    />
  );
};

export default ProjectTabsContainer;
