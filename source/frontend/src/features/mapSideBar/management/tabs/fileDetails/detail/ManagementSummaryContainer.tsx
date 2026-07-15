import { useCallback, useEffect, useState } from 'react';
import { generatePath, useHistory, useRouteMatch } from 'react-router-dom';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists, isValidId } from '@/utils';

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
  const [person, setPerson] = useState<ApiGen_Concepts_Person | null>(null);
  const [organization, setOrganization] = useState<ApiGen_Concepts_Organization | null>(null);
  const [primaryContact, setPrimaryContact] = useState<ApiGen_Concepts_Person | null>(null);
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

  const handleDeleteContact = useCallback(
    async (contactId: number) => {
      const result = await deleteContact(managementFile.id, contactId);
      if (result) {
        fetchContacts();
      }
    },
    [deleteContact, fetchContacts, managementFile.id],
  );

  const {
    getPersonDetail: { execute: getPerson, loading: getPersonLoading },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: getOrganizationLoading },
  } = useOrganizationRepository();

  const fetchData = useCallback(async () => {
    if (isValidId(managementFile.responsiblePayerPersonId)) {
      const returnedPerson = await getPerson(managementFile.responsiblePayerPersonId);
      if (exists(returnedPerson)) {
        setPerson(returnedPerson);
      }
    }
    if (isValidId(managementFile.responsiblePayerOrganizationId)) {
      const returnedOrganization = await getOrganization(
        managementFile.responsiblePayerOrganizationId,
      );
      if (exists(returnedOrganization)) {
        setOrganization(returnedOrganization);
      }
    }

    if (isValidId(managementFile.responsiblePayerPrimaryContactId)) {
      const returnedPrimaryContact = await getPerson(
        managementFile.responsiblePayerPrimaryContactId,
      );
      if (exists(returnedPrimaryContact)) {
        setPrimaryContact(returnedPrimaryContact);
      }
    }
  }, [
    getOrganization,
    getPerson,
    managementFile.responsiblePayerOrganizationId,
    managementFile.responsiblePayerPersonId,
    managementFile.responsiblePayerPrimaryContactId,
  ]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  useEffect(() => {
    fetchContacts();
  }, [fetchContacts]);

  if (getPersonLoading || getOrganizationLoading) {
    return <></>;
  }

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
      responsiblePayerPerson={person}
      responsiblePayerOrganization={organization}
      primaryContact={primaryContact}
    ></View>
  ) : null;
};

export default ManagementSummaryContainer;
