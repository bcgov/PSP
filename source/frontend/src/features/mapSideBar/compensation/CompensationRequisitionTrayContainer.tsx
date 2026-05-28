import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists, isValidId } from '@/utils';

import { CompensationRequisitionTrayViewProps } from './CompensationRequisitionTrayView';

export interface ICompensationRequisitionTrayContainerProps {
  fileType: ApiGen_CodeTypes_FileTypes;
  compensationRequisitionId?: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionTrayViewProps>>;
}

export const CompensationRequisitionTrayContainer: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionTrayContainerProps>
> = ({ compensationRequisitionId, fileType, onClose, View }) => {
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

  const fetchCompensationReq = useCallback(
    async (compensationRequisitionId: number) => {
      const compensationReq = await getCompensationRequisition(compensationRequisitionId);
      if (exists(compensationReq)) {
        if (isValidId(compensationReq.alternateProjectId)) {
          compensationReq.alternateProject = await getProject(compensationReq.alternateProjectId);
        }
        setLoadedCompensation(compensationReq);
      }

      return compensationReq;
    },
    [getCompensationRequisition, getProject],
  );

  const fetchProject = useCallback(async () => {
    if (file?.projectId) {
      const retrieved = await getProject(file?.projectId);
      setProject(retrieved);
    }
  }, [file?.projectId, getProject, setProject]);

  useEffect(() => {
    if (isValidId(compensationRequisitionId)) {
      fetchCompensationReq(compensationRequisitionId);
    }
  }, [compensationRequisitionId, fetchCompensationReq]);

  useEffect(() => setProjectLoading(loadingProject), [loadingProject, setProjectLoading]);

  useEffect(() => {
    if (!project) {
      fetchProject();
    }
  }, [project, fetchProject]);

  const castFile = (): ApiGen_Concepts_AcquisitionFile | ApiGen_Concepts_Lease => {
    switch (fileType) {
      case ApiGen_CodeTypes_FileTypes.Acquisition:
        return file as unknown as ApiGen_Concepts_AcquisitionFile;
      case ApiGen_CodeTypes_FileTypes.Lease:
        return file as unknown as ApiGen_Concepts_AcquisitionFile;
    }
  };

  return exists(loadedCompensation) ? (
    <View
      compensation={loadedCompensation}
      fileType={fileType}
      file={castFile()}
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
        fetchCompensationReq(loadedCompensation.id);
        setStaleLastUpdatedBy(true);
        setStaleFile(true);
      }}
    />
  ) : null;
};
