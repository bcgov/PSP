import * as React from 'react';
import { CellProps } from 'react-table';

import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import { ColumnWithProps, Table } from '@/components/Table';
import { Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import { Api_Person } from '@/models/api/Person';
import { formatApiPersonNames } from '@/utils/personUtils';

import { InterestHolderViewForm, InterestHolderViewRow } from '../update/models';

interface IPropertyHoldersViewTableProps {
  propertyInterestHolders: InterestHolderViewForm;
}

const getColumnsByProperty = (
  propertyInterestHolders: InterestHolderViewForm,
): ColumnWithProps<InterestHolderViewRow>[] => [
  {
    Header: `${propertyInterestHolders.identifier}`,
    accessor: 'person',
    align: 'left',
    minWidth: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<InterestHolderViewRow, Api_Person | null>) => {
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
    accessor: 'primaryContact',
    align: 'left',
    minWidth: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<InterestHolderViewRow, Api_Person | null>) => {
      return cellProps.row.original?.primaryContact ? (
        <StyledLink
          target="_blank"
          rel="noopener noreferrer"
          to={`/contact/${`P${cellProps.row.original.primaryContact?.id}`}`}
        >
          {formatApiPersonNames(cellProps.row.original?.primaryContact)}
        </StyledLink>
      ) : cellProps.row.original?.person ? (
        <>{'N/A'}</>
      ) : (
        <>{'No contacts available/selected'}</>
      );
    },
  },
  {
    Header: 'Interest Type',
    accessor: 'interestHolderProperty',
    align: 'left',
    minWidth: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<InterestHolderViewRow, Api_InterestHolderProperty | null>) => {
      const propertyInterestType = cellProps.row.original.interestHolderType?.description ?? '';

      return <>{propertyInterestType}</>;
    },
  },
];

const InterestHolderPropertiesTable: React.FunctionComponent<
  React.PropsWithChildren<IPropertyHoldersViewTableProps>
> = ({ propertyInterestHolders }) => {
  const isNonPayeeInterests = propertyInterestHolders.groupedPropertyInterests.find(
    ih => ih.interestHolderType?.id === 'NIP',
  );

  const columns = getColumnsByProperty(propertyInterestHolders);

  if (isNonPayeeInterests) {
    // If interest holders are of type non-interest payees then delete primary contact column
    columns.splice(1, 1);
  }

  return (
    <>
      <Table
        name="interest-holders-by-property-table"
        columns={columns}
        hideToolbar
        data={propertyInterestHolders.groupedPropertyInterests}
      />
    </>
  );
};

export default InterestHolderPropertiesTable;
