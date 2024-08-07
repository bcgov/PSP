import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { isValidId } from '@/utils';

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
  const { file, project, setProject, setProjectLoading, setStaleFile, setStaleLastUpdatedBy } =
    useContext(SideBarContext);

  const [editMode, setEditMode] = useState(false);
  const [show, setShow] = useState(true);

  const {
    getProject: { execute: getProject, loading: loadingProject },
  } = useProjectProvider();

  const [loadedCompensation, setLoadedCompensation] = useState<
    ApiGen_Concepts_CompensationRequisition | undefined
  >();

  const clientConstant = getSystemConstant(SystemConstants.CLIENT);
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  const {
    getCompensationRequisition: { execute: getCompensationRequisition, error, loading },
  } = useCompensationRequisitionRepository();

  const fetchCompensationReq = useCallback(async () => {
    if (isValidId(compensationRequisitionId)) {
      const compensationReq = await getCompensationRequisition(compensationRequisitionId);
      if (compensationReq) {
        setLoadedCompensation(compensationReq);
      }

      return compensationReq;
    }
  }, [compensationRequisitionId, getCompensationRequisition]);

  const fetchProject = useCallback(async () => {
    if (file?.projectId) {
      const retrieved = await getProject(file?.projectId);
      setProject(retrieved);
    }
  }, [file?.projectId, getProject, setProject]);

  useEffect(() => {
    fetchCompensationReq();
  }, [compensationRequisitionId, getCompensationRequisition, fetchCompensationReq]);

  useEffect(() => setProjectLoading(loadingProject), [loadingProject, setProjectLoading]);

  useEffect(() => {
    if (!project) {
      fetchProject();
    }
  }, [project, fetchProject]);

  return loadedCompensation ? (
    <View
      compensation={loadedCompensation}
      acquisitionFile={
        { ...file, fileChecklistItems: [] } as unknown as ApiGen_Concepts_AcquisitionFile
      }
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
        setStaleLastUpdatedBy(true);
        setStaleFile(true);
      }}
    ></View>
  ) : null;
};
