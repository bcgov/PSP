import { AxiosError } from 'axios';
import { useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFileOffer } from '@/models/api/DispositionFile';

import { IDispositionOfferFormProps } from '../form/DispositionOfferForm';
import { DispositionOfferFormModel } from '../models/DispositionOfferFormModel';

export interface IAddDispositionOfferContainerProps {
  dispositionFileId: number;
  View: React.FC<IDispositionOfferFormProps>;
  onSuccess: () => void;
}

const AddDispositionOfferContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddDispositionOfferContainerProps>
> = ({ dispositionFileId, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();

  const { setModalContent, setDisplayModal } = useModalContext();
  const [offerStatusError, setOfferStatusError] = useState(false);

  const backUrl = location.pathname.split('/offers/new')[0];
  const {
    postDispositionFileOffer: { execute: postDispositionOffer, loading },
  } = useDispositionProvider();
  const initialValues = new DispositionOfferFormModel(null, dispositionFileId);

  const handleSave = async (newOffer: Api_DispositionFileOffer) => {
    setOfferStatusError(false);
    return postDispositionOffer(dispositionFileId, newOffer);
  };

  const handleSuccess = async () => {
    onSuccess();
    history.push(backUrl);
  };

  const onCreateError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 409) {
      setOfferStatusError(true);
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
    <View
      initialValues={initialValues}
      showOfferStatusError={offerStatusError}
      loading={loading}
      onSave={handleSave}
      onSuccess={handleSuccess}
      onCancel={() => history.push(backUrl)}
      onError={onCreateError}
    ></View>
  );
};

export default AddDispositionOfferContainer;
