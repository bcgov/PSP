import { useCallback, useEffect, useState } from 'react';
import { generatePath, useHistory, useRouteMatch } from 'react-router-dom';

import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { exists } from '@/utils';

import ManagementStatusUpdateSolver from './ManagementStatusUpdateSolver';
import { IManagementSummaryViewProps } from './ManagementSummaryView';

export interface IManagementSummaryContainerProps {
  managementFile: ApiGen_Concepts_ManagementFile;
  View: React.FunctionComponent<React.PropsWithChildren<IManagementSummaryViewProps>>;
  onFileEdit: () => void;
}

const ManagementSummaryContainer: React.FunctionComponent<IManagementSummaryContainerProps> = ({
  managementFile,
  View,
  onFileEdit,
}) => {
  const history = useHistory();
  const statusSolver = new ManagementStatusUpdateSolver(managementFile);
  const [fileContacts, setFileContacts] = useState<ApiGen_Concepts_ManagementFileContact[]>([]);

  const {
    getAllManagementFileContacts: {
      execute: retrieveManagementFileContacts,
      loading: loadingManagementFileContacts,
    },
    deleteManagementContact: { execute: deleteContact, loading: loadingDelete },
  } = useManagementFileRepository();

  const matchFile = useRouteMatch<{ id: string }>();

  const fetchContacts = useCallback(async () => {
    const response = await retrieveManagementFileContacts(managementFile.id);
    if (exists(response)) {
      setFileContacts(response);
    }
  }, [managementFile.id, retrieveManagementFileContacts]);

  const handleAddContact = (): void => {
    const path = generatePath(matchFile.path, {
      id: matchFile.params.id,
      detailType: FileTabType.FILE_DETAILS,
    });

    history.push(`${path}/UpdateContactContainer?edit=true`);
  };

  const handleEditContact = (contactId: number): void => {
    const path = generatePath(matchFile.path, {
      id: matchFile.params.id,
      detailType: FileTabType.FILE_DETAILS,
    });

    history.push(`${path}/UpdateContactContainer/${contactId}?edit=true`);
  };

  const handleDeleteContact = useCallback(
    async (contactId: number) => {
      const result = await deleteContact(managementFile.id, contactId);
      if (result) {
        fetchContacts();
      }
    },
    [deleteContact, fetchContacts, managementFile.id],
  );

  useEffect(() => {
    fetchContacts();
  }, [fetchContacts]);

  return managementFile ? (
    <View
      managementFile={managementFile}
      managementFileContacts={fileContacts}
      fileStatusSolver={statusSolver}
      isLoading={loadingManagementFileContacts || loadingDelete}
      onFileEdit={onFileEdit}
      onAddContact={handleAddContact}
      onEditContact={handleEditContact}
      onDeleteContact={handleDeleteContact}
    ></View>
  ) : null;
};

export default ManagementSummaryContainer;
