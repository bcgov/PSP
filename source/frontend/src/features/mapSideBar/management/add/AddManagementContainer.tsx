import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useEditPropertiesNotifier } from '@/hooks/useEditPropertiesNotifier';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId } from '@/utils';

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
  const { setModalContent, setDisplayModal } = useModalContext();
  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  const {
    addManagementFileApi: { execute: addManagementFileApi, loading },
  } = useManagementFileRepository();

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

  const mapMachine = useMapStateMachine();

  const { featuresWithAddresses, bcaLoading } = useEditPropertiesNotifier(
    formikRef,
    'fileProperties',
  );

  const initialForm = useMemo(() => {
    const managementForm = new ManagementFormModel();
    return managementForm;
  }, []);

  // Require user confirmation before adding a property to file
  // This is the flow for Map Marker -> right-click -> create Management File
  useEffect(() => {
    const runAsync = async () => {
      const incomingProperties =
        featuresWithAddresses?.map(f => PropertyForm.fromFeatureDataset(f.feature)) ?? [];
      if (exists(incomingProperties) && exists(formikRef.current) && needsUserConfirmation) {
        if (incomingProperties.length > 0) {
          // Check all properties for confirmation
          const needsConfirmation = await Promise.all(
            incomingProperties.map(formProperty => confirmBeforeAdd(formProperty)),
          );
          if (needsConfirmation.some(confirm => confirm)) {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: (
                <>
                  <p>
                    One or more properties have already been added to one or more management files.
                  </p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: () => {
                // allow the property to be added to the file being created
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', incomingProperties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
              handleCancel: () => {
                // clear out the properties array as the user did not agree to the popup
                incomingProperties.splice(0, incomingProperties.length);
                formikRef.current.resetForm();
                formikRef.current.setFieldValue('properties', incomingProperties);
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
    featuresWithAddresses,
    needsUserConfirmation,
    setDisplayModal,
    setModalContent,
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
      mapMachine.processCreation();
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
