import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { isValidId } from '@/utils';

import { useGenerateAgreement } from '../../../acquisition/common/GenerateForm/hooks/useGenerateAgreement';
import AcquisitionFileStatusUpdateSolver from '../../../acquisition/tabs/fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import DispositionStatusUpdateSolver from '../../../disposition/tabs/fileDetails/detail/DispositionStatusUpdateSolver';
import { IAgreementViewProps } from './AgreementView';

export interface IAgreementContainerProps {
  fileId: number;
  fileType: 'acquisition' | 'disposition';
  View: React.FunctionComponent<React.PropsWithChildren<IAgreementViewProps>>;
}

export const AgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAgreementContainerProps>
> = ({ fileId, fileType, View }) => {
  const [agreements, setAgreements] = useState<ApiGen_Concepts_Agreement[]>([]);
  const { setStaleLastUpdatedBy } = useContext(SideBarContext);

  // Providers
  const agreementProvider = useAgreementProvider();
  const acquisitionProvider = useAcquisitionProvider();
  const dispositionProvider = useDispositionProvider();

  // Dynamic provider selection
  const getFile =
    fileType === 'acquisition'
      ? acquisitionProvider.getAcquisitionFile
      : dispositionProvider.getDispositionFile;
  const getAgreements =
    fileType === 'acquisition'
      ? agreementProvider.getAcquisitionAgreements
      : agreementProvider.getDispositionFileAgreements;
  const deleteAgreement =
    fileType === 'acquisition'
      ? agreementProvider.deleteAcquisitionAgreement
      : agreementProvider.deleteDispositionAgreement;

  const generateAgreement = useGenerateAgreement();

  const { execute: getFileExecute, loading: loadingFile, response: fileData } = getFile;
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

  const statusSolver =
    fileType === 'acquisition'
      ? new AcquisitionFileStatusUpdateSolver(fileData?.fileStatusTypeCode)
      : new DispositionStatusUpdateSolver(fileData as ApiGen_Concepts_DispositionFile);

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
