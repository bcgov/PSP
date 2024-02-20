import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import React from 'react';
import { toast } from 'react-toastify';

import * as API from '@/constants/API';
import { ChecklistFormModel } from '@/features/mapSideBar/shared/tabs/checklist/update/models';
import { IUpdateChecklistFormProps } from '@/features/mapSideBar/shared/tabs/checklist/update/UpdateChecklistForm';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';

export interface IDispositionChecklistContainerProps {
  formikRef: React.Ref<FormikProps<ChecklistFormModel>>;
  dispositionFile?: ApiGen_Concepts_DispositionFile;
  onSuccess: () => void;
  View: React.FC<IUpdateChecklistFormProps>;
}

export const UpdateDispositionChecklistContainer: React.FC<IDispositionChecklistContainerProps> = ({
  formikRef,
  dispositionFile,
  onSuccess,
  View,
}) => {
  const { getByType } = useLookupCodeHelpers();
  const {
    putDispositionChecklist: { execute: updateDispositionChecklist },
  } = useDispositionProvider();

  const sectionTypes = getByType(API.DISPOSITION_CHECKLIST_SECTION_TYPES);

  const initialValues =
    dispositionFile !== undefined
      ? ChecklistFormModel.fromApi(dispositionFile, sectionTypes)
      : new ChecklistFormModel();

  const saveChecklist = async (apiDispositionFile: ApiGen_Concepts_FileWithChecklist) => {
    return updateDispositionChecklist(apiDispositionFile);
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
      sectionTypeName={API.DISPOSITION_CHECKLIST_SECTION_TYPES}
      statusTypeName={API.DISPOSITION_CHECKLIST_ITEM_STATUS_TYPES}
      prefix="dsp"
    />
  );
};
