import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import * as API from 'constants/API';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_Project } from 'models/api/Project';
import { useCallback, useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaBriefcase } from 'react-icons/fa';
import { Link, useHistory } from 'react-router-dom';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';

import SidebarFooter from '../../shared/SidebarFooter';
import { useAddProjectForm } from '../hooks/useAddProjectFormManagement';
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

  const onSuccess = async (proj: Api_Project) => {
    formikRef.current?.resetForm();
    await search();
    history.replace(`/mapview/sidebar/project/${proj.id}`);
  };

  const helper = useAddProjectForm({ onSuccess });

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
        <StyledRow>
          <Col>
            <p>
              Before creating a project, <Link to={'/project/list'}>do a search</Link> to ensure the
              the project you're creating doesn't already exist.
            </p>
          </Col>
        </StyledRow>
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
  height: auto;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;

const StyledRow = styled(Row)`
  margin-top: 1.5rem;
  margin-bottom: 2rem;
  margin-left: 0;
  margin-right: 0;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
  text-align: left;
`;
