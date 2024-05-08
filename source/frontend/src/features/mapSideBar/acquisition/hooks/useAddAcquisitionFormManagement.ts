import { AxiosError } from 'axios';
import { FormikProps } from 'formik/dist/types';
import { useCallback } from 'react';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId } from '@/utils';

import { AddAcquisitionFileYupSchema } from '../add/AddAcquisitionFileYupSchema';
import { AcquisitionForm } from '../add/models';

export interface IUseAddAcquisitionFormManagementProps {
  /** Optional - callback to execute after acquisition file has been added to the datastore */
  onSuccess?: (acquisitionFile: ApiGen_Concepts_AcquisitionFile) => Promise<void>;
  initialForm?: AcquisitionForm;
  selectedFeature: LocationFeatureDataset | null;
  formikRef: React.RefObject<FormikProps<AcquisitionForm>>;
}

/**
 * Hook that provides form state and submit handlers for Add Acquisition File.
 */
export function useAddAcquisitionFormManagement(props: IUseAddAcquisitionFormManagementProps) {
  const { addAcquisitionFile } = useAcquisitionProvider();
  const { setModalContent, setDisplayModal } = useModalContext();
  const { onSuccess } = props;
  const { bcaLoading, initialProperty } = useInitialMapSelectorProperties(props.selectedFeature);
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to add Acquisition File');

  // save handler
  const handleSubmit = useCallback(
    async (values: AcquisitionForm, setSubmitting: (isSubmitting: boolean) => void) => {
      return withUserOverride(
        async (userOverrideCodes: UserOverrideCode[]) => {
          try {
            const acquisitionFile = values.toApi();
            const response = await addAcquisitionFile.execute(acquisitionFile, userOverrideCodes);
            if (exists(response) && isValidId(response?.id)) {
              if (typeof onSuccess === 'function') {
                await onSuccess(response);
              }
            }
          } finally {
            setSubmitting(false);
          }
        },
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
    },
    [addAcquisitionFile, onSuccess, setDisplayModal, setModalContent, withUserOverride],
  );
  if (props.initialForm?.properties.length && initialProperty) {
    props.initialForm.properties[0].address = initialProperty.address;
  }

  return {
    handleSubmit,
    initialValues: bcaLoading ? new AcquisitionForm() : props.initialForm ?? new AcquisitionForm(),
    validationSchema: AddAcquisitionFileYupSchema,
    loading: addAcquisitionFile.loading || bcaLoading,
  };
}
