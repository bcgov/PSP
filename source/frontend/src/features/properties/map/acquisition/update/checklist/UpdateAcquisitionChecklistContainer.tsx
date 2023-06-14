import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import React from 'react';
import { toast } from 'react-toastify';

import * as API from '@/constants/API';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { IApiError } from '@/interfaces/IApiError';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

import { AcquisitionChecklistFormModel } from './models';
import { IUpdateAcquisitionChecklistFormProps } from './UpdateAcquisitionChecklistForm';

export interface IAcquisitionChecklistContainerProps {
  formikRef: React.Ref<FormikProps<AcquisitionChecklistFormModel>>;
  acquisitionFile?: Api_AcquisitionFile;
  onSuccess: () => void;
  View: React.FC<IUpdateAcquisitionChecklistFormProps>;
}

export const UpdateAcquisitionChecklistContainer: React.FC<IAcquisitionChecklistContainerProps> = ({
  formikRef,
  acquisitionFile,
  onSuccess,
  View,
}) => {
  const { getByType } = useLookupCodeHelpers();
  const {
    updateAcquisitionChecklist: { execute: updateAcquisitionChecklist },
  } = useAcquisitionProvider();

  const sectionTypes = getByType(API.ACQUISITION_CHECKLIST_SECTION_TYPES);

  const initialValues =
    acquisitionFile !== undefined
      ? AcquisitionChecklistFormModel.fromApi(acquisitionFile, sectionTypes)
      : new AcquisitionChecklistFormModel();

  const saveChecklist = async (apiAcquisitionFile: Api_AcquisitionFile) => {
    return updateAcquisitionChecklist(apiAcquisitionFile);
  };

  const onUpdateSuccess = async (apiAcquisitionFile: Api_AcquisitionFile) => {
    onSuccess && onSuccess();
  };

  // generic error handler.
  const onError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 400) {
      toast.error(e?.response.data.error);
    } else {
      toast.error('Unable to save. Please try again.');
    }
  };

  return (
    <View
      formikRef={formikRef}
      initialValues={initialValues}
      onSave={saveChecklist}
      onSuccess={onUpdateSuccess}
      onError={onError}
    />
  );
};
