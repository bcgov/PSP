import { ColumnWithProps, Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { IProjectSearchResult } from 'interfaces';
import { useCallback } from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import { prettyFormatDate } from 'utils';

const columns: ColumnWithProps<IProjectSearchResult>[] = [
  {
    Header: 'Project #',
    accessor: 'code',
    align: 'center',
    clickable: false,
    sortable: false,
    width: 5,
    maxWidth: 20,
    Cell: (props: CellProps<IProjectSearchResult>) => (
      <Link to={`/mapview/sidebar/project/${props.row.original.id}`}>
        {props.row.original.code}
      </Link>
    ),
  },
  {
    Header: 'Project name',
    accessor: 'description',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 45,
    maxWidth: 45,
    Cell: (props: CellProps<IProjectSearchResult>) => (
      <Link to={`/mapview/sidebar/project/${props.row.original.id}`}>
        {props.row.original.description}
      </Link>
    ),
  },
  {
    Header: 'Region',
    accessor: 'region',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 20,
    maxWidth: 20,
  },
  {
    Header: 'Status',
    accessor: 'status',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 20,
    maxWidth: 20,
  },
  {
    Header: 'Last updated by',
    accessor: 'lastUpdatedBy',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Updated date',
    accessor: 'lastUpdatedDate',
    align: 'left',
    sortable: false,
    width: 10,
    Cell: (props: CellProps<IProjectSearchResult>) => {
      const updateDate = props.row.original.lastUpdatedDate;
      return (
        <>
          <span>{prettyFormatDate(updateDate)}</span>
        </>
      );
    },
  },
];

export interface IProjectSearchResultsProps {
  results: IProjectSearchResult[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<IProjectSearchResult>;
  setSort: (value: TableSort<IProjectSearchResult>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

export const ProjectSearchResults = (props: IProjectSearchResultsProps) => {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<IProjectSearchResult>
      name="projectsTable"
      columns={columns}
      data={results ?? []}
      externalSort={{ sort: sort, setSort: setSort }}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="Project details do not exist in PIMS inventory"
      totalItems={props.totalItems}
      {...rest}
    ></Table>
  );
};
