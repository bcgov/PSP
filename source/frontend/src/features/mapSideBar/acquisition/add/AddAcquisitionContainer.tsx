import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik/dist/types';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import AcquisitionFileIcon from '@/assets/images/acquisition-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { useQuery } from '@/hooks/use-query';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, firstOrNull, isValidId } from '@/utils';

import { PropertyForm } from '../../shared/models';
import SidebarFooter from '../../shared/SidebarFooter';
import { StyledFormWrapper } from '../../shared/styles';
import { AddAcquisitionFileYupSchema } from './AddAcquisitionFileYupSchema';
import { IAddAcquisitionFormProps } from './AddAcquisitionForm';
import { AcquisitionForm } from './models';

export interface IAddAcquisitionContainerProps {
  onClose: (nextLocation?: string) => void;
  onSuccess: (newAcquisitionId: number) => void;
  View: React.FC<IAddAcquisitionFormProps>;
}

export const AddAcquisitionContainer: React.FC<IAddAcquisitionContainerProps> = props => {
  const { onClose, onSuccess, View } = props;
  const history = useHistory();
  const formikRef = useRef<FormikProps<AcquisitionForm>>(null);
  const [isValid, setIsValid] = useState<boolean>(true);
  const { setModalContent, setDisplayModal } = useModalContext();

  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  const {
    getAcquisitionFile: { execute: getAcquisitionFile, response: parentAcquisitionFile },
    addAcquisitionFile: { execute: addAcquisitionFile, loading: addAcquisitionFileLoading },
  } = useAcquisitionProvider();

  // Check for parent acquisition file id for sub-files
  const params = useQuery();
  const parentId = params.get('parentId');
  const isSubFile = exists(parentId) && isValidId(Number(parentId));

  useEffect(() => {
    const fetchParentFile = async () => {
      if (exists(parentId) && isValidId(Number(parentId)) && !exists(parentAcquisitionFile)) {
        await getAcquisitionFile(Number(parentId));
      }
    };

    fetchParentFile();
  }, [getAcquisitionFile, parentAcquisitionFile, parentId]);

  const mapMachine = useMapStateMachine();
  const selectedFeatureDatasets = mapMachine.selectedFeatures ?? [];

  // Get PropertyForms with addresses for all selected features
  const { featuresWithAddresses, bcaLoading } =
    useFeatureDatasetsWithAddresses(selectedFeatureDatasets);

  const initialForm = useMemo(() => {
    const acquisitionForm = exists(parentAcquisitionFile)
      ? AcquisitionForm.fromParentFileApi(parentAcquisitionFile)
      : new AcquisitionForm();

    if (featuresWithAddresses?.length > 0 && !isSubFile) {
      acquisitionForm.properties = featuresWithAddresses.map(obj => {
        const property = PropertyForm.fromFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          property.address = obj.address;
        }
        return property;
      });

      const firstProperty = firstOrNull(acquisitionForm.properties);
      if (exists(firstProperty)) {
        acquisitionForm.region =
          firstProperty.regionName !== 'Cannot determine'
            ? firstProperty.region?.toString()
            : undefined;
      }
    }

    return acquisitionForm;
  }, [parentAcquisitionFile, featuresWithAddresses, isSubFile]);

  const handleSave = async () => {
    // Sets the formik field `isValid` to false at start
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    formikRef.current?.setSubmitting(true);
    return formikRef.current?.submitForm() ?? Promise.resolve();
  };

  const handleCancel = useCallback(() => {
    if (isSubFile) {
      // Go back to the main file (sub-files tab) if they cancel the action without saving
      onClose(`/mapview/sidebar/acquisition/${parentId}/subFiles`);
    } else {
      onClose();
    }
  }, [isSubFile, onClose, parentId]);

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

  // Require user confirmation before adding a property to file
  // This is the flow for Map Marker -> right-click -> create Acquisition File
  useEffect(() => {
    const runAsync = async () => {
      if (exists(initialForm) && exists(formikRef.current) && needsUserConfirmation) {
        if (initialForm.properties.length > 0) {
          // Check all properties for confirmation
          const needsConfirmation = await Promise.all(
            initialForm.properties.map(formProperty => confirmBeforeAdd(formProperty)),
          );
          if (needsConfirmation.some(confirm => confirm)) {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: (
                <>
                  <p>
                    One or more properties have already been added to one or more acquisition files.
                  </p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: () => {
                // allow the property to be added to the file being created
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', initialForm.properties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
              handleCancel: () => {
                // clear out the properties array as the user did not agree to the popup
                initialForm.properties.splice(0, initialForm.properties.length);
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', initialForm.properties);
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
  }, [confirmBeforeAdd, initialForm, needsUserConfirmation, setDisplayModal, setModalContent]);

  const checkState = useCallback(() => {
    return (isSubFile || formikRef?.current?.dirty) && !formikRef?.current?.isSubmitting;
  }, [formikRef, isSubFile]);

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to add Acquisition File');

  // navigate to read-only view after file has been created
  const handleSuccess = async (acqFile: ApiGen_Concepts_AcquisitionFile) => {
    if (acqFile.fileProperties?.find(ap => !ap.property?.address && !ap.property?.id)) {
      toast.warn(
        'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
        { autoClose: 15000 },
      );
    }

    mapMachine.refreshMapProperties();
    onSuccess(acqFile.id);
  };

  const handleSubmit = async (
    values: AcquisitionForm,
    formikHelpers: FormikHelpers<AcquisitionForm>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      formikHelpers?.setSubmitting(true);
      const acquisitionFile = values.toApi();
      const response = await addAcquisitionFile(acquisitionFile, userOverrideCodes);

      if (exists(response) && isValidId(response?.id)) {
        formikHelpers?.resetForm();
        handleSuccess(response);
      }
    } finally {
      mapMachine.processCreation();
      formikHelpers?.setSubmitting(false);
    }
  };

  const loading = addAcquisitionFileLoading || bcaLoading;

  return (
    <MapSideBarLayout
      showCloseButton
      title={isSubFile ? 'Create Acquisition Sub-Interest File' : 'Create Acquisition File'}
      icon={<AcquisitionFileIcon title="Acquisition file Icon" fill="currentColor" />}
      onClose={handleCancel}
      footer={
        <SidebarFooter
          isOkDisabled={loading}
          onSave={handleSave}
          onCancel={handleCancel}
          displayRequiredFieldError={isValid === false}
        />
      }
    >
      <StyledFormWrapper>
        <LoadingBackdrop show={loading} parentScreen={true} />
        <View
          formikRef={formikRef}
          parentId={isSubFile ? Number(parentId) : null}
          initialValues={initialForm}
          onSubmit={(values: AcquisitionForm, formikHelpers: FormikHelpers<AcquisitionForm>) => {
            return withUserOverride(
              (userOverrideCodes: UserOverrideCode[]) =>
                handleSubmit(values, formikHelpers, userOverrideCodes),
              [],
              (axiosError: AxiosError<IApiError>) => {
                formikHelpers?.setSubmitting(false);
                setModalContent({
                  variant: 'error',
                  title: 'Error',
                  message: axiosError?.response?.data.error,
                  okButtonText: 'Close',
                });
                setDisplayModal(true);
              },
            );
          }}
          validationSchema={AddAcquisitionFileYupSchema}
          confirmBeforeAdd={confirmBeforeAdd}
        />
      </StyledFormWrapper>
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

export default AddAcquisitionContainer;
