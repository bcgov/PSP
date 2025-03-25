import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { FaExclamationCircle } from 'react-icons/fa';
import { toast } from 'react-toastify';

import * as API from '@/constants/API';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import useApiUserOverride, { CustomModalOptions } from '@/hooks/useApiUserOverride';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import AcquisitionFileStatusUpdateSolver from '../detail/AcquisitionFileStatusUpdateSolver';
import { UpdateAcquisitionSummaryFormModel } from './models';
import { UpdateAcquisitionFileYupSchema } from './UpdateAcquisitionFileYupSchema';
import { IUpdateAcquisitionFormProps } from './UpdateAcquisitionForm';

export interface IUpdateAcquisitionContainerProps {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  onSuccess: () => void;
  View: React.FC<IUpdateAcquisitionFormProps>;
}

export const RemoveSelfContractorContent = (): React.ReactNode => {
  return (
    <>
      <p>
        Contractors cannot remove themselves from a file. Please contact the admin at{' '}
        <a href="mailto: pims@gov.bc.ca">pims@gov.bc.ca</a>
      </p>
    </>
  );
};

export const UpdateAcquisitionContainer = React.forwardRef<
  FormikProps<UpdateAcquisitionSummaryFormModel>,
  IUpdateAcquisitionContainerProps
>((props, formikRef) => {
  const { getByType } = useLookupCodeHelpers();

  const { acquisitionFile, onSuccess, View } = props;
  const { setModalContent, setDisplayModal } = useModalContext();
  const fileStatusTypeCodes = getByType(API.ACQUISITION_FILE_STATUS_TYPES);

  const {
    updateAcquisitionFile: { execute: updateAcquisitionFile },
  } = useAcquisitionProvider();

  // Customize the look and feel of the user-override modal. For region change confirmation, the modal should be of type "info" (blue)
  const customModalOptions = new Map<UserOverrideCode, CustomModalOptions>([
    [UserOverrideCode.UPDATE_REGION, { variant: 'info', title: 'Different Ministry region' }],
    [
      UserOverrideCode.UPDATE_SUBFILES_PROJECT_PRODUCT,
      { variant: 'info', title: 'Different Project or Product' },
    ],
  ]);

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_AcquisitionFile | void>
  >('Failed to update Acquisition File', { modalOptions: customModalOptions });

  const handleSubmit = async (
    acquisitionFile: ApiGen_Concepts_AcquisitionFile,
    formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const response = await updateAcquisitionFile(acquisitionFile, userOverrideCodes);
      if (isValidId(response?.id)) {
        if (acquisitionFile.fileProperties?.find(ap => !ap.property?.address && !ap.property?.id)) {
          toast.warn(
            'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
            { autoClose: 15000 },
          );
        }
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <StyledFormWrapper>
      <View
        formikRef={formikRef}
        initialValues={UpdateAcquisitionSummaryFormModel.fromApi(acquisitionFile)}
        onSubmit={async (
          values: UpdateAcquisitionSummaryFormModel,
          formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
        ) => {
          const updatedFile = values.toApi();

          const currentFileSolver = new AcquisitionFileStatusUpdateSolver(
            acquisitionFile.fileStatusTypeCode,
          );
          const updatedFileSolver = new AcquisitionFileStatusUpdateSolver(
            updatedFile.fileStatusTypeCode,
          );

          if (!currentFileSolver.isAdminProtected() && updatedFileSolver.isAdminProtected()) {
            const statusCode = fileStatusTypeCodes.find(x => x.id === values.fileStatusTypeCode);
            setModalContent({
              variant: 'info',
              title: 'Confirm status change',
              message: (
                <>
                  <p>
                    You marked this file as {statusCode.name}. If you save it, only the
                    administrator can turn it back on. You will still see it in the management
                    table.
                  </p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: async () => {
                withUserOverride(
                  (userOverrideCodes: UserOverrideCode[]) =>
                    handleSubmit(updatedFile, formikHelpers, userOverrideCodes),
                  [],
                  (axiosError: AxiosError<IApiError>) => {
                    setModalContent({
                      variant: 'error',
                      title: 'Error',
                      headerIcon: <FaExclamationCircle size={22} />,
                      message: axiosError?.response?.data.error,
                      okButtonText: 'Close',
                    });
                    setDisplayModal(true);
                  },
                );
              },
              handleCancel: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          } else {
            await withUserOverride(
              (userOverrideCodes: UserOverrideCode[]) =>
                handleSubmit(updatedFile, formikHelpers, userOverrideCodes),
              [],
              (axiosError: AxiosError<IApiError>) => {
                setModalContent({
                  variant: 'error',
                  title: 'Error',
                  headerIcon: <FaExclamationCircle size={22} />,
                  message: axiosError?.response?.data.error,
                  okButtonText: 'Close',
                });
                setDisplayModal(true);
              },
            );
          }
        }}
        validationSchema={UpdateAcquisitionFileYupSchema}
      />
    </StyledFormWrapper>
  );
});

export default UpdateAcquisitionContainer;
