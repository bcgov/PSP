import { useCallback, useEffect, useState } from 'react';

import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_CompensationPayee } from '@/models/api/CompensationPayee';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';

import { useGenerateH120 } from '../../../../mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH120';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: Api_CompensationRequisition;
  acquisitionFile: Api_AcquisitionFile;
  clientConstant: string;
  gstConstant: number;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailContainerProps>
> = ({
  compensation,
  setEditMode,
  View,
  clientConstant,
  acquisitionFile,
  gstConstant,
  loading,
}) => {
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
      const payee = await getCompensationRequisitionPayee(compensation.id);
      if (payee) {
        setCompensationPayee(payee);
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
      gstConstant={gstConstant}
      onGenerate={onGenerate}
    ></View>
  ) : null;
};
