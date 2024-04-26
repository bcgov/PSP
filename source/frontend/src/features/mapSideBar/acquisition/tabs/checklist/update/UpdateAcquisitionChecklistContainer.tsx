import { AxiosError } from 'axios';
import { FormikProps } from 'formik/dist/types';
import React from 'react';
import { toast } from 'react-toastify';

import * as API from '@/constants/API';
import { ChecklistFormModel } from '@/features/mapSideBar/shared/tabs/checklist/update/models';
import { IUpdateChecklistFormProps } from '@/features/mapSideBar/shared/tabs/checklist/update/UpdateChecklistForm';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';

export interface IAcquisitionChecklistContainerProps {
  formikRef: React.Ref<FormikProps<ChecklistFormModel>>;
  acquisitionFile?: ApiGen_Concepts_AcquisitionFile;
  onSuccess: () => void;
  View: React.FC<IUpdateChecklistFormProps>;
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
      ? ChecklistFormModel.fromApi(acquisitionFile, sectionTypes)
      : new ChecklistFormModel();

  const saveChecklist = async (apiAcquisitionFile: ApiGen_Concepts_FileWithChecklist) => {
    return updateAcquisitionChecklist(apiAcquisitionFile);
  };

  const onUpdateSuccess = async () => {
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
      sectionTypeName={API.ACQUISITION_CHECKLIST_SECTION_TYPES}
      statusTypeName={API.ACQUISITION_CHECKLIST_ITEM_STATUS_TYPES}
      prefix="acq"
    />
  );
};
