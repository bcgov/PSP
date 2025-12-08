import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useRef, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { usePropertyFormSyncronizer } from '@/hooks/usePropertyFormSyncronizer';
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
  const [needsFirstTimeConfirmation, setNeedsFirstTimeConfirmation] = useState<boolean>(true);

  const {
    addDispositionFileApi: { execute: addDispositionFileApi, loading },
  } = useDispositionProvider();

  const mapMachine = useMapStateMachine();

  const initialForm = new DispositionFormModel();

  // Warn user that property is part of an existing disposition file
  const confirmProperty = useCallback(
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

  const { featuresWithAddresses, isLoading } = usePropertyFormSyncronizer(
    formikRef,
    confirmBeforeAdd,
  );

  useEffect(() => {
    if (featuresWithAddresses?.length > 0 && !formikRef?.current?.values?.regionCode) {
      const firstPropertyFeature = firstOrNull(featuresWithAddresses)?.feature;

      if (exists(firstPropertyFeature)) {
        const firstProperty = PropertyForm.fromLocationFeatureDataset(firstPropertyFeature);
        formikRef?.current?.setFieldValue(
          'regionCode',
          firstProperty.regionName !== 'Cannot determine' ? firstProperty.region : undefined,
        );
      }
    }
  }, [featuresWithAddresses]);

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
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <View
      formikRef={formikRef}
      dispositionInitialValues={initialForm}
      loading={loading || isLoading}
      displayFormInvalid={!isFormValid}
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
