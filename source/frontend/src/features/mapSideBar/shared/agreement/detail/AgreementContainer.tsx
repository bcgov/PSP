import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { isValidId } from '@/utils';

import { useGenerateAgreement } from '../../../acquisition/common/GenerateForm/hooks/useGenerateAgreement';
import AcquisitionFileStatusUpdateSolver from '../../../acquisition/tabs/fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import DispositionStatusUpdateSolver from '../../../disposition/tabs/fileDetails/detail/DispositionStatusUpdateSolver';
import { IAgreementViewProps } from './AgreementView';

export interface IAgreementContainerProps {
  fileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IAgreementViewProps>>;
  getFile:
    | ReturnType<typeof useAcquisitionProvider>['getAcquisitionFile']
    | ReturnType<typeof useDispositionProvider>['getDispositionFile'];
  getProperties:
    | ReturnType<typeof useAcquisitionProvider>['getAcquisitionProperties']
    | ReturnType<typeof useDispositionProvider>['getDispositionProperties'];
  getAgreements:
    | ReturnType<typeof useAgreementProvider>['getAcquisitionAgreements']
    | ReturnType<typeof useAgreementProvider>['getDispositionFileAgreements'];
  deleteAgreement:
    | ReturnType<typeof useAgreementProvider>['deleteAcquisitionAgreement']
    | ReturnType<typeof useAgreementProvider>['deleteDispositionAgreement'];
  statusSolver: AcquisitionFileStatusUpdateSolver | DispositionStatusUpdateSolver;
  isAcquisition?: boolean;
}

export const AgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAgreementContainerProps>
> = ({
  fileId,
  View,
  getFile,
  getAgreements,
  getProperties,
  deleteAgreement,
  statusSolver,
  isAcquisition,
}) => {
  const [agreements, setAgreements] = useState<ApiGen_Concepts_Agreement[]>([]);
  const { setStaleLastUpdatedBy } = useContext(SideBarContext);

  const generateAgreement = useGenerateAgreement(getFile, getProperties, isAcquisition);
  const { execute: getFileExecute, loading: loadingFile } = getFile;
  const { execute: getAgreementsExecute, loading: loadingAgreements } = getAgreements;
  const { execute: deleteAgreementExecute, loading: deletingAgreement } = deleteAgreement;

  const { file, fileLoading } = useContext(SideBarContext);
  if (!isValidId(file?.id) && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  const fetchData = useCallback(async () => {
    const agreementsPromise = getAgreementsExecute(fileId);
    const filePromise = getFileExecute(fileId);
    const [agreementsResponse] = await Promise.all([agreementsPromise, filePromise]);
    if (agreementsResponse) {
      setAgreements(agreementsResponse);
    }
  }, [fileId, getFileExecute, getAgreementsExecute]);

  const handleAgreementDeleted = async (agreementId: number) => {
    if (isValidId(fileId)) {
      await deleteAgreementExecute(fileId, agreementId);
      setStaleLastUpdatedBy(true);
      const updatedAgreements = await getAgreementsExecute(fileId);
      if (updatedAgreements) {
        setAgreements(updatedAgreements);
      }
    }
  };

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return file?.id ? (
    <View
      loading={loadingAgreements || loadingFile || deletingAgreement}
      agreements={agreements}
      onGenerate={generateAgreement}
      onDelete={handleAgreementDeleted}
      isFileFinalStatus={!statusSolver.canEditOrDeleteAgreement()}
    />
  ) : null;
};

export default AgreementContainer;
