import { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { DispositionAppraisalFormModel } from '@/features/mapSideBar/disposition/models/DispositionAppraisalFormModel';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFileAppraisal } from '@/models/api/DispositionFile';

import { IDispositionAppraisalFormProps } from '../form/DispositionAppraisalForm';

export interface IUpdateDispositionAppraisalContainerProps {
  dispositionFileId: number;
  View: React.FC<IDispositionAppraisalFormProps>;
  onSuccess: () => void;
}

const UpdateDispositionAppraisalContainer: React.FunctionComponent<
  React.PropsWithChildren<IUpdateDispositionAppraisalContainerProps>
> = ({ dispositionFileId, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const backUrl = location.pathname.split(`/appraisal`)[0];
  const initialValues = new DispositionAppraisalFormModel(null, dispositionFileId, null);

  const [dispositionAppraisal, setdispositionAppraisal] =
    useState<DispositionAppraisalFormModel>(initialValues);
  const {
    getDispositionAppraisal: { execute: getDispositionAppraisal, loading: loadingAppraisal },
    postDispositionAppraisal: { execute: postDispositionAppraisal, loading: creatingAppraisal },
    putDispositionAppraisal: { execute: putDispositionAppraisal, loading: updatingAppraisal },
  } = useDispositionProvider();

  const fetchAppraisalInformation = useCallback(async () => {
    let dispositionAppraisalModel: DispositionAppraisalFormModel;
    const response = await getDispositionAppraisal(dispositionFileId);
    if (response != null) {
      dispositionAppraisalModel = DispositionAppraisalFormModel.fromApi(response);
    } else {
      dispositionAppraisalModel = new DispositionAppraisalFormModel(null, dispositionFileId, null);
    }

    setdispositionAppraisal(dispositionAppraisalModel);
  }, [dispositionFileId, getDispositionAppraisal]);

  const handleSave = async (appraisal: Api_DispositionFileAppraisal) => {
    if (dispositionAppraisal?.id) {
      return putDispositionAppraisal(dispositionFileId, dispositionAppraisal?.id, appraisal);
    } else {
      return postDispositionAppraisal(dispositionFileId, appraisal);
    }
  };

  const handleSuccess = async () => {
    onSuccess();
    history.push(backUrl);
  };

  // generic error handler.
  const onError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 400) {
      toast.error(e?.response.data.error);
    } else {
      toast.error('Unable to save. Please try again.');
    }
  };

  useEffect(() => {
    fetchAppraisalInformation();
  }, [fetchAppraisalInformation]);

  return (
    <View
      initialValues={dispositionAppraisal}
      loading={loadingAppraisal || creatingAppraisal || updatingAppraisal}
      onSave={handleSave}
      onSuccess={handleSuccess}
      onCancel={() => history.push(backUrl)}
      onError={onError}
    ></View>
  );
};

export default UpdateDispositionAppraisalContainer;
