import axios, { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { useGenerateH120 } from '@/features/properties/map/acquisition/common/GenerateForm/hooks/useGenerateH120';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { IApiError } from '@/interfaces/IApiError';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_CompensationPayee } from '@/models/api/CompensationPayee';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';

import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: Api_CompensationRequisition;
  acquisitionFile: Api_AcquisitionFile;
  clientConstant: string;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailContainerProps>
> = ({ compensation, setEditMode, View, clientConstant, acquisitionFile, loading }) => {
  const onGenerate = useGenerateH120();
  const [compensationPayee, setCompensationPayee] = useState<Api_CompensationPayee | undefined>();
  const {
    getCompensationRequisitionPayee: {
      execute: getCompensationRequisitionPayee,
      loading: loadingPayee,
    },
  } = useCompensationRequisitionRepository();

  const fetchCompensationPayee = useCallback(async () => {
    if (!!compensation.id) {
      try {
        const payee = await getCompensationRequisitionPayee(compensation.id);
        setCompensationPayee(payee);
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError.response?.status === 404) {
            setCompensationPayee(undefined);
          } else {
            toast.error(axiosError.response?.data.error);
          }
        }
      }
    }
  }, [compensation, getCompensationRequisitionPayee]);

  useEffect(() => {
    fetchCompensationPayee();
  }, [fetchCompensationPayee]);

  return compensation ? (
    <View
      loading={loading || loadingPayee}
      compensation={compensation}
      compensationPayee={compensationPayee}
      acqFileProject={acquisitionFile?.project}
      acqFileProduct={acquisitionFile?.product}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      onGenerate={onGenerate}
    ></View>
  ) : null;
};
