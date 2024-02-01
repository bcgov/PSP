import axios, { AxiosError } from 'axios';
import React, { useCallback, useContext } from 'react';

import { FileTypes } from '@/constants';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

import { ICompensationListViewProps } from './CompensationListView';

export interface ICompensationListContainerProps {
  fileId: number;
  file: ApiGen_Concepts_AcquisitionFile;
  View: React.FunctionComponent<React.PropsWithChildren<ICompensationListViewProps>>;
}

/**
 * Page that displays compensation list information.
 */
export const CompensationListContainer: React.FC<ICompensationListContainerProps> = ({
  fileId,
  file,
  View,
}: ICompensationListContainerProps) => {
  const {
    getAcquisitionCompensationRequisitions: {
      execute: getAcquisitionCompensationRequisitions,
      response: compensations,
    },
    postAcquisitionCompensationRequisition: { execute: postAcquisitionCompensationRequisition },
    updateAcquisitionFile: { execute: putAcquisitionFile },
  } = useAcquisitionProvider();
  const {
    deleteCompensation: { execute: deleteCompensation },
  } = useCompensationRequisitionRepository();

  const { staleFile, setStaleFile, setFile, setStaleLastUpdatedBy } = useContext(SideBarContext);
  const { setModalContent, setDisplayModal } = useModalContext();

  const fetchData = useCallback(async () => {
    await getAcquisitionCompensationRequisitions(fileId);
  }, [getAcquisitionCompensationRequisitions, fileId]);

  const onUpdateTotalCompensation = async (
    totalAllowableCompensation: number | null,
  ): Promise<number | null> => {
    if (file) {
      const updatedFile: ApiGen_Concepts_AcquisitionFile = {
        ...file,
        totalAllowableCompensation: totalAllowableCompensation ?? null,
      };
      try {
        const response = await putAcquisitionFile(updatedFile, []);
        if (response) {
          setFile({
            ...updatedFile,
            rowVersion: response.rowVersion,
            fileType: FileTypes.Acquisition,
          });
          setStaleLastUpdatedBy(true);
          setStaleFile(true);
          return response.totalAllowableCompensation ?? null;
        }
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          setModalContent({
            variant: 'error',
            title: 'Invalid value',
            message: axiosError?.response?.data?.error,
            okButtonText: 'Close',
          });
          setDisplayModal(true);
          throw Error(axiosError.response?.data?.error);
        }
      }
    }
    return file.totalAllowableCompensation ?? null;
  };

  const onAddCompensationRequisition = (fileId: number) => {
    const defaultCompensationRequisition: ApiGen_Concepts_CompensationRequisition = {
      id: null,
      acquisitionFileId: fileId,
      alternateProjectId: null,
      alternateProject: null,
      isDraft: true,
      fiscalYear: null,
      yearlyFinancialId: null,
      yearlyFinancial: null,
      chartOfAccountsId: null,
      chartOfAccounts: null,
      responsibilityId: null,
      responsibility: null,
      finalizedDate: null,
      agreementDate: null,
      expropriationNoticeServedDate: null,
      expropriationVestingDate: null,
      advancedPaymentServedDate: null,
      generationDate: null,
      specialInstruction: null,
      detailedRemarks: null,
      financials: [],
      acquisitionFile: null,
      acquisitionOwnerId: null,
      acquisitionOwner: null,
      interestHolderId: null,
      interestHolder: null,
      acquisitionFileTeamId: null,
      acquisitionFileTeam: null,
      legacyPayee: null,
      isPaymentInTrust: null,
      gstNumber: null,
      ...getEmptyBaseAudit(),
    };

    postAcquisitionCompensationRequisition(fileId, defaultCompensationRequisition).then(
      async newCompensationReq => {
        if (newCompensationReq?.id) {
          setStaleLastUpdatedBy(true);
          setStaleFile(true);
        }
      },
    );
  };

  React.useEffect(() => {
    if (compensations === undefined || staleFile) {
      fetchData();
    }
  }, [fetchData, staleFile, compensations]);

  return (
    <View
      onUpdateTotalCompensation={onUpdateTotalCompensation}
      acquisitionFile={file}
      compensations={compensations || []}
      onAdd={async () => onAddCompensationRequisition(fileId)}
      onDelete={async (compensationId: number) => {
        setModalContent({
          ...getDeleteModalProps(),
          handleOk: async () => {
            const result = await deleteCompensation(compensationId);
            if (result === true) {
              setStaleLastUpdatedBy(true);
              setStaleFile(true);
            }
            setDisplayModal(false);
          },
          handleCancel: () => {
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      }}
    />
  );
};

export default CompensationListContainer;
