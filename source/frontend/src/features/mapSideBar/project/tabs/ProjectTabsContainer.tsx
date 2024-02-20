import Claims from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';

import { ProjectContainerState, ProjectPageNames } from '../ProjectContainer';
import ProjectSummaryView from './projectDetails/detail/ProjectSummaryView';
import { ProjectTabNames, ProjectTabs, ProjectTabView } from './ProjectTabs';

export interface IProjectTabsContainerProps {
  project?: ApiGen_Concepts_Project;
  setProject: (project: ApiGen_Concepts_Project) => void;
  setContainerState: (value: Partial<ProjectContainerState>) => void;
  onEdit?: () => object;
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

  if (project?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentListContainer
          parentId={project?.id.toString()}
          relationshipType={ApiGen_CodeTypes_DocumentRelationType.Projects}
        />
      ),
      key: ProjectTabNames.documents,
      name: 'Documents',
    });
  }

  if (project?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: <NoteListView type={NoteTypes.Project} entityId={project?.id} />,
      key: ProjectTabNames.notes,
      name: 'Notes',
    });
  }

  const defaultTab = ProjectTabNames.projectDetails;

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
