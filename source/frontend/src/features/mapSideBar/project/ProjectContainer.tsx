import { FormikProps } from 'formik';
import { useCallback, useContext, useEffect, useReducer, useState } from 'react';
import * as Yup from 'yup';

import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';

import { SideBarContext } from '../context/sidebarContext';
import { ProjectForm } from './models';
import { ProjectTabNames } from './tabs/ProjectTabs';

export interface IProjectContainerViewProps {
  project?: ApiGen_Concepts_Project;
  viewTitle?: string;
  loadingProject?: boolean;
  activeTab?: ProjectTabNames;
  isEditing: boolean;
  showConfirmModal: boolean;
  isSubmitting: boolean;
  onSetContainerState: (value: Partial<ProjectContainerState>) => void;
  onClose: () => void;
  onSetProject: (project: ApiGen_Concepts_Project) => void;
  onSuccess: () => void;
  setIsValid: (value: boolean) => void;
  displayRequiredFieldsError: boolean;
}

export interface IProjectContainerProps {
  projectId: number;
  onClose: () => void;
  View: React.FC<IProjectContainerViewProps>;
}

export interface ProjectPageProps {
  isEditing: boolean;
  onEdit?: (isEditing: boolean) => void;
  formikRef: React.RefObject<FormikProps<ProjectForm>>;
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

// Interface for our internal state
export interface ProjectContainerState {
  isEditing: boolean;
  activeEditForm?: ProjectPageNames;
  activeTab?: ProjectTabNames;
  showConfirmModal: boolean;
  isSubmitting: boolean;
}

const initialState: ProjectContainerState = {
  isEditing: false,
  activeEditForm: undefined,
  activeTab: undefined,
  showConfirmModal: false,
  isSubmitting: false,
};

const ProjectContainer: React.FunctionComponent<
  React.PropsWithChildren<IProjectContainerProps>
> = ({ projectId, View, onClose }) => {
  const { setProject, setProjectLoading } = useContext(SideBarContext);

  const {
    getProject: { execute: getProject, loading: loadingProject },
  } = useProjectProvider();

  const [isValid, setIsValid] = useState<boolean>(false);

  const [containerState, setContainerState] = useReducer(
    (prevState: ProjectContainerState, newState: Partial<ProjectContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    initialState,
  );

  // Retrieve acquisition file from API and save it to local state and side-bar context
  const fetchProject = useCallback(async () => {
    const retrieved = await getProject(projectId);
    setProjectInstance(retrieved);
  }, [projectId, getProject]);

  const [project, setProjectInstance] = useState<ApiGen_Concepts_Project | undefined>(undefined);

  useEffect(() => {
    if (!project) {
      fetchProject();
    }
  }, [project, fetchProject]);

  useEffect(() => setProjectLoading(loadingProject), [loadingProject, setProjectLoading]);

  const onSuccess = async () => {
    await fetchProject();
    setContainerState({ activeEditForm: undefined, isEditing: false, isSubmitting: false });
  };

  const title = containerState.isEditing ? 'Update Project' : 'Project';
  return (
    <View
      project={project}
      viewTitle={title}
      activeTab={containerState.activeTab}
      loadingProject={loadingProject}
      isEditing={containerState.isEditing}
      showConfirmModal={containerState.showConfirmModal}
      isSubmitting={containerState.isSubmitting}
      onSetContainerState={setContainerState}
      onSetProject={setProject}
      onClose={onClose}
      onSuccess={onSuccess}
      setIsValid={setIsValid}
      displayRequiredFieldsError={containerState.isSubmitting && !isValid}
    />
  );
};

export default ProjectContainer;
