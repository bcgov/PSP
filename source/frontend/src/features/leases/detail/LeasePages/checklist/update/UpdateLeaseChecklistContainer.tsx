import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import React from 'react';
import { toast } from 'react-toastify';

import * as API from '@/constants/API';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { ChecklistFormModel } from '@/features/mapSideBar/shared/tabs/checklist/update/models';
import { IUpdateChecklistFormProps } from '@/features/mapSideBar/shared/tabs/checklist/update/UpdateChecklistForm';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';

export interface IUpdateLeaseChecklistContainerProps {
  formikRef: React.Ref<FormikProps<ChecklistFormModel>>;
  onSuccess: () => void;
  View: React.FC<IUpdateChecklistFormProps>;
}

const UpdateLeaseChecklistContainer: React.FC<IUpdateLeaseChecklistContainerProps> = ({
  formikRef,
  onSuccess,
  View,
}) => {
  const { lease } = React.useContext(LeaseStateContext);

  const { getByType } = useLookupCodeHelpers();
  const sectionTypes = getByType(API.LEASE_CHECKLIST_SECTION_TYPES);
  const {
    putLeaseChecklist: { execute: updateChecklist },
  } = useLeaseRepository();

  const initialValues =
    lease !== undefined
      ? ChecklistFormModel.fromApi(lease, sectionTypes)
      : new ChecklistFormModel();

  const saveChecklist = async (lease: ApiGen_Concepts_FileWithChecklist) => {
    return updateChecklist(lease);
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
      sectionTypeName={API.LEASE_CHECKLIST_SECTION_TYPES}
      statusTypeName={API.LEASE_CHECKLIST_ITEM_STATUS_TYPES}
      prefix="ls"
    ></View>
  );
};

export default UpdateLeaseChecklistContainer;
