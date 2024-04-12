import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import PropertySelectorPidSearchContainer from '@/components/propertySelector/search/PropertySelectorPidSearchContainer';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { usePropertyOperationRepository } from '@/hooks/repositories/usePropertyOperationRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, featuresetToMapProperty, isValidString } from '@/utils';

import { AddressForm, PropertyForm } from '../shared/models';
import { ConsolidationFormModel } from './AddConsolidationModel';
import { IAddConsolidationViewProps } from './AddConsolidationView';

export interface IAddConsolidationContainerProps {
  onClose?: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAddConsolidationViewProps>>;
}

const AddConsolidationContainer: React.FC<IAddConsolidationContainerProps> = ({
  onClose,
  View,
}) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const [initialForm, setInitialForm] = useState<ConsolidationFormModel>(
    new ConsolidationFormModel(),
  );
  const formikRef = useRef<FormikProps<ConsolidationFormModel>>(null);
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;
  const history = useHistory();
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();

  const {
    addPropertyOperationApi: { execute: addPropertyOperation, loading },
  } = usePropertyOperationRepository();

  const handleCancel = useCallback(() => {
    if (formikRef.current?.dirty) {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          onClose?.();
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    } else {
      onClose?.();
    }
  }, [onClose, setDisplayModal, setModalContent]);

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
      if (selectedFeatureDataset !== null) {
        // TODO: this is an odd conversion. the feature set should map directly to a Pims Property
        const propertyForm = PropertyForm.fromMapProperty(
          featuresetToMapProperty(selectedFeatureDataset),
        );
        if (isValidString(propertyForm.pid)) {
          propertyForm.address = selectedFeatureDataset.pimsFeature?.properties
            ? AddressForm.fromPimsView(selectedFeatureDataset.pimsFeature?.properties)
            : undefined;
          // TODO: Remove this once the conversion is cleaner
          propertyForm.isOwned = selectedFeatureDataset.pimsFeature?.properties.IS_OWNED;
          const consolidationFormModel = new ConsolidationFormModel();
          consolidationFormModel.sourceProperties = [propertyForm.toApi()];
          setInitialForm(consolidationFormModel);
        }
      }
    }
  }, [selectedFeatureDataset, getAddress]);

  useEffect(() => {
    if (exists(initialForm) && exists(formikRef.current)) {
      formikRef.current.resetForm();
      formikRef.current.setFieldValue('sourceProperties', initialForm.sourceProperties);
    }
  }, [initialForm]);

  const changeSidebar = mapMachine.changeSidebar;
  useEffect(() => {
    return () => changeSidebar();
  }, [changeSidebar]);

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_DispositionFile | void>
  >('Failed to create Consolidation');

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
              You are consolidating two or more properties into one. The old parent properties
              records will be retired, and a new child property will be created.
            </p>
            <p>
              If you proceed, you will be redirected to the new child property record, where you can
              view changes and make updates. Do you want to proceed?
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
    values: ConsolidationFormModel,
    formikHelpers: FormikHelpers<ConsolidationFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const propertyOperations = values.toApi();
      const response = await addPropertyOperation(propertyOperations, userOverrideCodes);

      if (response?.length) {
        handleSuccess(response);
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const handleSuccess = async (consolidations: ApiGen_Concepts_PropertyOperation[]) => {
    mapMachine.refreshMapProperties();
    if (consolidations.length === 0 || !consolidations[0].destinationProperty) {
      history.replace(`/mapview`);
    } else {
      history.replace(`/mapview/sidebar/property/${consolidations[0].destinationProperty?.id}`);
    }
  };

  return (
    <View
      formikRef={formikRef}
      loading={loading || bcaLoading}
      onSubmit={(
        values: ConsolidationFormModel,
        formikHelpers: FormikHelpers<ConsolidationFormModel>,
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
      consolidationInitialValues={initialForm}
      displayFormInvalid={!isFormValid}
      getPrimaryAddressByPid={getAddress}
      PropertySelectorPidSearchComponent={PropertySelectorPidSearchContainer}
      MapSelectorComponent={MapSelectorContainer}
    ></View>
  );
};

export default AddConsolidationContainer;
