import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { getDeleteModalProps, useModalContext } from 'hooks/useModalContext';
import { Api_Compensation } from 'models/api/Compensation';
import React, { useCallback, useContext } from 'react';

import { SideBarContext } from '../../context/sidebarContext';
import { ICompensationListViewProps } from './CompensationListView';

export interface ICompensationListContainerProps {
  fileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<ICompensationListViewProps>>;
}

/**
 * Page that displays compensation list information.
 */
export const CompensationListContainer: React.FunctionComponent<
  React.PropsWithChildren<ICompensationListContainerProps>
> = ({ fileId, View }: ICompensationListContainerProps) => {
  const {
    getAcquisitionCompensationRequisitions: {
      execute: getAcquisitionCompensationRequisitions,
      response: compensations,
    },
    postAcquisitionCompensationRequisition: { execute: postAcquisitionCompensationRequisition },
  } = useAcquisitionProvider();

  const {
    deleteCompensation: { execute: deleteCompensation },
  } = useCompensationRequisitionRepository();

  const { staleFile, setStaleFile } = useContext(SideBarContext);
  const { setModalContent, setDisplayModal } = useModalContext();

  const fetchData = useCallback(async () => {
    await getAcquisitionCompensationRequisitions(fileId);
  }, [getAcquisitionCompensationRequisitions, fileId]);

  const onAddCompensationRequisition = (fileId: number) => {
    const defaultCompensationRequisition: Api_Compensation = {
      id: null,
      acquisitionFileId: fileId,
      isDraft: true,
      fiscalYear: null,
      agreementDate: null,
      expropriationNoticeServedDate: null,
      expropriationVestingDate: null,
      generationDate: null,
      specialInstruction: null,
      detailedRemarks: null,
      isDisabled: null,
      financials: [],
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
    />
  );
};

export default CompensationListContainer;
