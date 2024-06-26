import { FormikProps } from 'formik';
import { useCallback, useRef } from 'react';
import { FaBriefcase } from 'react-icons/fa';

import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';

import SidebarFooter from '../shared/SidebarFooter';
import { StyledFormWrapper } from '../shared/styles';
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
  displayRequiredFieldsError,
  onClose,
  onSetProject,
  onSetContainerState,
  onSuccess,
  setIsValid,
}) => {
  const close = useCallback(() => onClose && onClose(), [onClose]);

  const handleSaveClick = () => {
    if (formikRef !== undefined) {
      setIsValid(formikRef.current?.isValid ?? false);
      onSetContainerState({ isSubmitting: true });
      formikRef.current?.setSubmitting(true);
      return formikRef.current?.submitForm() ?? Promise.resolve();
    }
    return Promise.resolve();
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
      icon={<FaBriefcase size={26} />}
      header={<ProjectHeader project={project} />}
      footer={
        isEditing && (
          <SidebarFooter
            isOkDisabled={false}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
            displayRequiredFieldError={displayRequiredFieldsError}
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
          variant="info"
          display={showConfirmModal}
          title={'Confirm Changes'}
          message={
            <>
              <p>If you choose to cancel now, your changes will not be saved.</p>
              <p>Do you want to proceed?</p>
            </>
          }
          handleOk={handleCancelConfirm}
          handleCancel={() => onSetContainerState({ showConfirmModal: false })}
          okButtonText="Yes"
          cancelButtonText="No"
          show
        />
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default ProjectContainerView;
