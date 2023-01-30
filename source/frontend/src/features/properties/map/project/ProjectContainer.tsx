import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { FormikProps } from 'formik';
import { useProjectProvider } from 'hooks/providers/useProjectProvider';
import { IProjectForm } from 'interfaces/IProject';
import { Api_Project } from 'models/api/Project';
import { useCallback, useContext, useEffect, useReducer, useState } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import styled from 'styled-components';
import * as Yup from 'yup';

import { SideBarContext } from '../context/sidebarContext';
import { ProjectForm } from './add/models';
import ProjectHeader from './common/ProjectHeader';
import { ProjectTabNames } from './ProjectTabs';
import ViewSelector from './ViewSelector';

export interface IProjectContainerProps {
  projectId: number;
  onClose: () => void;
}

export interface ProjectPageProps {
  isEditing: boolean;
  onEdit?: (isEditing: boolean) => void;
  formikRef: React.RefObject<FormikProps<ProjectForm | IProjectForm>>;
}

export interface IProjectPage {
  component: React.FunctionComponent<React.PropsWithChildren<ProjectPageProps>>;
  title: string;
  description?: string;
  validation?: Yup.ObjectSchema<any>;
  claims?: string[] | string;
  editable?: boolean;
}

export enum ProjectPageNames {
  DETAILS = 'details',
}

export const projectPages: Map<ProjectPageNames, IProjectPage> = new Map<
  ProjectPageNames,
  IProjectPage
>([
  // [
  //   ProjectPageNames.DETAILS,
  //   {
  //     component: UpdateProjectContainer,
  //     title: 'Project details',
  //     validation: AddProjectYupSchema,
  //   },
  // ],
]);

// Interface for our internal state
export interface ProjectContainerState {
  isEditing: boolean;
  activeEditForm?: ProjectPageNames;
  activeTab?: ProjectTabNames;
  showConfirmModal: boolean;
}

const initialState: ProjectContainerState = {
  isEditing: false,
  activeEditForm: undefined,
  activeTab: undefined,
  showConfirmModal: false,
};

const ProjectContainer: React.FunctionComponent<
  React.PropsWithChildren<IProjectContainerProps>
> = props => {
  const { projectId, onClose } = props;
  const { setProject, setProjectLoading } = useContext(SideBarContext);

  const {
    getProject: { execute: getProject, loading: loadingProject },
  } = useProjectProvider();

  const [containerState, setContainerState] = useReducer(
    (prevState: ProjectContainerState, newState: Partial<ProjectContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    initialState,
  );

  // Retrieve acquisition file from API and save it to local state and side-bar context
  const fetchProject = useCallback(async () => {
    var retrieved = await getProject(projectId);
    setProjectInstance(retrieved);
  }, [projectId, getProject]);

  const [project, setProjectInstance] = useState<Api_Project | undefined>(undefined);

  useEffect(() => {
    if (project === undefined) {
      fetchProject();
    }
  }, [project, fetchProject]);

  useEffect(() => setProjectLoading(loadingProject), [loadingProject, setProjectLoading]);
  const close = useCallback(() => onClose && onClose(), [onClose]);

  if (loadingProject) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <MapSideBarLayout
      showCloseButton
      onClose={close}
      title={containerState.isEditing ? 'Update Project' : 'Project'}
      icon={<FaBriefcase className="mr-2 mb-2" size={32} />}
      header={<ProjectHeader project={project} />}
    >
      <StyledFormWrapper>
        <ViewSelector
          project={project}
          setProject={setProject}
          isEditing={containerState.isEditing}
          activeTab={containerState.activeTab}
          setContainerState={setContainerState}
        />
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default ProjectContainer;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
