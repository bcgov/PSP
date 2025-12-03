import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import React from 'react';

import * as API from '@/constants/API';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_ManagementFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ManagementFileStatusTypes';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import { ManagementFormModel } from '../models/ManagementFormModel';
import ManagementStatusUpdateSolver from '../tabs/fileDetails/detail/ManagementStatusUpdateSolver';
import { IUpdateManagementFormProps } from './UpdateManagementForm';

export interface IUpdateManagementContainerProps {
  managementFile: ApiGen_Concepts_ManagementFile;
  onSuccess: (updateProperties?: boolean, updateFile?: boolean) => void;
  View: React.FC<IUpdateManagementFormProps>;
}

export const UpdateManagementContainer = React.forwardRef<
  FormikProps<ManagementFormModel>,
  IUpdateManagementContainerProps
>((props, formikRef) => {
  const { managementFile, onSuccess, View } = props;
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getByType } = useLookupCodeHelpers();

  const dispositionStatusTypes = getByType(API.DISPOSITION_FILE_STATUS_TYPES);

  const statusSolver = new ManagementStatusUpdateSolver(managementFile);

  const {
    putManagementFile: { execute: updateManagementFile, loading },
  } = useManagementFileRepository();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_ManagementFile | void>
  >('Failed to update Management File');

  const handleSubmit = async (
    values: ManagementFormModel,
    formikHelpers: FormikHelpers<ManagementFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const managementFile = values.toApi();
      if (managementFile.id) {
        const response = await updateManagementFile(
          managementFile.id,
          managementFile,
          userOverrideCodes,
        );

        if (isValidId(response?.id)) {
          formikHelpers?.resetForm();
          const activeTypeCodes = [
            ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE.toString(),
            ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT.toString(),
            ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD.toString(),
          ];

          //refresh the map properties if this disposition file was set to a final state.
          onSuccess(
            !!managementFile.fileStatusTypeCode?.id &&
              !activeTypeCodes.includes(managementFile.fileStatusTypeCode.id),
            true,
          );
        }
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const handleError = async (axiosError: AxiosError<IApiError>): Promise<void> => {
    setModalContent({
      variant: 'error',
      title: 'Error',
      message: axiosError?.response?.data.error,
      okButtonText: 'Close',
    });
    setDisplayModal(true);
  };

  if (!managementFile) {
    return null;
  }

  return (
    <View
      formikRef={formikRef}
      initialValues={ManagementFormModel.fromApi(managementFile)}
      canEditDetails={statusSolver.canEditDetails()}
      onSubmit={async (
        values: ManagementFormModel,
        formikHelpers: FormikHelpers<ManagementFormModel>,
      ) => {
        const updatedFile = values.toApi();
        const currentFileSolver = new ManagementStatusUpdateSolver(managementFile);
        const updatedFileSolver = new ManagementStatusUpdateSolver(updatedFile);

        if (!currentFileSolver.isAdminProtected() && updatedFileSolver.isAdminProtected()) {
          const statusCode = dispositionStatusTypes.find(x => x.id === values.fileStatusTypeCode);
          setModalContent({
            variant: 'info',
            title: 'Confirm status change',
            message: (
              <>
                <p>
                  You marked this file as {statusCode.name}. If you save it, only the administrator
                  can turn it back on. You will still see it in the management table.
                </p>
                <p>Do you want to acknowledge and proceed?</p>
              </>
            ),
            okButtonText: 'Yes',
            cancelButtonText: 'No',
            handleOk: async () => {
              await withUserOverride(
                (userOverrideCodes: UserOverrideCode[]) =>
                  handleSubmit(values, formikHelpers, userOverrideCodes),
                [],
                (axiosError: AxiosError<IApiError>) => {
                  handleError(axiosError);
                },
              );
            },
            handleCancel: () => setDisplayModal(false),
          });
          setDisplayModal(true);
        } else {
          await withUserOverride(
            (userOverrideCodes: UserOverrideCode[]) =>
              handleSubmit(values, formikHelpers, userOverrideCodes),
            [],
            (axiosError: AxiosError<IApiError>) => {
              handleError(axiosError);
            },
          );
        }
      }}
      loading={loading}
    ></View>
  );
});

export default UpdateManagementContainer;
