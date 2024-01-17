import { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFileSale } from '@/models/api/DispositionFile';

import { IUpdateDispositionSaleViewProps } from './UpdateDispostionSaleView';

export interface IUpdateDispositionSaleContainerProps {
  dispositionFileId: number;
  View: React.FC<IUpdateDispositionSaleViewProps>;
}

const UpdateDispositionSaleContainer: React.FunctionComponent<
  React.PropsWithChildren<IUpdateDispositionSaleContainerProps>
> = ({ dispositionFileId, View }) => {
  const history = useHistory();
  const location = useLocation();
  const backUrl = location.pathname.split(`/sale/update`)[0];

  const initialValues = new DispositionSaleFormModel(null, dispositionFileId, null);
  const [dispositionSale, setDispositionSale] = useState<DispositionSaleFormModel>(initialValues);

  const {
    getDispositionFileSale: { execute: getDispositionSale, loading: loadingSale },
    postDispositionFileSale: { execute: postDispositionSale, loading: creatingSale },
    putDispositionFileSale: { execute: putDispositionSale, loading: updatingSale },
  } = useDispositionProvider();

  // generic error handler.
  const onError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 400) {
      toast.error(e?.response.data.error);
    } else {
      toast.error('Unable to save. Please try again.');
    }
  };

  const fetchSaleInformation = useCallback(async () => {
    const response = await getDispositionSale(dispositionFileId);

    if (response && response.id) {
      const saleModel = DispositionSaleFormModel.fromApi(response);
      setDispositionSale(saleModel);
    }
  }, [dispositionFileId, getDispositionSale]);

  const handleSucces = async () => {
    history.push(backUrl);
  };

  const handleSave = async (dispositionSale: Api_DispositionFileSale) => {
    if (dispositionSale.id) {
      return putDispositionSale(dispositionFileId, dispositionSale.id, dispositionSale);
    }
    return postDispositionSale(dispositionFileId, dispositionSale);
  };

  useEffect(() => {
    fetchSaleInformation();
  }, [fetchSaleInformation]);

  return (
    <View
      initialValues={dispositionSale}
      loading={loadingSale || creatingSale || updatingSale}
      onSave={handleSave}
      onSuccess={handleSucces}
      onCancel={() => history.push(backUrl)}
      onError={onError}
    ></View>
  );
};

export default UpdateDispositionSaleContainer;
