import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import * as API from 'constants/API';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_Project } from 'models/api/Project';
import { useCallback, useRef } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';

import SidebarFooter from '../../shared/SidebarFooter';
import { useAddProjectFormManagement } from '../hooks/useAddProjectFormManagement';
import AddProjectForm from './AddProjectForm';
import { ProjectForm } from './models';

export interface IAddProjectContainerProps {
  onClose?: () => void;
}

const AddProjectContainer: React.FC<React.PropsWithChildren<IAddProjectContainerProps>> = props => {
  const { onClose } = props;
  const history = useHistory();
  const { search } = useMapSearch();

  const { getOptionsByType, getByType } = useLookupCodeHelpers();
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);
  const regionTypeCodes = getByType(API.REGION_TYPES).map(c => mapLookupCode(c));

  const formikRef = useRef<FormikProps<ProjectForm>>(null);

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const handleSave = () => {
    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  // navigate to read-only view after file has been created
  const onSuccess = async (proj: Api_Project) => {
    formikRef.current?.resetForm();
    await search();
    history.replace(`/mapview/sidebar/project/${proj.id}`);
  };

  const helper = useAddProjectFormManagement({ onSuccess });

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Project"
      icon={<FaBriefcase className="mr-2 mb-2" size={32} />}
      onClose={close}
      footer={
        <SidebarFooter
          isOkDisabled={formikRef.current?.isSubmitting}
          onSave={handleSave}
          onCancel={close}
        />
      }
    >
      <StyledFormWrapper>
        <p>my form here</p>
        <AddProjectForm
          ref={formikRef}
          initialValues={helper.initialValues}
          projectStatusOptions={projectStatusTypeCodes}
          projectRegionOptions={regionTypeCodes}
          onSubmit={helper.handleSubmit}
          validationSchema={helper.validationSchema}
        />
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default AddProjectContainer;

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
