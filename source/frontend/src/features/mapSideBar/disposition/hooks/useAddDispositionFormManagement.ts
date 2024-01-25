import { FormikProps } from 'formik';
import { useCallback } from 'react';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId } from '@/utils';

import { DispositionFormModel } from '../models/DispositionFormModel';

export interface IUseAddDispositionFormmanagementProps {
  formikRef: React.RefObject<FormikProps<DispositionFormModel>>;
  onSuccess?: (dispositionFile: ApiGen_Concepts_DispositionFile) => Promise<void>;
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
        const dispositionFile = values.toApi();
        const response = await addDispositionFileApi.execute(dispositionFile, userOverrideCodes);
        if (exists(response) && isValidId(response?.id)) {
          if (typeof onSuccess === 'function') {
            await onSuccess(response);
            setSubmitting(false);
          }
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
