import { useCallback, useEffect, useState } from 'react';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import {
  Api_DispositionFile,
  Api_DispositionFileOffer,
  Api_DispositionFileSale,
} from '@/models/api/DispositionFile';

import { IOffersAndSaleContainerViewProps } from './OffersAndSaleContainerView';

export interface IOffersAndSaleContainerProps {
  dispositionFile?: Api_DispositionFile;
  View: React.FC<IOffersAndSaleContainerViewProps>;
}

const OffersAndSaleContainer: React.FunctionComponent<IOffersAndSaleContainerProps> = ({
  dispositionFile,
  View,
}) => {
  const {
    getDispositionFileOffers: { execute: getDispositionFileOffers, loading: loadingOffers },
    getDispositionFileSale: { execute: getDispositionFileSale, loading: loadingSale },
    deleteDispositionOffer: { execute: deleteDispositionOffer, loading: deletingOffer },
  } = useDispositionProvider();
  const [dispositionOffers, setDispositionOffers] = useState<Api_DispositionFileOffer[]>([]);
  const [dispositionSale, setDispositionSale] = useState<Api_DispositionFileSale | null>(null);

  const fetchDispositionInformation = useCallback(async () => {
    if (dispositionFile?.id) {
      const dispositionOffersPromise = getDispositionFileOffers(dispositionFile?.id);
      const dispositionSalePromise = getDispositionFileSale(dispositionFile?.id);

      const [offersResponse, saleResponse] = await Promise.all([
        dispositionOffersPromise,
        dispositionSalePromise,
      ]);

      if (offersResponse) {
        setDispositionOffers(offersResponse);
      }

      if (saleResponse) {
        setDispositionSale(saleResponse ?? null);
      }
    }
  }, [dispositionFile?.id, getDispositionFileOffers, getDispositionFileSale]);

  const handleOfferDeleted = async (offerId: number) => {
    if (dispositionFile?.id) {
      await deleteDispositionOffer(dispositionFile?.id, offerId);
      var updatedOffers = await getDispositionFileOffers(dispositionFile?.id);
      if (updatedOffers) {
        setDispositionOffers(updatedOffers);
      }
    }
  };

  useEffect(() => {
    fetchDispositionInformation();
  }, [fetchDispositionInformation]);

  return dispositionFile ? (
    <View
      loading={loadingOffers || loadingSale || deletingOffer}
      dispositionFile={dispositionFile}
      dispositionOffers={dispositionOffers}
      dispositionSale={dispositionSale}
      onDispositionOfferDeleted={handleOfferDeleted}
    ></View>
  ) : null;
};

export default OffersAndSaleContainer;
