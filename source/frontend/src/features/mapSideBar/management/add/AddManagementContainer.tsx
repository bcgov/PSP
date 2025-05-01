import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useManagementProvider } from '@/hooks/repositories/useManagementProvider';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, featuresetToMapProperty, isValidId } from '@/utils';

import { PropertyForm } from '../../shared/models';
import { ManagementFormModel } from '../models/ManagementFormModel';
import { IAddManagementContainerViewProps } from './AddManagementContainerView';

export interface IAddManagementContainerProps {
  onClose: () => void;
  onSuccess: (newManagementId: number) => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAddManagementContainerViewProps>>;
}

const AddManagementContainer: React.FC<IAddManagementContainerProps> = ({
  onClose,
  onSuccess,
  View,
}) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const formikRef = useRef<FormikProps<ManagementFormModel>>(null);
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;

  const { setModalContent, setDisplayModal } = useModalContext();
  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  const {
    addManagementFileApi: { execute: addManagementFileApi, loading },
  } = useManagementProvider();

  // Warn user that property is part of an existing management file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      if (isValidId(propertyForm.apiId)) {
        const response = await getPropertyAssociations(propertyForm.apiId);
        const fileAssociations = response?.managementAssociations ?? [];
        const otherFiles = fileAssociations.filter(a => exists(a.id));
        return otherFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations],
  );

  const initialForm = useMemo(() => {
    const managementForm = new ManagementFormModel();
    // support creating a new management file from the map popup
    if (selectedFeatureDataset !== null) {
      const property = PropertyForm.fromMapProperty(
        featuresetToMapProperty(selectedFeatureDataset),
      );
      managementForm.fileProperties = [property];
    }
    return managementForm;
  }, [selectedFeatureDataset]);

  const { bcaLoading, initialProperty } = useInitialMapSelectorProperties(selectedFeatureDataset);
  if (initialForm?.fileProperties.length && initialProperty) {
    initialForm.fileProperties[0].address = initialProperty.address;
  }

  // Require user confirmation before adding a property to file
  // This is the flow for Map Marker -> right-click -> create Management File
  useEffect(() => {
    const runAsync = async () => {
      if (exists(initialForm) && exists(formikRef.current) && needsUserConfirmation) {
        if (initialForm.fileProperties.length > 0) {
          const formProperty = initialForm.fileProperties[0];
          if (await confirmBeforeAdd(formProperty)) {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: (
                <>
                  <p>This property has already been added to one or more management files.</p>
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
  }, [
    confirmBeforeAdd,
    initialForm,
    needsUserConfirmation,
    setDisplayModal,
    setModalContent,
    bcaLoading,
  ]);

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
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_ManagementFile | void>
  >('Failed to create Management File');

  const handleSuccess = async (management: ApiGen_Concepts_ManagementFile) => {
    mapMachine.refreshMapProperties();
    onSuccess(management.id);
  };

  const handleSubmit = async (
    values: ManagementFormModel,
    formikHelpers: FormikHelpers<ManagementFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      formikHelpers.setSubmitting(true);
      const managementFile = values.toApi();
      const response = await addManagementFileApi(managementFile, userOverrideCodes);

      if (response?.id) {
        formikHelpers?.resetForm();
        handleSuccess(response);
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <View
      formikRef={formikRef}
      managementInitialValues={initialForm}
      loading={loading || bcaLoading}
      displayFormInvalid={!isFormValid}
      confirmBeforeAdd={confirmBeforeAdd}
      onSave={handleSave}
      onCancel={handleCancel}
      onSubmit={(
        values: ManagementFormModel,
        formikHelpers: FormikHelpers<ManagementFormModel>,
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

export default AddManagementContainer;
