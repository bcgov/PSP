import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { useCallback } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import styled from 'styled-components';

import ProjectHeader from './common/ProjectHeader';
import { IProjectContainerViewProps } from './ProjectContainer';
import ViewSelector from './ViewSelector';

const ProjectContainerView: React.FC<IProjectContainerViewProps> = ({
  project,
  viewTitle,
  loadingProject,
  activeTab,
  isEditing,
  onClose,
  onSetProject,
  onSetContainerState,
}) => {
  const close = useCallback(() => onClose && onClose(), [onClose]);

  if (loadingProject) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <MapSideBarLayout
      showCloseButton
      onClose={close}
      title={viewTitle}
      icon={<FaBriefcase className="mr-2 mb-2" size={32} />}
      header={<ProjectHeader project={project} />}
    >
      <StyledFormWrapper>
        <ViewSelector
          project={project}
          setProject={onSetProject}
          isEditing={isEditing}
          activeTab={activeTab}
          setContainerState={onSetContainerState}
        />
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default ProjectContainerView;

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
