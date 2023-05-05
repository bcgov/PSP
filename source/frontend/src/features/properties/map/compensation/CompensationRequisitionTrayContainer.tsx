import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { Api_Compensation } from 'models/api/Compensation';
import { useCallback, useEffect, useState } from 'react';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';

import { CompensationRequisitionTrayViewProps } from './CompensationRequisitionTrayView';

export interface ICompensationRequisitionTrayContainerProps {
  compensationRequisitionId?: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionTrayViewProps>>;
}

export const CompensationRequisitionTrayContainer: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionTrayContainerProps>
> = ({ compensationRequisitionId, onClose, View }) => {
  const { getSystemConstant } = useSystemConstants();

  const [editMode, setEditMode] = useState(false);
  const [loadedCompensation, setLoadedCompensation] = useState<Api_Compensation | undefined>();

  const clientConstant = getSystemConstant(SystemConstants.CLIENT);
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  const {
    getCompensationRequisition: { execute: getCompensationRequisition, error, loading },
  } = useCompensationRequisitionRepository();

  const {
    getAcquisitionProject: {
      execute: getAcquisitionProject,
      response: acqFileProject,
      loading: loadingProject,
    },
    getAcquisitionProduct: {
      execute: getAcquisitionProduct,
      response: acqFileProduct,
      loading: loadingProduct,
    },
  } = useAcquisitionProvider();

  const fetchCompensationReq = useCallback(async () => {
    if (!!compensationRequisitionId) {
      const compensationReq = await getCompensationRequisition(compensationRequisitionId);
      if (compensationReq) {
        setLoadedCompensation(compensationReq);
        getAcquisitionProject(compensationReq.acquisitionFileId);
        getAcquisitionProduct(compensationReq.acquisitionFileId);
      }

      return compensationReq;
    }
  }, [
    compensationRequisitionId,
    getAcquisitionProduct,
    getAcquisitionProject,
    getCompensationRequisition,
  ]);

  useEffect(() => {
    fetchCompensationReq();
  }, [
    compensationRequisitionId,
    getCompensationRequisition,
    getAcquisitionProject,
    getAcquisitionProduct,
    fetchCompensationReq,
  ]);

  return loadedCompensation ? (
    <View
      compensation={loadedCompensation}
      acqFileProject={acqFileProject}
      acqFileProduct={acqFileProduct}
      clientConstant={clientConstant?.value ?? ''}
      gstConstant={gstDecimal}
      onClose={onClose}
      loading={loading || loadingProject || loadingProduct}
      error={!!error}
      editMode={editMode}
      setEditMode={setEditMode}
      onUpdate={() => {
        fetchCompensationReq();
      }}
    ></View>
  ) : null;
};
