import { ColumnWithProps, Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import { ILeaseSearchResult } from 'interfaces';
import { useCallback } from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import LeaseProperties from './LeaseProperties';
import LeaseTenants from './LeaseTenants';

const maxPropertyDisplayCount = 2;

const columns: ColumnWithProps<ILeaseSearchResult>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'right',
    clickable: true,
    width: 10,
    Cell: (props: CellProps<ILeaseSearchResult>) => (
      <Link to={`/lease/${props.row.original.id}`}>{props.row.original.lFileNo}</Link>
    ),
  },
  {
    Header: 'Tenant Names',
    accessor: 'tenantNames',
    align: 'left',
    width: 40,
    maxWidth: 100,
    Cell: (props: CellProps<ILeaseSearchResult>) => {
      props.row.original.tenantNames.push(...['Carlitos Romero', 'Federico Suzes', 'James Raynor']);
      return (
        <LeaseTenants
          tenantNames={props.row.original.tenantNames}
          maxDisplayCount={maxPropertyDisplayCount}
        ></LeaseTenants>
      );
    },
  },
  {
    Header: 'Program Name',
    accessor: 'programName',
    align: 'left',
    width: 40,
    maxWidth: 80,
  },
  {
    Header: 'Properties',
    accessor: 'properties',
    align: 'left',

    Cell: (props: CellProps<ILeaseSearchResult>) => {
      // TODO: just for testing
      props.row.original.properties.push(...props.row.original.properties);

      return (
        <LeaseProperties
          properties={props.row.original.properties}
          maxDisplayCount={maxPropertyDisplayCount}
        ></LeaseProperties>
      );
    },
  },
];

export interface ILeaseSearchResultsProps {
  results: ILeaseSearchResult[];
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ILeaseSearchResult>;
  setSort?: (value: TableSort<ILeaseSearchResult>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
}

export function LeaseSearchResults(props: ILeaseSearchResultsProps) {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, ...rest } = props;

  // results sort handler
  const handleSortChange = useCallback(
    (column: string, nextSortDirection: SortDirection) => {
      if (!setSort) return null;

      let nextSort: TableSort<ILeaseSearchResult>;

      // add new column to sort criteria
      if (nextSortDirection) {
        nextSort = { ...sort, [column]: nextSortDirection };
      } else {
        // remove column from sort criteria
        nextSort = { ...sort };
        delete (nextSort as any)[column];
      }
      setSort(nextSort);
    },
    [setSort, sort],
  );

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<ILeaseSearchResult>
      name="leasesTable"
      columns={columns}
      data={results ?? []}
      sort={sort}
      onSortChange={handleSortChange}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="Lease / License details do not exist in PIMS inventory"
      {...rest}
    ></Table>
  );
}
