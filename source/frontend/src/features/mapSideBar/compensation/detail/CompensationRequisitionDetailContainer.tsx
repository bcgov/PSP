import { useCallback, useEffect, useState } from 'react';

import { LeaseStatusUpdateSolver } from '@/features/leases/models/LeaseStatusUpdateSolver';
import { useGenerateH120 } from '@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH120';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { isValidId } from '@/utils';

import AcquisitionFileStatusUpdateSolver from '../../acquisition/tabs/fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: ApiGen_Concepts_CompensationRequisition;
  fileType: ApiGen_CodeTypes_FileTypes;
  file: ApiGen_Concepts_AcquisitionFile | ApiGen_Concepts_Lease;
  clientConstant: string;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<CompensationRequisitionDetailViewProps>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  CompensationRequisitionDetailContainerProps
> = ({ compensation, setEditMode, View, clientConstant, fileType, file, loading }) => {
  const onGenerate = useGenerateH120();

  const [compensationRequisitionProperties, setCompensationRequisitionProperties] = useState<
    ApiGen_Concepts_FileProperty[]
  >([]);

  const statusSolver =
    fileType === ApiGen_CodeTypes_FileTypes.Acquisition
      ? new AcquisitionFileStatusUpdateSolver(file?.fileStatusTypeCode)
      : new LeaseStatusUpdateSolver(file?.fileStatusTypeCode);

  const {
    getCompensationRequisitionProperties: {
      execute: getCompensationProperties,
      loading: loadingCompReqProperties,
    },
    getCompensationRequisitionAcqPayees: {
      execute: getCompensationAcqPayees,
      loading: loadingCompReqAcqPayees,
      response: compReqAcqPayees,
    },
    getCompensationRequisitionLeasePayees: {
      execute: getCompensationLeasePayees,
      loading: loadingCompReqLeasePayees,
      response: compReqLeasePayees,
    },
  } = useCompensationRequisitionRepository();

  const fetchCompensationProperties = useCallback(async () => {
    if (isValidId(compensation.id)) {
      const compReqProperties = await getCompensationProperties(fileType, compensation.id);
      setCompensationRequisitionProperties(compReqProperties);
    }
  }, [compensation.id, fileType, getCompensationProperties]);

  const fetchCompensationPayees = useCallback(async () => {
    if (isValidId(compensation.id) && fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
      await getCompensationAcqPayees(compensation.id);
    } else if (isValidId(compensation.id) && fileType === ApiGen_CodeTypes_FileTypes.Lease) {
      await getCompensationLeasePayees(compensation.id);
    }
  }, [compensation.id, fileType, getCompensationAcqPayees, getCompensationLeasePayees]);

  useEffect(() => {
    fetchCompensationProperties();
  }, [fetchCompensationProperties]);

  useEffect(() => {
    fetchCompensationPayees();
  }, [fetchCompensationPayees]);

  return compensation ? (
    <View
      fileType={fileType}
      loading={
        loading || loadingCompReqProperties || loadingCompReqAcqPayees || loadingCompReqLeasePayees
      }
      fileProduct={file.product}
      fileProject={file.project}
      compensation={compensation}
      compensationProperties={compensationRequisitionProperties}
      compensationAcqPayees={compReqAcqPayees}
      compensationLeasePayees={compReqLeasePayees}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      onGenerate={onGenerate}
      isFileFinalStatus={!statusSolver?.canEditOrDeleteCompensation()}
    />
  ) : null;
};
