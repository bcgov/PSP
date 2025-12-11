import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useRef, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { usePropertyFormSyncronizer } from '@/hooks/usePropertyFormSyncronizer';
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
  const [needsFirstTimeConfirmation, setNeedsFirstTimeConfirmation] = useState<boolean>(true);
  const mapMachine = useMapStateMachine();
  const initialForm = new ManagementFormModel();

  const {
    addManagementFileApi: { execute: addManagementFileApi, loading },
  } = useManagementFileRepository();

  // Warn user that property is part of an existing management file
  const confirmProperty = useCallback(
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

  // Require user confirmation before adding a property to file
  const confirmBeforeAdd = useCallback(
    async (
      newPropertyForms: PropertyForm[],
      isValidCallback: (isValid: boolean, newProperties: PropertyForm[]) => void,
    ) => {
      const needsConfirmation = await Promise.all(
        newPropertyForms.map(formProperty => confirmProperty(formProperty)),
      );
      if (needsFirstTimeConfirmation && needsConfirmation.some(x => x === true)) {
        // show the user confirmation modal only once when creating a file
        setNeedsFirstTimeConfirmation(false);
        setModalContent({
          variant: 'warning',
          title: 'User Override Required',
          message: (
            <>
              <p>One or more properties have already been added to one or more management files.</p>
              <p>Do you want to acknowledge and proceed?</p>
            </>
          ),
          okButtonText: 'Yes',
          cancelButtonText: 'No',
          handleOk: () => {
            // allow the property to be added to the file being created
            isValidCallback(true, newPropertyForms);
            setDisplayModal(false);
          },
          handleCancel: () => {
            isValidCallback(false, []);
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        isValidCallback(true, newPropertyForms);
      }
    },
    [confirmProperty, needsFirstTimeConfirmation, setDisplayModal, setModalContent],
  );

  const { isLoading } = usePropertyFormSyncronizer(formikRef, confirmBeforeAdd);

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
      loading={loading || isLoading}
      displayFormInvalid={!isFormValid}
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
