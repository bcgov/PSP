import axios, { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
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

  const { onSuccess } = props;
  const { bcaLoading, initialProperty } = useInitialMapSelectorProperties(props.selectedFeature);
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to add Acquisition File');

  // save handler
  const handleSubmit = useCallback(
    async (values: AcquisitionForm, setSubmitting: (isSubmitting: boolean) => void) => {
      return withUserOverride(async (userOverrideCodes: UserOverrideCode[]) => {
        try {
          const acquisitionFile = values.toApi();
          const response = await addAcquisitionFile.execute(acquisitionFile, userOverrideCodes);
          if (exists(response) && isValidId(response?.id)) {
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
    [addAcquisitionFile, onSuccess, withUserOverride],
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
