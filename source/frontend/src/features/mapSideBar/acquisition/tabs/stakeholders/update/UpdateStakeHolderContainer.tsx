import axios, { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useEffect } from 'react';
import { FaExclamationCircle } from 'react-icons/fa';
import { toast } from 'react-toastify';

import { InterestHolderType } from '@/constants/interestHolderTypes';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

import { StakeHolderForm } from './models';
import { IUpdateStakeHolderFormProps } from './UpdateStakeHolderForm';

export interface IUpdateStakeHolderContainerProps {
  View: React.FC<IUpdateStakeHolderFormProps>;
  formikRef: React.Ref<FormikProps<StakeHolderForm>>;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  onSuccess: () => void;
}

export const UpdateStakeHolderContainer: React.FunctionComponent<
  IUpdateStakeHolderContainerProps
> = ({ View, formikRef, acquisitionFile, onSuccess }) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    getAcquisitionInterestHolders: {
      execute: getInterestHolders,
      response: apiInterestHolders,
      loading,
    },
  } = useInterestHolderRepository();

  useEffect(() => {
    if (acquisitionFile?.id) {
      getInterestHolders(acquisitionFile.id);
    }
  }, [acquisitionFile.id, getInterestHolders]);

  const {
    updateAcquisitionInterestHolders: { execute: updateInterestHolders },
  } = useInterestHolderRepository();

  const saveInterestHolders = async (
    interestHolders: StakeHolderForm,
    formikHelpers: FormikHelpers<StakeHolderForm>,
  ) => {
    try {
      if (acquisitionFile?.id) {
        const stakeholders = StakeHolderForm.toApi(interestHolders);
        // Add the other non interest holder contacts
        const otherInterestHolders =
          apiInterestHolders?.filter(
            x => x.interestHolderType?.id !== InterestHolderType.INTEREST_HOLDER,
          ) || [];

        const result = await updateInterestHolders(
          acquisitionFile?.id,
          stakeholders.concat(otherInterestHolders),
        );
        if (result !== undefined) {
          onSuccess();
        }
        return result;
      }
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError?.response?.status === 409) {
          toast.error(axiosError?.response.data.error);
          formikHelpers.resetForm();
        } else {
          if (axiosError.response?.status === 400) {
            setModalContent({
              variant: 'error',
              title: 'Error',
              headerIcon: <FaExclamationCircle size={22} />,
              message: axiosError?.response?.data.error,
              okButtonText: 'Close',
              handleCancel: () => formikHelpers.resetForm(),
            });
            setDisplayModal(true);
          } else {
            toast.error('Unable to save. Please try again.');
          }
        }
      }
    } finally {
      formikHelpers.setSubmitting(false);
    }
  };

  return (
    <View
      formikRef={formikRef}
      file={acquisitionFile}
      onSubmit={saveInterestHolders}
      loading={loading}
      interestHolders={StakeHolderForm.fromApi(apiInterestHolders ?? [])}
    ></View>
  );
};

export default UpdateStakeHolderContainer;
