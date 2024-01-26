import { useCallback, useEffect, useState } from 'react';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import {
  Api_DispositionFile,
  Api_DispositionFileAppraisal,
  Api_DispositionFileOffer,
} from '@/models/api/DispositionFile';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';

import { IOffersAndSaleContainerViewProps } from './OffersAndSaleContainerView';

export interface IOffersAndSaleContainerProps {
  dispositionFile?: Api_DispositionFile;
  View: React.FC<IOffersAndSaleContainerViewProps>;
  onSuccess: () => void;
}

const OffersAndSaleContainer: React.FunctionComponent<IOffersAndSaleContainerProps> = ({
  dispositionFile,
  View,
  onSuccess,
}) => {
  const {
    getDispositionFileOffers: { execute: getDispositionFileOffers, loading: loadingOffers },
    getDispositionFileSale: { execute: getDispositionFileSale, loading: loadingSale },
    getDispositionAppraisal: { execute: getDispositionAppraisal, loading: loadingAppraisal },
    deleteDispositionOffer: { execute: deleteDispositionOffer, loading: deletingOffer },
  } = useDispositionProvider();
  const [dispositionOffers, setDispositionOffers] = useState<Api_DispositionFileOffer[]>([]);
  const [dispositionSale, setDispositionSale] =
    useState<ApiGen_Concepts_DispositionFileSale | null>(null);
  const [dispositionAppraisal, setdispositionAppraisal] =
    useState<Api_DispositionFileAppraisal | null>(null);

  const fetchDispositionInformation = useCallback(async () => {
    if (dispositionFile?.id) {
      const dispositionOffersPromise = getDispositionFileOffers(dispositionFile?.id);
      const dispositionSalePromise = getDispositionFileSale(dispositionFile?.id);
      const dispositionAppraisalPromise = getDispositionAppraisal(dispositionFile?.id);

      const [offersResponse, saleResponse, appraisalResponse] = await Promise.all([
        dispositionOffersPromise,
        dispositionSalePromise,
        dispositionAppraisalPromise,
      ]);

      if (offersResponse) {
        setDispositionOffers(offersResponse);
      }

      if (saleResponse) {
        setDispositionSale(saleResponse ?? null);
      }

      setdispositionAppraisal(appraisalResponse ?? null);
    }
  }, [
    dispositionFile?.id,
    getDispositionAppraisal,
    getDispositionFileOffers,
    getDispositionFileSale,
  ]);

  const handleOfferDeleted = async (offerId: number) => {
    if (dispositionFile?.id) {
      await deleteDispositionOffer(dispositionFile?.id, offerId);
      var updatedOffers = await getDispositionFileOffers(dispositionFile?.id);
      if (updatedOffers) {
        setDispositionOffers(updatedOffers);
      }
      onSuccess();
    }
  };

  useEffect(() => {
    fetchDispositionInformation();
  }, [fetchDispositionInformation]);

  return dispositionFile ? (
    <View
      loading={loadingOffers || loadingSale || loadingAppraisal || deletingOffer}
      dispositionFile={dispositionFile}
      dispositionOffers={dispositionOffers}
      dispositionSale={dispositionSale}
      dispositionAppraisal={dispositionAppraisal}
      onDispositionOfferDeleted={handleOfferDeleted}
    ></View>
  ) : null;
};

export default OffersAndSaleContainer;
