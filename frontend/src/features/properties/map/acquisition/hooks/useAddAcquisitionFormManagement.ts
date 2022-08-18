import { FormikHelpers } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { useCallback } from 'react';

import { AddAcquisitionFileYupSchema } from '../add/AddAcquisitionFileYupSchema';
import { AcquisitionForm } from '../add/models';
import { useAcquisitionProvider } from './useAcquisitionProvider';

export interface IUseAddAcquisitionFormManagementProps {
  /** Optional - callback to execute after acquisition file has been added to the datastore */
  onSuccess?: (acquisitionFile: Api_AcquisitionFile) => void;
}

/**
 * Hook that provides form state and submit handlers for Add Acquisition File.
 */
export function useAddAcquisitionFormManagement(props: IUseAddAcquisitionFormManagementProps) {
  const { addAcquisitionFile } = useAcquisitionProvider();
  const { onSuccess } = props;

  // save handler
  const handleSubmit = useCallback(
    async (values: AcquisitionForm, formikHelpers: FormikHelpers<AcquisitionForm>) => {
      const acquisitionFile = values.toApi();
      const response = await addAcquisitionFile.execute(acquisitionFile);
      formikHelpers?.setSubmitting(false);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess(response);
        }
      }
    },
    [addAcquisitionFile, onSuccess],
  );

  return {
    handleSubmit,
    initialValues: new AcquisitionForm(),
    validationSchema: AddAcquisitionFileYupSchema,
  };
}
