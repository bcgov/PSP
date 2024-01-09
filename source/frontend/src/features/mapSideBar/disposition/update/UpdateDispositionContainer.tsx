import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import React from 'react';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { DispositionFormModel } from '../models/DispositionFormModel';
import { IUpdateDispositionViewProps } from './UpdateDispositionView';

export interface IUpdateDispositionContainerProps {
  formikRef: React.Ref<FormikProps<DispositionFormModel>>;
  dispositionFile: Api_DispositionFile;
  onSuccess: () => void;
  View: React.FC<IUpdateDispositionViewProps>;
}

const UpdateDispositionContainer: React.FC<IUpdateDispositionContainerProps> = ({
  formikRef,
  dispositionFile,
  onSuccess,
  View,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    putDispositionFile: { execute: updateDispositionFile, loading },
  } = useDispositionProvider();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<Api_DispositionFile | void>
  >('Failed to update Disposition File');

  const onUpdateSuccess = async (apiDispositionFile: Api_DispositionFile) => {
    onSuccess && onSuccess();
  };

  const handleSubmit = async (
    values: DispositionFormModel,
    formikHelpers: FormikHelpers<DispositionFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const dispositionFile = values.toApi();
      if (dispositionFile.id) {
        const response = await updateDispositionFile(
          dispositionFile.id,
          dispositionFile,
          userOverrideCodes,
        );

        if (!!response?.id) {
          formikHelpers?.resetForm();
          onUpdateSuccess(response);
        }
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <View
      formikRef={formikRef}
      dispositionInitialValues={DispositionFormModel.fromApi(dispositionFile)}
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
      loading={loading}
    ></View>
  );
};

export default UpdateDispositionContainer;
