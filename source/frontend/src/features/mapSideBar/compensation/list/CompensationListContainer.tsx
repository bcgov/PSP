import axios, { AxiosError } from 'axios';
import React, { useCallback, useContext } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcquisitionProperty } from '@/models/api/generated/ApiGen_Concepts_CompReqAcquisitionProperty';
import { ApiGen_Concepts_CompReqLeaseProperty } from '@/models/api/generated/ApiGen_Concepts_CompReqLeaseProperty';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

import { ICompensationListViewProps } from './CompensationListView';

export interface ICompensationListContainerProps {
  fileType: ApiGen_CodeTypes_FileTypes;
  file: ApiGen_Concepts_AcquisitionFile | ApiGen_Concepts_Lease;
  View: React.FunctionComponent<React.PropsWithChildren<ICompensationListViewProps>>;
}

/**
 * Page that displays compensation list information.
 */
export const CompensationListContainer: React.FC<ICompensationListContainerProps> = ({
  fileType,
  file,
  View,
}: ICompensationListContainerProps) => {
  const sidebar = useContext(SideBarContext);
  const history = useHistory();
  const location = useLocation();
  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    updateAcquisitionFile: { execute: putAcquisitionFile },
  } = useAcquisitionProvider();

  const {
    updateLease: { execute: updateLease },
  } = useLeaseRepository();

  const {
    deleteCompensation: { execute: deleteCompensation },
    getFileCompensationRequisitions: {
      execute: fetchCompensationRequisitions,
      response: compensations,
    },
    postCompensationRequisition: { execute: createCompensationRequisition },
  } = useCompensationRequisitionRepository();

  const fetchData = useCallback(async () => {
    await fetchCompensationRequisitions(fileType, file.id);
  }, [fetchCompensationRequisitions, file, fileType]);

  const onUpdateTotalCompensation = async (
    totalAllowableCompensation: number | null,
  ): Promise<number | null> => {
    if (file) {
      let updatedFileResponse: ApiGen_Concepts_File | null;

      try {
        switch (fileType) {
          case ApiGen_CodeTypes_FileTypes.Acquisition:
            {
              const updatedAcquisitionFile: ApiGen_Concepts_AcquisitionFile = {
                ...(file as ApiGen_Concepts_AcquisitionFile),
                totalAllowableCompensation: totalAllowableCompensation ?? null,
              };

              updatedFileResponse = await putAcquisitionFile(updatedAcquisitionFile, []);
            }
            break;
          case ApiGen_CodeTypes_FileTypes.Lease:
            {
              const updatedLease = file as ApiGen_Concepts_Lease;
              updatedLease.totalAllowableCompensation = totalAllowableCompensation;

              updatedFileResponse = await updateLease(updatedLease, []);
            }
            break;
          default:
            updatedFileResponse = null;
            break;
        }

        if (updatedFileResponse) {
          sidebar.setFile({
            ...updatedFileResponse,
            rowVersion: updatedFileResponse.rowVersion,
            fileType: fileType,
          });
          sidebar.setStaleLastUpdatedBy(true);
          sidebar.setStaleFile(true);

          if (fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
            return (file as ApiGen_Concepts_AcquisitionFile).totalAllowableCompensation ?? 0;
          } else if (fileType === ApiGen_CodeTypes_FileTypes.Lease) {
            return (file as ApiGen_Concepts_Lease).totalAllowableCompensation ?? 0;
          }
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
  };

  const getDefaultCompensationRequisition = (
    fileType: ApiGen_CodeTypes_FileTypes,
    fileId: number,
  ): ApiGen_Concepts_CompensationRequisition | null => {
    const newCompensationRequisition: ApiGen_Concepts_CompensationRequisition = {
      id: null,
      acquisitionFileId: null,
      leaseId: null,
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
      compReqLeaseStakeholder: null,
      compReqAcquisitionProperties: null,
      compReqLeaseProperties: null,
      ...getEmptyBaseAudit(),
    };

    switch (fileType) {
      case ApiGen_CodeTypes_FileTypes.Acquisition:
        newCompensationRequisition.acquisitionFileId = fileId;
        newCompensationRequisition.leaseId = null;
        newCompensationRequisition.compReqAcquisitionProperties =
          file.fileProperties?.map(x => {
            return {
              compensationRequisitionPropertyId: null,
              compensationRequisitionId: null,
              propertyAcquisitionFileId: x.id,
              acquisitionFileProperty: null,
            } as ApiGen_Concepts_CompReqAcquisitionProperty;
          }) || [];
        break;
      case ApiGen_CodeTypes_FileTypes.Lease:
        newCompensationRequisition.leaseId = fileId;
        newCompensationRequisition.acquisitionFileId = null;
        newCompensationRequisition.compReqLeaseProperties =
          file.fileProperties?.map(x => {
            return {
              compensationRequisitionPropertyId: null,
              compensationRequisitionId: null,
              propertyLeaseId: x.id,
              leaseProperty: null,
            } as ApiGen_Concepts_CompReqLeaseProperty;
          }) || [];
        break;
      default:
        return null;
    }

    return newCompensationRequisition;
  };

  const onAddCompensationRequisition = () => {
    try {
      createCompensationRequisition(
        fileType,
        getDefaultCompensationRequisition(fileType, file.id),
      ).then(async newCompensationReq => {
        if (newCompensationReq?.id) {
          sidebar.setStaleLastUpdatedBy(true);
          sidebar.setStaleFile(true);
        }
      });
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError.response?.status === 400) {
          toast.error(axiosError.response.data.error);
        } else {
          toast.error('Unable to save. Please try again.');
        }
      }
    }
  };

  React.useEffect(() => {
    if (compensations === undefined || sidebar.staleFile) {
      fetchData();
    }
  }, [fetchData, sidebar.staleFile, compensations]);

  return (
    <View
      fileType={fileType}
      file={file}
      onUpdateTotalCompensation={onUpdateTotalCompensation}
      compensations={compensations || []}
      onAdd={async () => onAddCompensationRequisition()}
      onDelete={async (compensationId: number) => {
        setModalContent({
          ...getDeleteModalProps(),
          handleOk: async () => {
            const result = await deleteCompensation(compensationId);
            if (result) {
              sidebar.setStaleLastUpdatedBy(true);
              sidebar.setStaleFile(true);
              const backUrl = location.pathname.split('/compensation-requisition')[0];
              history.push(backUrl);
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
