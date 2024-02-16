import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { featuresetToMapProperty } from '@/utils';

import { PropertyForm } from '../../shared/models';
import { DispositionFormModel } from '../models/DispositionFormModel';
import { IAddDispositionContainerViewProps } from './AddDispositionContainerView';

export interface IAddDispositionContainerProps {
  onClose?: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAddDispositionContainerViewProps>>;
}

const AddDispositionContainer: React.FC<IAddDispositionContainerProps> = ({ onClose, View }) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const formikRef = useRef<FormikProps<DispositionFormModel>>(null);
  const history = useHistory();
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;

  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    addDispositionFileApi: { execute: addDispositionFileApi, loading },
  } = useDispositionProvider();

  const initialForm = useMemo(() => {
    const dispositionForm = new DispositionFormModel();
    // support creating a new disposition file from the map popup
    if (selectedFeatureDataset !== null) {
      dispositionForm.fileProperties = [
        PropertyForm.fromMapProperty(featuresetToMapProperty(selectedFeatureDataset)),
      ];
    }
    return dispositionForm;
  }, [selectedFeatureDataset]);

  const { bcaLoading, initialProperty } = useInitialMapSelectorProperties(selectedFeatureDataset);
  if (initialForm?.fileProperties.length && initialProperty) {
    initialForm.fileProperties[0].address = initialProperty.address;
  }

  useEffect(() => {
    if (!!initialForm && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('fileProperties', initialForm.fileProperties);
    }
  }, [initialForm]);

  const handleCancel = useCallback(() => onClose && onClose(), [onClose]);

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
    history.replace(`/mapview/sidebar/disposition/${disposition.id}`);
  };

  const handleSubmit = async (
    values: DispositionFormModel,
    formikHelpers: FormikHelpers<DispositionFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
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
      loading={loading || bcaLoading}
      displayFormInvalid={!isFormValid}
      onSave={handleSave}
      onCancel={handleCancel}
      onSubmit={(
        values: DispositionFormModel,
        formikHelpers: FormikHelpers<DispositionFormModel>,
      ) =>
        withUserOverride(
          (userOverrideCodes: UserOverrideCode[]) =>
            handleSubmit(values, formikHelpers, userOverrideCodes),
          [],
          (axiosError: AxiosError<IApiError>) => {
            setModalContent({
              variant: 'error',
              title: 'Warning',
              message: axiosError?.response?.data.error,
              okButtonText: 'Close',
            });
            setDisplayModal(true);
          },
        )
      }
    ></View>
  );
};

export default AddDispositionContainer;
