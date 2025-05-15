import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import React from 'react';

import { ModalSize } from '@/components/common/GenericModal';
import * as API from '@/constants/API';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_DispositionFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_DispositionFileStatusTypes';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import { DispositionFormModel } from '../../../../models/DispositionFormModel';
import DispositionStatusUpdateSolver from '../DispositionStatusUpdateSolver';
import { IUpdateDispositionFormProps } from './UpdateDispositionForm';

export interface IUpdateDispositionContainerProps {
  dispositionFile: ApiGen_Concepts_DispositionFile;
  onSuccess: (updateProperties?: boolean, updateFile?: boolean) => void;
  View: React.FC<IUpdateDispositionFormProps>;
}

export const UpdateDispositionContainer = React.forwardRef<
  FormikProps<DispositionFormModel>,
  IUpdateDispositionContainerProps
>((props, formikRef) => {
  const { dispositionFile, onSuccess, View } = props;
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getByType } = useLookupCodeHelpers();

  const dispositionStatusTypes = getByType(API.DISPOSITION_FILE_STATUS_TYPES);

  const {
    putDispositionFile: { execute: updateDispositionFile, loading },
  } = useDispositionProvider();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_DispositionFile | void>
  >('Failed to update Disposition File');

  const handleSubmit = async (
    values: DispositionFormModel,
    formikHelpers: FormikHelpers<DispositionFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const dispositionFile = values.toApi();
      if (dispositionFile.id) {
        const response = await updateDispositionFile(
          dispositionFile.id,
          dispositionFile,
          userOverrideCodes,
        );

        if (isValidId(response?.id)) {
          formikHelpers?.resetForm();
          const activeTypeCodes = [
            ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE.toString(),
            ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT.toString(),
            ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD.toString(),
          ];
          //refresh the map properties if this disposition file was set to a final state.
          onSuccess(
            !!dispositionFile.fileStatusTypeCode?.id &&
              !activeTypeCodes.includes(dispositionFile.fileStatusTypeCode.id),
            true,
          );
        }
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const RemoveSelfContractorModalContent = (): React.ReactNode => {
    return (
      <>
        <p>
          Contractors cannot remove themselves from a Disposition file. <br />
          Please contact the admin at <a href="mailto: pims@gov.bc.ca">pims@gov.bc.ca</a>
        </p>
      </>
    );
  };

  const handleError = async (axiosError: AxiosError<IApiError>): Promise<void> => {
    switch (axiosError?.response?.data.type) {
      case 'ContractorNotInTeamException':
        setModalContent({
          variant: 'error',
          title: 'Error',
          modalSize: ModalSize.LARGE,
          message: RemoveSelfContractorModalContent(),
          okButtonText: 'Close',
        });
        setDisplayModal(true);
        break;
      default: {
        setModalContent({
          variant: 'error',
          title: 'Error',
          message: axiosError?.response?.data.error,
          okButtonText: 'Close',
        });
        setDisplayModal(true);
      }
    }
  };

  if (!dispositionFile) {
    return null;
  }

  return (
    <View
      formikRef={formikRef}
      initialValues={DispositionFormModel.fromApi(dispositionFile)}
      onSubmit={async (
        values: DispositionFormModel,
        formikHelpers: FormikHelpers<DispositionFormModel>,
      ) => {
        const updatedFile = values.toApi();
        const currentFileSolver = new DispositionStatusUpdateSolver(dispositionFile);
        const updatedFileSolver = new DispositionStatusUpdateSolver(updatedFile);

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

export default UpdateDispositionContainer;
