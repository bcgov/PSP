import axios, { AxiosError } from 'axios';
import React, { useCallback, useContext } from 'react';
import { toast } from 'react-toastify';

import { FileTypes } from '@/constants';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';

import { ICompensationListViewProps } from './CompensationListView';

export interface ICompensationListContainerProps {
  fileId: number;
  file: Api_AcquisitionFile;
  View: React.FunctionComponent<React.PropsWithChildren<ICompensationListViewProps>>;
}

/**
 * Page that displays compensation list information.
 */
export const CompensationListContainer: React.FunctionComponent<
  React.PropsWithChildren<ICompensationListContainerProps>
> = ({ fileId, file, View }: ICompensationListContainerProps) => {
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

  const { staleFile, setStaleFile, setFile } = useContext(SideBarContext);
  const { setModalContent, setDisplayModal } = useModalContext();

  const fetchData = useCallback(async () => {
    await getAcquisitionCompensationRequisitions(fileId);
  }, [getAcquisitionCompensationRequisitions, fileId]);

  const onUpdateTotalCompensation = async (
    totalAllowableCompensation: number | null,
  ): Promise<number | null> => {
    if (file) {
      const updatedFile = {
        ...file,
        totalAllowableCompensation: totalAllowableCompensation ?? undefined,
      };
      try {
        const response = await putAcquisitionFile(updatedFile, []);
        if (response) {
          setFile({
            ...updatedFile,
            rowVersion: response.rowVersion,
            fileType: FileTypes.Acquisition,
          });
          setStaleFile(true);
          return response.totalAllowableCompensation ?? null;
        }
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          toast.error(axiosError.response?.data.error, { autoClose: 20000 });
          throw Error(axiosError.response?.data.error);
        }
      }
    }
    return file.totalAllowableCompensation ?? null;
  };

  const onAddCompensationRequisition = (fileId: number) => {
    const defaultCompensationRequisition: Api_CompensationRequisition = {
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
      isDisabled: null,
      financials: [],
      acquisitionFile: null,
      acquisitionOwnerId: null,
      acquisitionOwner: null,
      interestHolderId: null,
      interestHolder: null,
      acquisitionFilePersonId: null,
      acquisitionFilePerson: null,
      legacyPayee: null,
      isPaymentInTrust: null,
      gstNumber: null,
    };

    postAcquisitionCompensationRequisition(fileId, defaultCompensationRequisition).then(
      async newCompensationReq => {
        if (newCompensationReq?.id) {
          setStaleFile(true);
        }
      },
    );
  };

  React.useEffect(() => {
    if (compensations === undefined || staleFile) {
      fetchData();
    }
  }, [fetchData, staleFile, setStaleFile, compensations]);

  return (
    <View
      onUpdateTotalCompensation={onUpdateTotalCompensation}
      compensations={compensations || []}
      onAdd={async () => onAddCompensationRequisition(fileId)}
      onDelete={async (compensationId: number) => {
        setModalContent({
          ...getDeleteModalProps(),
          handleOk: async () => {
            const result = await deleteCompensation(compensationId);
            if (result === true) {
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
      totalAllowableCompensation={file?.totalAllowableCompensation || 0}
    />
  );
};

export default CompensationListContainer;
