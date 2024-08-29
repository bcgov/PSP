import { FormikProps } from 'formik/dist/types';
import { Location } from 'history';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import AcquisitionFileIcon from '@/assets/images/acquisition-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists, isValidId } from '@/utils';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { StyledFormWrapper } from '../../shared/styles';
import { useAddAcquisitionFormManagement } from '../hooks/useAddAcquisitionFormManagement';
import { AddAcquisitionForm } from './AddAcquisitionForm';
import { AcquisitionForm } from './models';

export interface IAddAcquisitionContainerProps {
  onClose: () => void;
  onSuccess: (newAcquisitionId: number) => void;
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
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  // Warn user that property is part of an existing acquisition file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm) => {
      if (isValidId(propertyForm.apiId)) {
        const response = await getPropertyAssociations(propertyForm.apiId);
        const acquisitionAssociations = response?.acquisitionAssociations ?? [];
        const otherAcqFiles = acquisitionAssociations.filter(a => exists(a.id));
        return otherAcqFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations],
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
    props.onSuccess(acqFile.id);
  };

  const helper = useAddAcquisitionFormManagement({
    onSuccess,
    initialForm,
    selectedFeature: selectedFeatureDataset,
    formikRef,
  });

  const handleCancel = () => {
    onClose();
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

  const checkState = useCallback(
    (location: Location) => {
      return (
        !location.pathname.startsWith('/mapview/sidebar/acquisition/') &&
        formikRef?.current?.dirty &&
        !formikRef?.current?.isSubmitting
      );
    },
    [formikRef],
  );

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Acquisition File"
      icon={
        <AcquisitionFileIcon
          title="Acquisition file Icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
        />
      }
      onClose={handleCancel}
      footer={
        <SidebarFooter
          isOkDisabled={helper.loading}
          onSave={handleSave}
          onCancel={handleCancel}
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
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

export default AddAcquisitionContainer;
