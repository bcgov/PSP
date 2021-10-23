import { ColumnWithProps, Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import { ILeaseSearchResult } from 'interfaces';
import { useCallback } from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

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
  const { results, sort, setSort, setPageSize, setPageIndex, ...rest } = props;

  const updateSortCriteria = useCallback(
    (prevValue: TableSort<ILeaseSearchResult> = {}) => {
      return function sortFn(column: string, nextSortDirection: SortDirection) {
        if (!setSort) return;

        let nextSort: TableSort<ILeaseSearchResult>;

        // add new column to sort criteria
        if (nextSortDirection) {
          nextSort = { ...prevValue, [column]: nextSortDirection };
        } else {
          // remove column from sort criteria
          nextSort = { ...sort };
          delete (nextSort as any)[column];
        }
        setSort(nextSort);
      };
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
      onSortChange={updateSortCriteria(sort)}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      {...rest}
    ></Table>
  );
}

const columns: ColumnWithProps<ILeaseSearchResult>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'right',
    clickable: true,
    Cell: (props: CellProps<ILeaseSearchResult>) => (
      <Link to={`/lease/${props.row.original.id}`}>{props.row.original.lFileNo}</Link>
    ),
  },
  {
    Header: 'Tenant Name',
    accessor: 'tenantName',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Program Name',
    accessor: 'programName',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'PID/PIN',
    accessor: 'pidOrPin',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Civic Address',
    accessor: 'address',
    align: 'left',
    clickable: true,
  },
];
