import { FormikProps } from 'formik';
import { useCallback, useRef } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import styled from 'styled-components';

import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';

import SidebarFooter from '../shared/SidebarFooter';
import ProjectHeader from './common/ProjectHeader';
import { IProjectContainerViewProps } from './ProjectContainer';
import ViewSelector from './ViewSelector';

const ProjectContainerView: React.FC<IProjectContainerViewProps> = ({
  project,
  viewTitle,
  loadingProject,
  activeTab,
  isEditing,
  showConfirmModal,
  isSubmitting,
  onClose,
  onSetProject,
  onSetContainerState,
  onSuccess,
}) => {
  const close = useCallback(() => onClose && onClose(), [onClose]);

  const handleSaveClick = () => {
    if (formikRef !== undefined) {
      onSetContainerState({ isSubmitting: true });
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const formikRef = useRef<FormikProps<any>>(null);

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    onSetContainerState({
      showConfirmModal: false,
      isEditing: false,
      activeEditForm: undefined,
    });
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        onSetContainerState({ showConfirmModal: true });
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

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
      footer={
        isEditing && (
          <SidebarFooter
            isOkDisabled={false}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
          />
        )
      }
    >
      <StyledFormWrapper>
        <ViewSelector
          ref={formikRef}
          project={project}
          setProject={onSetProject}
          isEditing={isEditing}
          activeTab={activeTab}
          setContainerState={onSetContainerState}
          onSuccess={onSuccess}
        />
        <GenericModal
          display={showConfirmModal}
          title={'Confirm changes'}
          message={
            <>
              <div>If you cancel now, this project will not be saved.</div>
              <br />
              <strong>Are you sure you want to Cancel?</strong>
            </>
          }
          handleOk={handleCancelConfirm}
          handleCancel={() => onSetContainerState({ showConfirmModal: false })}
          okButtonText="Ok"
          cancelButtonText="Resume editing"
          show
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
