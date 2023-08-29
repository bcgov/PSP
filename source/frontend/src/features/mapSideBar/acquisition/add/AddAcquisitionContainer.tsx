import { FormikProps } from 'formik';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { ReactComponent as RealEstateAgent } from '@/assets/images/real-estate-agent.svg';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { PropertyForm } from '../../shared/models';
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
  const [isValid, setIsValid] = useState<boolean>(true);

  const close = useCallback(() => onClose && onClose(), [onClose]);
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;

  const initialForm = useMemo(() => {
    const acquisitionForm = new AcquisitionForm();
    if (selectedFeatureDataset !== null) {
      const property = PropertyForm.fromMapProperty(
        featuresetToMapProperty(selectedFeatureDataset),
      );
      acquisitionForm.properties = [property];
      acquisitionForm.region =
        property.regionName !== 'Cannot determine' ? property.region?.toString() : undefined;
    }
    return acquisitionForm;
  }, [selectedFeatureDataset]);

  useEffect(() => {
    if (!!selectedFeatureDataset && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('properties', [
        PropertyForm.fromMapProperty(featuresetToMapProperty(selectedFeatureDataset)),
      ]);
    }
  }, [initialForm, selectedFeatureDataset]);

  const handleSave = async () => {
    // Sets the formik field `isValid` to false at start
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  // navigate to read-only view after file has been created
  const onSuccess = async (acqFile: Api_AcquisitionFile) => {
    if (acqFile.fileProperties?.find(ap => !ap.property?.address && !ap.property?.id)) {
      toast.warn(
        'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
        { autoClose: 15000 },
      );
    }

    mapMachine.refreshMapProperties();
    history.replace(`/mapview/sidebar/acquisition/${acqFile.id}`);
    formikRef.current?.resetForm({ values: AcquisitionForm.fromApi(acqFile) });
  };

  const helper = useAddAcquisitionFormManagement({
    onSuccess,
    initialForm,
    selectedFeature: selectedFeatureDataset,
    formikRef,
  });

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
          isOkDisabled={helper.loading}
          onSave={handleSave}
          onCancel={close}
          isValid={isValid}
        />
      }
    >
      <StyledFormWrapper>
        <LoadingBackdrop show={helper.loading} parentScreen={true} />
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
