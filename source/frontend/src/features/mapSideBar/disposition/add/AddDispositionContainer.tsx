import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, firstOrNull, isValidId } from '@/utils';

import { PropertyForm } from '../../shared/models';
import { DispositionFormModel } from '../models/DispositionFormModel';
import { IAddDispositionContainerViewProps } from './AddDispositionContainerView';

export interface IAddDispositionContainerProps {
  onClose: () => void;
  onSuccess: (newDispositionId: number) => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAddDispositionContainerViewProps>>;
}

const AddDispositionContainer: React.FC<IAddDispositionContainerProps> = ({
  onClose,
  onSuccess,
  View,
}) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const formikRef = useRef<FormikProps<DispositionFormModel>>(null);

  const { setModalContent, setDisplayModal } = useModalContext();
  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  const {
    addDispositionFileApi: { execute: addDispositionFileApi, loading },
  } = useDispositionProvider();

  // Warn user that property is part of an existing disposition file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      if (isValidId(propertyForm.apiId)) {
        const response = await getPropertyAssociations(propertyForm.apiId);
        const fileAssociations = response?.dispositionAssociations ?? [];
        const otherFiles = fileAssociations.filter(a => exists(a.id));
        return otherFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations],
  );

  const mapMachine = useMapStateMachine();
  const selectedFeatureDatasets = mapMachine.selectedFeatures ?? [];

  // Get PropertyForms with addresses for all selected features
  const { featuresWithAddresses, bcaLoading } =
    useFeatureDatasetsWithAddresses(selectedFeatureDatasets);

  const initialForm = useMemo(() => {
    const dispositionForm = new DispositionFormModel();
    // support creating a new disposition file from the map popup
    if (featuresWithAddresses?.length > 0) {
      dispositionForm.fileProperties = featuresWithAddresses.map(obj => {
        const property = PropertyForm.fromFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          property.address = obj.address;
        }
        return property;
      });
      // auto-select file region based upon the location of the first property added to the file
      const firstProperty = firstOrNull(dispositionForm.fileProperties);
      if (exists(firstProperty)) {
        dispositionForm.regionCode =
          firstProperty.regionName !== 'Cannot determine'
            ? firstProperty.region?.toString() ?? null
            : null;
      }
    }
    return dispositionForm;
  }, [featuresWithAddresses]);

  // Require user confirmation before adding a property to file
  // This is the flow for Map Marker -> right-click -> create Disposition File
  useEffect(() => {
    const runAsync = async () => {
      if (exists(initialForm) && exists(formikRef.current) && needsUserConfirmation) {
        if (initialForm.fileProperties.length > 0) {
          // Check all properties for confirmation
          const needsConfirmation = await Promise.all(
            initialForm.fileProperties.map(formProperty => confirmBeforeAdd(formProperty)),
          );
          if (needsConfirmation.some(confirm => confirm)) {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: (
                <>
                  <p>
                    One or more properties have already been added to one or more disposition files.
                  </p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: () => {
                // allow the property to be added to the file being created
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('fileProperties', initialForm.fileProperties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
              handleCancel: () => {
                // clear out the properties array as the user did not agree to the popup
                initialForm.fileProperties.splice(0, initialForm.fileProperties.length);
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('fileProperties', initialForm.fileProperties);
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

  const handleCancel = useCallback(() => onClose(), [onClose]);

  const handleSave = async () => {
    await formikRef?.current?.validateForm();
    if (formikRef?.current?.isValid) {
      setIsFormValid(true);
    } else {
      setIsFormValid(false);
    }

    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_DispositionFile | void>
  >('Failed to create Disposition File');

  const handleSuccess = async (disposition: ApiGen_Concepts_DispositionFile) => {
    mapMachine.refreshMapProperties();
    onSuccess(disposition.id);
  };

  const handleSubmit = async (
    values: DispositionFormModel,
    formikHelpers: FormikHelpers<DispositionFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      formikHelpers.setSubmitting(true);
      const dispositionFile = values.toApi();
      const response = await addDispositionFileApi(dispositionFile, userOverrideCodes);

      if (response?.id) {
        formikHelpers?.resetForm();
        handleSuccess(response);
      }
    } finally {
      mapMachine.processCreation();
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <View
      formikRef={formikRef}
      dispositionInitialValues={initialForm}
      loading={loading || bcaLoading}
      displayFormInvalid={!isFormValid}
      confirmBeforeAdd={confirmBeforeAdd}
      onSave={handleSave}
      onCancel={handleCancel}
      onSubmit={(
        values: DispositionFormModel,
        formikHelpers: FormikHelpers<DispositionFormModel>,
      ) => {
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
        );
      }}
    ></View>
  );
};

export default AddDispositionContainer;
