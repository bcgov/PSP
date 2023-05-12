import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Compensation } from 'models/api/Compensation';
import { useCallback, useContext, useEffect, useState } from 'react';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';

import { SideBarContext } from '../context/sidebarContext';
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
  const { file } = useContext(SideBarContext);

  const [editMode, setEditMode] = useState(false);
  const [show, setShow] = useState(true);

  const [loadedCompensation, setLoadedCompensation] = useState<Api_Compensation | undefined>();

  const clientConstant = getSystemConstant(SystemConstants.CLIENT);
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  const {
    getCompensationRequisition: { execute: getCompensationRequisition, error, loading },
  } = useCompensationRequisitionRepository();

  const fetchCompensationReq = useCallback(async () => {
    if (!!compensationRequisitionId) {
      const compensationReq = await getCompensationRequisition(compensationRequisitionId);
      if (compensationReq) {
        setLoadedCompensation(compensationReq);
      }

      return compensationReq;
    }
  }, [compensationRequisitionId, getCompensationRequisition]);

  useEffect(() => {
    fetchCompensationReq();
  }, [compensationRequisitionId, getCompensationRequisition, fetchCompensationReq]);

  return loadedCompensation ? (
    <View
      compensation={loadedCompensation}
      acquistionFile={file as Api_AcquisitionFile}
      clientConstant={clientConstant?.value ?? ''}
      gstConstant={gstDecimal}
      onClose={onClose}
      loading={loading}
      error={!!error}
      editMode={editMode}
      setEditMode={setEditMode}
      show={show}
      setShow={setShow}
      onUpdate={() => {
        fetchCompensationReq();
      }}
    ></View>
  ) : null;
};
