import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { FormikProps } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { useCallback, useRef } from 'react';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import SidebarFooter from '../../shared/SidebarFooter';
import { useAddAcquisitionFormManagement } from '../hooks/useAddAcquisitionFormManagement';
import { AddAcquisitionForm } from './AddAcquisitionForm';
import { AcquisitionForm } from './models';

export interface IAddAcquisitionContainerProps {
  onClose?: () => void;
}

export const AddAcquisitionContainer: React.FC<IAddAcquisitionContainerProps> = props => {
  const { onClose } = props;
  const history = useHistory();
  const formikRef = useRef<FormikProps<AcquisitionForm>>(null);

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const handleSave = () => {
    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  // navigate to read-only view after file has been created
  const onSuccess = (acqFile: Api_AcquisitionFile) => {
    formikRef.current?.resetForm();
    history.replace(`/mapview/sidebar/acquisition/${acqFile.id}`);
  };

  const helper = useAddAcquisitionFormManagement({ onSuccess });

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Acquisition File"
      icon={
        <RealEstateAgent
          title="Acquisition file Icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
          className="mr-2"
        />
      }
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
        <AddAcquisitionForm
          ref={formikRef}
          initialValues={helper.initialValues}
          onSubmit={helper.handleSubmit}
          validationSchema={helper.validationSchema}
        />
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default AddAcquisitionContainer;

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
