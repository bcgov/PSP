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
    Header: 'Interest Type',
    accessor: 'interestHolderProperty',
    align: 'left',
    minWidth: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<InterestHolderViewRow, Api_InterestHolderProperty | null>) => {
      return (
        <>{cellProps.row.original.interestHolderProperty?.interestTypeCode?.description ?? ''}</>
      );
    },
  },
];

const InterestHolderPropertiesTable: React.FunctionComponent<
  React.PropsWithChildren<IPropertyHoldersViewTableProps>
> = ({ propertyInterestHolders }) => {
  return (
    <>
      <Table
        name="interest-holders-by-property-table"
        columns={getColumnsByProperty(propertyInterestHolders)}
        hideToolbar
        data={propertyInterestHolders.groupedPropertyInterests}
      />
    </>
  );
};

export default InterestHolderPropertiesTable;
