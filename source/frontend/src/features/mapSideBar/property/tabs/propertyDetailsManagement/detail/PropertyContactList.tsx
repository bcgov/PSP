import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaEdit, FaTrash } from 'react-icons/fa';
import { CellProps } from 'react-table';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import { StyledRemoveIconButton } from '@/components/common/buttons/RemoveButton';
import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import { ColumnWithProps, Table } from '@/components/Table';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';
import { stringToFragment } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

interface IPropertyContactListProps {
  propertyContacts: ApiGen_Concepts_PropertyContact[];
  handleEdit: (contactId: number) => void;
  handleDelete: (contactId: number) => void;
}

export function createContactTableColumns(
  onEdit: (propertyContactId: number) => void,
  onDelete: (propertyContactId: number) => void,
) {
  const columns: ColumnWithProps<ApiGen_Concepts_PropertyContact>[] = [
    {
      Header: 'Contact',
      align: 'left',
      minWidth: 60,
      maxWidth: 60,
      Cell: (
        cellProps: CellProps<
          ApiGen_Concepts_PropertyContact,
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
          ApiGen_Concepts_PropertyContact,
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
        cellProps: CellProps<ApiGen_Concepts_PropertyContact, ApiGen_Concepts_Property | undefined>,
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
      Cell: (cellProps: CellProps<ApiGen_Concepts_PropertyContact>) => {
        const { hasClaim } = useKeycloakWrapper();
        return (
          <Row className="no-gutters">
            {hasClaim(Claims.PROPERTY_EDIT) && (
              <Col>
                <LinkButton
                  icon={<FaEdit size={'2rem'} />}
                  onClick={() => cellProps.row.original.id && onEdit(cellProps.row.original.id)}
                ></LinkButton>
              </Col>
            )}
            {hasClaim(Claims.PROPERTY_EDIT) && (
              <StyledRemoveIconButton
                id={`contact-delete-${cellProps.row.id}`}
                data-testid={`contact-delete-${cellProps.row.id}`}
                onClick={() => cellProps.row.original.id && onDelete(cellProps.row.original.id)}
                title="Delete contact"
              >
                <FaTrash size="2rem" />
              </StyledRemoveIconButton>
            )}
          </Row>
        );
      },
    },
  ];

  return columns;
}

const PropertyContactList: React.FunctionComponent<IPropertyContactListProps> = ({
  propertyContacts,
  handleEdit,
  handleDelete,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const onEdit = (id: number) => {
    handleEdit(id);
  };

  const onDeleteConfirm = useCallback(
    (id: number) => {
      handleDelete(id);
    },
    [handleDelete],
  );

  const onDelete = useCallback(
    (id: number) => {
      setModalContent({
        variant: 'info',
        title: 'Confirm delete',
        message: 'This contact will be removed from the Property contacts. Do you wish to proceed?',
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
    <Table<ApiGen_Concepts_PropertyContact>
      name="PropertyContactsTable"
      manualSortBy={false}
      lockPageSize={true}
      manualPagination={true}
      hidePagination
      hideToolbar
      totalItems={propertyContacts.length}
      columns={createContactTableColumns(onEdit, onDelete)}
      data={propertyContacts ?? []}
      noRowsMessage="No property contacts found"
    />
  );
};

export default PropertyContactList;
