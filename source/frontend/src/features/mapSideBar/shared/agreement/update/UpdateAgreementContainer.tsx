import axios, { AxiosError } from 'axios';
import { FormikHelpers } from 'formik/dist/types';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';

import { IUpdateAgreementFormProps } from '../common/UpdateAgreementForm';
import { AgreementFormModel } from '../models/AgreementFormModel';

export interface IUpdateAcquisitionAgreementContainerProps {
  fileId: number;
  agreementId: number;
  fileType: 'acquisition' | 'disposition';
  View: React.FC<IUpdateAgreementFormProps>;
  onSuccess: () => void;
}

const UpdateAgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAcquisitionAgreementContainerProps>
> = ({ fileId, agreementId, fileType, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const { setModalContent, setDisplayModal } = useModalContext();
  const [initialValues, setInitialValues] = useState<AgreementFormModel | null>(null);

  const backUrl = location.pathname.split(`/${agreementId}/update`)[0];

  const agreementProvider = useAgreementProvider();

  // Select correct hooks based on fileType
  const updateAgreement =
    fileType === 'acquisition'
      ? agreementProvider.updateAcquisitionAgreement.execute
      : agreementProvider.updateDispositionAgreement.execute;
  const getAgreement =
    fileType === 'acquisition'
      ? agreementProvider.getAcquisitionAgreementById.execute
      : agreementProvider.getDispositionAgreementById.execute;
  const updatingAgreement =
    fileType === 'acquisition'
      ? agreementProvider.updateAcquisitionAgreement.loading
      : agreementProvider.updateDispositionAgreement.loading;
  const fetchingAgreement =
    fileType === 'acquisition'
      ? agreementProvider.getAcquisitionAgreementById.loading
      : agreementProvider.getDispositionAgreementById.loading;

  const handleSuccess = async () => {
    onSuccess();
    history.push(backUrl);
  };

  const onCreateError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 409) {
      setModalContent({
        variant: 'error',
        title: 'Error',
        message: 'You can only have one offer with status of "Accepted".',
        okButtonText: 'Close',
        handleOk: () => setDisplayModal(false),
      });
      setDisplayModal(true);
    } else {
      if (e?.response?.status === 400) {
        toast.error(e?.response.data.error);
      } else {
        toast.error('Unable to save. Please try again.');
      }
    }
  };

  const handleSubmit = async (
    values: AgreementFormModel,
    formikHelpers: FormikHelpers<AgreementFormModel>,
  ) => {
    try {
      const agreementSaved = await updateAgreement(fileId, agreementId, values.toApi());
      if (agreementSaved) {
        handleSuccess();
      }
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        onCreateError && onCreateError(axiosError);
      }
    } finally {
      formikHelpers.setSubmitting(false);
    }
  };

  const fetchAgreement = useCallback(async () => {
    const agreement = await getAgreement(fileId, agreementId);
    if (agreement) {
      const agreementFormModel = AgreementFormModel.fromApi(agreement);
      setInitialValues(agreementFormModel);
    }
  }, [fileId, agreementId, getAgreement]);

  useEffect(() => {
    fetchAgreement();
  }, [fetchAgreement]);

  return (
    <View
      initialValues={initialValues}
      isLoading={fetchingAgreement || updatingAgreement}
      onSubmit={handleSubmit}
      onCancel={() => history.push(backUrl)}
      fileType={fileType}
    ></View>
  );
};

export default UpdateAgreementContainer;
