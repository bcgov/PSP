import { AxiosError } from 'axios';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';

import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementView';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';

export interface IAddAcquisitionAgreementContainerProps {
  acquisitionFileId: number;
  View: React.FC<IUpdateAcquisitionAgreementViewProps>;
  onSuccess: () => void;
}

const AddAcquisitionAgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddAcquisitionAgreementContainerProps>
> = ({ acquisitionFileId, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const { setModalContent, setDisplayModal } = useModalContext();
  const initialValues = new AcquisitionAgreementFormModel(acquisitionFileId);

  const backUrl = location.pathname.split('/add')[0];

  const {
    addAcquisitionAgreement: { execute: postAcquisitionAgreement, loading },
  } = useAgreementProvider();

  const handleSave = async (newAgreement: ApiGen_Concepts_Agreement) => {
    return postAcquisitionAgreement(acquisitionFileId, newAgreement);
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

  return (
    initialValues && (
      <View
        initialValues={initialValues}
        isLoading={loading}
        onSave={handleSave}
        onSuccess={handleSuccess}
        onCancel={() => history.push(backUrl)}
        onError={onCreateError}
      ></View>
    )
  );
};

export default AddAcquisitionAgreementContainer;
