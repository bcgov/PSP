import { useRequisitionCompensationRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { getDeleteModalProps, useModalContext } from 'hooks/useModalContext';
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
    getFileCompensations: { execute: getCompensations, response: compensations },
    deleteCompensation: { execute: deleteCompensation },
  } = useRequisitionCompensationRepository();

  const { staleFile, setStaleFile } = useContext(SideBarContext);
  const { setModalContent, setDisplayModal } = useModalContext();

  const fetchData = useCallback(async () => {
    await getCompensations(fileId);
  }, [getCompensations, fileId]);

  React.useEffect(() => {
    if (compensations === undefined || staleFile) {
      fetchData();
    }
  }, [fetchData, staleFile, setStaleFile, compensations]);

  return (
    <View
      compensations={compensations || []}
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
