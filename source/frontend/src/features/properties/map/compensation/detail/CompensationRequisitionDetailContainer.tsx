import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { useCallback, useEffect, useState } from 'react';

import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface ICompensationRequisitionDetailContainerProps {
  compensationRequisitionId?: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionDetailContainerProps>
> = ({ compensationRequisitionId, onClose, View }) => {
  const [editMode, setEditMode] = useState(false);
  const [display, setDisplay] = useState(false);
  const {
    getCompensationRequisition: { execute: getCompensationRequisition, response, error, loading },
  } = useCompensationRequisitionRepository();

  const fetchCompensationReq = useCallback(async () => {
    console.log(compensationRequisitionId);
    if (!!compensationRequisitionId) {
      const activity = await getCompensationRequisition(compensationRequisitionId);
      return activity;
    }
  }, [compensationRequisitionId, getCompensationRequisition]);

  useEffect(() => {
    fetchCompensationReq();
  }, [compensationRequisitionId, getCompensationRequisition, fetchCompensationReq]);

  return !!compensationRequisitionId ? (
    <View
      onClose={onClose}
      loading={loading}
      error={!!error}
      compensation={response}
      editMode={editMode}
      setEditMode={setEditMode}
    ></View>
  ) : null;
};
