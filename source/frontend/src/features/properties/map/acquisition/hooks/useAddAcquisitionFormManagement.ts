import { FormikProps } from 'formik';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import useApiUserOverride from 'hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from 'hooks/useInitialMapSelectorProperties';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { UserOverrideCode } from 'models/api/UserOverrideCode';
import { useCallback } from 'react';

import { AddAcquisitionFileYupSchema } from '../add/AddAcquisitionFileYupSchema';
import { AcquisitionForm } from '../add/models';

export interface IUseAddAcquisitionFormManagementProps {
  /** Optional - callback to execute after acquisition file has been added to the datastore */
  onSuccess?: (acquisitionFile: Api_AcquisitionFile) => Promise<void>;
  initialForm?: AcquisitionForm;
  selectedFeature: Feature<Geometry, GeoJsonProperties> | null;
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
        const acquisitionFile = values.toApi();
        const response = await addAcquisitionFile.execute(acquisitionFile, userOverrideCodes);
        if (!!response?.id) {
          if (typeof onSuccess === 'function') {
            await onSuccess(response);
          }
        }
        setSubmitting(false);
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
