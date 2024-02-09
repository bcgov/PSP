import { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFileOffer } from '@/models/api/DispositionFile';

import { IDispositionOfferFormProps } from '../form/DispositionOfferForm';
import { DispositionOfferFormModel } from '../models/DispositionOfferFormModel';

export interface IUpdateDispositionOfferContainerProps {
  dispositionFileId: number;
  dispositionOfferId: number;
  View: React.FC<IDispositionOfferFormProps>;
  onSuccess: () => void;
}

const UpdateDispositionOfferContainer: React.FunctionComponent<
  React.PropsWithChildren<IUpdateDispositionOfferContainerProps>
> = ({ dispositionFileId, dispositionOfferId, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const backUrl = location.pathname.split(`/offers/${dispositionOfferId}/update`)[0];

  const [offerStatusError, setOfferStatusError] = useState(false);
  const { setModalContent, setDisplayModal } = useModalContext();
  const [dispositionOffer, setDispositionOffer] = useState<DispositionOfferFormModel | null>(null);
  const {
    getDispositionOffer: { execute: getDispositionOffer, loading: loadingOffer },
    putDispositionOffer: { execute: putDispositionOffer, loading: updatingOffer },
  } = useDispositionProvider();

  const fetchOfferInformation = useCallback(async () => {
    const dispositionOffer = await getDispositionOffer(dispositionFileId, dispositionOfferId);

    if (dispositionOffer) {
      const offerModel = DispositionOfferFormModel.fromApi(dispositionOffer);
      setDispositionOffer(offerModel);
    }
  }, [dispositionFileId, dispositionOfferId, getDispositionOffer]);

  const handleSave = async (newOffer: Api_DispositionFileOffer) => {
    setOfferStatusError(false);
    return putDispositionOffer(dispositionFileId, dispositionOfferId, newOffer);
  };

  const handleSucces = async () => {
    onSuccess();
    history.push(backUrl);
  };

  const onUpdateError = (e: AxiosError<IApiError>) => {
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

  useEffect(() => {
    fetchOfferInformation();
  }, [fetchOfferInformation]);

  return (
    <View
      initialValues={dispositionOffer}
      showOfferStatusError={offerStatusError}
      loading={loadingOffer || updatingOffer}
      onSave={handleSave}
      onSuccess={handleSucces}
      onCancel={() => history.push(backUrl)}
      onError={onUpdateError}
    ></View>
  );
};

export default UpdateDispositionOfferContainer;
