import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { useCallback, useEffect, useState } from 'react';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';

import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface ICompensationRequisitionDetailContainerProps {
  compensationRequisitionId?: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionDetailContainerProps>
> = ({ compensationRequisitionId, onClose, View }) => {
  const { getSystemConstant } = useSystemConstants();

  const [editMode, setEditMode] = useState(false);
  const clientConstant = getSystemConstant(SystemConstants.CLIENT);
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  // const [display, setDisplay] = useState(false);
  const {
    getCompensationRequisition: { execute: getCompensationRequisition, response, error, loading },
  } = useCompensationRequisitionRepository();

  const fetchCompensationReq = useCallback(async () => {
    if (!!compensationRequisitionId) {
      const compensationReq = await getCompensationRequisition(compensationRequisitionId);
      return compensationReq;
    }
  }, [compensationRequisitionId, getCompensationRequisition]);

  useEffect(() => {
    fetchCompensationReq();
  }, [compensationRequisitionId, getCompensationRequisition, fetchCompensationReq]);

  return !!compensationRequisitionId ? (
    <View
      compensation={response}
      clientConstant={clientConstant?.value ?? ''}
      gstConstant={gstDecimal}
      onClose={onClose}
      loading={loading}
      error={!!error}
      editMode={editMode}
      setEditMode={setEditMode}
    ></View>
  ) : null;
};
