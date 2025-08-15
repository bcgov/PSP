import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useRef, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import PropertySelectorPidSearchContainer from '@/components/propertySelector/search/PropertySelectorPidSearchContainer';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { usePropertyOperationRepository } from '@/hooks/repositories/usePropertyOperationRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, featuresetToMapProperty, firstOrNull, isValidString } from '@/utils';

import { AddressForm, PropertyForm } from '../shared/models';
import { SubdivisionFormModel } from './AddSubdivisionModel';
import { IAddSubdivisionViewProps } from './AddSubdivisionView';

export interface IAddSubdivisionContainerProps {
  onClose: () => void;
  onSuccess: (propertyId: number | undefined) => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAddSubdivisionViewProps>>;
}

const AddSubdivisionContainer: React.FC<IAddSubdivisionContainerProps> = ({
  onClose,
  onSuccess,
  View,
}) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const [initialForm, setInitialForm] = useState<SubdivisionFormModel>(new SubdivisionFormModel());
  const formikRef = useRef<FormikProps<SubdivisionFormModel>>(null);
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = firstOrNull(mapMachine.selectedFeatures);
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();

  const {
    addPropertyOperationApi: { execute: addPropertyOperation, loading },
  } = usePropertyOperationRepository();
  const handleCancel = useCallback(() => {
    onClose();
  }, [onClose]);

  const getAddress = useCallback(
    async (pid: string): Promise<AddressForm | undefined> => {
      const bcaSummary = await getPrimaryAddressByPid(pid, 30000);
      return bcaSummary?.address ? AddressForm.fromBcaAddress(bcaSummary?.address) : undefined;
    },
    [getPrimaryAddressByPid],
  );

  useEffect(() => {
    loadInitialProperty();

    async function loadInitialProperty() {
      // support creating a new subdivision from the map popup
      if (selectedFeatureDataset !== null) {
        const propertyForm = PropertyForm.fromMapProperty(
          featuresetToMapProperty(selectedFeatureDataset),
        );
        if (isValidString(propertyForm.pid)) {
          // TODO: This should work with multiple properties
          const pimsFeature = selectedFeatureDataset.pimsFeature;
          propertyForm.address = pimsFeature?.properties
            ? AddressForm.fromPimsView(pimsFeature?.properties)
            : undefined;
          const subdivisionFormModel = new SubdivisionFormModel();
          subdivisionFormModel.sourceProperty = propertyForm.toApi();
          subdivisionFormModel.sourceProperty.isOwned = pimsFeature.properties.IS_OWNED;
          setInitialForm(subdivisionFormModel);
        }
      }
    }
  }, [selectedFeatureDataset, getAddress]);

  useEffect(() => {
    if (exists(initialForm) && exists(formikRef.current)) {
      formikRef.current.resetForm();
      formikRef.current.setFieldValue('sourceProperty', initialForm.sourceProperty);
    }
  }, [initialForm]);

  const setFilePropertyLocations = mapMachine.setFilePropertyLocations;
  useEffect(() => {
    return () => setFilePropertyLocations([]);
  }, [setFilePropertyLocations]);

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_DispositionFile | void>
  >('Failed to create Subdivision');

  const handleSave = async () => {
    await formikRef?.current?.validateForm();
    if (formikRef?.current?.isValid) {
      setIsFormValid(true);

      setModalContent({
        variant: 'info',
        cancelButtonText: 'No',
        okButtonText: 'Yes',
        title: 'Are you sure?',
        message: (
          <>
            <p>
              You are subdividing a property into two or more properties. The old parent property
              record will be retired, and the new child properties will be created
            </p>
            <p>
              If you proceed, you will be redirected to the old parent property record, where you
              can view changes and make updates to the new properties. Do you want to proceed?
            </p>
          </>
        ),
        handleOk: () => {
          formikRef.current?.setSubmitting(true);
          formikRef.current?.submitForm();
          setDisplayModal(false);
        },
        handleCancel: () => setDisplayModal(false),
      });
      setDisplayModal(true);
    } else {
      setIsFormValid(false);
    }
  };

  const handleSubmit = async (
    values: SubdivisionFormModel,
    formikHelpers: FormikHelpers<SubdivisionFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const propertyOperations = values.toApi();
      const response = await addPropertyOperation(propertyOperations, userOverrideCodes);

      if (response?.length) {
        handleSuccess(propertyOperations);
      }
    } finally {
      mapMachine.processCreation();
      formikHelpers?.setSubmitting(false);
    }
  };

  const handleSuccess = async (subdivisions: ApiGen_Concepts_PropertyOperation[]) => {
    mapMachine.refreshMapProperties();
    if (subdivisions.length === 0 || !subdivisions[0].sourceProperty) {
      onSuccess(undefined);
    } else {
      onSuccess(subdivisions[0].sourceProperty?.id ?? undefined);
    }
  };

  return (
    <View
      formikRef={formikRef}
      loading={loading || bcaLoading}
      onSubmit={(
        values: SubdivisionFormModel,
        formikHelpers: FormikHelpers<SubdivisionFormModel>,
      ) =>
        withUserOverride(
          (userOverrideCodes: UserOverrideCode[]) =>
            handleSubmit(values, formikHelpers, userOverrideCodes),
          [],
          (axiosError: AxiosError<IApiError>) => {
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
      onCancel={handleCancel}
      onSave={handleSave}
      subdivisionInitialValues={initialForm}
      displayFormInvalid={!isFormValid}
      getPrimaryAddressByPid={getAddress}
      PropertySelectorPidSearchComponent={PropertySelectorPidSearchContainer}
      MapSelectorComponent={MapSelectorContainer}
    />
  );
};

export default AddSubdivisionContainer;
