import { ColumnWithProps, Table } from 'components/Table';
import { ILease } from 'interfaces';
import React from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

export interface ILeaseSearchResultsProps {
  results?: ILease[];
}

export function LeaseSearchResults({ results }: ILeaseSearchResultsProps) {
  return <Table<ILease> name="leasesTable" columns={columns} data={results ?? []}></Table>;
}

const columns: ColumnWithProps<ILease>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'right',
    clickable: true,
    Cell: (props: CellProps<ILease>) => 'N/A', // TODO:
    // Cell: (props: CellProps<ILease>) => (
    //   <Link to={`/lease/${props.row.original.id}`}>{props.row.original.lFileNo}</Link>
    // ),
  },
  {
    Header: 'Tenant Name',
    align: 'left',
    clickable: true,
    Cell: (props: CellProps<ILease>) => 'N/A', // TODO:
    // Cell: (props: CellProps<ILease>) =>
    //   props.row.original.persons ? props.row.original.persons[0]?.fullName : '',
  },
  {
    Header: 'Program Name',
    accessor: 'programName',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'PID/PIN',
    align: 'left',
    clickable: true,
    Cell: (props: CellProps<ILease>) => 'N/A', // TODO:
  },
  {
    Header: 'Civic Address',
    align: 'left',
    clickable: true,
    Cell: (props: CellProps<ILease>) => 'N/A', // TODO:
  },
];
