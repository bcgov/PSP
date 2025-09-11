import { AxiosError } from 'axios';
import { dequal } from 'dequal';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useEditPropertiesMode } from '@/hooks/useEditPropertiesMode';
import { useEnrichWithPimsFeatures } from '@/hooks/useEnrichWithPimsFeatures';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { arePropertyFormsEqual, exists, firstOrNull, isValidId } from '@/utils';

import { useAddLease } from '../hooks/useAddLease';
import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '../models';
import { IAddLeaseFormProps } from './AddLeaseForm';

export interface IAddLeaseContainerProps {
  onClose: () => void;
  onSuccess: (newLeaseId: number) => void;
  View: React.FC<IAddLeaseFormProps>;
}

export const AddLeaseContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseContainerProps>
> = props => {
  const { onClose, onSuccess, View } = props;
  const history = useHistory();
  const formikRef = useRef<FormikProps<LeaseFormModel>>(null);
  const { setModalContent, setDisplayModal } = useModalContext();

  const withUserOverride = useApiUserOverride('Failed to save Lease File');
  const {
    addLease: { execute: addLease, loading: addLeaseLoading },
  } = useAddLease();
  const {
    datasets,
    loading: pimsFeatureLoading,
    enrichWithPimsFeatures,
  } = useEnrichWithPimsFeatures();

  const [isValid, setIsValid] = useState<boolean>(true);

  // Support creating a new lease file from the worklist/quick-info
  const mapMachine = useMapStateMachine();
  const selectedFeatureDatasets = mapMachine.selectedFeatures;
  const prevSelectedRef = useRef<typeof mapMachine.selectedFeatures>();
  const processCreation = mapMachine.processCreation;

  useEditPropertiesMode();

  // track whether we've already shown the confirmation modal for this session
  const hasWarnedRef = useRef(false);

  // Enrich selected features with PIMS features
  // This will add pimsFeature to each SelectedFeatureDataset if it exists
  useEffect(() => {
    if (
      selectedFeatureDatasets?.length > 0 &&
      !dequal(prevSelectedRef.current, selectedFeatureDatasets)
    ) {
      hasWarnedRef.current = false; // reset the warning for new selection
      prevSelectedRef.current = selectedFeatureDatasets;
      enrichWithPimsFeatures(selectedFeatureDatasets);
    }
  }, [selectedFeatureDatasets, enrichWithPimsFeatures]);

  // Get PropertyForms with addresses for all selected features
  const { featuresWithAddresses, bcaLoading } = useFeatureDatasetsWithAddresses(datasets ?? []);

  // Convert SelectedFeatureDataset to PropertyForm
  const propertyForms = useMemo(
    () =>
      featuresWithAddresses.map(obj => {
        const property = PropertyForm.fromFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          property.address = obj.address;
        }
        return property;
      }),
    [featuresWithAddresses],
  );
  // This effect is used to update the file properties when "add to open file" is clicked in the worklist.
  useEffect(() => {
    if (exists(formikRef.current) && propertyForms.length > 0) {
      const existingProperties =
        formikRef.current?.values?.properties?.map(lp => lp?.property) ?? [];
      const uniqueProperties = propertyForms.filter(newProperty => {
        return !existingProperties.some(existingProperty =>
          arePropertyFormsEqual(existingProperty, newProperty),
        );
      });

      const duplicatesSkipped = propertyForms.length - uniqueProperties.length;

      // If there are unique properties, add them to the formik values
      if (uniqueProperties.length > 0) {
        const allProperties = [...existingProperties, ...uniqueProperties].map(obj => {
          const leaseProperty = FormLeaseProperty.fromFeatureDataset(obj.toFeatureDataset());
          if (exists(obj.address) && exists(leaseProperty.property)) {
            leaseProperty.property.address = obj.address;
          }
          return leaseProperty;
        });
        formikRef.current?.setFieldValue('properties', allProperties);
        formikRef.current?.setFieldTouched('properties', true);
        toast.success(`Added ${uniqueProperties.length} new property(s) to the file.`);
      }

      if (duplicatesSkipped > 0) {
        toast.warn(`Skipped ${duplicatesSkipped} duplicate property(s).`);
      }
      processCreation();
    }
  }, [processCreation, propertyForms]);

  const initialForm = useMemo<LeaseFormModel>(() => {
    const leaseForm = getDefaultFormLease();

    if (featuresWithAddresses?.length > 0) {
      // auto-select file region based upon the location of the property
      const firstProperty = firstOrNull(
        featuresWithAddresses?.map(f => PropertyForm.fromFeatureDataset(f.feature)),
      );
      if (exists(firstProperty)) {
        leaseForm.regionId =
          firstProperty?.regionName !== 'Cannot determine'
            ? firstProperty?.region?.toString()
            : undefined;
      }
    }

    return leaseForm;
  }, [featuresWithAddresses]);

  // Require user confirmation before adding non-inventory properties to a lease.
  useEffect(() => {
    if (exists(initialForm.properties) && exists(formikRef.current) && !hasWarnedRef.current) {
      const needsWarning = initialForm.properties.some(
        formProperty => exists(formProperty.property) && !isValidId(formProperty.property.apiId),
      );
      if (needsWarning) {
        hasWarnedRef.current = true; // mark as shown

        setModalContent({
          variant: 'info',
          title: 'Not inventory property',
          message:
            'You have selected a property not previously in the inventory. Do you want to add this property to the lease?',
          okButtonText: 'Add',
          cancelButtonText: 'Cancel',
          handleOk: () => {
            // allow the PIMS properties to be added to the lease being created
            setDisplayModal(false);
            formikRef.current?.setFieldValue('properties', initialForm.properties);
          },
          handleCancel: () => {
            // clear out the properties array as the user did not agree to the popup
            initialForm.properties.splice(0, initialForm.properties.length);
            formikRef.current?.setFieldValue('properties', []);
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      }
    }
  }, [initialForm.properties, setDisplayModal, setModalContent]);

  const saveLeaseFile = async (
    leaseFormModel: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    formikHelpers.setSubmitting(true);
    try {
      const leaseApi = LeaseFormModel.toApi(leaseFormModel);
      const response = await addLease(leaseApi, userOverrideCodes);

      if (exists(response) && isValidId(response?.id)) {
        handleSuccess(response);
      }
    } finally {
      mapMachine.processCreation();
      formikHelpers.setSubmitting(false);
    }
  };

  const handleSuccess = async (apiLease: ApiGen_Concepts_Lease) => {
    if (apiLease.fileProperties?.find(p => !p.property?.address && !p.property?.id)) {
      toast.warn(
        'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
        { autoClose: 15000 },
      );
    }
    mapMachine.refreshMapProperties();
    onSuccess(apiLease.id);
  };

  const handleSave = async () => {
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  const handleCancel = useCallback(() => {
    mapMachine.processCreation();
    onClose();
  }, [mapMachine, onClose]);

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  const loading = addLeaseLoading || bcaLoading || pimsFeatureLoading;

  return (
    <MapSideBarLayout
      title="Create Lease/Licence"
      icon={<LeaseIcon title="Lease and Licence Icon" fill="currentColor" />}
      footer={
        <SidebarFooter
          isOkDisabled={loading}
          onSave={handleSave}
          onCancel={handleCancel}
          displayRequiredFieldError={isValid === false}
        />
      }
      showCloseButton
      onClose={handleCancel}
    >
      <LoadingBackdrop show={loading} parentScreen={true} />
      <View
        onSubmit={(values: LeaseFormModel, formikHelpers: FormikHelpers<LeaseFormModel>) =>
          withUserOverride(
            (useOverrideCodes: UserOverrideCode[]) =>
              saveLeaseFile(values, formikHelpers, useOverrideCodes),
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
          )
        }
        formikRef={formikRef}
        initialValues={initialForm}
      />
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

export default AddLeaseContainer;
