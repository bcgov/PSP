import axios, { AxiosError } from 'axios';
import { FormikHelpers } from 'formik/dist/types';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';

import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementForm';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';

export interface IUpdateAcquisitionAgreementContainerProps {
  acquisitionFileId: number;
  agreementId: number;
  View: React.FC<IUpdateAcquisitionAgreementViewProps>;
  onSuccess: () => void;
}

const UpdateAcquisitionAgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAcquisitionAgreementContainerProps>
> = ({ acquisitionFileId, agreementId, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const { setModalContent, setDisplayModal } = useModalContext();
  const [initialValues, setInitialValues] = useState<AcquisitionAgreementFormModel | null>(null);

  const backUrl = location.pathname.split(`/${agreementId}/update`)[0];

  const {
    updateAcquisitionAgreement: { execute: updateAcquisitionAgreement, loading: updatingAgreement },
    getAcquisitionAgreementById: { execute: getAgreement, loading: fetchingAgreement },
  } = useAgreementProvider();

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
    values: AcquisitionAgreementFormModel,
    formikHelpers: FormikHelpers<AcquisitionAgreementFormModel>,
  ) => {
    try {
      const agreementSaved = await updateAcquisitionAgreement(
        acquisitionFileId,
        agreementId,
        values.toApi(),
      );
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

  const fetchAcquisitionAgreement = useCallback(async () => {
    const agreement = await getAgreement(acquisitionFileId, agreementId);

    if (agreement) {
      const agreementFormModel = AcquisitionAgreementFormModel.fromApi(agreement);
      setInitialValues(agreementFormModel);
    }
  }, [acquisitionFileId, agreementId, getAgreement]);

  useEffect(() => {
    fetchAcquisitionAgreement();
  }, [fetchAcquisitionAgreement]);

  return (
    <View
      initialValues={initialValues}
      isLoading={fetchingAgreement || updatingAgreement}
      onSubmit={handleSubmit}
      onCancel={() => history.push(backUrl)}
    ></View>
  );
};

export default UpdateAcquisitionAgreementContainer;
