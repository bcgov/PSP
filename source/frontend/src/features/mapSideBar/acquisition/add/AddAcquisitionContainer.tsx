import { FormikProps } from 'formik/dist/types';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import RealEstateAgent from '@/assets/images/real-estate-agent.svg?react';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists, isValidId, isValidString } from '@/utils';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { useAddAcquisitionFormManagement } from '../hooks/useAddAcquisitionFormManagement';
import { AddAcquisitionForm } from './AddAcquisitionForm';
import { AcquisitionForm } from './models';

export interface IAddAcquisitionContainerProps {
  onClose: () => void;
}

export const AddAcquisitionContainer: React.FC<IAddAcquisitionContainerProps> = props => {
  const { onClose } = props;
  const history = useHistory();
  const formikRef = useRef<FormikProps<AcquisitionForm>>(null);
  const [isValid, setIsValid] = useState<boolean>(true);
  const { setModalContent, setDisplayModal } = useModalContext();

  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;

  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const {
    getPropertyByPidWrapper: { execute: getPropertyByPid },
    getPropertyByPinWrapper: { execute: getPropertyByPin },
  } = usePimsPropertyRepository();
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  // Warn user that property is part of an existing acquisition file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm) => {
      let apiId;
      try {
        if (isValidId(propertyForm.apiId)) {
          apiId = propertyForm.apiId;
        } else if (isValidString(propertyForm.pid)) {
          const result = await getPropertyByPid(propertyForm.pid);
          apiId = result?.id;
        } else if (isValidString(propertyForm.pin)) {
          const result = await getPropertyByPin(Number(propertyForm.pin));
          apiId = result?.id;
        }
      } catch (e) {
        apiId = 0;
      }

      if (isValidId(apiId)) {
        const response = await getPropertyAssociations(apiId);
        const acquisitionAssociations = response?.acquisitionAssociations ?? [];
        const otherAcqFiles = acquisitionAssociations.filter(a => exists(a.id));
        return otherAcqFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations, getPropertyByPid, getPropertyByPin],
  );

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
  const onSuccess = async (acqFile: ApiGen_Concepts_AcquisitionFile) => {
    if (acqFile.fileProperties?.find(ap => !ap.property?.address && !ap.property?.id)) {
      toast.warn(
        'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
        { autoClose: 15000 },
      );
    }

    mapMachine.refreshMapProperties();
    history.replace(`/mapview/sidebar/acquisition/${acqFile.id}`);
  };

  const helper = useAddAcquisitionFormManagement({
    onSuccess,
    initialForm,
    selectedFeature: selectedFeatureDataset,
    formikRef,
  });

  const cancelFunc = () => {
    if (!formikRef.current?.dirty) {
      formikRef.current?.resetForm();
      onClose();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          formikRef.current?.resetForm();
          setDisplayModal(false);
          onClose();
        },
      });
      setDisplayModal(true);
    }
  };

  const { initialValues } = helper;
  // Require user confirmation before adding a property to file
  // This is the flow for Map Marker -> right-click -> create Acquisition File
  useEffect(() => {
    const runAsync = async () => {
      if (exists(initialValues) && exists(formikRef.current) && needsUserConfirmation) {
        if (initialValues.properties.length > 0) {
          const formProperty = initialValues.properties[0];
          if (await confirmBeforeAdd(formProperty)) {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: (
                <>
                  <p>This property has already been added to one or more acquisition files.</p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: () => {
                // allow the property to be added to the file being created
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', initialValues.properties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
              handleCancel: () => {
                // clear out the properties array as the user did not agree to the popup
                initialValues.properties.splice(0, initialValues.properties.length);
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', initialValues.properties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
            });
            setDisplayModal(true);
          }
        }
      }
    };

    runAsync();
  }, [
    confirmBeforeAdd,
    initialValues,
    needsUserConfirmation,
    selectedFeatureDataset,
    setDisplayModal,
    setModalContent,
  ]);

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
      onClose={cancelFunc}
      footer={
        <SidebarFooter
          isOkDisabled={helper.loading}
          onSave={handleSave}
          onCancel={cancelFunc}
          displayRequiredFieldError={isValid === false}
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
          confirmBeforeAdd={confirmBeforeAdd}
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
