import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import React from 'react';

import { ModalSize } from '@/components/common/GenericModal';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import { DispositionFormModel } from '../../../../models/DispositionFormModel';
import { IUpdateDispositionFormProps } from './UpdateDispositionForm';

export interface IUpdateDispositionContainerProps {
  dispositionFile: ApiGen_Concepts_DispositionFile;
  onSuccess: () => void;
  View: React.FC<IUpdateDispositionFormProps>;
}

export const UpdateDispositionContainer = React.forwardRef<
  FormikProps<DispositionFormModel>,
  IUpdateDispositionContainerProps
>((props, formikRef) => {
  const { dispositionFile, onSuccess, View } = props;
  const { setModalContent, setDisplayModal } = useModalContext();

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
          onSuccess();
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
          variant: 'warning',
          title: 'Warning',
          message: axiosError?.response?.data.error,
          okButtonText: 'Close',
        });
        setDisplayModal(true);
      }
    }
  };

  return (
    <View
      formikRef={formikRef}
      initialValues={DispositionFormModel.fromApi(dispositionFile)}
      onSubmit={(
        values: DispositionFormModel,
        formikHelpers: FormikHelpers<DispositionFormModel>,
      ) =>
        withUserOverride(
          (userOverrideCodes: UserOverrideCode[]) =>
            handleSubmit(values, formikHelpers, userOverrideCodes),
          [],
          (axiosError: AxiosError<IApiError>) => {
            handleError(axiosError);
          },
        )
      }
      loading={loading}
    ></View>
  );
});

export default UpdateDispositionContainer;
