import { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';

import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementView';
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

  const handleSave = async (updatedAgreement: ApiGen_Concepts_Agreement) => {
    return updateAcquisitionAgreement(acquisitionFileId, agreementId, updatedAgreement);
  };

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
      onSave={handleSave}
      onSuccess={handleSuccess}
      onCancel={() => history.push(backUrl)}
      onError={onCreateError}
    ></View>
  );
};

export default UpdateAcquisitionAgreementContainer;
