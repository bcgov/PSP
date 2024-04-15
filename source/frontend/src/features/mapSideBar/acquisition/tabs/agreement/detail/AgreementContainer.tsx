import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { isValidId } from '@/utils';

import { useGenerateAgreement } from '../../../common/GenerateForm/hooks/useGenerateAgreement';
import StatusUpdateSolver from '../../fileDetails/detail/statusUpdateSolver';
import { IAgreementViewProps } from './AgreementView';

export interface IAgreementContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IAgreementViewProps>>;
}

export const AgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAgreementContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [statusUpdateSolver, setStatusUpdateSolver] = useState<StatusUpdateSolver>(
    new StatusUpdateSolver(),
  );
  const [acquisitionAgreements, setAcquisitionAgreements] = useState<ApiGen_Concepts_Agreement[]>(
    [],
  );

  const {
    getAcquisitionAgreements: { execute: getAgreements, loading: loadingAgreements },
    deleteAcquisitionAgreement: { execute: deleteAgreement, loading: deletingAgreement },
  } = useAgreementProvider();

  const {
    getAcquisitionFile: { execute: getAcquisition, loading: loadingAcquisition },
  } = useAcquisitionProvider();

  const generateAgreement = useGenerateAgreement();

  const { file, fileLoading } = useContext(SideBarContext);
  if (!isValidId(file?.id) && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  const fetchData = useCallback(async () => {
    const agreementsPromise = getAgreements(acquisitionFileId);
    const acquisitionFilePromise = getAcquisition(acquisitionFileId);

    const [agreementsResponse, acquisitionFileResponse] = await Promise.all([
      agreementsPromise,
      acquisitionFilePromise,
    ]);

    if (agreementsResponse) {
      setAcquisitionAgreements(agreementsResponse);
    }

    if (acquisitionFileResponse) {
      const solverStatus = new StatusUpdateSolver(acquisitionFileResponse);
      setStatusUpdateSolver(solverStatus);
    }
  }, [acquisitionFileId, getAcquisition, getAgreements]);

  const handleAgreementDeleted = async (agreementId: number) => {
    if (isValidId(acquisitionFileId)) {
      await deleteAgreement(acquisitionFileId, agreementId);
      const updatedAgreements = await getAgreements(acquisitionFileId);
      if (updatedAgreements) {
        setAcquisitionAgreements(updatedAgreements);
      }
    }
  };

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return file?.id ? (
    <>
      <View
        loading={loadingAgreements || loadingAcquisition || deletingAgreement}
        agreements={acquisitionAgreements}
        statusUpdateSolver={statusUpdateSolver}
        onGenerate={generateAgreement}
        onDelete={handleAgreementDeleted}
      ></View>
    </>
  ) : null;
};

export default AgreementContainer;
