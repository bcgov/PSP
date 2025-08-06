import { useCallback } from 'react';
import { FaEdit } from 'react-icons/fa';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import { RemoveIconButton } from '@/components/common/buttons/RemoveButton';
import { InlineFlexDiv, StyledLink } from '@/components/common/styles';
import { ColumnWithProps, Table } from '@/components/Table';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { stringToFragment } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

interface IManagementFileContacstListProps {
  managementFileContacts: ApiGen_Concepts_ManagementFileContact[];
  onContactEdit: (contactId: number) => void;
  onContactDelete: (contactId: number) => void;
}

export function createContactTableColumns(
  onEdit: (managementFileContactId: number) => void,
  onDelete: (managementFileContactId: number) => void,
) {
  const columns: ColumnWithProps<ApiGen_Concepts_ManagementFileContact>[] = [
    {
      Header: 'Contact',
      align: 'left',
      minWidth: 60,
      maxWidth: 60,
      Cell: (
        cellProps: CellProps<
          ApiGen_Concepts_ManagementFileContact,
          ApiGen_Concepts_Organization | ApiGen_Concepts_Person | undefined
        >,
      ) => {
        return (
          <StyledLink
            target="_blank"
            rel="noopener noreferrer"
            to={`/contact/${
              cellProps.row.original.person
                ? `P${cellProps.row.original.person.id}`
                : `O${cellProps.row.original.organization?.id}`
            }`}
          >
            {cellProps.row.original?.person
              ? formatApiPersonNames(cellProps.row.original?.person)
              : cellProps.row.original?.organization?.name ?? ''}
          </StyledLink>
        );
      },
    },
    {
      Header: 'Primary Contact',
      align: 'left',
      minWidth: 60,
      maxWidth: 60,
      Cell: (
        cellProps: CellProps<
          ApiGen_Concepts_ManagementFileContact,
          ApiGen_Concepts_Organization | ApiGen_Concepts_Person | undefined
        >,
      ) => {
        const isOrganization = cellProps.row.original.organization !== null;
        const primaryContact = cellProps.row.original.primaryContact;
        if (primaryContact !== null) {
          return (
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={`/contact/${`P${primaryContact.id}`}`}
            >
              {formatApiPersonNames(primaryContact)}
            </StyledLink>
          );
        } else {
          return <>{isOrganization ? 'No contacts available' : 'Not applicable'}</>;
        }
      },
    },
    {
      Header: 'Purpose',
      align: 'left',
      minWidth: 60,
      maxWidth: 60,
      Cell: (
        cellProps: CellProps<
          ApiGen_Concepts_ManagementFileContact,
          ApiGen_Concepts_Property | undefined
        >,
      ) => {
        return stringToFragment(cellProps.row.original.purpose || '');
      },
    },
    {
      Header: 'Actions',
      align: 'left',
      sortable: false,
      width: 25,
      maxWidth: 25,
      Cell: (cellProps: CellProps<ApiGen_Concepts_ManagementFileContact>) => {
        const { hasClaim } = useKeycloakWrapper();
        return (
          <ActionsDiv>
            {hasClaim(Claims.MANAGEMENT_EDIT) && (
              <LinkButton
                icon={<FaEdit size={'2rem'} />}
                onClick={() => cellProps.row.original.id && onEdit(cellProps.row.original.id)}
              ></LinkButton>
            )}

            {hasClaim(Claims.MANAGEMENT_EDIT) && (
              <RemoveIconButton
                id={`contact-delete-${cellProps.row.id}`}
                data-testId={`contact-delete-${cellProps.row.id}`}
                onRemove={() => cellProps.row.original.id && onDelete(cellProps.row.original.id)}
                title="Delete contact"
              />
            )}
          </ActionsDiv>
        );
      },
    },
  ];

  return columns;
}

const ManagementFileContactsList: React.FunctionComponent<IManagementFileContacstListProps> = ({
  managementFileContacts,
  onContactEdit,
  onContactDelete,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const handleEdit = (id: number) => {
    onContactEdit(id);
  };

  const onDeleteConfirm = useCallback(
    (id: number) => {
      onContactDelete(id);
    },
    [onContactDelete],
  );

  const handleDelete = useCallback(
    (id: number) => {
      setModalContent({
        variant: 'info',
        title: 'Confirm delete',
        message: 'This contact will be removed from the Management File. Do you wish to proceed?',
        okButtonText: 'Confirm',
        cancelButtonText: 'Cancel',
        handleOk: () => {
          onDeleteConfirm(id);
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    },
    [setDisplayModal, setModalContent, onDeleteConfirm],
  );

  return (
    <Table<ApiGen_Concepts_ManagementFileContact>
      name="ManagementFileContactsContactsTable"
      manualSortBy={false}
      manualPagination={false}
      totalItems={managementFileContacts.length}
      columns={createContactTableColumns(handleEdit, handleDelete)}
      data={managementFileContacts ?? []}
      noRowsMessage="No contacts found"
    />
  );
};

export default ManagementFileContactsList;

const ActionsDiv = styled(InlineFlexDiv)`
  justify-content: center;
  align-items: center;
  flex-grow: 1;
  align-content: space-between;
`;
