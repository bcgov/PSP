import axios, { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { DispositionFormModel } from '../models/DispositionFormModel';

export interface IUseAddDispositionFormmanagementProps {
  formikRef: React.RefObject<FormikProps<DispositionFormModel>>;
  onSuccess?: (dispositionFile: Api_DispositionFile) => Promise<void>;
}

const useAddDispositionFormManagement = (props: IUseAddDispositionFormmanagementProps) => {
  const { addDispositionFileApi } = useDispositionProvider();

  const { onSuccess } = props;
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to add Disposition File');

  // save handler
  const handleSubmit = useCallback(
    async (values: DispositionFormModel, setSubmitting: (isSubmitting: boolean) => void) => {
      return withUserOverride(async (userOverrideCodes: UserOverrideCode[]) => {
        try {
          const dispositionFile = values.toApi();
          const response = await addDispositionFileApi.execute(dispositionFile, userOverrideCodes);
          if (!!response?.id) {
            if (typeof onSuccess === 'function') {
              await onSuccess(response);
            }
          }
        } catch (e) {
          const axiosError = e as AxiosError<IApiError>;
          if (axios.isAxiosError(e) && e.response?.status === 409) {
            toast.error(axiosError?.response?.data.error);
          }
        } finally {
          setSubmitting(false);
        }
      });
    },
    [addDispositionFileApi, onSuccess, withUserOverride],
  );

  return {
    handleSubmit,
    loading: addDispositionFileApi.loading,
  };
};

export default useAddDispositionFormManagement;
